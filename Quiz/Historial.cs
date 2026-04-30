using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Quiz
{
    public partial class Historial : Form
    {
        string connStr = "server=localhost;database=quiz_bd;uid=root;pwd=1234;";

        public Historial()
        {
            InitializeComponent();
            cargaPartidas();
        }

        private void boton_regreso_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cargaPartidas()
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                string query = @"
                    SELECT c.nombre AS categoria, pj.aciertos, pj.errores, p.fecha
                    FROM partida_jugadores pj
                    INNER JOIN partidas p ON pj.id_partida = p.id_partida
                    INNER JOIN categorias c ON p.id_categoria = c.id_categoria
                    WHERE pj.id_jugador = @jugador
                    ORDER BY p.fecha DESC;
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@jugador", Inicio.idJugadorGlobal);

                MySqlDataReader reader = cmd.ExecuteReader();

                string texto = "";

                while (reader.Read())
                {
                    string categoria = reader["categoria"].ToString();
                    int correctas = Convert.ToInt32(reader["aciertos"]);
                    int incorrectas = Convert.ToInt32(reader["errores"]);
                    DateTime fecha = Convert.ToDateTime(reader["fecha"]);

                    texto += $"Categoria: {categoria} | Correctas: {correctas} | Incorrectas: {incorrectas} | {fecha}\n";
                }

                pantalla.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold);
                pantalla.Text = texto;
            }
        }
    }

}
