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
			this.Text = "Lobby";
			this.Size = new Size(500, 400);

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
					lblEstado.Text = j.Listo ? "Listo" : "No listo";
					lblEstado.ForeColor = j.Listo ? Color.Green : Color.Red;
				}
				else
				{
					lblNombre.Text = "Vacío";
					lblEstado.Text = "-";
					lblEstado.ForeColor = Color.Black;
				}
			}

			bool todosListos = jugadores.Count > 0 && jugadores.All(j => j.Listo);

			lblEstadoGeneral.Text = todosListos
				? "Todos listos ✅ Iniciando partida..."
				: "Esperando jugadores o listos...";
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
				while (true)
				{
					int bytes = stream.Read(buffer, 0, buffer.Length);
					if (bytes == 0) break;

					string json = Encoding.UTF8.GetString(buffer, 0, bytes);
					dynamic data = JsonConvert.DeserializeObject(json);

					if (data.tipo == "lobby_update")
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
					else if (data.tipo == "iniciar_partida")
					{
						if (!partidaIniciada)
						{
							partidaIniciada = true;
							string categoria = data.categoria.ToString();

							this.Invoke((MethodInvoker)delegate {
								Preguntas ventanaPreguntas = new Preguntas(categoria, Inicio.nombreJugadorGlobal, cliente);
								ventanaPreguntas.Show();
								this.Hide();
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
				string json = JsonConvert.SerializeObject(obj);
				byte[] data = Encoding.UTF8.GetBytes(json);
				stream.Write(data, 0, data.Length);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al enviar mensaje: {ex.Message}");
			}
		}
	}
}