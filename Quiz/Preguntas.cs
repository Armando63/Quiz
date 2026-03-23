using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using WMPLib;

namespace Quiz
{
	public partial class Preguntas : Form
	{
		private int categoriaId;
		private int indiceActual = 0;
		private int puntaje = 0;
		private int incorrecta = 0;

		private List<Pregunta> preguntas;
		private List<Panel> panelesOpciones;

		private WindowsMediaPlayer reproductor = new WindowsMediaPlayer();

		string connStr = "server=localhost;database=quiz_bd;uid=root;pwd=1234;";

		public Preguntas(int categoriaId)
		{
			InitializeComponent();
			this.categoriaId = categoriaId;
		}

		private void Preguntas_Load(object sender, EventArgs e)
		{
			panelesOpciones = new List<Panel>()
			{
				pnlOpcion1, pnlOpcion2, pnlOpcion3, pnlOpcion4
			};

			foreach (var pnl in panelesOpciones)
			{
				pnl.Click += Opcion_Click;

				foreach (Control ctrl in pnl.Controls)
				{
					ctrl.Click += (s, args) =>
					{
						if (ctrl is Button) return; // 🔥 evita que botón responda
						Opcion_Click(pnl, args);
					};
				}
			}

			CargarPreguntas();
			MostrarPregunta();
		}

		private void CargarPreguntas()
		{
			preguntas = new List<Pregunta>();

			using (MySqlConnection conn = new MySqlConnection(connStr))
			{
				conn.Open();

				string query = @"
                    SELECT id_pregunta, texto 
                    FROM preguntas
                    WHERE id_categoria = @cat
                    ORDER BY RAND()
                    LIMIT 12;
                ";

				MySqlCommand cmd = new MySqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@cat", categoriaId);

				var reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					preguntas.Add(new Pregunta
					{
						Id = Convert.ToInt32(reader["id_pregunta"]),
						Texto = reader["texto"].ToString()
					});
				}
			}
		}

		private List<Opcion> ObtenerOpciones(int preguntaId)
		{
			List<Opcion> opciones = new List<Opcion>();

			using (MySqlConnection conn = new MySqlConnection(connStr))
			{
				conn.Open();

				string query = @"
                    SELECT contenido, es_correcta, tipo
                    FROM opciones
                    WHERE id_pregunta = @id;
                ";

				MySqlCommand cmd = new MySqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@id", preguntaId);

				var reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					opciones.Add(new Opcion
					{
						Texto = reader["contenido"].ToString(),
						EsCorrecta = Convert.ToInt32(reader["es_correcta"]) == 1,
						Tipo = reader["tipo"].ToString()
					});
				}
			}

			return opciones;
		}

		private string ObtenerRutaAudio(string nombreArchivo)
		{
			string basePath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\Resources\audios"));
			string ruta = Path.Combine(basePath, nombreArchivo);

			if (File.Exists(ruta)) return ruta;

			string soloNombre = Path.GetFileName(nombreArchivo);
			ruta = Path.Combine(basePath, soloNombre);

			if (File.Exists(ruta)) return ruta;

			return null;
		}

		private void MostrarPregunta()
		{
			reproductor.controls.stop(); // detener audio anterior

			if (indiceActual >= preguntas.Count)
			{
				FinalizarQuiz();
				return;
			}

			var pregunta = preguntas[indiceActual];

			lblPregunta.Text = pregunta.Texto;
			lblNumero.Text = $"Pregunta {indiceActual + 1} de {preguntas.Count}";
			lblPuntaje.Text = $"Puntaje: {puntaje}";

			var opciones = ObtenerOpciones(pregunta.Id)
							.OrderBy(x => Guid.NewGuid())
							.ToList();

			for (int i = 0; i < opciones.Count; i++)
			{
				var op = opciones[i];
				var pnl = panelesOpciones[i];

				pnl.BackColor = Color.Transparent;
				pnl.Tag = op.EsCorrecta;

				Label lbl = pnl.Controls.OfType<Label>().FirstOrDefault();
				PictureBox pic = pnl.Controls.OfType<PictureBox>().FirstOrDefault();
				Button btn = pnl.Controls.OfType<Button>().FirstOrDefault();

				if (lbl != null) lbl.Visible = false;
				if (pic != null) pic.Visible = false;
				if (btn != null) btn.Visible = false;

				if (op.Tipo == "texto")
				{
					if (lbl != null)
					{
						lbl.Text = op.Texto;
						lbl.Visible = true;
					}
				}
				else if (op.Tipo == "imagen")
				{
					string basePath = Path.Combine(Application.StartupPath, @"..\..\Resources");
					string ruta = Path.Combine(basePath, op.Texto);

					if (pic != null && File.Exists(ruta))
					{
						pic.Image = Image.FromFile(ruta);
						pic.SizeMode = PictureBoxSizeMode.Zoom;
						pic.Visible = true;
					}
				}
				else if (op.Tipo == "audio")
				{
					string ruta = ObtenerRutaAudio(op.Texto);

					if (lbl != null)
					{
						lbl.Text = "Escucha el audio";
						lbl.Visible = true;
					}

					if (btn != null)
					{
						btn.Text = "🔊";
						btn.Visible = true;

						btn.Click += (s, args) =>
						{
							if (!File.Exists(ruta))
							{
								MessageBox.Show("Audio no encontrado");
								return;
							}

							reproductor.controls.stop();
							reproductor.URL = ruta;
							reproductor.controls.play();
						};
					}
				}
			}
		}

		private void Opcion_Click(object sender, EventArgs e)
		{
			Panel pnl = sender as Panel;
			bool correcta = (bool)pnl.Tag;

			reproductor.controls.stop(); // detener audio al responder

			if (correcta)
			{
				pnl.BackColor = Color.Green;
				puntaje++;
			}
			else
			{
				pnl.BackColor = Color.Red;
				incorrecta++;
			}

			lblPuntaje.Text = $"Puntaje: {puntaje}";

			Timer t = new Timer();
			t.Interval = 800;
			t.Tick += (s, ev) =>
			{
				t.Stop();
				indiceActual++;
				MostrarPregunta();
			};
			t.Start();
		}

		private void FinalizarQuiz()
		{
			reproductor.controls.stop();

			MessageBox.Show($"¡Quiz finalizado!\n\nPuntaje: {puntaje}/{preguntas.Count}");
			InsertarPartidas();
			this.Close();
		}

		private void InsertarPartidas()
		{
			using (MySqlConnection conn = new MySqlConnection(connStr))
			{
				conn.Open();

				string query = @"
                    INSERT INTO partidas (id_categoria, aciertos, errores)
                    VALUES (@categoria, @correctas, @incorrectas);
                ";

				MySqlCommand cmd = new MySqlCommand(query, conn);
				cmd.Parameters.AddWithValue("@categoria", categoriaId);
				cmd.Parameters.AddWithValue("@correctas", puntaje);
				cmd.Parameters.AddWithValue("@incorrectas", incorrecta);
				cmd.ExecuteNonQuery();
			}
		}
	}

	public class Pregunta
	{
		public int Id { get; set; }
		public string Texto { get; set; }
	}

	public class Opcion
	{
		public string Texto { get; set; }
		public bool EsCorrecta { get; set; }
		public string Tipo { get; set; }
	}
}