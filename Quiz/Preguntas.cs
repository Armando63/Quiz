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
	public partial class Preguntas : Form
	{
		private int categoriaId;

		public Preguntas(int categoriaID)
		{
			InitializeComponent();
			this.categoriaId = categoriaId;
		}

		private void Preguntas_Load(object sender, EventArgs e)
		{

		}
	}
}
