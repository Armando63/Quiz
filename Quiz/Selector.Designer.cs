namespace Quiz
{
    partial class Selector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Selector));
			this.boton_regreso = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.pnlFauna = new System.Windows.Forms.Panel();
			this.pnlVideojuegos = new System.Windows.Forms.Panel();
			this.pnlPeliculas = new System.Windows.Forms.Panel();
			this.pnlAnime = new System.Windows.Forms.Panel();
			this.pnlDesastres = new System.Windows.Forms.Panel();
			this.pnlDeportes = new System.Windows.Forms.Panel();
			this.pnlMusica = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// boton_regreso
			// 
			this.boton_regreso.BackColor = System.Drawing.Color.IndianRed;
			this.boton_regreso.Location = new System.Drawing.Point(12, 531);
			this.boton_regreso.Name = "boton_regreso";
			this.boton_regreso.Size = new System.Drawing.Size(105, 42);
			this.boton_regreso.TabIndex = 5;
			this.boton_regreso.Text = "REGRESAR";
			this.boton_regreso.UseVisualStyleBackColor = false;
			this.boton_regreso.Click += new System.EventHandler(this.boton_regreso_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.label1.Location = new System.Drawing.Point(76, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 69);
			this.label1.TabIndex = 8;
			// 
			// pnlFauna
			// 
			this.pnlFauna.Location = new System.Drawing.Point(257, 388);
			this.pnlFauna.Name = "pnlFauna";
			this.pnlFauna.Size = new System.Drawing.Size(155, 103);
			this.pnlFauna.TabIndex = 9;
			// 
			// pnlVideojuegos
			// 
			this.pnlVideojuegos.Location = new System.Drawing.Point(665, 388);
			this.pnlVideojuegos.Name = "pnlVideojuegos";
			this.pnlVideojuegos.Size = new System.Drawing.Size(147, 99);
			this.pnlVideojuegos.TabIndex = 10;
			// 
			// pnlPeliculas
			// 
			this.pnlPeliculas.Location = new System.Drawing.Point(877, 210);
			this.pnlPeliculas.Name = "pnlPeliculas";
			this.pnlPeliculas.Size = new System.Drawing.Size(146, 98);
			this.pnlPeliculas.TabIndex = 11;
			// 
			// pnlAnime
			// 
			this.pnlAnime.Location = new System.Drawing.Point(665, 210);
			this.pnlAnime.Name = "pnlAnime";
			this.pnlAnime.Size = new System.Drawing.Size(147, 99);
			this.pnlAnime.TabIndex = 12;
			// 
			// pnlDesastres
			// 
			this.pnlDesastres.Location = new System.Drawing.Point(462, 210);
			this.pnlDesastres.Name = "pnlDesastres";
			this.pnlDesastres.Size = new System.Drawing.Size(149, 98);
			this.pnlDesastres.TabIndex = 13;
			// 
			// pnlDeportes
			// 
			this.pnlDeportes.Location = new System.Drawing.Point(260, 207);
			this.pnlDeportes.Name = "pnlDeportes";
			this.pnlDeportes.Size = new System.Drawing.Size(145, 98);
			this.pnlDeportes.TabIndex = 14;
			// 
			// pnlMusica
			// 
			this.pnlMusica.Location = new System.Drawing.Point(56, 206);
			this.pnlMusica.Name = "pnlMusica";
			this.pnlMusica.Size = new System.Drawing.Size(145, 100);
			this.pnlMusica.TabIndex = 15;
			// 
			// Selector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.ClientSize = new System.Drawing.Size(1033, 585);
			this.Controls.Add(this.pnlMusica);
			this.Controls.Add(this.pnlDeportes);
			this.Controls.Add(this.pnlDesastres);
			this.Controls.Add(this.pnlAnime);
			this.Controls.Add(this.pnlPeliculas);
			this.Controls.Add(this.pnlVideojuegos);
			this.Controls.Add(this.pnlFauna);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.boton_regreso);
			this.Name = "Selector";
			this.Text = "Selector";
			this.Load += new System.EventHandler(this.Selector_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
		private System.Windows.Forms.Button boton_regreso;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlFauna;
		private System.Windows.Forms.Panel pnlVideojuegos;
		private System.Windows.Forms.Panel pnlPeliculas;
		private System.Windows.Forms.Panel pnlAnime;
		private System.Windows.Forms.Panel pnlDesastres;
		private System.Windows.Forms.Panel pnlDeportes;
		private System.Windows.Forms.Panel pnlMusica;
	}
}