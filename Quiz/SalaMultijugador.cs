using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

		private bool escuchando = true;
		private bool partidaIniciada = false;

		private string bufferMensajes = "";

		private string ipServidor;

		// Acepta la IP del servidor como parámetro
		public SalaMultijugador(string ip = "127.0.0.1")
		{
			InitializeComponent();

			ipServidor = ip;

			// CORRECCIÓN #5: Informar a Preguntas.cs la IP donde está la API
			Preguntas.IpServidorApi = ipServidor;

			InicializarUI();
			ConectarAlServidor(ipServidor);
		}

		private void InicializarUI()
		{
			this.Text = "Sala Multijugador";
			this.Size = new Size(550, 430);
			this.StartPosition = FormStartPosition.CenterScreen;
			this.BackColor = Color.FromArgb(30, 30, 45);
			this.FormClosing += SalaMultijugador_FormClosing;

			lblEstadoGeneral = new Label()
			{
				Text = "Esperando jugadores...",
				Width = 450,
				Height = 35,
				Location = new Point(40, 20),
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Segoe UI", 12, FontStyle.Bold),
				ForeColor = Color.White
			};

			// CORRECCIÓN #2: Eliminada la línea duplicada this.Controls.Add(lblEstadoGeneral)
			this.Controls.Add(lblEstadoGeneral);

			for (int i = 0; i < 4; i++)
			{
				Panel panel = new Panel()
				{
					Size = new Size(450, 55),
					Location = new Point(40, 70 + (i * 65)),
					BackColor = Color.FromArgb(50, 50, 70)
				};

				Label lblNombre = new Label()
				{
					Name = "lblNombre",
					Text = "Vacío",
					ForeColor = Color.White,
					Font = new Font("Segoe UI", 11, FontStyle.Bold),
					Location = new Point(15, 15),
					AutoSize = true
				};

				Label lblEstado = new Label()
				{
					Name = "lblEstado",
					Text = "No listo",
					ForeColor = Color.Red,
					Font = new Font("Segoe UI", 10, FontStyle.Bold),
					Location = new Point(320, 15),
					AutoSize = true
				};

				panel.Controls.Add(lblNombre);
				panel.Controls.Add(lblEstado);

				paneles.Add(panel);
				this.Controls.Add(panel);
			}

			Button btnListo = new Button()
			{
				Text = "LISTO",
				Size = new Size(180, 40),
				Location = new Point(40, 340),
				BackColor = Color.FromArgb(76, 175, 80),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Font = new Font("Segoe UI", 11, FontStyle.Bold)
			};

			btnListo.Click += (s, e) =>
			{
				EnviarMensaje(new
				{
					tipo = "ready",
					listo = true
				});

				btnListo.Enabled = false;
			};

			this.Controls.Add(btnListo);

			Button btnSalir = new Button()
			{
				Text = "SALIR",
				Size = new Size(180, 40),
				Location = new Point(310, 340),
				BackColor = Color.FromArgb(244, 67, 54),
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat,
				Font = new Font("Segoe UI", 11, FontStyle.Bold)
			};

			btnSalir.Click += (s, e) =>
			{
				EnviarMensaje(new
				{
					tipo = "leave"
				});

				this.Close();
			};

			this.Controls.Add(btnSalir);
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
				MessageBox.Show("Error al conectar: " + ex.Message);
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

					if (bytes <= 0)
						break;

					bufferMensajes += Encoding.UTF8.GetString(buffer, 0, bytes);

					while (bufferMensajes.Contains("\n"))
					{
						int index = bufferMensajes.IndexOf("\n");

						string linea = bufferMensajes.Substring(0, index).Trim();

						bufferMensajes = bufferMensajes.Substring(index + 1);

						if (string.IsNullOrWhiteSpace(linea))
							continue;

						ProcesarMensaje(linea);
					}
				}
			}
			catch
			{
				// Hilo terminado limpiamente
			}
		}

		private void ProcesarMensaje(string json)
		{
			JObject data = JObject.Parse(json);

			string tipo = data["tipo"].ToString();

			if (tipo == "lobby_update")
			{
				this.Invoke((MethodInvoker)delegate
				{
					jugadores.Clear();

					foreach (var jugador in data["jugadores"])
					{
						jugadores.Add(new Jugador()
						{
							Nombre = jugador["nombre"].ToString(),
							Listo = (bool)jugador["listo"]
						});
					}

					ActualizarLobby();
				});
			}
			else if (tipo == "iniciar_partida")
			{
				if (partidaIniciada)
					return;

				partidaIniciada = true;

				string categoria = data["categoria"].ToString();

				this.Invoke((MethodInvoker)delegate
				{
					// Detener escucha DENTRO del Invoke, después de que el form
					// ya procesó el mensaje, para evitar que el hilo muera antes
					// de ejecutar el Invoke
					escuchando = false;

					Preguntas preguntas = new Preguntas(
						categoria,
						Inicio.nombreJugadorGlobal,
						cliente
					);

					preguntas.Show();

					this.Hide();
				});
			}
		}

		private void ActualizarLobby()
		{
			for (int i = 0; i < paneles.Count; i++)
			{
				Panel panel = paneles[i];

				Label lblNombre = panel.Controls.Find("lblNombre", true)[0] as Label;
				Label lblEstado = panel.Controls.Find("lblEstado", true)[0] as Label;

				if (i < jugadores.Count)
				{
					lblNombre.Text = jugadores[i].Nombre;
					lblEstado.Text = jugadores[i].Listo ? "✓ LISTO" : "○ NO LISTO";
					lblEstado.ForeColor = jugadores[i].Listo ? Color.Lime : Color.Red;
				}
				else
				{
					lblNombre.Text = "Vacío";
					lblEstado.Text = "-";
					lblEstado.ForeColor = Color.White;
				}
			}

			int listos = jugadores.Count(j => j.Listo);

			lblEstadoGeneral.Text = $"Jugadores listos: {listos}/{jugadores.Count}";
		}

		private void EnviarMensaje(object obj)
		{
			try
			{
				string json = JsonConvert.SerializeObject(obj) + "\n";

				byte[] data = Encoding.UTF8.GetBytes(json);

				stream.Write(data, 0, data.Length);
			}
			catch
			{
			}
		}

		private void SalaMultijugador_FormClosing(object sender, FormClosingEventArgs e)
		{
			escuchando = false;

			try { stream?.Close(); } catch { }
			try { cliente?.Close(); } catch { }
		}
	}
}