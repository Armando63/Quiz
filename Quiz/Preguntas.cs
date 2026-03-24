using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private List<Opcion> opcionesActuales;

        private WindowsMediaPlayer reproductor = new WindowsMediaPlayer();

        // Variables para el dibujo
        private Rectangle[] areasOpciones;
        private Rectangle areaPregunta;
        private Rectangle areaNumero;
        private Rectangle areaPuntaje;
        private Rectangle areaProgreso;

        private int opcionSeleccionada = -1;
        private bool mostrandoResultado = false;
        private bool resultadoCorrecto = false;
        private DateTime tiempoResultado;

        // Estados para animaciones
        private int hoverIndex = -1;

        // Fuentes
        private Font fuenteTitulo;
        private Font fuentePregunta;
        private Font fuenteOpcion;
        private Font fuenteNumero;

        // Colores
        private Color colorFondo = Color.FromArgb(30, 30, 45);
        private Color colorPanel = Color.FromArgb(45, 45, 60);
        private Color colorTexto = Color.White;
        private Color colorCorrecto = Color.FromArgb(76, 175, 80);
        private Color colorIncorrecto = Color.FromArgb(244, 67, 54);
        private Color colorHover = Color.FromArgb(66, 66, 88);
        private Color colorBorde = Color.FromArgb(100, 100, 120);

        string connStr = "server=localhost;database=quiz_bd;uid=root;pwd=1234;";

        public Preguntas(int categoriaId)
        {
            InitializeComponent();
            this.categoriaId = categoriaId;

            // Configurar el formulario para dibujo personalizado
            this.DoubleBuffered = true;
            this.Paint += Preguntas_Paint;
            this.MouseClick += Preguntas_MouseClick;
            this.MouseMove += Preguntas_MouseMove;
            this.Resize += Preguntas_Resize;

            // Inicializar fuentes
            fuenteTitulo = new Font("Segoe UI", 18, FontStyle.Bold);
            fuentePregunta = new Font("Segoe UI", 14, FontStyle.Regular);
            fuenteOpcion = new Font("Segoe UI", 12, FontStyle.Regular);
            fuenteNumero = new Font("Segoe UI", 10, FontStyle.Regular);

            areasOpciones = new Rectangle[4];
        }

        private void Preguntas_Resize(object sender, EventArgs e)
        {
            CalcularAreas();
            Invalidate();
        }

        private void CalcularAreas()
        {
            int ancho = this.ClientSize.Width;
            int alto = this.ClientSize.Height;
            int margen = 20;

            // Área del título/progreso
            areaNumero = new Rectangle(margen, margen, 200, 40);
            areaPuntaje = new Rectangle(ancho - 150, margen, 130, 40);

            // Área de la pregunta
            areaPregunta = new Rectangle(margen, 80, ancho - (margen * 2), 80);

            // Área de progreso visual
            areaProgreso = new Rectangle(margen, 170, ancho - (margen * 2), 10);

            // Áreas para las 4 opciones
            int anchoOpcion = (ancho - (margen * 3)) / 2;
            int altoOpcion = 100;
            int inicioY = 200;

            areasOpciones[0] = new Rectangle(margen, inicioY, anchoOpcion, altoOpcion);
            areasOpciones[1] = new Rectangle(ancho - anchoOpcion - margen, inicioY, anchoOpcion, altoOpcion);
            areasOpciones[2] = new Rectangle(margen, inicioY + altoOpcion + margen, anchoOpcion, altoOpcion);
            areasOpciones[3] = new Rectangle(ancho - anchoOpcion - margen, inicioY + altoOpcion + margen, anchoOpcion, altoOpcion);
        }

        private void Preguntas_Load(object sender, EventArgs e)
        {
            CalcularAreas();
            CargarPreguntas();
            CargarSiguientePregunta();
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

        private void CargarSiguientePregunta()
        {
            if (indiceActual >= preguntas.Count)
            {
                FinalizarQuiz();
                return;
            }

            var pregunta = preguntas[indiceActual];
            opcionesActuales = ObtenerOpciones(pregunta.Id);
            opcionesActuales = opcionesActuales.OrderBy(x => Guid.NewGuid()).ToList();

            opcionSeleccionada = -1;
            mostrandoResultado = false;
            Invalidate();
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

            // Intentar con extensión .mp3 si no tiene
            if (!nombreArchivo.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
            {
                ruta = Path.Combine(basePath, nombreArchivo + ".mp3");
                if (File.Exists(ruta)) return ruta;
            }

            return null;
        }

        private void DibujarOpcion(Graphics g, int index, Rectangle area, Opcion opcion)
        {
            bool esHover = (hoverIndex == index && !mostrandoResultado);
            bool esSeleccionada = (opcionSeleccionada == index);

            // Color de fondo
            Color fondo;
            if (mostrandoResultado && esSeleccionada)
            {
                fondo = opcion.EsCorrecta ? colorCorrecto : colorIncorrecto;
            }
            else if (esHover)
            {
                fondo = colorHover;
            }
            else
            {
                fondo = colorPanel;
            }

            // Dibujar fondo con bordes redondeados
            using (GraphicsPath path = ObtenerPathRedondeado(area, 10))
            using (SolidBrush brush = new SolidBrush(fondo))
            {
                g.FillPath(brush, path);

                // Dibujar borde
                using (Pen pen = new Pen(colorBorde, 2))
                {
                    g.DrawPath(pen, path);
                }
            }

            // Dibujar contenido según tipo
            if (opcion.Tipo == "texto")
            {
                // Dibujar texto
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                using (SolidBrush brushTexto = new SolidBrush(colorTexto))
                {
                    g.DrawString(opcion.Texto, fuenteOpcion, brushTexto, area, sf);
                }
            }
            else if (opcion.Tipo == "imagen")
            {
                string basePath = Path.Combine(Application.StartupPath, @"..\..\Resources");
                string ruta = Path.Combine(basePath, opcion.Texto);

                if (File.Exists(ruta))
                {
                    using (Image img = Image.FromFile(ruta))
                    {
                        // Mantener proporción de la imagen
                        Rectangle imgArea = new Rectangle(
                            area.X + 10,
                            area.Y + 10,
                            area.Width - 20,
                            area.Height - 20
                        );
                        g.DrawImage(img, imgArea);
                    }
                }
                else
                {
                    // Si no hay imagen, mostrar texto de error
                    StringFormat sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString("[Imagen no encontrada]", fuenteOpcion, Brushes.Red, area, sf);
                }
            }
            else if (opcion.Tipo == "audio")
            {
                // Dibujar icono de audio y texto
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                // Dibujar icono de altavoz
                Rectangle iconoArea = new Rectangle(area.X + 20, area.Y + (area.Height / 2) - 15, 30, 30);
                DibujarIconoAudio(g, iconoArea);

                // Dibujar texto
                string texto = "🎵 Click para escuchar\nClick otra vez para responder";
                using (Font fuentePequeña = new Font("Segoe UI", 10))
                {
                    Rectangle textoArea = new Rectangle(area.X + 60, area.Y, area.Width - 70, area.Height);
                    g.DrawString(texto, fuentePequeña, new SolidBrush(colorTexto), textoArea, sf);
                }
            }
        }

        private void DibujarIconoAudio(Graphics g, Rectangle area)
        {
            // Dibujar un icono simple de altavoz
            using (Pen pen = new Pen(colorTexto, 2))
            {
                // Cuerpo del altavoz
                Point[] altavoz = new Point[]
                {
                    new Point(area.X + 5, area.Y + 10),
                    new Point(area.X + 5, area.Y + area.Height - 10),
                    new Point(area.X + 15, area.Y + area.Height - 10),
                    new Point(area.X + 25, area.Y + area.Height - 5),
                    new Point(area.X + 25, area.Y + 5),
                    new Point(area.X + 15, area.Y + 10)
                };
                g.DrawLines(pen, altavoz);

                // Ondas de sonido
                g.DrawLine(pen, area.X + 30, area.Y + 10, area.X + 35, area.Y + area.Height / 2);
                g.DrawLine(pen, area.X + 35, area.Y + area.Height / 2, area.X + 30, area.Y + area.Height - 10);
                g.DrawLine(pen, area.X + 38, area.Y + 5, area.X + 45, area.Y + area.Height / 2);
                g.DrawLine(pen, area.X + 45, area.Y + area.Height / 2, area.X + 38, area.Y + area.Height - 5);
            }
        }

        private GraphicsPath ObtenerPathRedondeado(Rectangle rect, int radio)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radio, radio, 180, 90);
            path.AddArc(rect.X + rect.Width - radio, rect.Y, radio, radio, 270, 90);
            path.AddArc(rect.X + rect.Width - radio, rect.Y + rect.Height - radio, radio, radio, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - radio, radio, radio, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void Preguntas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Dibujar fondo
            using (SolidBrush brush = new SolidBrush(colorFondo))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }

            // Dibujar información de número de pregunta y puntaje
            string textoNumero = $"Pregunta {indiceActual + 1} de {preguntas.Count}";
            string textoPuntaje = $"Puntaje: {puntaje}";

            using (SolidBrush brushTexto = new SolidBrush(colorTexto))
            {
                g.DrawString(textoNumero, fuenteNumero, brushTexto, areaNumero);
                g.DrawString(textoPuntaje, fuenteNumero, brushTexto, areaPuntaje);
            }

            // Dibujar barra de progreso
            float progreso = (float)(indiceActual) / preguntas.Count;
            int anchoProgreso = (int)(areaProgreso.Width * progreso);

            using (SolidBrush brushFondo = new SolidBrush(colorPanel))
            {
                g.FillRectangle(brushFondo, areaProgreso);
            }

            using (SolidBrush brushProgreso = new SolidBrush(Color.FromArgb(76, 175, 80)))
            {
                g.FillRectangle(brushProgreso, areaProgreso.X, areaProgreso.Y, anchoProgreso, areaProgreso.Height);
            }

            // Dibujar pregunta
            if (indiceActual < preguntas.Count)
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                using (SolidBrush brushTexto = new SolidBrush(colorTexto))
                using (GraphicsPath path = ObtenerPathRedondeado(areaPregunta, 15))
                using (SolidBrush brushFondo = new SolidBrush(colorPanel))
                {
                    g.FillPath(brushFondo, path);
                    g.DrawString(preguntas[indiceActual].Texto, fuentePregunta, brushTexto, areaPregunta, sf);
                }
            }

            // Dibujar opciones
            if (opcionesActuales != null)
            {
                for (int i = 0; i < opcionesActuales.Count && i < areasOpciones.Length; i++)
                {
                    DibujarOpcion(g, i, areasOpciones[i], opcionesActuales[i]);
                }
            }
        }

        private void Preguntas_MouseClick(object sender, MouseEventArgs e)
        {
            if (mostrandoResultado) return;

            // Verificar si se hizo clic en alguna opción
            for (int i = 0; i < areasOpciones.Length && i < opcionesActuales.Count; i++)
            {
                if (areasOpciones[i].Contains(e.Location))
                {
                    ManejarClickOpcion(i);
                    break;
                }
            }
        }

        private void ManejarClickOpcion(int index)
        {
            Opcion opcion = opcionesActuales[index];

            // Si es audio, manejar reproducción
            if (opcion.Tipo == "audio")
            {
                string ruta = ObtenerRutaAudio(opcion.Texto);
                if (!string.IsNullOrEmpty(ruta) && File.Exists(ruta))
                {
                    // Verificar si ya se está reproduciendo
                    if (reproductor.playState == WMPPlayState.wmppsPlaying)
                    {
                        // Si ya está sonando, este es el segundo click - responder
                        reproductor.controls.stop();
                        EvaluarRespuesta(index, opcion);
                    }
                    else
                    {
                        // Primer click - reproducir audio
                        reproductor.controls.stop();
                        reproductor.URL = ruta;
                        reproductor.controls.play();
                    }
                }
                else
                {
                    MessageBox.Show("Archivo de audio no encontrado", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Para texto o imagen, responder directamente
                EvaluarRespuesta(index, opcion);
            }
        }

        private void EvaluarRespuesta(int index, Opcion opcion)
        {
            opcionSeleccionada = index;
            mostrandoResultado = true;
            resultadoCorrecto = opcion.EsCorrecta;

            if (opcion.EsCorrecta)
            {
                puntaje++;
            }
            else
            {
                incorrecta++;
            }

            tiempoResultado = DateTime.Now;
            Invalidate();

            // Temporizador para pasar a la siguiente pregunta
            Timer t = new Timer();
            t.Interval = 1000;
            t.Tick += (s, ev) =>
            {
                t.Stop();
                indiceActual++;
                CargarSiguientePregunta();
            };
            t.Start();
        }

        private void Preguntas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mostrandoResultado) return;

            int nuevoHover = -1;
            for (int i = 0; i < areasOpciones.Length && i < opcionesActuales?.Count; i++)
            {
                if (areasOpciones[i].Contains(e.Location))
                {
                    nuevoHover = i;
                    break;
                }
            }

            if (nuevoHover != hoverIndex)
            {
                hoverIndex = nuevoHover;
                Invalidate();
            }
        }

        private void FinalizarQuiz()
        {
            reproductor.controls.stop();

            DialogResult result = MessageBox.Show(
                $"¡Quiz finalizado!\n\nPuntaje: {puntaje} de {preguntas.Count}\n" +
                $"Correctas: {puntaje}\nIncorrectas: {incorrecta}\n\n" +
                $"¿Quieres ver los resultados?",
                "Quiz Finalizado",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information
            );

            InsertarPartidas();
            this.Close();
        }

        private void InsertarPartidas()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar partida: {ex.Message}");
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