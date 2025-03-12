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
        Errores Errores;
        List<string> TablaLex;
        bool errorActivado = false;

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
            //DgvConstantes.Rows.Clear();
            //DgvIdentificadores.Rows.Clear();
            DgvLexica.Rows.Clear();
            Analisis = new Analisis(errorActivado);
            Errores = new Errores();
            if (Errores.ErrorSimboloDesco(txtEntrada, txtError) == false)
            {
                TablaLex = Analisis.Analizador(txtEntrada, DgvLexica, txtError);
                if (Analisis.errorActivado == false)
                {
                    Analisis.Sintaxis(TablaLex, txtError);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cmbEjemplo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEjemplo.SelectedIndex == 0)
            {
                txtEntrada.Text = "SELECT *\r\nFROM PROFESORES\r\nWHERE EDAD >45 AND GRADO=MAE' OR GRADO='DOC'";
            }
            if (cmbEjemplo.SelectedIndex == 1)
            {
                txtEntrada.Text = "SELECT ANOMBRE\r\nFROM ALUMNOS,INSCRITOS,\r\nWHERE ALUMNOS.A#=INSCRITOS.A# AND\r\nINSCRITOS.SEMESTRE='2010I'";
            }
            if (cmbEjemplo.SelectedIndex == 2)
            {
                txtEntrada.Text = "SELECT ANOMBRE\r\nFROM ALUMNOS\r\nWHERE A# IN(SELECT A#\r\nFROM INSCRITOS\r\nWHERE P# IN (SELECT P#\r\nFROM PROFESORES\r\nWHERE GRADO='MAE'))\r\nAND C# IN (SELECT C#\r\nFROM\r\nWHERE CNOMBRE='ISC')";
            }
            if (cmbEjemplo.SelectedIndex == 3)
            {
                txtEntrada.Text = "SELECT ANOMBRE\r\nFROM ALUMNOS A,INSCRITOS I,CARRERAS C\r\nWHERE A.A#=I.A# AND A.C#=C.C#\r\nAND I.SEMESTRE='2010I' C.CNOMBRE='ITC'";
            }
            if (cmbEjemplo.SelectedIndex == 4)
            {
                txtEntrada.Text = "SELECT A#,ANOMBRE\r\nFROM ALUMNOS\r\nWHERE C# IN (SELECT C#\r\nFROM CARRERAS\r\nWHERE SEMESTRES=9)\r\nAND A# (SELECT A#\r\nFROM INSCRITOS\r\nWHERE SEMESTRE='2010I')";
            }
            if (cmbEjemplo.SelectedIndex == 5)
            {
                txtEntrada.Text = "SELECT ANOMBRE\r\nFROM ALUMNOS,INSCRITOS,CARRERAS\r\nWHERE ALUMNOS.A#=INSCRITOS.A# AND ALUMNOS.C#=CARRERAS.C#\r\nAND INSCRITOS.SEMESTRE='2010I'\r\nAND CARRERAS.CNOMBRE='ISC\r\nAND ALUMNOS.GENERACION='2010'";
            }
            if (cmbEjemplo.SelectedIndex == 6)
            {
                txtEntrada.Text = "SELECT ANOMBRE\r\nFROM ALUMNOS,INSCRITOS,CARRERAS\r\nWHERE ALUMNOS.A#=INSCRITOS.A# AND ALUMNOS.C#=CARRERAS.C#\r\nAND INSCRITOS.SEMESTRE '2010I'\r\nAND CARRERAS.CNOMBRE='ISC'\r\nAND ALUMNOS.GENERACION='2010'";
            }

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            //DgvConstantes.Rows.Clear();
            //DgvIdentificadores.Rows.Clear();
            DgvLexica.Rows.Clear();
            txtError.Text = string.Empty;
            txtEntrada.Text = string.Empty;
            txtError.BackColor = Color.White;
        }
    }

}
