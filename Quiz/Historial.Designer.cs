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
            this.pantalla = new System.Windows.Forms.Label();
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
            // pantalla
            // 
            this.pantalla.AutoSize = true;
            this.pantalla.BackColor = System.Drawing.Color.Transparent;
            this.pantalla.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pantalla.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.pantalla.Location = new System.Drawing.Point(139, 118);
            this.pantalla.Name = "pantalla";
            this.pantalla.Size = new System.Drawing.Size(0, 69);
            this.pantalla.TabIndex = 2;
            // 
            // Historial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.pantalla);
            this.Controls.Add(this.boton_regreso);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Historial";
            this.Text = "Historial";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button boton_regreso;
        private System.Windows.Forms.Label pantalla;
    }
}