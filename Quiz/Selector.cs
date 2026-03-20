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
	public partial class Selector : Form
	{

		public Selector()
		{
			InitializeComponent();
		}

		private void Selector_Load(object sender, EventArgs e)
		{
			// Estilizar paneles (ajusta nombres)
			ConfigurarPanel(pnlMusica, Color.Cyan, 3);
			ConfigurarPanel(pnlDeportes, Color.HotPink, 2);
			ConfigurarPanel(pnlDesastres, Color.MediumPurple, 1);
			ConfigurarPanel(pnlAnime, Color.Orange, 5);
			ConfigurarPanel(pnlPeliculas, Color.Turquoise, 6);
			ConfigurarPanel(pnlFauna, Color.Gold, 7);
			ConfigurarPanel(pnlVideojuegos, Color.DeepSkyBlue, 4);
		}

		private void ConfigurarPanel(Panel pnl, Color colorBorde, int categoriaId)
		{
			pnl.BackColor = Color.Transparent;
			pnl.Cursor = Cursors.Hand;

			pnl.Paint += (s, e) =>
			{
				ControlPaint.DrawBorder(
					e.Graphics,
					pnl.ClientRectangle,
					colorBorde,
					ButtonBorderStyle.Solid
				);
			};

			pnl.MouseEnter += (s, e) =>
			{
				pnl.BackColor = Color.FromArgb(40, colorBorde);
			};

			pnl.MouseLeave += (s, e) =>
			{
				pnl.BackColor = Color.Transparent;
			};

			// aqui se abre el form que contednrá las preguntas
			pnl.Click += (s, e) =>
			{
				Preguntas preg = new Preguntas(categoriaId);
				preg.ShowDialog();
				this.Hide();
				this.Close();
			};
		}

		private void boton_regreso_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}