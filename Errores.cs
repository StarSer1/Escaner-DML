using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escaner_DML
{
    public class Errores
    {
        public void ErrorPalabraReservada(TextBox txtError, int lineas)
        {
            txtError.Text = "Error 2:201: Linea " + lineas + " Se esperaba Palabra Reservada";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void ErrorIdentificador(TextBox txtError, int lineas)
        {
            txtError.Text = "Error 2:204: Linea " + lineas + " Se esperaba Identificador";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void ErrorConstante(TextBox txtError, int lineas)
        {
            txtError.Text = "Error 2:206: Linea " + lineas + " Se esperaba Constante";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void ErrorOperador(TextBox txtError, int lineas)
        {
            txtError.Text = "Error 2:207: Linea " + lineas + " Se esperaba Operador";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void ErrorOperadorRelacional(TextBox txtError, int lineas)
        {
            txtError.Text = "Error 2:208: Linea " + lineas + " Se esperaba Operador Relacional";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        
        public void ErrorSintactico(TextBox txtError, int lineas)
        {
            txtError.Text = "Error 2:200: Linea " + lineas + " Error de Sintaxis";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        
        public void SinError(TextBox txtError, int lineas)
        {
            txtError.Text = "Sin Error";
            txtError.BackColor = Color.FromArgb(232, 255, 223);
        }

        public bool ErrorParentesis (DataGridView dgvCons, DataGridView dgvIden, DataGridView dgvLex, TextBox txtError, int acum)
        {
            if (acum < 0)
            {
                dgvCons.Rows.Clear();
                dgvIden.Rows.Clear();
                dgvLex.Rows.Clear();
                txtError.BackColor = Color.FromArgb(255, 137, 137);
                txtError.Text = "Error 101: Faltan parentesis abierto";
                return true;
            }
            if (acum > 0)
            {
                dgvCons.Rows.Clear();
                dgvIden.Rows.Clear();
                dgvLex.Rows.Clear();
                txtError.BackColor = Color.FromArgb(255, 137, 137);
                txtError.Text = "Error 102: Faltan parentesis cerrado";
                return true;
            }
            if (acum == 0)
            {
                txtError.BackColor = Color.FromArgb(232, 255, 223);
                txtError.Text = "Sin Error";
                return false;
            }
            return false;
        }
        public bool ErroresComillas(TextBox txtError, int acum, int lineas)
        {
            if (acum % 2 != 0)
            {
                txtError.BackColor = Color.FromArgb(255, 137, 137);
                txtError.Text = "Error 2:205: Linea " + lineas + " Se esperaba Delimitador"; return true;
            }
            else if (txtError.Text == "Sin Error")
            {
                txtError.BackColor = Color.FromArgb(232, 255, 223);
                txtError.Text = "Sin Error";
                return false;
            }
            return false;
        }
        public bool ErrorSimboloDesco(RichTextBox txt, TextBox txtError)
        {
            int lineaError = 0;
            int primeraLineaError = -1;
            bool errorB = false;
            string pattern = @"[$%&/^|?¿]";
            MatchCollection matches = Regex.Matches(txt.Text, pattern);
            foreach (Match match in matches)
            {
                //txt.Select(match.Index, match.Length);
                //txt.SelectionColor = Color.Red;

                if (primeraLineaError == -1)
                {
                    lineaError = txt.GetLineFromCharIndex(match.Index);
                    primeraLineaError = lineaError + 1;
                }
                txtError.BackColor = Color.FromArgb(255, 137, 137);
                txtError.Text = "Error 1:101: Linea "+(lineaError+1)+" Simbolo Desconocido: " + match.Value;
            }
            if (matches.Count > 0)
            {
                errorB = true;
                //error = true;
            }
            return errorB;
        }


    }
}
