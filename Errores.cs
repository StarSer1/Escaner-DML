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
        public void ErrorMaestro(TextBox txtError, int lineas, string palEsperada)
        {
            txtError.Text = "Error 2:200: Linea " + lineas + " Se esperaba: "+palEsperada;
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarIdentificadorInvalido(TextBox txtError, int lineas, string identificador)
        {
            txtError.Text = $"Error 3:315 Línea {lineas:D2} El identificador { identificador} no es válido.";
            
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void nombreAtributoDuplicado(TextBox txtError, int lineas, string atributo)
        {
            txtError.Text = "Error 3:306: Linea " + lineas + " El nombre del atributo " + atributo + " esta duplicado";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarNombreAtributo(TextBox txtError, int lineas, string atributo)
        {
            txtError.Text = "3:302: Linea " + lineas + " El nombre del atributo '" + atributo + "' se especifica más de una vez";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarExistirAtributo(TextBox txtError, int lineas, string atributo, List<(int noTabla, string nombreTabla, int cantidadAtributos, int cantidadRestricciones)> tablas, int numeroTablasChecker)
        {
            txtError.Text = "3:303: Linea " + lineas + " El nombre del atributo '" + atributo + "' no existe en la tabla '" + tablas.FirstOrDefault(t => t.noTabla == numeroTablasChecker).nombreTabla + "'";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarAtributoNoValido(TextBox txtError, int lineas, string atributo, ref List<(int noTabla, string nombreTabla, int cantidadAtributos, int cantidadRestricciones)> tablas, int numeroTablasChecker)
        {
            txtError.Text = "3:305: Linea " + lineas + " Se hace referencia al atributo '" + atributo + "' no valido en la tabla '" + tablas.FirstOrDefault(t => t.noTabla == numeroTablasChecker).nombreTabla + "'";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarDupRestriccion(TextBox txtError, int lineas, string restriccion)
        {
            txtError.Text = "3:304: Linea " + lineas + " El nombre de la restriccion '" + restriccion + "' ya se encuentra registrado en la base de datos";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarCantidadAtributos(TextBox txtError, int lineas)
        {
            txtError.Text = "3:307: Linea " + lineas + " Los valores especificados, no corresponden a la definicion de la tabla";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarExistirTabla(TextBox txtError, int lineas)
        {
            txtError.Text = "3:309: Linea " + lineas + " La tabla a la que se hace referencia no existe";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarCantidadBytes(TextBox txtError, int lineas)
        {
            txtError.Text = "3:308: Linea " + lineas + " Los datos de la cadena o binarios se truncarían";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarNombreEnTabla(TextBox txtError, string atributo, int lineas)
        {
            txtError.Text = "3:311 El nombre del atributo "+atributo+" no es válido.";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarAmbiguedad(TextBox txtError, string atributo, int lineas)
        {
            txtError.Text = "3:312 El nombre del atributo "+ atributo + " es ambigüo.";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarTablaNoValida(TextBox txtError, int lineas, string nombreTabla)
        {
            txtError.Text = $"Error 3:314 Línea {lineas:D2} El nombre de la tabla { nombreTabla} no es válido.";
            
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarNombreEnTablaWhere(TextBox txtError, string atributo, int lineas)
        {
            txtError.Text = atributo;
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarTablaNoValidaAlias(TextBox txtError, string atributo, int lineas)
        {
            txtError.Text = atributo;
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
        public void validarAtributoSubconsultaInvalido(TextBox txtError, int lineas, string atributo)
        {
            txtError.Text = $"Error 3:312 Línea {lineas:D2} El atributo {atributo} no es válido.";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
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
        public bool ErrorParentesisDDL(TextBox txtError, int acum)
        {
            if (acum < 0)
            {
                txtError.BackColor = Color.FromArgb(255, 137, 137);
                txtError.Text = "Error 101: Faltan parentesis abierto";
                return true;
            }
            if (acum > 0)
            {
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
