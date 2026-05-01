using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

		public SalaMultijugador()
		{
			InitializeComponent();
			InicializarUI();

			// Agregar SOLO al jugador actual
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
				this.Close(); // regresa al Inicio automáticamente
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
				jugadores[0].Listo = !jugadores[0].Listo;
				ActualizarLobby();
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
				? "Todos listos ✅"
				: "Esperando jugadores o listos...";
		}

		// 🔥 Simulación (para pruebas)
		private void SimularJugadores()
		{
			jugadores.Add(new Jugador { Nombre = "Iván", Listo = true });
			jugadores.Add(new Jugador { Nombre = "Luis", Listo = false });
			jugadores.Add(new Jugador { Nombre = "Ana", Listo = true });
		}

		// 🔹 Método que usarías luego con sockets/API
		public void ActualizarJugador(string nombre, bool listo)
		{
			var jugador = jugadores.FirstOrDefault(j => j.Nombre == nombre);
			if (jugador != null)
				jugador.Listo = listo;
			else if (jugadores.Count < 4)
				jugadores.Add(new Jugador { Nombre = nombre, Listo = listo });

			ActualizarLobby();
		}
	}
}