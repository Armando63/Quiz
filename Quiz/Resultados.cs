using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Quiz
{
	public partial class Resultados : Form
	{
		string connStr = "server=localhost;database=quiz_bd;uid=root;pwd=1234;";

		int puntaje;
		int total;
		bool esMultijugador;
		dynamic resultadosMultijugador;
		string jugadorActual;

		private Label lblResultado;
		private Label txtMensaje;
		private DataGridView dgvJugadores;
		private Button btnCerrar;
		private TabControl tabControl;

		public Resultados(int puntaje, int total)
		{
			this.puntaje = puntaje;
			this.total = total;
			this.esMultijugador = false;
			InitializeComponent();
			ConfigurarFormulario();
		}

		public Resultados(dynamic resultados, string nombreJugador, int puntajePersonal)
		{
			this.resultadosMultijugador = resultados;
			this.jugadorActual = nombreJugador;
			this.puntaje = puntajePersonal;
			this.total = 0;
			this.esMultijugador = true;
			InitializeComponent();
			ConfigurarFormulario();
		}

		private void ConfigurarFormulario()
		{
			this.Text = esMultijugador ? "Resultados Multijugador" : "Resultados";
			this.Size = new Size(700, 550);
			this.BackColor = Color.FromArgb(30, 30, 45);
			this.StartPosition = FormStartPosition.CenterScreen;

			tabControl = new TabControl();
			tabControl.Location = new Point(20, 20);
			tabControl.Size = new Size(640, 440);
			tabControl.Font = new Font("Segoe UI", 10, FontStyle.Bold);

			if (esMultijugador)
			{
				TabPage tabPartidaActual = new TabPage("🎮 Resultados de la Partida");
				ConfigurarTabPartidaActual(tabPartidaActual);
				tabControl.TabPages.Add(tabPartidaActual);

				TabPage tabTuRendimiento = new TabPage("📊 Tu Rendimiento");
				ConfigurarTabTuRendimiento(tabTuRendimiento);
				tabControl.TabPages.Add(tabTuRendimiento);

				TabPage tabHistorial = new TabPage("🏆 Historial General");
				ConfigurarTabHistorial(tabHistorial);
				tabControl.TabPages.Add(tabHistorial);
			}
			else
			{
				TabPage tabResultados = new TabPage("🎯 Tus Resultados");
				ConfigurarTabResultadosSingleplayer(tabResultados);
				tabControl.TabPages.Add(tabResultados);

				TabPage tabHistorial = new TabPage("🏆 Historial General");
				ConfigurarTabHistorial(tabHistorial);
				tabControl.TabPages.Add(tabHistorial);
			}

			this.Controls.Add(tabControl);

			btnCerrar = new Button();
			btnCerrar.Text = "Cerrar";
			btnCerrar.Location = new Point(270, 470);
			btnCerrar.Size = new Size(140, 40);
			btnCerrar.BackColor = Color.FromArgb(244, 67, 54);
			btnCerrar.ForeColor = Color.White;
			btnCerrar.FlatStyle = FlatStyle.Flat;
			btnCerrar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
			btnCerrar.Click += (s, e) => { this.Close(); };
			this.Controls.Add(btnCerrar);

			this.Load += (s, e) => { if (!esMultijugador) MostrarResultadoSingleplayer(); };
		}

		private void ConfigurarTabPartidaActual(TabPage tab)
		{
			tab.BackColor = Color.FromArgb(45, 45, 60);

			Label lblTitulo = new Label()
			{
				Text = "🏆 RESULTADOS DE LA PARTIDA 🏆",
				Location = new Point(20, 15),
				Size = new Size(600, 35),
				Font = new Font("Segoe UI", 14, FontStyle.Bold),
				ForeColor = Color.FromArgb(255, 215, 0),
				TextAlign = ContentAlignment.MiddleCenter
			};
			tab.Controls.Add(lblTitulo);

			DataGridView dgvPartidaActual = new DataGridView()
			{
				Location = new Point(20, 60),
				Size = new Size(580, 300),
				BackgroundColor = Color.White,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
				AllowUserToAddRows = false,
				ReadOnly = true,
				RowHeadersVisible = false
			};

			dgvPartidaActual.ColumnCount = 4;
			dgvPartidaActual.Columns[0].Name = "Puesto";
			dgvPartidaActual.Columns[1].Name = "Jugador";
			dgvPartidaActual.Columns[2].Name = "Puntaje";
			dgvPartidaActual.Columns[3].Name = "Medalla";
			dgvPartidaActual.Columns[0].Width = 60;
			dgvPartidaActual.Columns[1].Width = 180;
			dgvPartidaActual.Columns[2].Width = 100;
			dgvPartidaActual.Columns[3].Width = 80;

			var jugadoresOrdenados = ((System.Collections.IEnumerable)resultadosMultijugador)
				.Cast<dynamic>()
				// CORRECCIÓN: Usar Convert.ToInt32 para evitar RuntimeBinderException
				// cuando Newtonsoft deserializa el número como long en lugar de int
				.OrderByDescending(j => Convert.ToInt32(j.puntaje))
				.ToList();

			int puesto = 1;
			foreach (var jugador in jugadoresOrdenados)
			{
				string nombre = jugador.nombre.ToString();
				// CORRECCIÓN: Convert.ToInt32 en lugar de cast directo (int)
				int puntajeJugador = Convert.ToInt32(jugador.puntaje);
				string medalla = puesto == 1 ? "🥇" : (puesto == 2 ? "🥈" : (puesto == 3 ? "🥉" : $"#{puesto}"));

				DataGridViewRow row = new DataGridViewRow();
				row.CreateCells(dgvPartidaActual, puesto, nombre, puntajeJugador, medalla);

				if (nombre == jugadorActual)
				{
					row.DefaultCellStyle.BackColor = Color.FromArgb(0, 120, 215);
					row.DefaultCellStyle.ForeColor = Color.White;
					row.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
				}

				dgvPartidaActual.Rows.Add(row);
				puesto++;
			}

			tab.Controls.Add(dgvPartidaActual);

			var ganador = jugadoresOrdenados.First();
			Label lblGanador = new Label()
			{
				// CORRECCIÓN: Convert.ToInt32 también aquí
				Text = $"👑 GANADOR: {ganador.nombre} con {Convert.ToInt32(ganador.puntaje)} puntos! 👑",
				Location = new Point(20, 380),
				Size = new Size(580, 40),
				Font = new Font("Segoe UI", 12, FontStyle.Bold),
				ForeColor = Color.FromArgb(76, 175, 80),
				TextAlign = ContentAlignment.MiddleCenter,
				BackColor = Color.FromArgb(60, 60, 80)
			};
			tab.Controls.Add(lblGanador);
		}

		private void ConfigurarTabTuRendimiento(TabPage tab)
		{
			tab.BackColor = Color.FromArgb(45, 45, 60);

			Label lblJugador = new Label()
			{
				Text = $"Jugador: {jugadorActual}",
				Location = new Point(20, 20),
				Size = new Size(580, 30),
				Font = new Font("Segoe UI", 12, FontStyle.Bold),
				ForeColor = Color.White
			};
			tab.Controls.Add(lblJugador);

			var jugadorInfo = ((System.Collections.IEnumerable)resultadosMultijugador)
				.Cast<dynamic>()
				.FirstOrDefault(j => j.nombre.ToString() == jugadorActual);

			if (jugadorInfo != null)
			{
				// CORRECCIÓN: Convert.ToInt32 para evitar error de tipo en dynamic
				int puntajeObtenido = Convert.ToInt32(jugadorInfo.puntaje);
				int totalPreguntas = Convert.ToInt32(jugadorInfo.total_preguntas);
				double porcentaje = totalPreguntas > 0 ? (double)puntajeObtenido / totalPreguntas * 100 : 0;

				Panel panelStats = new Panel()
				{
					Location = new Point(20, 60),
					Size = new Size(580, 200),
					BackColor = Color.FromArgb(60, 60, 80)
				};

				Label lblPuntaje = new Label()
				{
					Text = $"⭐ Puntaje: {puntajeObtenido} / {totalPreguntas}",
					Location = new Point(20, 20),
					Size = new Size(540, 35),
					Font = new Font("Segoe UI", 14, FontStyle.Bold),
					ForeColor = Color.FromArgb(255, 215, 0)
				};
				panelStats.Controls.Add(lblPuntaje);

				Label lblPorcentaje = new Label()
				{
					Text = $"📊 Porcentaje: {porcentaje:F1}%",
					Location = new Point(20, 65),
					Size = new Size(540, 30),
					Font = new Font("Segoe UI", 12),
					ForeColor = Color.White
				};
				panelStats.Controls.Add(lblPorcentaje);

				ProgressBar progressBar = new ProgressBar()
				{
					Location = new Point(20, 105),
					Size = new Size(540, 30),
					Minimum = 0,
					Maximum = 100,
					Value = Math.Min((int)porcentaje, 100), // protección contra valor > 100
					Style = ProgressBarStyle.Continuous
				};
				panelStats.Controls.Add(progressBar);

				string mensaje = porcentaje >= 80 ? "🔥 ¡Excelente! Eres un campeón!" :
								(porcentaje >= 60 ? "👍 ¡Bien hecho! Sigue mejorando." :
								(porcentaje >= 40 ? "📚 Puedes hacerlo mejor." : "😅 ¡No te rindas!"));

				Label lblMensaje = new Label()
				{
					Location = new Point(20, 150),
					Size = new Size(540, 40),
					Font = new Font("Segoe UI", 10, FontStyle.Italic),
					ForeColor = Color.LightGreen,
					Text = mensaje
				};
				panelStats.Controls.Add(lblMensaje);

				tab.Controls.Add(panelStats);
			}
			else
			{
				// CORRECCIÓN: Manejar el caso en que el jugador no se encuentre en los resultados
				Label lblNoEncontrado = new Label()
				{
					Text = "No se encontraron datos de tu rendimiento.",
					Location = new Point(20, 60),
					Size = new Size(580, 30),
					Font = new Font("Segoe UI", 11),
					ForeColor = Color.LightCoral
				};
				tab.Controls.Add(lblNoEncontrado);
			}
		}

		private void ConfigurarTabResultadosSingleplayer(TabPage tab)
		{
			tab.BackColor = Color.FromArgb(45, 45, 60);

			lblResultado = new Label()
			{
				Location = new Point(20, 20),
				AutoSize = true,
				Font = new Font("Segoe UI", 16, FontStyle.Bold),
				ForeColor = Color.White
			};
			tab.Controls.Add(lblResultado);

			txtMensaje = new Label()
			{
				Location = new Point(20, 60),
				AutoSize = true,
				Font = new Font("Segoe UI", 12, FontStyle.Italic),
				ForeColor = Color.LightGreen
			};
			tab.Controls.Add(txtMensaje);
		}

		private void ConfigurarTabHistorial(TabPage tab)
		{
			tab.BackColor = Color.FromArgb(45, 45, 60);

			Label lblTitulo = new Label()
			{
				Text = "📜 HISTORIAL DE JUGADORES 📜",
				Location = new Point(20, 15),
				Size = new Size(600, 30),
				Font = new Font("Segoe UI", 12, FontStyle.Bold),
				ForeColor = Color.FromArgb(255, 215, 0),
				TextAlign = ContentAlignment.MiddleCenter
			};
			tab.Controls.Add(lblTitulo);

			dgvJugadores = new DataGridView()
			{
				Location = new Point(20, 55),
				Size = new Size(580, 310),
				BackgroundColor = Color.White,
				AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
				AllowUserToAddRows = false,
				ReadOnly = true
			};
			tab.Controls.Add(dgvJugadores);

			CargarHistorialGeneral();
		}

		private void MostrarResultadoSingleplayer()
		{
			if (lblResultado != null)
				lblResultado.Text = $"Puntaje: {puntaje} / {total}";

			if (txtMensaje != null && total > 0)
			{
				double porcentaje = (double)puntaje / total * 100;
				if (porcentaje >= 80)
					txtMensaje.Text = "🔥 Excelente! Eres un pro del quiz";
				else if (porcentaje >= 50)
					txtMensaje.Text = "👍 Bien hecho, pero puedes mejorar";
				else
					txtMensaje.Text = "😅 Necesitas practicar más";
			}
		}

		private void CargarHistorialGeneral()
		{
			try
			{
				using (MySqlConnection conn = new MySqlConnection(connStr))
				{
					conn.Open();

					string query = @"
                        SELECT 
                            j.nombre AS Jugador,
                            MAX(pj.puntaje) AS Mejor_Puntaje,
                            COUNT(pj.id_partida) AS Partidas_Jugadas,
                            ROUND(AVG(pj.puntaje), 1) AS Promedio
                        FROM partida_jugadores pj
                        JOIN jugadores j ON pj.id_jugador = j.id_jugador
                        GROUP BY j.id_jugador, j.nombre
                        ORDER BY Mejor_Puntaje DESC
                        LIMIT 20;
                    ";

					MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
					DataTable dt = new DataTable();
					da.Fill(dt);

					if (dgvJugadores != null)
						dgvJugadores.DataSource = dt;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error al cargar historial: " + ex.Message);
			}
		}
	}
}