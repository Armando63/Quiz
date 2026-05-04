using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Quiz
{
    public partial class Preguntas : Form
    {
        private int categoriaId;
        private int indiceActual = 0;
        private int puntaje = 0;
        private int incorrecta = 0;

        private List<Opcion> opcionesActuales;
        private List<ApiPregunta> apiPreguntas;  

        private WindowsMediaPlayer reproductor = new WindowsMediaPlayer();

        // Variables para el dibujo
        private Rectangle[] areasOpciones;
        private Rectangle areaPregunta;
        private Rectangle areaNumero;
        private Rectangle areaPuntaje;
        private Rectangle areaProgreso;

        private int opcionSeleccionada = -1;
        private bool mostrandoResultado = false;

        // Estados para animaciones y audio
        private int hoverIndex = -1;
        private int opcionAudioActual = -1;  // Índice de la opción de audio que se está reproduciendo

        // Fuentes
        private Font fuentePregunta;
        private Font fuenteOpcion;
        private Font fuenteNumero;
        private Font fuentePuntaje;

        // Colores
        private Color colorFondo = Color.FromArgb(30, 30, 45);
        private Color colorPanel = Color.FromArgb(45, 45, 60);
        private Color colorTexto = Color.White;
        private Color colorCorrecto = Color.FromArgb(76, 175, 80);
        private Color colorIncorrecto = Color.FromArgb(244, 67, 54);
        private Color colorHover = Color.FromArgb(66, 66, 88);
        private Color colorBorde = Color.FromArgb(100, 100, 120);
        private Color colorProgreso = Color.FromArgb(33, 150, 243);
        private Color colorAudioActivo = Color.FromArgb(33, 150, 243);

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
            fuentePregunta = new Font("Segoe UI", 16, FontStyle.Bold);
            fuenteOpcion = new Font("Segoe UI", 12, FontStyle.Regular);
            fuenteNumero = new Font("Segoe UI", 10, FontStyle.Regular);
            fuentePuntaje = new Font("Segoe UI", 12, FontStyle.Bold);

            areasOpciones = new Rectangle[4];
            opcionAudioActual = -1;
        }

        public class ApiPregunta
        {
            public string texto { get; set; }
            public List<string> opciones { get; set; }
            public int correcta { get; set; }
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
            int margen = 30;

            // Área del número de pregunta
            areaNumero = new Rectangle(margen, margen, 200, 35);

            // Área del puntaje
            areaPuntaje = new Rectangle(ancho - 150, margen, 130, 40);

            // Área de la pregunta
            areaPregunta = new Rectangle(margen, 90, ancho - (margen * 2), 90);

            // Área de progreso
            areaProgreso = new Rectangle(margen, 200, ancho - (margen * 2), 8);

            // Áreas para las 4 opciones
            int anchoOpcion = (ancho - (margen * 3)) / 2;
            int altoOpcion = 120;
            int inicioY = 240;

            areasOpciones[0] = new Rectangle(margen, inicioY, anchoOpcion, altoOpcion);
            areasOpciones[1] = new Rectangle(ancho - anchoOpcion - margen, inicioY, anchoOpcion, altoOpcion);
            areasOpciones[2] = new Rectangle(margen, inicioY + altoOpcion + margen, anchoOpcion, altoOpcion);
            areasOpciones[3] = new Rectangle(ancho - anchoOpcion - margen, inicioY + altoOpcion + margen, anchoOpcion, altoOpcion);
        }

        private async void Preguntas_Load(object sender, EventArgs e)
        {
            CalcularAreas();
           await CargarPreguntas();
            CargarSiguientePregunta();
        }


        private async Task CargarPreguntas()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"http://127.0.0.1:8000/preguntas?categoria={categoriaId}";

                var response = await client.GetStringAsync(url);

                apiPreguntas = JsonConvert.DeserializeObject<List<ApiPregunta>>(response);

                apiPreguntas = apiPreguntas.OrderBy(x => Guid.NewGuid()).ToList();
            }
        }

        private void CargarSiguientePregunta()
        {
            if (apiPreguntas == null || indiceActual >= apiPreguntas.Count)
            {
                FinalizarQuiz();
                return;
            }

            // Detener audio
            reproductor.controls.stop();
            opcionAudioActual = -1;

            var pregunta = apiPreguntas[indiceActual];

            opcionesActuales = new List<Opcion>();

            for (int i = 0; i < pregunta.opciones.Count; i++)
            {
                string contenido = pregunta.opciones[i];

                string tipo;

                if (contenido.EndsWith(".jpg") || contenido.EndsWith(".png"))
                    tipo = "imagen";
                else if (contenido.EndsWith(".mp3"))
                    tipo = "audio";
                else
                    tipo = "texto";

                opcionesActuales.Add(new Opcion
                {
                    Texto = contenido,
                    EsCorrecta = (i == pregunta.correcta),
                    Tipo = tipo
                });
            }

            // Mezclar opciones
            opcionesActuales = opcionesActuales.OrderBy(x => Guid.NewGuid()).ToList();

            opcionSeleccionada = -1;
            mostrandoResultado = false;

            Invalidate();
        }
   
        private string ObtenerRutaAudio(string nombreArchivo)
        {
            // Buscar en la carpeta Resources/audios
            string[] posiblesRutas = {
                Path.Combine(Application.StartupPath, "Resources", "audios"),
                Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\Resources\audios")),
                Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\Resources\audios"))
            };

            foreach (var basePath in posiblesRutas)
            {
                if (Directory.Exists(basePath))
                {
                    string ruta = Path.Combine(basePath, nombreArchivo);
                    if (File.Exists(ruta)) return ruta;

                    string soloNombre = Path.GetFileName(nombreArchivo);
                    ruta = Path.Combine(basePath, soloNombre);
                    if (File.Exists(ruta)) return ruta;

                    if (!nombreArchivo.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                    {
                        ruta = Path.Combine(basePath, nombreArchivo + ".mp3");
                        if (File.Exists(ruta)) return ruta;
                    }
                }
            }

            return null;
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

        private void DibujarOpcion(Graphics g, int index, Rectangle area, Opcion opcion)
        {
            bool esHover = (hoverIndex == index && !mostrandoResultado);
            bool esSeleccionada = (opcionSeleccionada == index);
            bool esAudioActivo = (opcionAudioActual == index && !mostrandoResultado);

            // Color de fondo
            Color fondo;
            if (mostrandoResultado && esSeleccionada)
            {
                fondo = opcion.EsCorrecta ? colorCorrecto : colorIncorrecto;
            }
            else if (esAudioActivo)
            {
                fondo = colorAudioActivo;
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
            using (GraphicsPath path = ObtenerPathRedondeado(area, 15))
            using (SolidBrush brush = new SolidBrush(fondo))
            {
                g.FillPath(brush, path);

                // Dibujar borde (si es audio activo, borde más grueso)
                using (Pen pen = new Pen(esAudioActivo ? colorAudioActivo : colorBorde, esAudioActivo ? 3 : 2))
                {
                    g.DrawPath(pen, path);
                }
            }

            // Dibujar contenido según tipo
            if (opcion.Tipo == "texto")
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                using (SolidBrush brushTexto = new SolidBrush(colorTexto))
                {
                    Font fuenteUsar = fuenteOpcion;
                    if (opcion.Texto.Length > 50)
                    {
                        fuenteUsar = new Font("Segoe UI", 10);
                    }
                    g.DrawString(opcion.Texto, fuenteUsar, brushTexto, area, sf);
                    if (fuenteUsar != fuenteOpcion) fuenteUsar.Dispose();
                }
            }
            else if (opcion.Tipo == "imagen")
            {
                string basePath = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\Resources"));
                string ruta = Path.Combine(basePath, opcion.Texto);

                if (File.Exists(ruta))
                {
                    try
                    {
                        using (Image img = Image.FromFile(ruta))
                        {
                            // Calcular área de imagen manteniendo la proporción
                            Rectangle imgArea = ObtenerAreaImagenProporcional(img, area);
                            g.DrawImage(img, imgArea);
                        }
                    }
                    catch
                    {
                        StringFormat sf = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };
                        g.DrawString("[Error al cargar imagen]", fuenteOpcion, Brushes.Red, area, sf);
                    }
                }
                else
                {
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
                // Dibujar icono de audio
                Rectangle iconoArea = new Rectangle(area.X + 20, area.Y + (area.Height / 2) - 20, 40, 40);
                DibujarIconoAudio(g, iconoArea, esAudioActivo);

                // Dibujar texto
                string texto = esAudioActivo ? "🔊   Reproduciendo...\nClick para responder" : "🎵 Click para escuchar\nClick otra vez para responder";
                Rectangle textoArea = new Rectangle(area.X + 70, area.Y, area.Width - 80, area.Height);
                using (Font fuentePequeña = new Font("Segoe UI", 10))
                using (SolidBrush brushTexto = new SolidBrush(colorTexto))
                {
                    StringFormat sfAudio = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(texto, fuentePequeña, brushTexto, textoArea, sfAudio);
                }
            }
        }

        private Rectangle ObtenerAreaImagenProporcional(Image img, Rectangle contenedor)
        {
            float proporcionImagen = (float)img.Width / img.Height;
            float proporcionContenedor = (float)contenedor.Width / contenedor.Height;

            int nuevoAncho, nuevoAlto;
            int offsetX, offsetY;

            if (proporcionImagen > proporcionContenedor)
            {
                // Ajuste por anchura
                nuevoAncho = contenedor.Width - 20;
                nuevoAlto = (int)(nuevoAncho / proporcionImagen);
                offsetX = 10;
                offsetY = (contenedor.Height - nuevoAlto) / 2;
            }
            else
            {
                // Ajustar por altura
                nuevoAlto = contenedor.Height - 20;
                nuevoAncho = (int)(nuevoAlto * proporcionImagen);
                offsetX = (contenedor.Width - nuevoAncho) / 2;
                offsetY = 10;
            }

            return new Rectangle(
                contenedor.X + offsetX,
                contenedor.Y + offsetY,
                nuevoAncho,
                nuevoAlto
            );
        }

        private void DibujarIconoAudio(Graphics g, Rectangle area, bool activo)
        {
            Color colorIcono = activo ? colorAudioActivo : colorTexto;

            using (Pen pen = new Pen(colorIcono, 2))
            {
                // Altavoz
                Point[] altavoz = new Point[]
                {
                    new Point(area.X + 5, area.Y + 12),
                    new Point(area.X + 5, area.Y + area.Height - 12),
                    new Point(area.X + 18, area.Y + area.Height - 12),
                    new Point(area.X + 28, area.Y + area.Height - 5),
                    new Point(area.X + 28, area.Y + 5),
                    new Point(area.X + 18, area.Y + 12)
                };
                g.DrawLines(pen, altavoz);

                // Ondas de sonido
                g.DrawLine(pen, area.X + 33, area.Y + 8, area.X + 40, area.Y + area.Height / 2);
                g.DrawLine(pen, area.X + 40, area.Y + area.Height / 2, area.X + 33, area.Y + area.Height - 8);
                g.DrawLine(pen, area.X + 43, area.Y + 3, area.X + 52, area.Y + area.Height / 2);
                g.DrawLine(pen, area.X + 52, area.Y + area.Height / 2, area.X + 43, area.Y + area.Height - 3);
            }
        }

        private void Preguntas_Paint(object sender, PaintEventArgs e)
        {
            if (apiPreguntas == null || apiPreguntas.Count == 0)
                return;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Dibujar fondo
            using (SolidBrush brush = new SolidBrush(colorFondo))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }

            if (apiPreguntas == null || indiceActual >= apiPreguntas.Count) return;

            // Dibujar número de pregunta
            string textoNumero = $"Pregunta {indiceActual + 1} de {apiPreguntas.Count}";
            using (SolidBrush brushTexto = new SolidBrush(colorTexto))
            {
                g.DrawString(textoNumero, fuenteNumero, brushTexto, areaNumero);
            }

            // Dibujar puntaje
            string textoPuntaje = $"⭐ {puntaje}";
            using (SolidBrush brushTexto = new SolidBrush(Color.FromArgb(255, 215, 0)))
            {
                g.DrawString(textoPuntaje, fuentePuntaje, brushTexto, areaPuntaje);
            }

            // Dibujar barra de progreso
            float progreso = (float)(indiceActual) / apiPreguntas.Count;
            int anchoProgreso = (int)(areaProgreso.Width * progreso);

            using (SolidBrush brushFondo = new SolidBrush(Color.FromArgb(80, 80, 100)))
            {
                g.FillRectangle(brushFondo, areaProgreso);
            }

            using (SolidBrush brushProgreso = new SolidBrush(colorProgreso))
            {
                g.FillRectangle(brushProgreso, areaProgreso.X, areaProgreso.Y, anchoProgreso, areaProgreso.Height);
            }

            // Dibujar pregunta
            StringFormat sfPregunta = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            using (GraphicsPath path = ObtenerPathRedondeado(areaPregunta, 20))
            using (SolidBrush brushFondo = new SolidBrush(colorPanel))
            using (SolidBrush brushTexto = new SolidBrush(colorTexto))
            {
                g.FillPath(brushFondo, path);

                using (Pen pen = new Pen(colorProgreso, 2))
                {
                    g.DrawPath(pen, path);
                }

                g.DrawString(apiPreguntas[indiceActual].texto, fuentePregunta, brushTexto, areaPregunta, sfPregunta);
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
            if (mostrandoResultado || opcionesActuales == null) return;

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

            if (opcion.Tipo == "audio")
            {
                string ruta = ObtenerRutaAudio(opcion.Texto);
                if (!string.IsNullOrEmpty(ruta) && File.Exists(ruta))
                {
                    // Si es la misma opción de audio que se está reproduciendo
                    if (opcionAudioActual == index)
                    {
                        // Segundo click en la misma opción - responder
                        reproductor.controls.stop();
                        opcionAudioActual = -1;
                        EvaluarRespuesta(index, opcion);
                    }
                    else
                    {
                        // Click en una opción de audio diferente
                        // Detener el audio anterior
                        reproductor.controls.stop();

                        // Reproducir el nuevo audio
                        reproductor.URL = ruta;
                        reproductor.controls.play();

                        // Actualizar la opción de audio actual
                        opcionAudioActual = index;

                        Invalidate();
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

            if (opcion.EsCorrecta)
            {
                puntaje++;
            }
            else
            {
                incorrecta++;
            }

            Invalidate();

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += (s, ev) =>
            {
                timer.Stop();
                timer.Dispose();
                indiceActual++;
                CargarSiguientePregunta();
            };
            timer.Start();
        }

        private void Preguntas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mostrandoResultado || opcionesActuales == null) return;

            int nuevoHover = -1;
            for (int i = 0; i < areasOpciones.Length && i < opcionesActuales.Count; i++)
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
            reproductor.controls.stop();

           InsertarPartidas();

            Resultados frm = new Resultados(puntaje, apiPreguntas.Count);
            frm.Show();

            this.Hide();
        }

        private void InsertarPartidas()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    // 1. Crear partida
                    string queryPartida = @"
                INSERT INTO partidas (id_categoria, fecha, estado, max_jugadores, codigo_sala, pregunta_actual, modo)
                VALUES (@categoria, NOW(), 'finalizada', 1, Null, 0, 'singleplayer');
                SELECT LAST_INSERT_ID();
            ";

                    MySqlCommand cmdPartida = new MySqlCommand(queryPartida, conn);
                    cmdPartida.Parameters.AddWithValue("@categoria", categoriaId);

                    int idPartida = Convert.ToInt32(cmdPartida.ExecuteScalar());

                    // 2. Insertar resultado del jugador
                    string queryJugador = @"
                INSERT INTO partida_jugadores (id_partida, id_jugador, puntaje, aciertos, errores, listo)
                VALUES (@partida, @jugador, @puntaje, @aciertos, @errores, 1);
            ";

                    MySqlCommand cmdJugador = new MySqlCommand(queryJugador, conn);
                    cmdJugador.Parameters.AddWithValue("@partida", idPartida);
                    cmdJugador.Parameters.AddWithValue("@jugador", Inicio.idJugadorGlobal);
                    cmdJugador.Parameters.AddWithValue("@puntaje", puntaje);
                    cmdJugador.Parameters.AddWithValue("@aciertos", puntaje);
                    cmdJugador.Parameters.AddWithValue("@errores", incorrecta);

                    cmdJugador.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar partida: {ex.Message}");
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
}