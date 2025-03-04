using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escaner_DML
{
    public class Errores
    {
        public void ErrorOperadorRelacional(TextBox txtError, int lineas)
        {
            txtError.Text = "Error 2:208: Linea " + lineas + " Se esperaba Operador Relacional";
            txtError.BackColor = Color.FromArgb(255, 137, 137);
        }
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
        public bool ErroresComillas(DataGridView dgvCons, DataGridView dgvIden, DataGridView dgvLex, TextBox txtError, int acum)
        {
            if (acum % 2 != 0)
            {
                dgvCons.Rows.Clear();
                dgvIden.Rows.Clear();
                dgvLex.Rows.Clear();
                txtError.BackColor = Color.FromArgb(255, 137, 137);
                txtError.Text = "Error 103: Faltan comillas";
                return true;
            }
            else if (txtError.Text == "Sin Error")
            {
                txtError.BackColor = Color.FromArgb(232, 255, 223);
                txtError.Text = "Sin Error";
                return false;
            }
            return false;
        }



    }
}
