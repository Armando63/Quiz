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
    public partial class Inicio : Form
    {
        string connStr = "server=localhost;database=quiz_bd;uid=root;pwd=1234;";

        public static int idJugadorGlobal;
        public static string nombreJugadorGlobal;

        public Inicio()
        {
            InitializeComponent();
        }

        
        private int RegistrarJugador(string nombre)
        {
            int idJugador = 0;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = "INSERT INTO jugadores(nombre, socket_id, fecha_registro) VALUES(@nombre, '', NOW()); SELECT LAST_INSERT_ID();";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", nombre);

                idJugador = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return idJugador;
        }

       
        private void btnHistorial_Click(object sender, EventArgs e)
        {
            this.Hide();
            Historial historial = new Historial();
            historial.ShowDialog();
            this.Show();
        }

        private void btnIndividual_Click_1(object sender, EventArgs e)
        {
            if (txtNombre.Text.Trim() == "")
            {
                MessageBox.Show("Ingresa tu nombre");
                return;
            }

            nombreJugadorGlobal = txtNombre.Text;
            idJugadorGlobal = RegistrarJugador(nombreJugadorGlobal);

            this.Hide();
            Selector select = new Selector(); // tu modo individual
            select.ShowDialog();
            this.Show();
        }

        private void btnMultijugador_Click_1(object sender, EventArgs e)
        {
            if (txtNombre.Text.Trim() == "")
            {
                MessageBox.Show("Ingresa tu nombre");
                return;
            }

            nombreJugadorGlobal = txtNombre.Text;
            idJugadorGlobal = RegistrarJugador(nombreJugadorGlobal);

            this.Hide();
            SalaMultijugador sala = new SalaMultijugador(); // nuevo form que luego creamos
            sala.ShowDialog();
            this.Show();
        }

        private void btnSalir_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
