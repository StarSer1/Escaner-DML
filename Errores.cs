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
        public void ErrorParentesis (DataGridView dgvCons, DataGridView dgvIden, DataGridView dgvLex, TextBox txtError, int acum)
        {
            if (acum < 0)
            {
                dgvCons.Rows.Clear();
                dgvIden.Rows.Clear();
                dgvLex.Rows.Clear();
                txtError.BackColor = Color.FromArgb(255, 137, 137);
                txtError.Text = "Error 101: Faltan parentesis abierto";
            }
            if (acum > 0)
            {
                dgvCons.Rows.Clear();
                dgvIden.Rows.Clear();
                dgvLex.Rows.Clear();
                txtError.BackColor = Color.FromArgb(255, 137, 137);
                txtError.Text = "Error 102: Faltan parentesis cerrado";
            }
            if (acum == 0)
            {
                txtError.BackColor = Color.FromArgb(232, 255, 223);
                txtError.Text = "Sin Error";
            }
        }
        public void ErroresComillas(DataGridView dgvCons, DataGridView dgvIden, DataGridView dgvLex, TextBox txtError, int acum)
        {
            if (acum % 2 != 0)
            {
                dgvCons.Rows.Clear();
                dgvIden.Rows.Clear();
                dgvLex.Rows.Clear();
                txtError.BackColor = Color.FromArgb(255, 137, 137);
                txtError.Text = "Error 103: Faltan comillas";
            }
            else if (txtError.Text == "Sin Error")
            {
                txtError.BackColor = Color.FromArgb(232, 255, 223);
                txtError.Text = "Sin Error";
            }
        }
    }
}
