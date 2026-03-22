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

                string query = @"SELECT * FROM partidas";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                string texto = "";

                while (reader.Read())
                {
                    int categoria = Convert.ToInt32(reader["id_categoria"]);
                    int correctas = Convert.ToInt32(reader["aciertos"]);
                    int incorrectas = Convert.ToInt32(reader["errores"]);
                    string categoriaNombre;
                    DateTime fecha = Convert.ToDateTime(reader["fecha"]);

                    switch (categoria)
                    {
                        case 1:
                            categoriaNombre = "Desastres Naturales";
                            break;
                        case 2:
                            categoriaNombre = "Deportes";
                            break;
                        case 3:
                            categoriaNombre = "Musica";
                            break;
                        case 4:
                            categoriaNombre = "Videojuegos";
                            break;
                        case 5:
                            categoriaNombre = "Anime";
                            break;
                        case 6:
                            categoriaNombre = "Peliculas";
                            break;
                        case 7:
                            categoriaNombre = "Fauna";
                            break;
                        default:
                            categoriaNombre = "Desconocida";
                            break;
                    }

                    texto += $"Categoria: {categoriaNombre} | Correctas: {correctas} | Incorrectas {incorrectas} | {fecha}\n";
                }

                pantalla.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold);
                pantalla.Text = texto;
            }
        }
    }

}
