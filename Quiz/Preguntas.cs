using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using WMPLib; // Agregar referencia a Windows Media Player

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

        Timer audioTimer;
        bool audioReproduciendose = false;

        // Para cada opción de audio, guardamos su propio WindowsMediaPlayer
        private Dictionary<Panel, WindowsMediaPlayer> reproductoresAudio;

        string connStr = "server=localhost;database=quiz_bd;uid=root;pwd=1234;";

        public Preguntas(int categoriaId)
        {
            InitializeComponent();
            this.categoriaId = categoriaId;
            reproductoresAudio = new Dictionary<Panel, WindowsMediaPlayer>();
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

        private string ObtenerRutaAudio(string nombreArchivo)
        {
            // Lista de posibles rutas donde podrían estar los audios
            string[] posiblesRutas = {
                // Ruta durante la ejecución normal
                Path.Combine(Application.StartupPath, "Resources", "audios"),
                
                // Ruta durante debugging en Visual Studio
                Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\Resources\audios")),
                
                // Otra ruta común en proyectos con Git
                Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\Resources\audios")),
                
                // Ruta absoluta por si está en el directorio del proyecto
                Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"Resources\audios")),
                
                // Ruta buscando en el directorio del proyecto
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\audios"))
            };

            foreach (var rutaBase in posiblesRutas)
            {
                if (Directory.Exists(rutaBase))
                {
                    // Intentar con el nombre exacto
                    string rutaCompleta = Path.Combine(rutaBase, nombreArchivo);
                    if (File.Exists(rutaCompleta))
                        return rutaCompleta;

                    // Intentar solo con el nombre del archivo (sin ruta)
                    string soloNombre = Path.GetFileName(nombreArchivo);
                    rutaCompleta = Path.Combine(rutaBase, soloNombre);
                    if (File.Exists(rutaCompleta))
                        return rutaCompleta;

                    // Si el archivo no tiene extensión, agregar .mp3
                    if (!nombreArchivo.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase) &&
                        !nombreArchivo.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                    {
                        rutaCompleta = Path.Combine(rutaBase, nombreArchivo + ".mp3");
                        if (File.Exists(rutaCompleta))
                            return rutaCompleta;
                    }
                }
            }

            return null;
        }

        private void DetenerAudioActual()
        {
            // Detener todos los reproductores de audio
            foreach (var reproductor in reproductoresAudio.Values)
            {
                if (reproductor != null && reproductor.playState == WMPPlayState.wmppsPlaying)
                {
                    reproductor.controls.stop();
                }
            }

            if (audioTimer != null)
            {
                audioTimer.Stop();
            }

            audioReproduciendose = false;
        }

        private void MostrarPregunta()
        {
            // Detener cualquier audio que se esté reproduciendo
            DetenerAudioActual();

            // Limpiar reproductores anteriores
            reproductoresAudio.Clear();

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

                panelesOpciones[i].BackColor = Color.Transparent;

                Label lbl = panelesOpciones[i].Controls.OfType<Label>().FirstOrDefault();
                PictureBox pic = panelesOpciones[i].Controls.OfType<PictureBox>().FirstOrDefault();

                if (op.Tipo == "texto")
                {
                    panelesOpciones[i].Tag = op.EsCorrecta;

                    if (lbl != null)
                    {
                        lbl.Text = op.Texto;
                        lbl.Visible = true;
                    }

                    if (pic != null) pic.Visible = false;
                }
                else if (op.Tipo == "imagen")
                {
                    panelesOpciones[i].Tag = op.EsCorrecta;

                    string basePath = Path.Combine(Application.StartupPath, @"..\..\Resources");
                    string ruta = Path.Combine(basePath, op.Texto);

                    if (pic != null && File.Exists(ruta))
                    {
                        pic.Image = Image.FromFile(ruta);
                        pic.SizeMode = PictureBoxSizeMode.Zoom;
                        pic.Visible = true;
                    }

                    if (lbl != null) lbl.Visible = false;
                }
                else if (op.Tipo == "audio")
                {
                    string ruta = ObtenerRutaAudio(op.Texto);

                    if (string.IsNullOrEmpty(ruta) || !File.Exists(ruta))
                    {
                        MessageBox.Show($"No se encontró el archivo de audio: {op.Texto}\n\nVerifica que el archivo exista en la carpeta Resources\\audios",
                                      "Error de Audio", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        if (lbl != null)
                        {
                            lbl.Text = "[Audio no encontrado]";
                            lbl.Visible = true;
                        }
                        if (pic != null) pic.Visible = false;
                        continue;
                    }

                    // Crear un reproductor de Windows Media Player para este audio
                    WindowsMediaPlayer wmp = new WindowsMediaPlayer();
                    wmp.URL = ruta;
                    wmp.settings.autoStart = false;
                    wmp.settings.volume = 100;

                    // Guardar el reproductor en el diccionario
                    reproductoresAudio[panelesOpciones[i]] = wmp;

                    // Guardar en el Tag la información necesaria
                    panelesOpciones[i].Tag = new object[] { op.EsCorrecta, ruta, wmp };

                    if (lbl != null)
                    {
                        lbl.Text = "🎵 Click para escuchar / Click otra vez para responder";
                        lbl.Visible = true;
                    }

                    if (pic != null) pic.Visible = false;
                }
            }
        }

        private void Opcion_Click(object sender, EventArgs e)
        {
            Panel pnl = sender as Panel;

            bool correcta;
            string rutaAudio = null;
            WindowsMediaPlayer wmp = null;

            if (pnl.Tag is object[] datos)
            {
                correcta = (bool)datos[0];
                rutaAudio = datos[1] as string;
                wmp = datos[2] as WindowsMediaPlayer;
            }
            else
            {
                correcta = (bool)pnl.Tag;
            }

            // PRIMER CLICK: reproducir audio
            if (wmp != null && !audioReproduciendose)
            {
                audioReproduciendose = true;

                // Detener cualquier otro audio que se esté reproduciendo
                foreach (var reproductor in reproductoresAudio.Values)
                {
                    if (reproductor != wmp && reproductor.playState == WMPPlayState.wmppsPlaying)
                    {
                        reproductor.controls.stop();
                    }
                }

                // Reproducir el audio seleccionado
                try
                {
                    wmp.controls.play();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al reproducir audio: {ex.Message}", "Error de Reproducción",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    audioReproduciendose = false;
                    return;
                }

                if (audioTimer != null)
                    audioTimer.Stop();

                audioTimer = new Timer();
                audioTimer.Interval = 15000; // 15 segundos 

                audioTimer.Tick += (s, ev) =>
                {
                    if (wmp != null && wmp.playState == WMPPlayState.wmppsPlaying)
                    {
                        wmp.controls.stop();
                    }
                    audioTimer.Stop();
                    audioReproduciendose = false;
                };

                audioTimer.Start();

                return; // No responde todavía
            }

            // SEGUNDO CLICK: responder
            // Detener el audio si se está reproduciendo
            if (wmp != null && wmp.playState == WMPPlayState.wmppsPlaying)
            {
                wmp.controls.stop();
            }

            if (audioTimer != null)
            {
                audioTimer.Stop();
            }

            audioReproduciendose = false;

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
            // Detener todos los audios
            foreach (var reproductor in reproductoresAudio.Values)
            {
                if (reproductor != null && reproductor.playState == WMPPlayState.wmppsPlaying)
                {
                    reproductor.controls.stop();
                }
            }

            if (audioTimer != null)
            {
                audioTimer.Stop();
                audioTimer.Dispose();
            }

            MessageBox.Show($"¡Quiz finalizado!\n\nPuntaje final: {puntaje} de {preguntas.Count}\nRespuestas correctas: {puntaje}\nRespuestas incorrectas: {incorrecta}",
                          "Resultado Final", MessageBoxButtons.OK, MessageBoxIcon.Information);
            InsertarPartidas();
            this.Close();
        }
        private void lblOpcion4_Click(object sender, EventArgs e)
        {
        }

        private void lblNumero_Click(object sender, EventArgs e)
        {
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

        // Método de diagnóstico (opcional)
        private void DiagnosticarAudios()
        {
            string[] posiblesRutas = {
                Path.Combine(Application.StartupPath, "Resources", "audios"),
                Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\Resources\audios")),
                Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\Resources\audios")),
                Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"Resources\audios")),
                Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Resources\audios"))
            };

            string mensaje = "=== DIAGNÓSTICO DE AUDIOS ===\n\n";
            bool algunaEncontrada = false;

            foreach (var ruta in posiblesRutas)
            {
                mensaje += $"Buscando en: {ruta}\n";
                if (Directory.Exists(ruta))
                {
                    var archivos = Directory.GetFiles(ruta, "*.mp3");
                    mensaje += $"✓ ENCONTRADA - {archivos.Length} archivos MP3\n";
                    if (archivos.Length > 0)
                    {
                        mensaje += "Archivos encontrados:\n";
                        foreach (var archivo in archivos.Take(5))
                        {
                            mensaje += $"  - {Path.GetFileName(archivo)}\n";
                        }
                        if (archivos.Length > 5)
                            mensaje += $"  ... y {archivos.Length - 5} más\n";
                    }
                    algunaEncontrada = true;
                }
                else
                {
                    mensaje += "✗ NO ENCONTRADA\n";
                }
                mensaje += "\n";
            }

            if (!algunaEncontrada)
            {
                mensaje += "ADVERTENCIA: No se encontró ninguna carpeta de audios.\n";
                mensaje += "Asegúrate de que la carpeta 'Resources\\audios' exista en tu proyecto.\n";
            }

            MessageBox.Show(mensaje, "Diagnóstico de Audios", MessageBoxButtons.OK, MessageBoxIcon.Information);
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