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
        List<(int noTabla, string nombreTabla, int cantidadAtributos, int cantidadRestricciones)> tablas = new List<(int, string, int, int)>();
        List<(int noTabla, int noAtributo, string nombreAtributo, string tipo, int longitud, int noNull, int noAtributoTabla)> atributos = new List<(int, int, string, string, int, int,int)>();
        List<(int noTabla, int noRestriccion, int Tipo, string nombreRestriccion, int atributoAsociado, int Tabla, int atributo)> restricciones = new List<(int, int, int, string, int, int, int)>();

        

        List<string> tokens = new List<string>();

        public Form1()
        {
            InitializeComponent();
            bool errorActivado = false;
            Analisis Analisis = new Analisis(errorActivado, tablas, atributos, restricciones);
            txtEntrada.Text = "CREATE TABLE DEPARTAMENTOS( \r\nD# CHAR(2) NOT NULL,\r\nDNOMBRE CHAR(6) NOT NULL,\r\nCONSTRAINT PK_DEPARTAMENTOS PRIMARY KEY (D#));\r\n\r\nINSERT INTO DEPARTAMENTOS VALUES ('D1','CIECOM');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D2','CIETIE');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D3','CIEING');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D4','CIEECO');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D5','CIEBAS');\r\n\r\nCREATE TABLE CARRERAS(\r\nC# CHAR(2) NOT NULL,\r\nCNOMBRE CHAR(3) NOT NULL,\r\nVIGENCIA CHAR(4) NOT NULL,\r\nSEMESTRES NUMERIC(2) NOT NULL,\r\nD# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_CARRERAS PRIMARY KEY (C#),\r\nCONSTRAINT FK_CARRERAS FOREIGN KEY (D#) REFERENCES DEPARTAMENTOS(D#));\r\n\r\nINSERT INTO CARRERAS VALUES ('C1','ISC','2010',9,'D1');\r\nINSERT INTO CARRERAS VALUES ('C2','LIN','2008',9,'D1');\r\nINSERT INTO CARRERAS VALUES ('C3','ICI','2009',10,'D2');\r\nINSERT INTO CARRERAS VALUES ('C4','IIN','2010',8,'D3');\r\nINSERT INTO CARRERAS VALUES ('C5','LAE','2011',8,'D4');\r\nINSERT INTO CARRERAS VALUES ('C6','ARQ','2010',10,'D2');\r\n\r\nCREATE TABLE ALUMNOS(\r\nA# CHAR(2) NOT NULL,\r\nANOMBRE CHAR(20) NOT NULL,\r\nGENERACION CHAR(4) NOT NULL,\r\nSEXO CHAR(1) NOT NULL,\r\nC# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_ALUMNOS PRIMARY KEY(A#),\r\nCONSTRAINT FK_ALUMNOS FOREIGN KEY(C#)REFERENCES CARRERAS(C#));\r\n\r\nINSERT INTO ALUMNOS VALUES ('A1','ALBA JESSICA','2009','F','C1');\r\nINSERT INTO ALUMNOS VALUES ('A2','CARREY JIM','2010','M','C2');\r\nINSERT INTO ALUMNOS VALUES ('A3','JOLIE ANGELINE','2011','F','C1');\r\nINSERT INTO ALUMNOS VALUES ('A4','SMITH WILL','2012','M','C6');\r\nINSERT INTO ALUMNOS VALUES ('A5','MESSI LIONEL','2010','M','C1');\r\nINSERT INTO ALUMNOS VALUES ('A6','ESPINOZA PAOLA','2012','F','C6');\r\nINSERT INTO ALUMNOS VALUES ('A7','BLANCO CUAHTEMOC','2009','M','C5');\r\nINSERT INTO ALUMNOS VALUES ('A8','SANCHEZ HUGO','2010','M','C4');\r\n\r\nCREATE TABLE MATERIAS(\r\nM# CHAR(2) NOT NULL,\r\nMNOMBRE CHAR(6) NOT NULL,\r\nCREDITOS NUMERIC(2) NOT NULL,\r\nC# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_MATERIAS PRIMARY KEY (M#),\r\nCONSTRAINT FK_MATERIAS FOREIGN KEY (C#) REFERENCES CARRERAS(C#));\r\n\r\nINSERT INTO MATERIAS VALUES ('M1','ESTDAT',10,'C1');\r\nINSERT INTO MATERIAS VALUES ('M2','FUNPRO',8,'C2');\r\nINSERT INTO MATERIAS VALUES ('M3','CIRCUI',10,'C1');\r\nINSERT INTO MATERIAS VALUES ('M4','PAISAJ',10,'C6');\r\nINSERT INTO MATERIAS VALUES ('M5','BASDAT',8,'C1');\r\nINSERT INTO MATERIAS VALUES ('M6','PROADM',6,'C5');\r\nINSERT INTO MATERIAS VALUES ('M7','CONTAB',6,'C5');\r\nINSERT INTO MATERIAS VALUES ('M8','ECONOM',8,'C2');\r\nINSERT INTO MATERIAS VALUES ('M9','RESMAT',10,'C3');\r\n\r\nCREATE TABLE PROFESORES(\r\nP# CHAR(2) NOT NULL, PNOMBRE CHAR(20) NOT NULL, EDAD NUMERIC(2) NOT NULL,\r\nSEXO CHAR(1)NOT NULL,\r\nESP CHAR(4) NOT NULL,\r\nGRADO CHAR(3) NOT NULL,\r\nD# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_PROFESORES PRIMARY KEY (P#),\r\nCONSTRAINT FK_PROFESORES FOREIGN KEY (D#) REFERENCES DEPARTAMENTOS(D#));\r\n\r\nINSERT INTO PROFESORES VALUES ('P1','DA VINCI LEONARDO',60,'M','PINT','LIC', 'D2');\r\nINSERT INTO PROFESORES VALUES ('P2','ARQUIMIDES',65,'M','QUIM','MAE','D3');\r\nINSERT INTO PROFESORES VALUES ('P3','TURING ALAN',43,'M','COMP','DOC','D1');\r\nINSERT INTO PROFESORES VALUES ('P4','EINSTEIN ALBERT',58,'M','GENI','DOC','D1');\r\nINSERT INTO PROFESORES VALUES ('P5','CURIE MARIE',45,'F','QUIM','LIC','D3');\r\nINSERT INTO PROFESORES VALUES ('P6','HAWKING WILLIAM',52,'M','FISI','DOC','D4');\r\nINSERT INTO PROFESORES VALUES ('P7','VON NEWMAN JOHN',47,'M','COMP','MAE','D1');\r\nINSERT INTO PROFESORES VALUES ('P8','NEWTON ISAAC',36,'M','FISI','LIC','D3');\r\nINSERT INTO PROFESORES VALUES ('P9','THATCHER MARGARET',64,'F','COMP','MAE','D1');\r\n\r\nCREATE TABLE INSCRITOS(\r\nR# CHAR(3) NOT NULL,\r\nA# CHAR(2) NOT NULL,\r\nM# CHAR(2) NOT NULL,\r\nP# CHAR(2) NOT NULL,\r\nTURNO CHAR(1) NOT NULL,\r\nSEMESTRE CHAR(6) NOT NULL,\r\nCALIFICACION NUMERIC(3) NOT NULL,\r\nCONSTRAINT PK_INSCRITOS PRIMARY KEY (R#),\r\nCONSTRAINT FK_INSCRITOS_01 FOREIGN KEY (A#) REFERENCES ALUMNOS(A#),\r\nCONSTRAINT FK_INSCRITOS_02 FOREIGN KEY (M#) REFERENCES MATERIAS(M#),\r\nCONSTRAINT FK_INSCRITOS_03 FOREIGN KEY (P#) REFERENCES PROFESORES(P#));\r\n\r\nINSERT INTO INSCRITOS VALUES ('R01','A1','M1','P3','M','2010I',60);\r\nINSERT INTO INSCRITOS VALUES ('R02','A1','M5','P4','M','2011I',75);\r\nINSERT INTO INSCRITOS VALUES ('R03','A1','M2','P3','V','2010I',78);\r\nINSERT INTO INSCRITOS VALUES ('R04','A1','M5','P4','M','2011II',80);\r\nINSERT INTO INSCRITOS VALUES ('R05','A2','M3','P6','V','2010I',86);\r\nINSERT INTO INSCRITOS VALUES ('R06','A2','M4','P7','V','2010I',90);\r\nINSERT INTO INSCRITOS VALUES ('R07','A3','M1','P2','M','2011I',70);\r\nINSERT INTO INSCRITOS VALUES ('R08','A3','M5','P9','V','2011II',82);\r\nINSERT INTO INSCRITOS VALUES ('R09','A4','M6','P6','M','2010I',90);\r\nINSERT INTO INSCRITOS VALUES ('R10','A4','M4','P9','V','2011II',88);\r\nINSERT INTO INSCRITOS VALUES ('R11','A4','M7','P9','M','2012I',90);\r\nINSERT INTO INSCRITOS VALUES ('R12','A5','M1','P3','M','2010I',78);\r\nINSERT INTO INSCRITOS VALUES ('R13','A5','M5','P3','V','2010II',86);\r\nINSERT INTO INSCRITOS VALUES ('R14','A5','M7','P9','V','2011II',76);\r\nINSERT INTO INSCRITOS VALUES ('R15','A5','M9','P5','V','2011I',92);\r\nINSERT INTO INSCRITOS VALUES ('R16','A7','M3','P4','M','2011II',78);\r\nINSERT INTO INSCRITOS VALUES ('R17','A7','M2','P8','M','2011I',86);\r\nINSERT INTO INSCRITOS VALUES ('R18','A7','M5','P7','M','2011I',94);";

            tokens = Analisis.Analizador(txtEntrada, DgvLexica, txtError, DtTablas, DtAtributos, DtRestricciones);
            Analisis.LLENADOTABLASPAPU(DtTablas, DtAtributos, DtRestricciones, tokens);
            txtEntrada.Text = "";
        }


        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
           // Analisis = new Analisis();
           // Analisis.MetodoError(txtEntrada.Text);
        }

        private void BtnAnalizar_Click(object sender, EventArgs e)
        {
            if (txtEntrada.Text != "")
            {
                string textoOriginal = txtEntrada.Text;

                // Paso 1: Eliminar saltos de línea y unir todo en una sola línea
                string textoEnUnaLinea = string.Join(" ", textoOriginal
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(linea => linea.Trim()));

                // Paso 2: Insertar salto de línea antes de cada '('
                string resultado = textoEnUnaLinea.Replace("(", "\n(");

                txtEntrada.Text = resultado;

                bool errorActivado = false;
                // Crear copias profundas de las listas
                List<(int noTabla, string nombreTabla, int cantidadAtributos, int cantidadRestricciones)> tablasTemp =
                    tablas.Select(t => (t.noTabla, t.nombreTabla, t.cantidadAtributos, t.cantidadRestricciones)).ToList();

                List<(int noTabla, int noAtributo, string nombreAtributo, string tipo, int longitud, int noNull, int noAtributoTemp)> atributosTemp =
                    atributos.Select(a => (a.noTabla, a.noAtributo, a.nombreAtributo, a.tipo, a.longitud, a.noNull, a.noAtributoTabla)).ToList();

                List<(int noTabla, int noRestriccion, int Tipo, string nombreRestriccion, int atributoAsociado, int Tabla, int atributo)> restriccionesTemp =
                    restricciones.Select(r => (r.noTabla, r.noRestriccion, r.Tipo, r.nombreRestriccion, r.atributoAsociado, r.Tabla, r.atributo)).ToList();

                Analisis Analisis = new Analisis(errorActivado, tablas, atributos, restricciones);
                Errores Errores = new Errores();
                DgvLexica.Rows.Clear();

                if (Errores.ErrorSimboloDesco(txtEntrada, txtError) == false)
                {
                    tokens = Analisis.Analizador(txtEntrada, DgvLexica, txtError, DtTablas, DtAtributos, DtRestricciones);

                    // Ahora las copias temporales no se verán afectadas por los cambios en las originales

                    if (Analisis.errorActivado == false)
                    {
                        Analisis.LLENADOTABLASPAPU(DtTablas, DtAtributos, DtRestricciones, tokens);
                        Analisis.LlenadoSelects(tokens);
                        try
                        {
                        while (tokens.Last() == "\n")
                        {
                            tokens.RemoveAt(tokens.Count - 1);
                        }

                        errorActivado = Analisis.Sintaxis(tokens, txtError);
                        if (errorActivado == true)
                        {
                            tablas = tablasTemp;
                            atributos = atributosTemp;
                            restricciones = restriccionesTemp;
                              
                        }
                        if(!errorActivado)
                                Analisis.consultaSQL(dataGridView1, txtEntrada, txtError);
                        }
                        catch
                        { }

                    }
                }
                txtEntrada.Text = textoOriginal;
                
            }

        }
        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            DtTablas.Rows.Clear();
            DtAtributos.Rows.Clear();
            DtRestricciones.Rows.Clear();
            tablas.Clear();
            atributos.Clear();
            restricciones.Clear();
            DgvLexica.Rows.Clear();
            txtError.Text = "";
            bool errorActivado = false;
            txtError.BackColor = Color.White;
            Analisis Analisis = new Analisis(errorActivado, tablas, atributos, restricciones);
            txtEntrada.Text = "DECLARE @sql NVARCHAR(MAX) = ''\r\n\r\nSELECT @sql += 'ALTER TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + \r\n               ' DROP CONSTRAINT ' + QUOTENAME(fk.name) + ';' + CHAR(13)\r\nFROM sys.foreign_keys fk\r\nJOIN sys.tables t ON fk.parent_object_id = t.object_id\r\nJOIN sys.schemas s ON t.schema_id = s.schema_id\r\n\r\nEXEC sp_executesql @sql\r\n\r\n-- Paso 2: Eliminar todas las tablas\r\nSET @sql = ''\r\n\r\nSELECT @sql += 'DROP TABLE ' + QUOTENAME(s.name) + '.' + QUOTENAME(t.name) + ';' + CHAR(13)\r\nFROM sys.tables t\r\nJOIN sys.schemas s ON t.schema_id = s.schema_id\r\n\r\nEXEC sp_executesql @sql";
            Analisis.consultaSQL(dataGridView1, txtEntrada, txtError);
            txtEntrada.Text = "";
        }

        private void BtnInicializar_Click(object sender, EventArgs e)
        {
            bool errorActivado = false;
            Analisis Analisis = new Analisis(errorActivado, tablas, atributos, restricciones);
            txtEntrada.Text = "CREATE TABLE DEPARTAMENTOS( \r\nD# CHAR(2) NOT NULL,\r\nDNOMBRE CHAR(6) NOT NULL,\r\nCONSTRAINT PK_DEPARTAMENTOS PRIMARY KEY (D#));\r\n\r\nINSERT INTO DEPARTAMENTOS VALUES ('D1','CIECOM');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D2','CIETIE');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D3','CIEING');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D4','CIEECO');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D5','CIEBAS');\r\n\r\nCREATE TABLE CARRERAS(\r\nC# CHAR(2) NOT NULL,\r\nCNOMBRE CHAR(3) NOT NULL,\r\nVIGENCIA CHAR(4) NOT NULL,\r\nSEMESTRES NUMERIC(2) NOT NULL,\r\nD# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_CARRERAS PRIMARY KEY (C#),\r\nCONSTRAINT FK_CARRERAS FOREIGN KEY (D#) REFERENCES DEPARTAMENTOS(D#));\r\n\r\nINSERT INTO CARRERAS VALUES ('C1','ISC','2010',9,'D1');\r\nINSERT INTO CARRERAS VALUES ('C2','LIN','2008',9,'D1');\r\nINSERT INTO CARRERAS VALUES ('C3','ICI','2009',10,'D2');\r\nINSERT INTO CARRERAS VALUES ('C4','IIN','2010',8,'D3');\r\nINSERT INTO CARRERAS VALUES ('C5','LAE','2011',8,'D4');\r\nINSERT INTO CARRERAS VALUES ('C6','ARQ','2010',10,'D2');\r\n\r\nCREATE TABLE ALUMNOS(\r\nA# CHAR(2) NOT NULL,\r\nANOMBRE CHAR(20) NOT NULL,\r\nGENERACION CHAR(4) NOT NULL,\r\nSEXO CHAR(1) NOT NULL,\r\nC# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_ALUMNOS PRIMARY KEY(A#),\r\nCONSTRAINT FK_ALUMNOS FOREIGN KEY(C#)REFERENCES CARRERAS(C#));\r\n\r\nINSERT INTO ALUMNOS VALUES ('A1','ALBA JESSICA','2009','F','C1');\r\nINSERT INTO ALUMNOS VALUES ('A2','CARREY JIM','2010','M','C2');\r\nINSERT INTO ALUMNOS VALUES ('A3','JOLIE ANGELINE','2011','F','C1');\r\nINSERT INTO ALUMNOS VALUES ('A4','SMITH WILL','2012','M','C6');\r\nINSERT INTO ALUMNOS VALUES ('A5','MESSI LIONEL','2010','M','C1');\r\nINSERT INTO ALUMNOS VALUES ('A6','ESPINOZA PAOLA','2012','F','C6');\r\nINSERT INTO ALUMNOS VALUES ('A7','BLANCO CUAHTEMOC','2009','M','C5');\r\nINSERT INTO ALUMNOS VALUES ('A8','SANCHEZ HUGO','2010','M','C4');\r\n\r\nCREATE TABLE MATERIAS(\r\nM# CHAR(2) NOT NULL,\r\nMNOMBRE CHAR(6) NOT NULL,\r\nCREDITOS NUMERIC(2) NOT NULL,\r\nC# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_MATERIAS PRIMARY KEY (M#),\r\nCONSTRAINT FK_MATERIAS FOREIGN KEY (C#) REFERENCES CARRERAS(C#));\r\n\r\nINSERT INTO MATERIAS VALUES ('M1','ESTDAT',10,'C1');\r\nINSERT INTO MATERIAS VALUES ('M2','FUNPRO',8,'C2');\r\nINSERT INTO MATERIAS VALUES ('M3','CIRCUI',10,'C1');\r\nINSERT INTO MATERIAS VALUES ('M4','PAISAJ',10,'C6');\r\nINSERT INTO MATERIAS VALUES ('M5','BASDAT',8,'C1');\r\nINSERT INTO MATERIAS VALUES ('M6','PROADM',6,'C5');\r\nINSERT INTO MATERIAS VALUES ('M7','CONTAB',6,'C5');\r\nINSERT INTO MATERIAS VALUES ('M8','ECONOM',8,'C2');\r\nINSERT INTO MATERIAS VALUES ('M9','RESMAT',10,'C3');\r\n\r\nCREATE TABLE PROFESORES(\r\nP# CHAR(2) NOT NULL, PNOMBRE CHAR(20) NOT NULL, EDAD NUMERIC(2) NOT NULL,\r\nSEXO CHAR(1)NOT NULL,\r\nESP CHAR(4) NOT NULL,\r\nGRADO CHAR(3) NOT NULL,\r\nD# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_PROFESORES PRIMARY KEY (P#),\r\nCONSTRAINT FK_PROFESORES FOREIGN KEY (D#) REFERENCES DEPARTAMENTOS(D#));\r\n\r\nINSERT INTO PROFESORES VALUES ('P1','DA VINCI LEONARDO',60,'M','PINT','LIC', 'D2');\r\nINSERT INTO PROFESORES VALUES ('P2','ARQUIMIDES',65,'M','QUIM','MAE','D3');\r\nINSERT INTO PROFESORES VALUES ('P3','TURING ALAN',43,'M','COMP','DOC','D1');\r\nINSERT INTO PROFESORES VALUES ('P4','EINSTEIN ALBERT',58,'M','GENI','DOC','D1');\r\nINSERT INTO PROFESORES VALUES ('P5','CURIE MARIE',45,'F','QUIM','LIC','D3');\r\nINSERT INTO PROFESORES VALUES ('P6','HAWKING WILLIAM',52,'M','FISI','DOC','D4');\r\nINSERT INTO PROFESORES VALUES ('P7','VON NEWMAN JOHN',47,'M','COMP','MAE','D1');\r\nINSERT INTO PROFESORES VALUES ('P8','NEWTON ISAAC',36,'M','FISI','LIC','D3');\r\nINSERT INTO PROFESORES VALUES ('P9','THATCHER MARGARET',64,'F','COMP','MAE','D1');\r\n\r\nCREATE TABLE INSCRITOS(\r\nR# CHAR(3) NOT NULL,\r\nA# CHAR(2) NOT NULL,\r\nM# CHAR(2) NOT NULL,\r\nP# CHAR(2) NOT NULL,\r\nTURNO CHAR(1) NOT NULL,\r\nSEMESTRE CHAR(6) NOT NULL,\r\nCALIFICACION NUMERIC(3) NOT NULL,\r\nCONSTRAINT PK_INSCRITOS PRIMARY KEY (R#),\r\nCONSTRAINT FK_INSCRITOS_01 FOREIGN KEY (A#) REFERENCES ALUMNOS(A#),\r\nCONSTRAINT FK_INSCRITOS_02 FOREIGN KEY (M#) REFERENCES MATERIAS(M#),\r\nCONSTRAINT FK_INSCRITOS_03 FOREIGN KEY (P#) REFERENCES PROFESORES(P#));\r\n\r\nINSERT INTO INSCRITOS VALUES ('R01','A1','M1','P3','M','2010I',60);\r\nINSERT INTO INSCRITOS VALUES ('R02','A1','M5','P4','M','2011I',75);\r\nINSERT INTO INSCRITOS VALUES ('R03','A1','M2','P3','V','2010I',78);\r\nINSERT INTO INSCRITOS VALUES ('R04','A1','M5','P4','M','2011II',80);\r\nINSERT INTO INSCRITOS VALUES ('R05','A2','M3','P6','V','2010I',86);\r\nINSERT INTO INSCRITOS VALUES ('R06','A2','M4','P7','V','2010I',90);\r\nINSERT INTO INSCRITOS VALUES ('R07','A3','M1','P2','M','2011I',70);\r\nINSERT INTO INSCRITOS VALUES ('R08','A3','M5','P9','V','2011II',82);\r\nINSERT INTO INSCRITOS VALUES ('R09','A4','M6','P6','M','2010I',90);\r\nINSERT INTO INSCRITOS VALUES ('R10','A4','M4','P9','V','2011II',88);\r\nINSERT INTO INSCRITOS VALUES ('R11','A4','M7','P9','M','2012I',90);\r\nINSERT INTO INSCRITOS VALUES ('R12','A5','M1','P3','M','2010I',78);\r\nINSERT INTO INSCRITOS VALUES ('R13','A5','M5','P3','V','2010II',86);\r\nINSERT INTO INSCRITOS VALUES ('R14','A5','M7','P9','V','2011II',76);\r\nINSERT INTO INSCRITOS VALUES ('R15','A5','M9','P5','V','2011I',92);\r\nINSERT INTO INSCRITOS VALUES ('R16','A7','M3','P4','M','2011II',78);\r\nINSERT INTO INSCRITOS VALUES ('R17','A7','M2','P8','M','2011I',86);\r\nINSERT INTO INSCRITOS VALUES ('R18','A7','M5','P7','M','2011I',94);";

            tokens = Analisis.Analizador(txtEntrada, DgvLexica, txtError, DtTablas, DtAtributos, DtRestricciones);
            Analisis.LLENADOTABLASPAPU(DtTablas, DtAtributos, DtRestricciones,tokens);

            txtEntrada.Text = "CREATE TABLE DEPARTAMENTOS( \r\nD# CHAR(2) NOT NULL,\r\nDNOMBRE CHAR(6) NOT NULL,\r\nCONSTRAINT PK_DEPARTAMENTOS PRIMARY KEY (D#));\r\n\r\nINSERT INTO DEPARTAMENTOS VALUES ('D1','CIECOM');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D2','CIETIE');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D3','CIEING');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D4','CIEECO');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D5','CIEBAS');\r\n\r\nCREATE TABLE CARRERAS(\r\nC# CHAR(2) NOT NULL,\r\nCNOMBRE CHAR(3) NOT NULL,\r\nVIGENCIA CHAR(4) NOT NULL,\r\nSEMESTRES NUMERIC(2) NOT NULL,\r\nD# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_CARRERAS PRIMARY KEY (C#),\r\nCONSTRAINT FK_CARRERAS FOREIGN KEY (D#) REFERENCES DEPARTAMENTOS(D#));\r\n\r\nINSERT INTO CARRERAS VALUES ('C1','ISC','2010',9,'D1');\r\nINSERT INTO CARRERAS VALUES ('C2','LIN','2008',9,'D1');\r\nINSERT INTO CARRERAS VALUES ('C3','ICI','2009',10,'D2');\r\nINSERT INTO CARRERAS VALUES ('C4','IIN','2010',8,'D3');\r\nINSERT INTO CARRERAS VALUES ('C5','LAE','2011',8,'D4');\r\nINSERT INTO CARRERAS VALUES ('C6','ARQ','2010',10,'D2');\r\n\r\nCREATE TABLE ALUMNOS(\r\nA# CHAR(2) NOT NULL,\r\nANOMBRE CHAR(20) NOT NULL,\r\nGENERACION CHAR(4) NOT NULL,\r\nSEXO CHAR(1) NOT NULL,\r\nC# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_ALUMNOS PRIMARY KEY(A#),\r\nCONSTRAINT FK_ALUMNOS FOREIGN KEY(C#)REFERENCES CARRERAS(C#));\r\n\r\nINSERT INTO ALUMNOS VALUES ('A1','ALBA JESSICA','2009','F','C1');\r\nINSERT INTO ALUMNOS VALUES ('A2','CARREY JIM','2010','M','C2');\r\nINSERT INTO ALUMNOS VALUES ('A3','JOLIE ANGELINE','2011','F','C1');\r\nINSERT INTO ALUMNOS VALUES ('A4','SMITH WILL','2012','M','C6');\r\nINSERT INTO ALUMNOS VALUES ('A5','MESSI LIONEL','2010','M','C1');\r\nINSERT INTO ALUMNOS VALUES ('A6','ESPINOZA PAOLA','2012','F','C6');\r\nINSERT INTO ALUMNOS VALUES ('A7','BLANCO CUAHTEMOC','2009','M','C5');\r\nINSERT INTO ALUMNOS VALUES ('A8','SANCHEZ HUGO','2010','M','C4');\r\n\r\nCREATE TABLE MATERIAS(\r\nM# CHAR(2) NOT NULL,\r\nMNOMBRE CHAR(6) NOT NULL,\r\nCREDITOS NUMERIC(2) NOT NULL,\r\nC# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_MATERIAS PRIMARY KEY (M#),\r\nCONSTRAINT FK_MATERIAS FOREIGN KEY (C#) REFERENCES CARRERAS(C#));\r\n\r\nINSERT INTO MATERIAS VALUES ('M1','ESTDAT',10,'C1');\r\nINSERT INTO MATERIAS VALUES ('M2','FUNPRO',8,'C2');\r\nINSERT INTO MATERIAS VALUES ('M3','CIRCUI',10,'C1');\r\nINSERT INTO MATERIAS VALUES ('M4','PAISAJ',10,'C6');\r\nINSERT INTO MATERIAS VALUES ('M5','BASDAT',8,'C1');\r\nINSERT INTO MATERIAS VALUES ('M6','PROADM',6,'C5');\r\nINSERT INTO MATERIAS VALUES ('M7','CONTAB',6,'C5');\r\nINSERT INTO MATERIAS VALUES ('M8','ECONOM',8,'C2');\r\nINSERT INTO MATERIAS VALUES ('M9','RESMAT',10,'C3');\r\n\r\nCREATE TABLE PROFESORES(\r\nP# CHAR(2) NOT NULL, PNOMBRE CHAR(20) NOT NULL, EDAD NUMERIC(2) NOT NULL,\r\nSEXO CHAR(1)NOT NULL,\r\nESP CHAR(4) NOT NULL,\r\nGRADO CHAR(3) NOT NULL,\r\nD# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_PROFESORES PRIMARY KEY (P#),\r\nCONSTRAINT FK_PROFESORES FOREIGN KEY (D#) REFERENCES DEPARTAMENTOS(D#));\r\n\r\nINSERT INTO PROFESORES VALUES ('P1','DA VINCI LEONARDO',60,'M','PINT','LIC', 'D2');\r\nINSERT INTO PROFESORES VALUES ('P2','ARQUIMIDES',65,'M','QUIM','MAE','D3');\r\nINSERT INTO PROFESORES VALUES ('P3','TURING ALAN',43,'M','COMP','DOC','D1');\r\nINSERT INTO PROFESORES VALUES ('P4','EINSTEIN ALBERT',58,'M','GENI','DOC','D1');\r\nINSERT INTO PROFESORES VALUES ('P5','CURIE MARIE',45,'F','QUIM','LIC','D3');\r\nINSERT INTO PROFESORES VALUES ('P6','HAWKING WILLIAM',52,'M','FISI','DOC','D4');\r\nINSERT INTO PROFESORES VALUES ('P7','VON NEWMAN JOHN',47,'M','COMP','MAE','D1');\r\nINSERT INTO PROFESORES VALUES ('P8','NEWTON ISAAC',36,'M','FISI','LIC','D3');\r\nINSERT INTO PROFESORES VALUES ('P9','THATCHER MARGARET',64,'F','COMP','MAE','D1');\r\n\r\nCREATE TABLE INSCRITOS(\r\nR# CHAR(3) NOT NULL,\r\nA# CHAR(2) NOT NULL,\r\nM# CHAR(2) NOT NULL,\r\nP# CHAR(2) NOT NULL,\r\nTURNO CHAR(1) NOT NULL,\r\nSEMESTRE CHAR(6) NOT NULL,\r\nCALIFICACION NUMERIC(3) NOT NULL,\r\nCONSTRAINT PK_INSCRITOS PRIMARY KEY (R#),\r\nCONSTRAINT FK_INSCRITOS_01 FOREIGN KEY (A#) REFERENCES ALUMNOS(A#),\r\nCONSTRAINT FK_INSCRITOS_02 FOREIGN KEY (M#) REFERENCES MATERIAS(M#),\r\nCONSTRAINT FK_INSCRITOS_03 FOREIGN KEY (P#) REFERENCES PROFESORES(P#));\r\n\r\nINSERT INTO INSCRITOS VALUES ('R01','A1','M1','P3','M','2010I',60);\r\nINSERT INTO INSCRITOS VALUES ('R02','A1','M5','P4','M','2011I',75);\r\nINSERT INTO INSCRITOS VALUES ('R03','A1','M2','P3','V','2010I',78);\r\nINSERT INTO INSCRITOS VALUES ('R04','A1','M5','P4','M','2011II',80);\r\nINSERT INTO INSCRITOS VALUES ('R05','A2','M3','P6','V','2010I',86);\r\nINSERT INTO INSCRITOS VALUES ('R06','A2','M4','P7','V','2010I',90);\r\nINSERT INTO INSCRITOS VALUES ('R07','A3','M1','P2','M','2011I',70);\r\nINSERT INTO INSCRITOS VALUES ('R08','A3','M5','P9','V','2011II',82);\r\nINSERT INTO INSCRITOS VALUES ('R09','A4','M6','P6','M','2010I',90);\r\nINSERT INTO INSCRITOS VALUES ('R10','A4','M4','P9','V','2011II',88);\r\nINSERT INTO INSCRITOS VALUES ('R11','A4','M7','P9','M','2012I',90);\r\nINSERT INTO INSCRITOS VALUES ('R12','A5','M1','P3','M','2010I',78);\r\nINSERT INTO INSCRITOS VALUES ('R13','A5','M5','P3','V','2010II',86);\r\nINSERT INTO INSCRITOS VALUES ('R14','A5','M7','P9','V','2011II',76);\r\nINSERT INTO INSCRITOS VALUES ('R15','A5','M9','P5','V','2011I',92);\r\nINSERT INTO INSCRITOS VALUES ('R16','A7','M3','P4','M','2011II',78);\r\nINSERT INTO INSCRITOS VALUES ('R17','A7','M2','P8','M','2011I',86);\r\nINSERT INTO INSCRITOS VALUES ('R18','A7','M5','P7','M','2011I',94);";
            Analisis.consultaSQL(dataGridView1, txtEntrada, txtError);
            txtEntrada.Text = "";
            txtError.BackColor = Color.White;
            txtError.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cmbEjemplo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEjemplo.SelectedIndex == 0)
            {
                txtEntrada.Text = "CREATE TABLE DEPARTAMENTOS( \r\nD# CHAR(2) NOT NULL,\r\nDNOMBRE CHAR(6) NOT NULL,\r\nCONSTRAINT PK_DEPARTAMENTOS PRIMARY KEY (D#));\r\n\r\nINSERT INTO DEPARTAMENTOS VALUES ('D1','CIECOM');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D2','CIETIE');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D3','CIEING');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D4','CIEECO');\r\nINSERT INTO DEPARTAMENTOS VALUES ('D5','CIEBAS');\r\n\r\nCREATE TABLE CARRERAS(\r\nC# CHAR(2) NOT NULL,\r\nCNOMBRE CHAR(3) NOT NULL,\r\nVIGENCIA CHAR(4) NOT NULL,\r\nSEMESTRES NUMERIC(2) NOT NULL,\r\nD# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_CARRERAS PRIMARY KEY (C#),\r\nCONSTRAINT FK_CARRERAS FOREIGN KEY (D#) REFERENCES DEPARTAMENTOS(D#));\r\n\r\nINSERT INTO CARRERAS VALUES ('C1','ISC','2010',9,'D1');\r\nINSERT INTO CARRERAS VALUES ('C2','LIN','2008',9,'D1');\r\nINSERT INTO CARRERAS VALUES ('C3','ICI','2009',10,'D2');\r\nINSERT INTO CARRERAS VALUES ('C4','IIN','2010',8,'D3');\r\nINSERT INTO CARRERAS VALUES ('C5','LAE','2011',8,'D4');\r\nINSERT INTO CARRERAS VALUES ('C6','ARQ','2010',10,'D2');\r\n\r\nCREATE TABLE ALUMNOS(\r\nA# CHAR(2) NOT NULL,\r\nANOMBRE CHAR(20) NOT NULL,\r\nGENERACION CHAR(4) NOT NULL,\r\nSEXO CHAR(1) NOT NULL,\r\nC# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_ALUMNOS PRIMARY KEY(A#),\r\nCONSTRAINT FK_ALUMNOS FOREIGN KEY(C#)REFERENCES CARRERAS(C#));\r\n\r\nINSERT INTO ALUMNOS VALUES ('A1','ALBA JESSICA','2009','F','C1');\r\nINSERT INTO ALUMNOS VALUES ('A2','CARREY JIM','2010','M','C2');\r\nINSERT INTO ALUMNOS VALUES ('A3','JOLIE ANGELINE','2011','F','C1');\r\nINSERT INTO ALUMNOS VALUES ('A4','SMITH WILL','2012','M','C6');\r\nINSERT INTO ALUMNOS VALUES ('A5','MESSI LIONEL','2010','M','C1');\r\nINSERT INTO ALUMNOS VALUES ('A6','ESPINOZA PAOLA','2012','F','C6');\r\nINSERT INTO ALUMNOS VALUES ('A7','BLANCO CUAHTEMOC','2009','M','C5');\r\nINSERT INTO ALUMNOS VALUES ('A8','SANCHEZ HUGO','2010','M','C4');\r\n\r\nCREATE TABLE MATERIAS(\r\nM# CHAR(2) NOT NULL,\r\nMNOMBRE CHAR(6) NOT NULL,\r\nCREDITOS NUMERIC(2) NOT NULL,\r\nC# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_MATERIAS PRIMARY KEY (M#),\r\nCONSTRAINT FK_MATERIAS FOREIGN KEY (C#) REFERENCES CARRERAS(C#));\r\n\r\nINSERT INTO MATERIAS VALUES ('M1','ESTDAT',10,'C1');\r\nINSERT INTO MATERIAS VALUES ('M2','FUNPRO',8,'C2');\r\nINSERT INTO MATERIAS VALUES ('M3','CIRCUI',10,'C1');\r\nINSERT INTO MATERIAS VALUES ('M4','PAISAJ',10,'C6');\r\nINSERT INTO MATERIAS VALUES ('M5','BASDAT',8,'C1');\r\nINSERT INTO MATERIAS VALUES ('M6','PROADM',6,'C5');\r\nINSERT INTO MATERIAS VALUES ('M7','CONTAB',6,'C5');\r\nINSERT INTO MATERIAS VALUES ('M8','ECONOM',8,'C2');\r\nINSERT INTO MATERIAS VALUES ('M9','RESMAT',10,'C3');\r\n\r\nCREATE TABLE PROFESORES(\r\nP# CHAR(2) NOT NULL, PNOMBRE CHAR(20) NOT NULL, EDAD NUMERIC(2) NOT NULL,\r\nSEXO CHAR(1)NOT NULL,\r\nESP CHAR(4) NOT NULL,\r\nGRADO CHAR(3) NOT NULL,\r\nD# CHAR(2) NOT NULL,\r\nCONSTRAINT PK_PROFESORES PRIMARY KEY (P#),\r\nCONSTRAINT FK_PROFESORES FOREIGN KEY (D#) REFERENCES DEPARTAMENTOS(D#));\r\n\r\nINSERT INTO PROFESORES VALUES ('P1','DA VINCI LEONARDO',60,'M','PINT','LIC', 'D2');\r\nINSERT INTO PROFESORES VALUES ('P2','ARQUIMIDES',65,'M','QUIM','MAE','D3');\r\nINSERT INTO PROFESORES VALUES ('P3','TURING ALAN',43,'M','COMP','DOC','D1');\r\nINSERT INTO PROFESORES VALUES ('P4','EINSTEIN ALBERT',58,'M','GENI','DOC','D1');\r\nINSERT INTO PROFESORES VALUES ('P5','CURIE MARIE',45,'F','QUIM','LIC','D3');\r\nINSERT INTO PROFESORES VALUES ('P6','HAWKING WILLIAM',52,'M','FISI','DOC','D4');\r\nINSERT INTO PROFESORES VALUES ('P7','VON NEWMAN JOHN',47,'M','COMP','MAE','D1');\r\nINSERT INTO PROFESORES VALUES ('P8','NEWTON ISAAC',36,'M','FISI','LIC','D3');\r\nINSERT INTO PROFESORES VALUES ('P9','THATCHER MARGARET',64,'F','COMP','MAE','D1');\r\n\r\nCREATE TABLE INSCRITOS(\r\nR# CHAR(3) NOT NULL,\r\nA# CHAR(2) NOT NULL,\r\nM# CHAR(2) NOT NULL,\r\nP# CHAR(2) NOT NULL,\r\nTURNO CHAR(1) NOT NULL,\r\nSEMESTRE CHAR(6) NOT NULL,\r\nCALIFICACION NUMERIC(3) NOT NULL,\r\nCONSTRAINT PK_INSCRITOS PRIMARY KEY (R#),\r\nCONSTRAINT FK_INSCRITOS_01 FOREIGN KEY (A#) REFERENCES ALUMNOS(A#),\r\nCONSTRAINT FK_INSCRITOS_02 FOREIGN KEY (M#) REFERENCES MATERIAS(M#),\r\nCONSTRAINT FK_INSCRITOS_03 FOREIGN KEY (P#) REFERENCES PROFESORES(P#));\r\n\r\nINSERT INTO INSCRITOS VALUES ('R01','A1','M1','P3','M','2010I',60);\r\nINSERT INTO INSCRITOS VALUES ('R02','A1','M5','P4','M','2011I',75);\r\nINSERT INTO INSCRITOS VALUES ('R03','A1','M2','P3','V','2010I',78);\r\nINSERT INTO INSCRITOS VALUES ('R04','A1','M5','P4','M','2011II',80);\r\nINSERT INTO INSCRITOS VALUES ('R05','A2','M3','P6','V','2010I',86);\r\nINSERT INTO INSCRITOS VALUES ('R06','A2','M4','P7','V','2010I',90);\r\nINSERT INTO INSCRITOS VALUES ('R07','A3','M1','P2','M','2011I',70);\r\nINSERT INTO INSCRITOS VALUES ('R08','A3','M5','P9','V','2011II',82);\r\nINSERT INTO INSCRITOS VALUES ('R09','A4','M6','P6','M','2010I',90);\r\nINSERT INTO INSCRITOS VALUES ('R10','A4','M4','P9','V','2011II',88);\r\nINSERT INTO INSCRITOS VALUES ('R11','A4','M7','P9','M','2012I',90);\r\nINSERT INTO INSCRITOS VALUES ('R12','A5','M1','P3','M','2010I',78);\r\nINSERT INTO INSCRITOS VALUES ('R13','A5','M5','P3','V','2010II',86);\r\nINSERT INTO INSCRITOS VALUES ('R14','A5','M7','P9','V','2011II',76);\r\nINSERT INTO INSCRITOS VALUES ('R15','A5','M9','P5','V','2011I',92);\r\nINSERT INTO INSCRITOS VALUES ('R16','A7','M3','P4','M','2011II',78);\r\nINSERT INTO INSCRITOS VALUES ('R17','A7','M2','P8','M','2011I',86);\r\nINSERT INTO INSCRITOS VALUES ('R18','A7','M5','P7','M','2011I',94);";
            }
            if (cmbEjemplo.SelectedIndex == 1)
            {
                txtEntrada.Text = "SELECT NOMBRE\r\nFROM ALUMNOS,INSCRITOS\r\nWHERE ALUMNOS.A#=INSCRITOS.A# AND\r\nINSCRITOS.SEMESTRE='2010I'";
            }
            if (cmbEjemplo.SelectedIndex == 2)
            {
                txtEntrada.Text = "SELECT A#, ANOMBRE\r\nFROM ALUMNOS,INSCRITOS\r\nWHERE ALUMNOS.A#=INSCRITOS.A# AND INSCRITOS.SEMESTRE='2010I'";
            }
            if (cmbEjemplo.SelectedIndex == 3)
            {
                txtEntrada.Text = "SELECT DISTINCT ANOMBRE\r\nFROM ALUMNO,INSCRITOS,CARRERAS\r\nWHERE ALUMNOS.A#=INSCRITOS.A# AND ALUMNOS.C#=CARRERAS.C#\r\nAND INSCRITOS.SEMESTRE='2010I'\r\nAND CARRERAS.CNOMBRE='ISC'\r\nAND ALUMNOS.GENERACION='2010'";
            }
            if (cmbEjemplo.SelectedIndex == 4)
            {
                txtEntrada.Text = "SELECT ANOMBRE\r\nFROM ALUMNOS,INSCRITOS,CARRERAS\r\nWHERE ALUMNOS.D#=INSCRITOS.A#\r\nAND ALUMNOS.C#=CARRERAS.C#\r\nAND INSCRITOS.SEMESTRE='2010I'\r\nAND CARRERAS.CNOMBRE='ISC'\r\nAND ALUMNOS.GENERACION='2010'";
            }
            if (cmbEjemplo.SelectedIndex == 5)
            {
                txtEntrada.Text = "SELECT ANOMBRE\r\nFROM ALUMNOS A,INSCRITOS I,CARRERAS C\r\nWHERE A.A#=I.C# AND A.C#=C.C# AND I.SEMESTRE='2010I'\r\nAND C.CNOMBRE='ISC' AND A.GENERACION='2010'";
            }
            if (cmbEjemplo.SelectedIndex == 6)
            {
                txtEntrada.Text = "SELECT DISTINCT ANOMBRE\r\nFROM ALUMNOS,INSCRITOS,CARRERAS\r\nWHERE ALUMNOS.A#=INSCRITOS.A# AND ALUMNO.C#=CARRERAS.C#\r\nAND INSCRITOS.SEMESTRE='2010I'\r\nAND CARRERAS.CNOMBRE='ISC'\r\nAND ALUMNOS.GENERACION='2010'";
            }
            if (cmbEjemplo.SelectedIndex == 7)
            {
                txtEntrada.Text = "SELECT A#,ANOMBRE\r\nFROM ALUMNOS\r\nWHERE C# IN (SELECT Z#\r\nFROM CARRERAS\r\nWHERE SEMESTRES=9)\r\nAND A# IN (SELECT A#\r\nFROM INSCRITOS\r\nWHERE SEMESTRE='2010I')";
            }
            if (cmbEjemplo.SelectedIndex == 8)
            {
                txtEntrada.Text = "SELECT ANOMBRE\r\nFROM ALUMNOS\r\nWHERE A# IN (SELECT A#\r\nFROM INSCRITOS\r\nWHERE SEMESTRE='2010I'\r\nAND M# IN (SELECT M#\r\nFROM MATERIAS\r\nWHERE MNOMBRE=ESTDAT))";
            }
            if (cmbEjemplo.SelectedIndex == 9)
            {
                txtEntrada.Text = "SELECT A#,ANOMBRE\r\nFROM ALUMNOS\r\nWHERE C# IN (SELECT C#\r\nFROM CARRERAS\r\nWHERE SEMESTRES=9)\r\nAND A# IN (SELECT A#\r\nFROM INSCRITOS\r\nWHERE SEMESTRE=2010)";
            }

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            //DgvConstantes.Rows.Clear();
            //DgvIdentificadores.Rows.Clear();
            DgvLexica.Rows.Clear();
            txtError.Text = "";
            txtEntrada.Text = "";
            txtError.BackColor = Color.White;
        }

    }

}
