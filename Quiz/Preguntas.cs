using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Quiz
{
	public partial class Preguntas : Form
	{
		private int categoriaId;
		private int indiceActual = 0;
		private int puntaje = 0;

		private List<Pregunta> preguntas;
		private List<Panel> panelesOpciones;

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
					ctrl.Click += (s, args) => Opcion_Click(pnl, args);
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

		private void MostrarPregunta()
		{
			if (indiceActual >= preguntas.Count)
			{
				FinalizarQuiz();
				return;
			}

			var pregunta = preguntas[indiceActual];

			lblPregunta.Text = pregunta.Texto;
			lblNumero.Text = $"Pregunta {indiceActual + 1} de {preguntas.Count}";
			lblPuntaje.Text = $"Puntaje: {puntaje}";

			var opciones = ObtenerOpciones(pregunta.Id);
			opciones = opciones.OrderBy(x => Guid.NewGuid()).ToList();

			for (int i = 0; i < opciones.Count; i++)
			{
				var op = opciones[i];

				panelesOpciones[i].Tag = op.EsCorrecta;
				panelesOpciones[i].BackColor = Color.Transparent;

				// Buscar controles dentro del panel
				Label lbl = panelesOpciones[i].Controls.OfType<Label>().FirstOrDefault();
				PictureBox pic = panelesOpciones[i].Controls.OfType<PictureBox>().FirstOrDefault();

				if (op.Tipo == "texto")
				{
					if (lbl != null)
					{
						lbl.Text = op.Texto;
						lbl.Visible = true;
					}

					if (pic != null)
					{
						pic.Visible = false;
					}
				}
				else if (op.Tipo == "imagen")
				{
					string basePath = System.IO.Path.Combine(Application.StartupPath, @"..\..\Resources");
					string ruta = System.IO.Path.Combine(basePath, op.Texto);

					if (pic != null)
					{
						pic.Image = Image.FromFile(ruta);
						pic.SizeMode = PictureBoxSizeMode.Zoom;
						pic.Visible = true;
					}

					if (lbl != null)
					{
						lbl.Visible = false;
					}
				}
			}
		}

		private void Opcion_Click(object sender, EventArgs e)
		{
			Panel pnl = sender as Panel;
			bool correcta = (bool)pnl.Tag;

			if (correcta)
			{
				pnl.BackColor = Color.Green;
				puntaje++;
			}
			else
			{
				pnl.BackColor = Color.Red;
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
			MessageBox.Show($"Puntaje final: {puntaje}");
			this.Close();
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