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
        List<string> TablaLex;
        public Form1()
        {
            InitializeComponent();
        }


        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
           // Analisis = new Analisis();
           // Analisis.MetodoError(txtEntrada.Text);
        }

        private void BtnAnalizar_Click(object sender, EventArgs e)
        {
            txtError.BackColor = Color.White;
            txtError.Text = "";
            DgvConstantes.Rows.Clear();
            DgvIdentificadores.Rows.Clear();
            DgvLexica.Rows.Clear();
            Analisis = new Analisis();
            TablaLex = Analisis.Analizador(txtEntrada, DgvConstantes, DgvIdentificadores, DgvLexica, txtError);
            Analisis.Sintaxis(TablaLex);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cmbEjemplo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEjemplo.SelectedIndex == 0)
            {
                txtEntrada.Text = "SELECT * \r\nFROM PROFESORES \r\nWHERE EDAD >45 AND GRADO='MAE' OR GRADO='DOC'";
            }
            if (cmbEjemplo.SelectedIndex == 1)
            {
                txtEntrada.Text = "SELECT ANOMBRE \r\nFROM ALUMNOS,INSCRITOS \r\nWHERE ALUMNOS.A#=INSCRITOS.A# AND INSCRITOS.SEMESTRE='2010I'";
            }
            if (cmbEjemplo.SelectedIndex == 2)
            {
                txtEntrada.Text = "SELECT M#,MNOMBRE \r\nFROM MATERIAS \r\nWHERE M# IN (SELECT M# \r\nFROM INSCRITOS \r\nWHERE A# IN (SELECT A# \r\nFROM ALUMNOS \r\nWHERE ANOMBRE='MESSI LIONEL'))";
            }
            if (cmbEjemplo.SelectedIndex == 3)
            {
                txtEntrada.Text = "SELECT ANOMBRE \r\nFROM ALUMNOS \r\nWHERE A# IN (SELECT A# \r\nFROM INSCRITOS \r\nWHERE CALIFICACION < 70 \r\nAND M# IN (SELECT M# \r\nFROM MATERIAS \r\nWHERE C# IN (SELECT C# \r\nFROM CARRERAS \r\nWHERE D# IN (SELECT D# \r\nFROM DEPARTAMENTOS \r\nWHERE DNOMBRE = 'CIECOM'))))";
            }
            
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            DgvConstantes.Rows.Clear();
            DgvIdentificadores.Rows.Clear();
            DgvLexica.Rows.Clear();
            txtError.Text = string.Empty;
            txtEntrada.Text = string.Empty;
            txtError.BackColor = Color.White;
        }
    }

}
