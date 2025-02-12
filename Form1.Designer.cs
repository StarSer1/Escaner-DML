namespace Escaner_DML
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
            this.BtnAnalizar = new System.Windows.Forms.Button();
            this.BtnSalir = new System.Windows.Forms.Button();
            this.DgvConstantes = new System.Windows.Forms.DataGridView();
            this.DgvIdentificadores = new System.Windows.Forms.DataGridView();
            this.DgvLexica = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEntrada = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmbEjemplo = new System.Windows.Forms.ComboBox();
            this.txtError = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DgvConstantes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvIdentificadores)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvLexica)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnAnalizar
            // 
            this.BtnAnalizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(242)))), ((int)(((byte)(255)))));
            this.BtnAnalizar.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAnalizar.Location = new System.Drawing.Point(233, 453);
            this.BtnAnalizar.Name = "BtnAnalizar";
            this.BtnAnalizar.Size = new System.Drawing.Size(83, 31);
            this.BtnAnalizar.TabIndex = 0;
            this.BtnAnalizar.Text = "Analizar";
            this.BtnAnalizar.UseVisualStyleBackColor = false;
            this.BtnAnalizar.Click += new System.EventHandler(this.BtnAnalizar_Click);
            // 
            // BtnSalir
            // 
            this.BtnSalir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(188)))), ((int)(((byte)(188)))));
            this.BtnSalir.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSalir.Location = new System.Drawing.Point(12, 453);
            this.BtnSalir.Name = "BtnSalir";
            this.BtnSalir.Size = new System.Drawing.Size(83, 31);
            this.BtnSalir.TabIndex = 1;
            this.BtnSalir.Text = "Salir";
            this.BtnSalir.UseVisualStyleBackColor = false;
            this.BtnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // DgvConstantes
            // 
            this.DgvConstantes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvConstantes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvConstantes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12});
            this.DgvConstantes.Location = new System.Drawing.Point(338, 44);
            this.DgvConstantes.Name = "DgvConstantes";
            this.DgvConstantes.RowHeadersVisible = false;
            this.DgvConstantes.Size = new System.Drawing.Size(240, 197);
            this.DgvConstantes.TabIndex = 2;
            // 
            // DgvIdentificadores
            // 
            this.DgvIdentificadores.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvIdentificadores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvIdentificadores.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column7,
            this.Column8});
            this.DgvIdentificadores.Location = new System.Drawing.Point(338, 271);
            this.DgvIdentificadores.Name = "DgvIdentificadores";
            this.DgvIdentificadores.RowHeadersVisible = false;
            this.DgvIdentificadores.Size = new System.Drawing.Size(240, 213);
            this.DgvIdentificadores.TabIndex = 3;
            // 
            // DgvLexica
            // 
            this.DgvLexica.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvLexica.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.DgvLexica.Location = new System.Drawing.Point(599, 44);
            this.DgvLexica.Name = "DgvLexica";
            this.DgvLexica.RowHeadersVisible = false;
            this.DgvLexica.Size = new System.Drawing.Size(401, 440);
            this.DgvLexica.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(334, 244);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 22);
            this.label1.TabIndex = 5;
            this.label1.Text = "Identificadores";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(334, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 22);
            this.label2.TabIndex = 5;
            this.label2.Text = "Constantes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(595, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 22);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tabla Léxica";
            // 
            // txtEntrada
            // 
            this.txtEntrada.Location = new System.Drawing.Point(12, 82);
            this.txtEntrada.Name = "txtEntrada";
            this.txtEntrada.Size = new System.Drawing.Size(304, 325);
            this.txtEntrada.TabIndex = 6;
            this.txtEntrada.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 22);
            this.label4.TabIndex = 5;
            this.label4.Text = "Entrada";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(211)))), ((int)(((byte)(177)))));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 493);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1012, 23);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(264, 18);
            this.toolStripStatusLabel1.Text = "David Alejandro De Los Santos Cruz";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(196, 18);
            this.toolStripStatusLabel2.Text = "Carlos Omar Celis Calzada";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(226, 18);
            this.toolStripStatusLabel3.Text = "Brandon Israel Nuñez Montoya";
            // 
            // cmbEjemplo
            // 
            this.cmbEjemplo.FormattingEnabled = true;
            this.cmbEjemplo.Items.AddRange(new object[] {
            "Ejemplo 1",
            "Ejemplo 2",
            "Ejemplo 3",
            "Ejemplo 4"});
            this.cmbEjemplo.Location = new System.Drawing.Point(12, 44);
            this.cmbEjemplo.Name = "cmbEjemplo";
            this.cmbEjemplo.Size = new System.Drawing.Size(304, 21);
            this.cmbEjemplo.TabIndex = 8;
            this.cmbEjemplo.SelectedIndexChanged += new System.EventHandler(this.cmbEjemplo_SelectedIndexChanged);
            // 
            // txtError
            // 
            this.txtError.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtError.Location = new System.Drawing.Point(85, 410);
            this.txtError.Name = "txtError";
            this.txtError.Size = new System.Drawing.Size(231, 22);
            this.txtError.TabIndex = 9;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(12, 410);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(75, 22);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "Mensaje:";
            // 
            // Column6
            // 
            this.Column6.FillWeight = 140F;
            this.Column6.HeaderText = "Identificador";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            this.Column7.FillWeight = 70F;
            this.Column7.HeaderText = "Valor";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.FillWeight = 90F;
            this.Column8.HeaderText = "Linea";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.FillWeight = 60F;
            this.Column9.HeaderText = "No.";
            this.Column9.Name = "Column9";
            // 
            // Column10
            // 
            this.Column10.FillWeight = 140F;
            this.Column10.HeaderText = "Constante";
            this.Column10.Name = "Column10";
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Tipo";
            this.Column11.Name = "Column11";
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Valor";
            this.Column12.Name = "Column12";
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.FillWeight = 70F;
            this.Column1.HeaderText = "No.";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.FillWeight = 70F;
            this.Column2.HeaderText = "Linea";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.FillWeight = 160F;
            this.Column3.HeaderText = "TOKEN";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.HeaderText = "Tipo";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column5.HeaderText = "Código";
            this.Column5.Name = "Column5";
            // 
            // BtnClear
            // 
            this.BtnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.BtnClear.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnClear.Location = new System.Drawing.Point(121, 453);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(83, 31);
            this.BtnClear.TabIndex = 11;
            this.BtnClear.Text = "Limpiar";
            this.BtnClear.UseVisualStyleBackColor = false;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(236)))), ((int)(((byte)(210)))));
            this.ClientSize = new System.Drawing.Size(1012, 516);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtError);
            this.Controls.Add(this.cmbEjemplo);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtEntrada);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DgvLexica);
            this.Controls.Add(this.DgvIdentificadores);
            this.Controls.Add(this.DgvConstantes);
            this.Controls.Add(this.BtnSalir);
            this.Controls.Add(this.BtnAnalizar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Escaner DML";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DgvConstantes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvIdentificadores)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvLexica)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnAnalizar;
        private System.Windows.Forms.Button BtnSalir;
        private System.Windows.Forms.DataGridView DgvConstantes;
        private System.Windows.Forms.DataGridView DgvIdentificadores;
        private System.Windows.Forms.DataGridView DgvLexica;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox txtEntrada;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ComboBox cmbEjemplo;
        private System.Windows.Forms.TextBox txtError;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.Button BtnClear;
    }
}

