﻿using System;
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
        new List<string> Tabla;
        public Form1()
        {
            InitializeComponent();
        }


        private void BtnSalir_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            Analisis = new Analisis();
            Analisis.MetodoError(txtEntrada.Text);
        }

        private void BtnAnalizar_Click(object sender, EventArgs e)
        {
            DgvConstantes.Rows.Clear();
            DgvIdentificadores.Rows.Clear();
            DgvLexica.Rows.Clear();
            Analisis = new Analisis();
            Analisis.Analizador(txtEntrada, DgvConstantes, DgvIdentificadores, DgvLexica);
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
                txtEntrada.Text = "SELECT ANOMBRE \r\nFROM ALUMNOS,INSCRITOS,CARRERAS \r\nWHERE ALUMNOS.A#=INSCRITOS.A# AND ALUMNOS.C#=CARRERAS.C# \r\nAND INSCRITOS.SEMESTRE='2010I' \r\nAND CARRERAS.CNOMBRE='ISC' \r\nAND LUMNOS.GENERACION='2010'";
            }
        }
    }

}
