using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escaner_DML
{
    public partial class Form1 : Form
    {
        Analisis Analisis;
        public Form1()
        {
            InitializeComponent();
        }


        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnAnalizar_Click(object sender, EventArgs e)
        {
            Analisis = new Analisis();
            Analisis.Analizador(txtEntrada);
        }
    }

}
