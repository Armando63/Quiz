using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz
{
    public partial class Inicio : Form
    {
        public Inicio()
        {

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
               Selector select = new Selector();
                select.Show();
                this.Close();

        }

		private void button2_Click(object sender, EventArgs e)
		{
			Historial select = new Historial();
			select.Show();
            
		}
	}
}
