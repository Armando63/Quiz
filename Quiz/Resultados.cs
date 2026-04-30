using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Quiz
{
    public partial class Resultados : Form
    {
        string connStr = "server=localhost;database=quiz_bd;uid=root;pwd=1234;";

        int puntaje;
        int total;

        public Resultados(int puntaje, int total)
        {
            InitializeComponent();
            this.puntaje = puntaje;
            this.total = total;
        }

        private void Resultados_Load(object sender, EventArgs e)
        {
            CargarJugadores();
            MostrarMensaje();
        }

        private void CargarJugadores()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
                    SELECT j.nombre, pj.puntaje, pj.aciertos, pj.errores
                    FROM partida_jugadores pj
                    INNER JOIN jugadores j ON pj.id_jugador = j.id_jugador
                    ORDER BY pj.puntaje DESC;
                ";

                MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvJugadores.DataSource = dt;
            }
        }

        private void MostrarMensaje()
        {
            double porcentaje = (double)puntaje / total * 100;

            if (porcentaje >= 80)
                txtMensaje.Text = "Excelente! Eres un pro del quiz";
            else if (porcentaje >= 50)
                txtMensaje.Text = "Bien hecho, pero puedes mejorar";
            else
                txtMensaje.Text = "Necesitas practicar más";
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}