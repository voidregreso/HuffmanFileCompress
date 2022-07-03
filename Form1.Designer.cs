namespace FileCompress
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.filepath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.hacer = new System.Windows.Forms.Button();
            this.encdecbox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // filepath
            // 
            this.filepath.Location = new System.Drawing.Point(14, 27);
            this.filepath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.filepath.Multiline = true;
            this.filepath.Name = "filepath";
            this.filepath.Size = new System.Drawing.Size(300, 39);
            this.filepath.TabIndex = 0;
            this.filepath.TextChanged += new System.EventHandler(this.filepath_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(326, 27);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 38);
            this.button1.TabIndex = 1;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.filepath);
            this.groupBox1.Controls.Add(this.hacer);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.encdecbox);
            this.groupBox1.Location = new System.Drawing.Point(18, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(379, 125);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose a file";
            // 
            // hacer
            // 
            this.hacer.Location = new System.Drawing.Point(280, 81);
            this.hacer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.hacer.Name = "hacer";
            this.hacer.Size = new System.Drawing.Size(56, 31);
            this.hacer.TabIndex = 4;
            this.hacer.Text = "Process!";
            this.hacer.UseVisualStyleBackColor = true;
            this.hacer.Click += new System.EventHandler(this.hacer_Click);
            // 
            // encdecbox
            // 
            this.encdecbox.AutoSize = true;
            this.encdecbox.Location = new System.Drawing.Point(34, 89);
            this.encdecbox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.encdecbox.Name = "encdecbox";
            this.encdecbox.Size = new System.Drawing.Size(216, 17);
            this.encdecbox.TabIndex = 3;
            this.encdecbox.Text = "Check for decode, otherwise for encode";
            this.encdecbox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 142);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Ernesto Textfile Compression Utility";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox filepath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox encdecbox;
        private System.Windows.Forms.Button hacer;
    }
}

