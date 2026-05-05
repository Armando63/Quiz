using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using System.Threading;

namespace Quiz
{
	public partial class SalaMultijugador : Form
	{
		class Jugador
		{
			public string Nombre { get; set; }
			public bool Listo { get; set; }
		}

		private List<Jugador> jugadores = new List<Jugador>();
		private List<Panel> paneles = new List<Panel>();
		private Label lblEstadoGeneral;
		private TcpClient cliente;
		private NetworkStream stream;
		private Thread hiloEscucha;
		private bool partidaIniciada = false;
		private string categoriaSeleccionada = "";
		private bool escuchando = true;

		public SalaMultijugador()
		{
			InitializeComponent();
			ConectarAlServidor("10.103.150.143"); // 👈 CAMBIA POR TU IP
			InicializarUI();

			jugadores.Add(new Jugador
			{
				Nombre = Inicio.nombreJugadorGlobal,
				Listo = false
			});

			ActualizarLobby();
		}

		private void InicializarUI()
		{
			this.Text = "Lobby - Esperando jugadores";
			this.Size = new Size(500, 400);
			this.FormClosing += SalaMultijugador_FormClosing;

			lblEstadoGeneral = new Label()
			{
				Text = "Esperando jugadores...",
				AutoSize = false,
				Width = 400,
				Height = 30,
				Location = new Point(50, 20),
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Arial", 12, FontStyle.Bold)
			};

			this.Controls.Add(lblEstadoGeneral);

			for (int i = 0; i < 4; i++)
			{
				Panel p = new Panel()
				{
					Size = new Size(400, 50),
					Location = new Point(50, 70 + (i * 60)),
					BackColor = Color.LightGray,
					Tag = i
				};

				Label lblNombre = new Label()
				{
					Name = "lblNombre",
					Text = "Vacío",
					Location = new Point(10, 15),
					AutoSize = true
				};

				Label lblEstado = new Label()
				{
					Name = "lblEstado",
					Text = "No listo",
					Location = new Point(300, 15),
					AutoSize = true
				};

				p.Controls.Add(lblNombre);
				p.Controls.Add(lblEstado);

				paneles.Add(p);
				this.Controls.Add(p);
			}

			Button btnSalir = new Button()
			{
				Text = "Regresar",
				Size = new Size(120, 35),
				Location = new Point(180, 320),
				BackColor = Color.LightCoral
			};

			btnSalir.Click += (s, e) =>
			{
				EnviarMensaje(new { tipo = "leave", nombre = Inicio.nombreJugadorGlobal });
				Thread.Sleep(100);
				this.Close();
			};

			this.Controls.Add(btnSalir);

			Button btnListo = new Button()
			{
				Text = "Listo",
				Size = new Size(120, 35),
				Location = new Point(50, 320),
				BackColor = Color.LightGreen
			};

			btnListo.Click += (s, e) =>
			{
				bool nuevoEstado = !jugadores.First(j => j.Nombre == Inicio.nombreJugadorGlobal).Listo;

				EnviarMensaje(new
				{
					tipo = "ready",
					nombre = Inicio.nombreJugadorGlobal,
					listo = nuevoEstado
				});
			};

			this.Controls.Add(btnListo);
		}

		private void SalaMultijugador_FormClosing(object sender, FormClosingEventArgs e)
		{
			// Asegurar que se cierra la conexión
			if (cliente != null && cliente.Connected)
			{
				EnviarMensaje(new { tipo = "leave", nombre = Inicio.nombreJugadorGlobal });
			}
		}

		private void ActualizarLobby()
		{
			for (int i = 0; i < paneles.Count; i++)
			{
				var panel = paneles[i];
				var lblNombre = panel.Controls.Find("lblNombre", true).First() as Label;
				var lblEstado = panel.Controls.Find("lblEstado", true).First() as Label;

				if (i < jugadores.Count)
				{
					var j = jugadores[i];
					lblNombre.Text = j.Nombre;
					lblEstado.Text = j.Listo ? "✓ Listo" : "○ No listo";
					lblEstado.ForeColor = j.Listo ? Color.Green : Color.Red;
				}
				else
				{
					lblNombre.Text = "⚫ Vacío";
					lblEstado.Text = "-";
					lblEstado.ForeColor = Color.Black;
				}
			}

			bool todosListos = jugadores.Count >= 2 && jugadores.All(j => j.Listo);

			if (todosListos && !partidaIniciada)
			{
				lblEstadoGeneral.Text = "✅ ¡Todos listos! Iniciando partida...";
				lblEstadoGeneral.ForeColor = Color.Green;
			}
			else if (jugadores.Count < 2)
			{
				lblEstadoGeneral.Text = $"⏳ Esperando más jugadores... ({jugadores.Count}/2 mínimo)";
				lblEstadoGeneral.ForeColor = Color.Yellow;
			}
			else
			{
				int listos = jugadores.Count(j => j.Listo);
				lblEstadoGeneral.Text = $"🎮 Jugadores listos: {listos}/{jugadores.Count}";
				lblEstadoGeneral.ForeColor = Color.White;
			}
		}

		private void ConectarAlServidor(string ip)
		{
			try
			{
				cliente = new TcpClient();
				cliente.Connect(ip, 5000);
				stream = cliente.GetStream();

				EnviarMensaje(new
				{
					tipo = "join",
					nombre = Inicio.nombreJugadorGlobal
				});

				hiloEscucha = new Thread(EscucharServidor);
				hiloEscucha.IsBackground = true;
				hiloEscucha.Start();

				Console.WriteLine($"Conectado al servidor {ip}:5000");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error al conectar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void EscucharServidor()
		{
			byte[] buffer = new byte[4096];

			try
			{
				while (escuchando)
				{
					int bytes = stream.Read(buffer, 0, buffer.Length);
					if (bytes == 0) break;

					string json = Encoding.UTF8.GetString(buffer, 0, bytes);
					Console.WriteLine($"Mensaje del servidor: {json}");

					dynamic data = JsonConvert.DeserializeObject(json);
					string tipo = data.tipo.ToString();

					if (tipo == "lobby_update")
					{
						this.Invoke((MethodInvoker)delegate {
							jugadores.Clear();
							foreach (var j in data.jugadores)
							{
								jugadores.Add(new Jugador
								{
									Nombre = j.nombre.ToString(),
									Listo = j.listo
								});
							}
							ActualizarLobby();
						});
					}
					else if (tipo == "iniciar_partida")
					{
						if (!partidaIniciada)
						{
							partidaIniciada = true;
							categoriaSeleccionada = data.categoria.ToString();

							Console.WriteLine($"🎮 ¡Partida iniciada! Categoría: {categoriaSeleccionada}");

							// DETENER escucha del lobby
							escuchando = false;

							this.Invoke((MethodInvoker)delegate {
								this.Hide();

								Preguntas ventanaPreguntas = new Preguntas(
									categoriaSeleccionada,
									Inicio.nombreJugadorGlobal,
									cliente
								);

								ventanaPreguntas.FormClosed += (s, args) => {
									this.Close();
								};

								ventanaPreguntas.Show();
							});
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en escucha: {ex.Message}");
			}
		}

		private void EnviarMensaje(object obj)
		{
			try
			{
				string json = JsonConvert.SerializeObject(obj) + "\n";
				byte[] data = Encoding.UTF8.GetBytes(json);
				stream.Write(data, 0, data.Length);
				Console.WriteLine($"Enviado: {json}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al enviar mensaje: {ex.Message}");
			}
	}


	}
}