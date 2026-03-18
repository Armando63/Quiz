namespace Quiz
{
    partial class Historial
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Historial));
			this.label1 = new System.Windows.Forms.Label();
			this.boton_regreso = new System.Windows.Forms.Button();
			this.combox_selec = new System.Windows.Forms.ComboBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.buton_ejecuta = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
			this.label1.Location = new System.Drawing.Point(139, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(783, 69);
			this.label1.TabIndex = 0;
			this.label1.Text = "HISTORIAL DE PARTIDAS";
			// 
			// boton_regreso
			// 
			this.boton_regreso.BackColor = System.Drawing.Color.IndianRed;
			this.boton_regreso.Location = new System.Drawing.Point(50, 421);
			this.boton_regreso.Name = "boton_regreso";
			this.boton_regreso.Size = new System.Drawing.Size(95, 33);
			this.boton_regreso.TabIndex = 1;
			this.boton_regreso.Text = "REGRESAR";
			this.boton_regreso.UseVisualStyleBackColor = false;
			this.boton_regreso.Click += new System.EventHandler(this.boton_regreso_Click);
			// 
			// combox_selec
			// 
			this.combox_selec.BackColor = System.Drawing.Color.White;
			this.combox_selec.FormattingEnabled = true;
			this.combox_selec.Location = new System.Drawing.Point(371, 149);
			this.combox_selec.Name = "combox_selec";
			this.combox_selec.Size = new System.Drawing.Size(228, 24);
			this.combox_selec.TabIndex = 3;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(302, 236);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(395, 171);
			this.textBox1.TabIndex = 4;
			// 
			// buton_ejecuta
			// 
			this.buton_ejecuta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
			this.buton_ejecuta.Location = new System.Drawing.Point(696, 150);
			this.buton_ejecuta.Name = "buton_ejecuta";
			this.buton_ejecuta.Size = new System.Drawing.Size(117, 23);
			this.buton_ejecuta.TabIndex = 5;
			this.buton_ejecuta.Text = "EJECUTAR";
			this.buton_ejecuta.UseVisualStyleBackColor = false;
			// 
			// Historial
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(1067, 554);
			this.Controls.Add(this.buton_ejecuta);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.combox_selec);
			this.Controls.Add(this.boton_regreso);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "Historial";
			this.Text = "Historial";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button boton_regreso;
		private System.Windows.Forms.ComboBox combox_selec;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button buton_ejecuta;
	}
}