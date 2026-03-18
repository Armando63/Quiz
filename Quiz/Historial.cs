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
    public partial class Historial : Form
    {
        public Historial()
        {
            InitializeComponent();
        }

		private void boton_regreso_Click(object sender, EventArgs e)
		{
			Inicio select = new Inicio();
		
			select.ShowDialog();
		}
	}
}
