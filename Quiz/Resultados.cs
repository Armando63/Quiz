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
    public partial class Resultados : Form
    {
        string connStr = "server=localhost;database=quiz_bd;uid=root;pwd=1234;";

        int puntaje;
        int total;

        // CONTROLES
        private Label lblResultado;
        private Label txtMensaje;
        private DataGridView dgvJugadores;
        private Button btnCerrar;

        public Resultados(int puntaje, int total)
        {
            InitializeComponent();
            this.puntaje = puntaje;
            this.total = total;

            CrearControles(); // Creamos todo desde código
        }

        private void Resultados_Load(object sender, EventArgs e)
        {
            MostrarResultado();
            MostrarMensaje();
            CargarJugadores();
        }

        // 🔹 CREAR CONTROLES
        private void CrearControles()
        {
            this.Text = "Resultados";
            this.Size = new Size(600, 500);
            this.BackColor = Color.FromArgb(30, 30, 45);

            // LABEL RESULTADO
            lblResultado = new Label();
            lblResultado.ForeColor = Color.White;
            lblResultado.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblResultado.Location = new Point(20, 20);
            lblResultado.AutoSize = true;

            // MENSAJE
            txtMensaje = new Label();
            txtMensaje.ForeColor = Color.LightGreen;
            txtMensaje.Font = new Font("Segoe UI", 12, FontStyle.Italic);
            txtMensaje.Location = new Point(20, 60);
            txtMensaje.AutoSize = true;

            // TABLA
            dgvJugadores = new DataGridView();
            dgvJugadores.Location = new Point(20, 100);
            dgvJugadores.Size = new Size(540, 250);
            dgvJugadores.BackgroundColor = Color.White;
            dgvJugadores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // BOTÓN CERRAR
            btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Location = new Point(240, 370);
            btnCerrar.Size = new Size(100, 40);
            btnCerrar.BackColor = Color.FromArgb(244, 67, 54);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Click += BtnCerrar_Click;

            // AGREGAR CONTROLES
            this.Controls.Add(lblResultado);
            this.Controls.Add(txtMensaje);
            this.Controls.Add(dgvJugadores);
            this.Controls.Add(btnCerrar);

            // EVENTO LOAD
            this.Load += Resultados_Load;
        }

        // 🔹 MOSTRAR PUNTAJE
        private void MostrarResultado()
        {
            lblResultado.Text = $"Puntaje: {puntaje} / {total}";
        }

        // 🔹 MENSAJE
        private void MostrarMensaje()
        {
            double porcentaje = (double)puntaje / total * 100;

            if (porcentaje >= 80)
                txtMensaje.Text = "🔥 Excelente! Eres un pro del quiz";
            else if (porcentaje >= 50)
                txtMensaje.Text = "👍 Bien hecho, pero puedes mejorar";
            else
                txtMensaje.Text = "😅 Necesitas practicar más";
        }

        // 🔹 CARGAR HISTORIAL
        private void CargarJugadores()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            j.nombre AS Jugador,
                            pj.puntaje AS Puntaje,
                            pj.aciertos AS Aciertos,
                            pj.errores AS Errores
                        FROM partida_jugadores pj
                        JOIN jugadores j ON pj.id_jugador = j.id_jugador
                        ORDER BY pj.puntaje DESC;
                    ";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvJugadores.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar resultados: " + ex.Message);
            }
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
