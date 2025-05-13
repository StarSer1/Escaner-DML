using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escaner_DML
{
    public partial class botonn : Button
    {
        protected override void OnClick(EventArgs e)
        {
            // 1. Encuentra el form que contiene este botón
            var parentForm = this.FindForm() as Form1;
            if (parentForm == null)
            {
                base.OnClick(e);
                return;
            }

            // 2. Pasa los mismos objetos que ya existen en el form
            var analisis = new Analisis(
                parentForm.errorActivado,
                parentForm.tablas,
                parentForm.atributos,
                parentForm.restricciones
            );
            

            // 3. Llama a A() pasándole el RichTextBox y el flag
            List<string> papus = analisis.A(parentForm.txtEntrada, parentForm.DgvLexica, parentForm.txtError, parentForm.DtTablas, parentForm.DtAtributos, parentForm.DtRestricciones);
            
            analisis.LLENADOTABLASPAPU(parentForm.DtTablas, parentForm.DtAtributos, parentForm.DtRestricciones, papus);
            parentForm.tablas = analisis.tablas;
            parentForm.atributos = analisis.atributos;
            parentForm.restricciones = analisis.restricciones;
            analisis.LlenadoSelects();

            // 4. Luego dispara el Click “visible”
            base.OnClick(e);
        }
    }
}
