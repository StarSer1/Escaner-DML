using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escaner_DML
{
    public class Analisis
    {
        Regex reservadas = new Regex(@"\b(SELECT|FROM|WHERE|IN|AND|OR|CREATE|TABLE|CHAR|NUMERIC|NOT|NULL|
        CONSTRAINT|KEY|PRIMARY|FOREIGN|REFERENCES|INSERT|INTO|VALUES)\b");
        Regex delimitadores = new Regex(@"[.,()'’‘;]");
        Regex operadores = new Regex(@"[+\-*/]");
        Regex relacionales = new Regex(@"(>=|<=|>|<|=)");
        Regex constantesTL = new Regex(@"\b\d+\b");
        Regex constantes = new Regex(@"'([^']*)'");
        public bool errorActivado = false;
        private SqlConnection sqlConnection;
        private SqlDataAdapter sqlDataAdapter;
        private DataSet dataSet;

        public Analisis(bool error,
            List<(int noTabla, string nombreTabla, int cantidadAtributos, int cantidadRestricciones)> tablas2,
            List<(int noTabla, int noAtributo, string nombreAtributo, string tipo, int longitud, int noNull, int noAtributoTabla)> atributos2,
            List<(int noTabla, int noRestriccion, int tipo, string nombreRestriccion, int atributoAsociado, int tabla, int atributo)> restricciones2)

        {
            this.errorActivado = error;
            this.tablas = tablas2;
            this.atributos = atributos2;
            this.restricciones = restricciones2;
            string connectionString = @"Data Source=DESKTOP-GQ6Q9HG\SQLEXPRESS;Initial Catalog=Escuela;Integrated Security=True;"; // Aquí debes colocar tu cadena de conexión
            sqlConnection = new SqlConnection(connectionString);
            sqlDataAdapter = new SqlDataAdapter();
            dataSet = new DataSet();
        }
        public List<string> Analizador2(RichTextBox texto, DataGridView dgvLex, TextBox txtError, DataGridView dgvTabla, DataGridView dgvAtributos, DataGridView dgvRestriccion)
        {
            string cadena = "";
            int linea = 1;
            texto.Text = texto.Text.ToUpper() + " ";
            bool comillas = false;
            bool sigo = false;
            bool sigoRelacional = false;
            for (int i = 0; i < texto.TextLength; i++)
            {
                string c = texto.Text[i].ToString();
                if (c == "\n")
                {

                }
                if (!delimitadores.IsMatch(c))
                {
                    if (sigoRelacional)
                    {
                        if (relacionales.IsMatch(c) && c != " ")
                        {
                            cadena += c;
                            tokens.Add(cadena);
                            if (cadena != "")
                                MostrarDgv(dgvLex, tokens.Last(), linea);
                            cadena = "";
                            c = "";
                        }
                        sigoRelacional = false;
                    }
                    if (c != " " && !relacionales.IsMatch(c) && !constantesTL.IsMatch(c))
                    {
                        if (c != "\n")
                            cadena += c;
                    }
                    if (relacionales.IsMatch(c))
                    {
                        if (c != " ")
                        {
                            tokens.Add(cadena);
                            if (cadena != "")
                                MostrarDgv(dgvLex, tokens.Last(), linea);
                            cadena = c;
                            if (i + 1 < texto.TextLength)
                            {
                                char siguienteChar = texto.Text[i + 1];

                                if (relacionales.IsMatch(siguienteChar.ToString()))
                                {
                                    sigoRelacional = true;
                                }
                                else
                                {
                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvLex, tokens.Last(), linea);
                                    cadena = "";
                                }
                            }
                        }
                    }
                    if (constantesTL.IsMatch(c))
                    {
                        if (i + 1 < texto.TextLength)
                        {
                            char siguienteChar = texto.Text[i + 1];

                            //si el siguiente es espacio en blanco
                            if (siguienteChar.ToString() == " ")
                            {
                                cadena += c;
                                tokens.Add(cadena);
                                if (cadena != "")
                                    MostrarDgv(dgvLex, tokens.Last(), linea);
                                cadena = "";
                            }
                            else if (constantesTL.IsMatch(siguienteChar.ToString()) || char.IsLetter(siguienteChar))
                            {
                                cadena += c;
                            }
                            else if (delimitadores.IsMatch(siguienteChar.ToString()))
                            {
                                cadena += c;
                            }
                        }
                    }
                    if (c == " " || c == "\n")
                    {
                        if (empezoComilla == false)
                        {
                            if (reservadas.IsMatch(cadena))
                            {
                                tokens.Add(cadena);
                                //tokens.Add(c);
                                if (cadena != "")
                                    MostrarDgv(dgvLex, tokens.Last(), linea);
                                cadena = "";
                            }
                            else if (tokens.Count != 0)
                            {
                                if (reservadas.IsMatch(tokens.Last()) && cadena != "")
                                {
                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvLex, tokens.Last(), linea);
                                    sigo = true;
                                    cadena = "";

                                }
                                else if (relacionales.IsMatch(tokens.Last()) || relacionales.IsMatch(cadena))
                                {
                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvLex, tokens.Last(), linea);
                                    cadena = "";

                                }
                                else if (delimitadores.IsMatch(tokens.Last()))
                                {
                                    if (c == "(")
                                        acumuladorParentesisAbierto++;
                                    else if (c == ")")
                                        acumuladorParentesisAbierto--;




                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvLex, tokens.Last(), linea);
                                    cadena = "";
                                }
                                else
                                {
                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvLex, tokens.Last(), linea);
                                    cadena = "";
                                }
                            }
                        }
                        else
                        {
                            cadena += " ";
                        }
                        tokens.Add(c);
                    }
                }
                else
                {
                    if ("'’‘".Contains(c))
                    {
                        if (empezoComilla == false)
                            tokens.Add(c);
                        empezoComilla = !empezoComilla;
                    }
                    if (c == "(")
                        acumuladorParentesisAbierto++;
                    else if (c == ")")
                        acumuladorParentesisAbierto--;

                    if (c == "’" || c == "‘")
                        acumuladorComillas1++;

                    if (c == "'")
                        acumuladorComillas3++;



                    if (comillas == false && Regex.IsMatch(c, @"['’‘]")) comillas = true;
                    else if (cadena != "" && comillas == true)
                    {
                        tokens.Add("'" + cadena + "'");
                        tokens.Add(c);
                        MostrarDgv(dgvLex, tokens.Last() + "~", linea);
                        comillas = false;
                    }
                    else if (cadena != "" && (c == ")" || c == ",") && constantesTL.IsMatch(cadena))
                    {
                        tokens.Add(cadena);
                        MostrarDgv(dgvLex, tokens.Last() + "~", linea);
                        tokens.Add(c);
                        MostrarDgv(dgvLex, c, linea);
                    }
                    else
                    {
                        tokens.Add(cadena);
                        if (tokens.Last() != "")
                            MostrarDgv(dgvLex, tokens.Last(), linea);
                        tokens.Add(c);
                        if (c != "")
                            MostrarDgv(dgvLex, c, linea);
                    }
                    cadena = "";



                }
                tokens.RemoveAll(item => (string.IsNullOrEmpty(item) || item == " "));
                if (c == "\n")
                    linea++;
            }
            //bool errorParentesis = Errores.ErrorParentesis(dgvCons, dgvIden, dgvLex, txtError, acumuladorParentesisAbierto);
            //if (errorComillas || errorParentesis)
            //errorActivado = true;
            //tokens = RemoverDuplicadosVacios(tokens);
            //LLENADOTABLASPAPU(dgvTabla, dgvAtributos, dgvRestriccion,);
            return tokens;
        }
        public void consultaSQL(DataGridView dgvResultados, RichTextBox texto, TextBox error)
        {
            string consultaSql = texto.Text;

            try
            {
                // Abrir la conexión
                sqlConnection.Open();

                // Configurar el adaptador y ejecutar la consulta
                sqlDataAdapter.SelectCommand = new SqlCommand(consultaSql, sqlConnection);
                dataSet.Clear(); // Limpiar datos anteriores
                sqlDataAdapter.Fill(dataSet);

                // Mostrar los resultados en el DataGridView
                dgvResultados.DataSource = dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                if (ex.Message == "No se puede encontrar la tabla 0.")
                {

                }
                else
                {
                    error.Text = ex.Message;
                    error.BackColor = Color.FromArgb(255, 137, 137);
                }

            }
            finally
            {
                // Cerrar la conexión
                sqlConnection.Close();
            }
        }

        Stack<string> pila = new Stack<string>();
        string[,] TablaSintacOG =
        {
//             4     8             10           11    12    13    14    15    50    51    52    53    54    61    62    72    99
            { null, null, "10 301 11 306 310", null, null, null, null, null, null, null, null, null, null, null, null, null, null}, // 300
            { "302", null, null, null, null, null, null, null, null, null, null, null, null, null, null, "72", null }, // 301
            { "304 303", null, null, null, null, null, null, null, null, null , null, null, null, null, null, null, null}, // 302 
            { null, null, null, "99", null, null, null, null, "50 302", null, null, null, null, null, null, null,  "99"}, // 303
            { "4 305", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null }, // 304
            { null , "99", null, "99", null, "99", "99", "99", "99", "51 4", null, "99", null, null, null, null, "99" }, // 305
            { "308 307 320", null, null, null, null, null, null, null, null, null, "99", null, null, null, null, null, null}, // 306
            {  null, null, null, null, "99", null, null, null, "50 306", null, null, "99", null, null, null, null, "99"},
            { "4 309", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null},
            { "4", null, null, null, "99", null, null, null, "99", null, null, "99", null, null, null, null, "99"},
            { null, null, null, null, "12 311", null, null, null, null, null, null, "99", null, null, null, null, "99" },
            { "313 312", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, "317 311", "317 311", null, null, null, "99", null, null, null, null, "99" },
            { "304 314", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
            { null, "315 316", null, null, null, "13 52 300 53", null, null, null, null, null, null, null, null, null, null, null },
            { null, "8", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
            { "304", null, null, null, null, null, null, null, null, null, null, null, "54 318 54", "319", null, null, null },
            { null, null, null, null, null, null, "14", "15", null, null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, "62", null, null },
            { null, null, null, null, null, null, null, null, null, null, null, null, null, "61", null, null, null },
            // 4    8     10    11    12
        };
        string[,] TablaSintacOG2 =
        {
            // 4     8     10    11    12    13    14    15                         16    18    19    20    22    24    25    26    27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, "16 17 4 52 202 53 55 201", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null}, //200
            // 4     8     10    11    12    13    14    15     16    18    19    20    22    24    25    26     27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, "200", null, null, null, null, null, null, null, "211", null, null, null, null, null, null, null, null, "99"}, //201
            //                     4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27    50    51    52    53    54    61    62    72    99
            { "4 203 52 61 53 204 205", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null}, //202
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, "18", "19", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null}, //203
            // 4     8     10    11    12    13    14    15    16    18    19       20    22    24    25    26    27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, "20 21", null, null, null, null, null, "99", null, null, null, null, null, null, null, null}, //204
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27        50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "50 206", null, null, "99", null, null, null, null, null}, //205
            //  4     8     10    11    12    13    14    15    16    18    19    20     22    24    25    26    27    50    51    52    53    54    61    62    72    99
            { "202", null, null, null, null, null, null, null, null, null, null, null, "207", null, null, null, null, null, null, null, null, null, null, null, null, null}, //206
            // 4     8     10    11    12    13    14    15    16    18    19    20                      22    24    25    26    27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, "22 4 208 52 4 53 209", null, null, null, null, null, null, null, null, null, null, null, null, null}, //207
            // 4     8     10    11    12    13    14    15    16    18    19    20    22       24       25    26    27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, "24 23", "25 23", null, null, null, null, null, null, null, null, null, null, null}, //208
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25                  26    27        50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "26 4 52 4 53 210", null, "50 207", null, null, "99", null, null, null, null, null}, //209
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27        50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "50 207", null, null, "99", null, null, null, null, null}, //210
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26                             27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "27 28 4 29 52 212 53 55 215", null, null, null, null, null, null, null, null, null}, //211
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27    50    51    52    53         54         61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "213 214", "213 214", null, null, null}, //212
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27    50    51    52    53          54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "54 62 54", "61", null, null, null}, //213
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27        50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "50 212", null, null, "99", null, null, null, null, null}, //214
            // 4     8     10    11    12    13    14    15     16    18    19    20    22    24    25    26     27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, "200", null, null, null, null, null, null, null, "211", null, null, null, null, null, null, null, null, "99"}, //215
            { null, null, "10 301 11 306 310", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null}, // 300
            { "302", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "72", null }, // 301
            { "304 303", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null , null, null, null, null, null, null, null}, // 302 
            { null, null, null, "99", null, null, null, null, null, null, null, null, null, null, null, null, null, "50 302", null, null, null, null, null, null, null,  "99"}, // 303
            { "4 305", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null }, // 304
            { null , "99", null, "99", null, "99", "99", "99", null, null, null, null, null, null, null, null, null, "99", "51 4", null, "99", null, null, null, null, "99" }, // 305
            { "308 307", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "99", null, null, null, null, null, null}, // 306
            {  null, null, null, null, "99", null, null, null, null, null, null, null, null, null, null, null, null, "50 306", null, null, "99", null, null, null, null, "99"},
            { "4 309", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null},
            { "4", null, null, null, "99", null, null, null, null, null, null, null, null, null, null, null, null, "99", null, null, "99", null, null, null, null, "99"},
            { null, null, null, null, "12 311", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "99", null, null, null, null, "99" },
            { "313 312", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, "317 311", "317 311", null, null, null, null, null, null, null, null, null, null, null, null, "99", null, null, null, null, "99" },
            { "304 314", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
            { null, "315 316", null, null, null, "13 52 300 53", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
            { null, "8", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
            { "304", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "54 318 54", "319", null, null, null },
            { null, null, null, null, null, null, "14", "15", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "62", null, null },
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "61", null, null, null }
            // 4    8     10    11    12
        };
        string[,] TablaSintac =
        {
            // 4     8     10    11    12    13    14    15                         16    18    19    20    22    24    25    26    27    50    51    52    53    54    61    62    72    99, 30
            { null, null, null, null, null, null, null, null, "16 17 4 700 52 202 53 55 201", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null}, //200
            // 4     8     10    11    12    13    14    15     16    18    19    20    22    24    25    26     27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, "200", null, null, null, null, null, null, null, "211", null, null, null, null, null, null, null, null, "99", null}, //201
            //                     4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27    50    51    52    53    54    61    62    72    99
            { "4 701 203 52 61 53 204 205", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null}, //202
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, "18", "19", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null}, //203
            // 4     8     10    11    12    13    14    15    16    18    19       20    22    24    25    26    27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, "20 21", null, null, null, null, null, "99", null, null, null, null, null, null, null, null, null}, //204
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27        50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "50 206", null, null, "99", null, null, null, null, null, null}, //205
            //  4     8     10    11    12    13    14    15    16    18    19    20     22    24    25    26    27    50    51    52    53    54    61    62    72    99
            { "202", null, null, null, null, null, null, null, null, null, null, null, "207", null, null, null, null, null, null, null, null, null, null, null, null, null, null}, //206
            // 4     8     10    11    12    13    14    15    16    18    19    20                      22    24    25    26    27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, "22 4 703 208 52 4 702 53 209", null, null, null, null, null, null, null, null, null, null, null, null, null, null}, //207
            // 4     8     10    11    12    13    14    15    16    18    19    20    22       24       25    26    27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, "24 23", "25 23", null, null, null, null, null, null, null, null, null, null, null, null}, //208
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25                  26    27        50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "26 4 709 52 4 708 53 210", null, "50 207", null, null, "99", null, null, null, null, null, null}, //209
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27        50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "50 207", null, null, "99", null, null, null, null, null, null}, //210
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26                             27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "27 28 4 706 29 52 212 53 705 55 215", null, null, null, null, null, null, null, null, null, null}, //211
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27    50    51    52    53         54         61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "213 214", "213 214", null, null, null, null}, //212
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27    50    51    52    53          54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "54 62 54 707", "61", null, null, null, null}, //213
            // 4     8     10    11    12    13    14    15    16    18    19    20    22    24    25    26    27        50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "50 212", null, null, "99", null, null, null, null, null, null}, //214
            // 4     8     10    11    12    13    14    15     16    18    19    20    22    24    25    26     27    50    51    52    53    54    61    62    72    99
            { null, null, null, null, null, null, null, null, "200", null, null, null, null, null, null, null, "211", null, null, null, null, null, null, null, null, "99", null}, //215
            { null, null, "10 320 301 11 306 310", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null}, // 300
            { "302", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "72", null , null}, // 301
            { "304 303", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null , null, null, null, null, null, null, null, null}, // 302 
            { null, null, null, "99", null, null, null, null, null, null, null, null, null, null, null, null, null, "50 302", null, null, null, null, null, null, null,  "99", null}, // 303
            { "4 800 305", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null , null}, // 304
            { null , "99", null, "99", null, "99", "99", "99", null, null, null, null, null, null, null, null, null, "99", "51 4 801", null, "99", null, null, null, null, "99" , null}, // 305
            { "308 307", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "99", null, null, null, null, null, null, null}, // 306
            {  null, null, null, null, "99", null, null, null, null, null, null, null, null, null, null, null, null, "50 306", null, null, "99", null, null, null, null, "99", null},
            { "4 803 309", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null},
            { "4", null, null, null, "99", null, null, null, null, null, null, null, null, null, null, null, null, "99", null, null, "99", null, null, null, null, "99", null},
            { null, null, null, null, "12 311", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "99", null, null, null, null, "99" , null},
            { "313 312", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null , null},
            { null, null, null, null, null, null, "317 311", "317 311", null, null, null, null, null, null, null, null, null, null, null, null, "99", null, null, null, null, "99" , null},
            { "304 314", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null , null},
            { null, "315 316 808", null, null, null, "13 52 300 53", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null , null},
            { null, "8", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
            { "304", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "54 318 54", "319", null, null, null , null},
            { null, null, null, null, null, null, "14", "15", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null , null},
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "62", null, null , null},
            { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "61", null, null, null , null},
            { "99",  null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,"30"} // 320
            // 4    8     10    11    12
        };

        int contador = 1;
        int noTabla = 1;
        int noAtributo = 1;
        int noRestriccion = 1;
        int valorIdentificador = 401;
        int valorConstante = 600;
        int acumuladorParentesisAbierto = 0;
        int acumuladorComillas1 = 0;
        int acumuladorComillas2 = 0;
        int acumuladorComillas3 = 0;
        bool empezoComilla = false;
        bool valorAntesN = false;
        Errores Errores = new Errores();

        // En la clase Analisis, agregar esta variable
        private Dictionary<int, List<List<string>>> datosTablas = new Dictionary<int, List<List<string>>>();

        Dictionary<string, int> tablaSimbolos = new Dictionary<string, int>
        {
            // Palabras Reservadas (1)
            { "SELECT", 10 }, { "FROM", 11 }, { "WHERE", 12 }, { "IN", 13 },
            { "AND", 14 }, { "OR", 15 }, { "CREATE", 16 }, { "TABLE", 17 },
            { "CHAR", 18 }, { "NUMERIC", 19 }, { "NOT", 20 }, { "NULL", 21 },
            { "CONSTRAINT", 22 }, { "KEY", 23 }, { "PRIMARY", 24 }, { "FOREIGN", 25 },
            { "REFERENCES", 26 }, { "INSERT", 27 }, { "INTO", 28 }, { "VALUES", 29 },
            { "DISTINCT", 30 },

            // Delimitadores (5)
            { ",", 50 }, { ".", 51 }, { "(", 52 }, { ")", 53 }, { "'", 54 }, {";", 55},

            // Constantes (6)
            { "d", 61 }, { "a", 62 },

            // Operadores (7)
            { "+", 70 }, { "-", 71 }, { "*", 72 }, {"/", 73},

            // Relacionales (8)
            { ">", 81 }, { "<", 82 }, { "=", 83 }, { ">=", 84 }, { "<=", 85 },

            // Espacio en Blanco
            {"$", 199 }
        };
        List<string> tokens = new List<string>();
        List<string> tokensConN = new List<string>();

        int dondevoy = 0;
        string actual = "";
        public string Analizador(RichTextBox texto, DataGridView dgvLex, TextBox txtError, DataGridView dgvTabla, DataGridView dgvAtributos, DataGridView dgvRestriccion)
        {
            string cadena = "";
            int linea = 1;
            texto.Text = texto.Text.ToUpper() + " ";
            bool comillas = false;
            bool sigo = false;
            bool sigoRelacional = false;
            for (int i = 0; i < texto.TextLength; i++)
            {
                string c = texto.Text[dondevoy].ToString();
                if ( c == "\n")
                {
                    valorAntesN = true;
                }
                if (!delimitadores.IsMatch(c))
                {
                    if (sigoRelacional)
                    {
                        if (relacionales.IsMatch(c) && c != " ")
                        {
                            cadena += c;
                            tokens.Add(cadena);
                            if (cadena != "")
                                MostrarDgv(dgvLex, tokens.Last(), linea);
                            cadena = "";
                            c = "";
                        }
                        sigoRelacional = false;
                    }
                    if (c != " " && !relacionales.IsMatch(c) && !constantesTL.IsMatch(c))
                    {
                        if (c != "\n")
                            cadena += c;
                        if(texto.Text[dondevoy+1] == '=')
                        {
                            return cadena;
                        }
                    }
                    if (relacionales.IsMatch(c))
                    {
                        if (c != " ")
                        {
                            tokens.Add(cadena);
                            if (cadena != "")
                                MostrarDgv(dgvLex, tokens.Last(), linea);
                            cadena = c;
                            if (i + 1 < texto.TextLength)
                            {
                                char siguienteChar = texto.Text[dondevoy + 1];

                                if (relacionales.IsMatch(siguienteChar.ToString()))
                                {
                                    sigoRelacional = true;
                                }
                                else
                                {
                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvLex, tokens.Last(), linea);
                                    cadena = "";
                                    return c;
                                }
                            }
                        }
                    }
                    if (constantesTL.IsMatch(c))
                    {
                        if (i + 1 < texto.TextLength)
                        {
                            char siguienteChar = texto.Text[dondevoy + 1];

                            //si el siguiente es espacio en blanco
                            if (siguienteChar.ToString() == " ")
                            {
                                cadena += c;
                                tokens.Add(cadena);
                                if (cadena != "")
                                    MostrarDgv(dgvLex, tokens.Last(), linea);
                                cadena = "";
                            }
                            else if (constantesTL.IsMatch(siguienteChar.ToString()) || char.IsLetter(siguienteChar))
                            {
                                cadena += c;
                            }
                            else if (delimitadores.IsMatch(siguienteChar.ToString()))
                            {
                                cadena += c;
                                if (siguienteChar == ')')
                                {
                                    return cadena;
                                }
                            }
                        }
                    }
                    if (c == " " || c == "\n")
                    {
                        actual = cadena;
                        if (empezoComilla == false)
                        {
                            if (reservadas.IsMatch(cadena))
                            {
                                tokens.Add(cadena);
                                //tokens.Add(c);
                                if (cadena != "")
                                    MostrarDgv(dgvLex, tokens.Last(), linea);
                                cadena = "";
                            }
                            else if (tokens.Count != 0)
                            {
                                if (reservadas.IsMatch(tokens.Last()) && cadena != "")
                                {
                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvLex, tokens.Last(), linea);
                                    sigo = true;
                                    cadena = "";

                                }
                                else if (relacionales.IsMatch(tokens.Last()) || relacionales.IsMatch(cadena))
                                {
                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvLex, tokens.Last(), linea);
                                    cadena = "";

                                }
                                else if (delimitadores.IsMatch(tokens.Last()))
                                {
                                    if (c == "(")
                                        acumuladorParentesisAbierto++;
                                    else if (c == ")")
                                        acumuladorParentesisAbierto--;

                                 


                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvLex, tokens.Last(), linea);
                                    cadena = "";
                                }
                                else
                                {
                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvLex, tokens.Last(), linea);
                                    cadena = "";
                                }
                            }
                            break;
                        }
                        else
                        {
                            cadena += " ";
                        }
                        tokens.Add(c);
                    }
                }
                else
                {
                    if ("'’‘".Contains(c))
                    {
                        if (empezoComilla == false) 
                            tokens.Add(c);
                        empezoComilla = !empezoComilla;
                    }
                    if (c == "(")
                        acumuladorParentesisAbierto++;
                    else if (c == ")")
                        acumuladorParentesisAbierto--;

                    if (c == "’" || c == "‘")
                        acumuladorComillas1++;

                    if (c == "'")
                        acumuladorComillas3++;



                    if (comillas == false && Regex.IsMatch(c, @"['’‘]")) comillas = true;
                    else if (cadena != "" && comillas == true)
                    {
                        tokens.Add("'"+cadena+"'");
                        tokens.Add(c);
                        MostrarDgv(dgvLex, tokens.Last() + "~", linea);
                        comillas = false;
                    }
                    else if (cadena != "" && (c == ")" || c == ",") && constantesTL.IsMatch(cadena))
                    {
                        tokens.Add(cadena);
                        MostrarDgv(dgvLex, tokens.Last() + "~", linea);
                        tokens.Add(c);
                        MostrarDgv(dgvLex, c, linea);
                    }
                    else
                    {
                        tokens.Add(cadena);
                        if (tokens.Last() != "")
                            MostrarDgv(dgvLex, tokens.Last(), linea);
                        tokens.Add(c);
                        if (c != "")
                            MostrarDgv(dgvLex, c, linea);
                        if (cadena == "")
                        {
                            actual = c;
                            if (actual == "(" || actual == ")" || actual == ",")
                            {
                                //dondevoy++;
                                if (char.IsDigit(texto.Text[dondevoy]))
                                {
                                    dondevoy--;
                                }
                            }
                            if (actual == ".")
                            {
                                
                            }
                        }
                        else
                        {
                            actual = cadena;
                            dondevoy--;
                        }
                        
                        
                        break;
                    }
                    cadena = "";



                }
                if (texto.Text[dondevoy+1]== '\'')
                {
                    return cadena;
                }
                if (c == "#")
                {
                    return cadena;
                }
                else if (c== "'")
                {
                    return c;
                }
                tokens.RemoveAll(item => (string.IsNullOrEmpty(item) || item == " "));
                if (c == "\n")
                    linea++;
                dondevoy++;
            }
            //bool errorParentesis = Errores.ErrorParentesis(dgvCons, dgvIden, dgvLex, txtError, acumuladorParentesisAbierto);
            //if (errorComillas || errorParentesis)
            //errorActivado = true;
            //tokens = RemoverDuplicadosVacios(tokens);
            //LLENADOTABLASPAPU(dgvTabla, dgvAtributos, dgvRestriccion,);
                return actual;
        }
        
        public void LlenadoSelects(List<string> tokens)
        {
            bool comenzoSelect = false;
            bool comenzoFrom = false;
            bool comenzoWhere = false;
            int linea = 1;

            List<string> tokens2 = new List<string>();

            tokens2.AddRange(tokens);
            tokens2.RemoveAll(elemento => elemento == "'");
            for (int i = 0; i < tokens2.Count; i++)
            {
                try
                {
                    if (tokens2[i] != "\n")
                    {
                        if (comenzoWhere)
                        {
                            if (tokens2[i + 1] == ".")
                            {
                                string tipo = "";                  
                                if (tokens2[i - 1] != "=" && tokens2[i+3] != "IN")
                                {
                                    if (constantes.IsMatch(tokens2[i + 4]))
                                        tipo = "CHAR";
                                    else if (constantesTL.IsMatch(tokens2[i + 4]))
                                        tipo = "NUMERIC";
                                    else
                                        tipo = "atributo";
                                }
                                listaWhere.Add((tokens2[i], tokens2[i + 2], tipo, linea));
                                i += 2;
                            }
                            else if (tokens2[i + 1] == "=")
                            {
                                string tipo = "";
                                if (tokens2[i - 1] != "=")
                                {
                                    if (constantes.IsMatch(tokens2[i + 2]))
                                        tipo = "CHAR";
                                    else if (constantesTL.IsMatch(tokens2[i + 2]))
                                        tipo = "NUMERIC";
                                    else
                                        tipo = "atributo";
                                }
                                listaWhere.Add(("", tokens2[i], tipo, linea));
                            }
                            //else if (tokens2[i + 1] == "IN")
                            //{
                            //    listaWhere.Add(("", tokens2[i], "", linea));
                            //    i += 2;
                            //}
                        }
                        if (tokens2[i] == "WHERE")
                        {
                            comenzoSelect = false;
                            comenzoFrom = false;
                            comenzoWhere = true;
                        }
                        if (comenzoFrom)
                        {
                            if (!delimitadores.IsMatch(tokens2[i]))
                            {
                                string tempAlias = "";
                                if (!delimitadores.IsMatch(tokens2[i + 1]) && !reservadas.IsMatch(tokens2[i + 1]) && tokens2[i + 1] != "\n")
                                    tempAlias = tokens2[i + 1];
                                listaFrom.Add((tokens2[i], tempAlias, linea));
                                if (tempAlias != "") i++;
                            }
                        }
                        if (tokens2[i] == "FROM")
                        {
                            comenzoSelect = false;
                            comenzoFrom = true;
                        }
                        if (comenzoSelect)
                        {
                            if (tokens[i] != "DISTINCT")
                            {
                                if (!delimitadores.IsMatch(tokens2[i]))
                                {
                                    if (tokens2[i + 1] == ".")
                                    {
                                        listaSelect.Add((tokens2[i], tokens2[i + 2], linea));
                                        i += 2;
                                    }
                                    else
                                        listaSelect.Add(("", tokens2[i], linea));
                                }
                            }
                        }

                        if (tokens2[i] == "SELECT")
                        {
                            comenzoSelect = true;
                        }
                    }
                    else
                        linea++;
                }
                catch
                {

                }
            }
            comenzoWhere = false;
        }
        public void LLENADOTABLASPAPU(
    DataGridView dtTab,
    DataGridView dtAtb,
    DataGridView dtRes,
    List<string> tokens)
        {
            List<string> tokens2 = new List<string>();
            
            int noTablaTemp = 2;
            int noAtributoTemp = 0;
            
            tokens2.AddRange(tokens);
            tokens2.RemoveAll(elemento => elemento == "\n");
            for (int i = 0; i < tokens2.Count; i++)
            {
                try
                {
                    if (tokens2[i] == "INSERT" && tokens2[i + 1] == "INTO")
                    {
                        string nombreTabla = tokens2[i + 2];
                        var tabla = tablas.FirstOrDefault(t => t.nombreTabla == nombreTabla);
                        if (tabla.Equals(default)) continue;

                        // Extraer valores entre VALUES (...) o hasta ;
                        List<string> valores = new List<string>();
                        int j = i + 4; // Saltar "INSERT INTO tabla VALUES ("
                        while (j < tokens2.Count && tokens2[j] != ")")
                        {
                            if (tokens2[j] != "," && tokens2[j] != "(")
                                valores.Add(tokens2[j].Trim('\''));
                            j++;
                        }

                        // Almacenar los datos
                        if (!datosTablas.ContainsKey(tabla.noTabla))
                            datosTablas[tabla.noTabla] = new List<List<string>>();
                        datosTablas[tabla.noTabla].Add(valores);
                    }
                }
                catch { }
            }
            for (int i = 0; i < tokens2.Count; i++)
            {
                try
                {

                        //TABLAS
                        if (tokens2[i] == "CREATE" && tokens2[i + 1] == "TABLE")
                        {
                            string tablaBuscar = tokens2[i + 2];
                            bool existeTabla = tablas.Any(t => t.nombreTabla == tablaBuscar);
                            dtTab.Rows.Add(noTabla, tokens2[i + 2], atributos);
                            tablas.Add((noTabla, tokens2[i + 2], 0, 0));
                            noTabla++;



                        }
                        //ATRIBUTOS
                        else if (delimitadores.IsMatch(tokens2[i - 1]) && (tokens2[i - 1] != ")" && tokens2[i] != ")"))
                        {
                            if ((tokens2[i + 1] == "CHAR" || tokens2[i + 1] == "NUMERIC") && (tokens2[i + 2] == "(" && tokens2[i + 4] == ")"))
                            {
                                if (tokens2[i + 5] == "NOT" && tokens2[i + 6] == "NULL")
                                {
                                    if (noTablaTemp == noTabla)
                                    {
                                        noAtributoTemp++;
                                    }
                                    else
                                    {
                                        noAtributoTemp = 1;
                                        noTablaTemp = noTabla;
                                    }
                                    dtAtb.Rows.Add(noTabla - 1, noAtributo, tokens2[i], tokens2[i + 1], tokens2[i + 3], 1, noAtributoTemp);
                                    atributos.Add((noTabla - 1, noAtributo, tokens2[i], tokens2[i + 1], Convert.ToInt32(tokens2[i + 3]), 1, noAtributoTemp));
                                    noAtributo++;
                                }
                                else
                                {
                                    if (noTablaTemp == noTabla)
                                    {
                                        noAtributoTemp++;
                                    }
                                    else
                                    {
                                        noAtributoTemp = 1;
                                        noTablaTemp = noTabla;
                                    }
                                    dtAtb.Rows.Add(noTabla - 1, noAtributo, tokens2[i], tokens2[i + 1], tokens2[i + 3], 0, noAtributoTemp);
                                    atributos.Add((noTabla - 1, noAtributo, tokens2[i], tokens2[i + 1], int.Parse(tokens2[i + 3]), 0, noAtributoTemp));
                                    noAtributo++;
                                }

                            }

                        }
                        //RESTRICCIONES
                        if (tokens2[i] == "CONSTRAINT")
                        {
                            if ((tokens2[i + 2] == "PRIMARY" || tokens2[i + 2] == "FOREIGN"))
                            {
                                if (tokens2[i + 2] == "PRIMARY")
                                {
                                    string nombreAt = tokens2[i + 5];
                                    int? atributoAsociado = atributos
                                    .Where(a => a.noTabla == noTabla - 1 && a.nombreAtributo == nombreAt)
                                    .Select(a => (int?)a.noAtributo) // Convertir a nullable para evitar valores por defecto
                                    .FirstOrDefault();
                                    if (atributoAsociado != null)
                                    {
                                        dtRes.Rows.Add(noTabla - 1, noRestriccion, 1, tokens2[i + 1], atributoAsociado, "-", "-");
                                        restricciones.Add((noTabla - 1, noRestriccion, 1, tokens2[i + 1], int.Parse(Convert.ToString(atributoAsociado)), -1, -1));
                                        noRestriccion++;
                                    }
                                    else
                                    {
                                        dtRes.Rows.Add(noTabla - 1, noRestriccion, 1, tokens2[i + 1], "N/A", "-", "-");
                                        restricciones.Add((noTabla - 1, noRestriccion, 1, tokens2[i + 1], -1, -1, -1));
                                        noRestriccion++;
                                    }

                                }
                                else if (tokens2[i + 2] == "FOREIGN")
                                {
                                    string nombreAt = tokens2[i + 5];

                                    int? atributoAsociado = atributos
                                    .Where(a => a.noTabla == noTabla - 1 && a.nombreAtributo == nombreAt)
                                    .Select(a => (int?)a.noAtributo)
                                    .FirstOrDefault();

                                    int? noTablaDT = tablas
                                    .Where(t => t.nombreTabla == tokens2[i + 8])
                                    .Select(t => (int?)t.noTabla)
                                    .FirstOrDefault();

                                    int? noAtributo = atributos
                                    .Where(a => a.noTabla == noTablaDT && a.nombreAtributo == tokens2[i + 10])
                                    .Select(a => (int?)a.noAtributo)
                                    .FirstOrDefault();
                                    if (atributoAsociado != null && noTablaDT != null && noAtributo != null)
                                    {
                                        dtRes.Rows.Add(noTabla - 1, noRestriccion, 2, tokens2[i + 1], atributoAsociado, int.Parse(Convert.ToString(noTablaDT)), int.Parse(Convert.ToString(noAtributo)));
                                        restricciones.Add((noTabla - 1, noRestriccion, 2, tokens2[i + 1], int.Parse(Convert.ToString(atributoAsociado)), int.Parse(Convert.ToString(noTablaDT)), int.Parse(Convert.ToString(noAtributo))));
                                        noRestriccion++;
                                    }
                                    else
                                    {
                                        dtRes.Rows.Add(noTabla - 1, noRestriccion, 2, tokens2[i + 1], "N/A", "N/A", "N/A");
                                        restricciones.Add((noTabla - 1, noRestriccion, 2, tokens2[i + 1], -1, -1, -1));
                                        noRestriccion++;
                                    }

                                }
                            }
                        }
                       
                }
                catch
                {

                }

            }
            List<(int noTabla, string nombreTabla, int cantidadAtributos, int cantidadRestricciones)> tablasConConteo = new List<(int, string, int, int)>();

            foreach (var tabla in tablas)
            {
                int contadorAtributos = atributos.Count(a => a.noTabla == tabla.noTabla);
                int contadorRestricciones = restricciones.Count(r => r.noTabla == tabla.noTabla);

                tablasConConteo.Add((tabla.noTabla, tabla.nombreTabla, contadorAtributos, contadorRestricciones));
            }

            tablas = tablasConConteo;

            dtTab.Rows.Clear();

            foreach (var tabla in tablas)
            {
                dtTab.Rows.Add(tabla.noTabla, tabla.nombreTabla, tabla.cantidadAtributos, tabla.cantidadRestricciones);
            }




        }

        public bool Sintaxis(List<string> tokens, List<string> tokens2, TextBox texto, RichTextBox ennt, DataGridView dgvLex, TextBox txtError, DataGridView dgvTabla, DataGridView dgvAtributos, DataGridView dgvRestriccion)
        {
            try
            {
                bool primeravez = true;
                acumuladorComillas2 = acumuladorComillas2 - 1;
                texto.BackColor = Color.White;
                texto.Text = "";
                bool error = false;
                int lineas = 1;
                string KparaN = "";
                pila.Push("199");
                if (tokens.First() == "INSERT")
                    pila.Push("211");
                else if (tokens.First() == "CREATE")
                    pila.Push("200");
                else if (tokens.First() == "SELECT")
                    pila.Push("300");
                //tokensConN = tokens;
                tokens2 = tokens2.Where(s => s != "\n").ToList();
                tokens = tokens.Where(s => s != "\n").ToList();
                tokens.Add("$");
                int apun = 0;
                int apunN = 0;
                string equis = "";
                bool errorSintactico = false;
                string K = "";
                int numeroTablaChecker = 0;
                int numeroTablaCheckerRef = 0;
                
                
                do
                {
                    string X = pila.Pop();
                    if (Regex.IsMatch(X, @"^7\d{2}$")|| Regex.IsMatch(X, @"^8\d{2}$"))
                    {
                        if (X == "700")
                            errorSintactico = Validar_NombreTabla(K, out errorSintactico, ref numeroTablaChecker);
                        if (errorSintactico == true)
                        {
                            Errores.nombreAtributoDuplicado(texto,lineas, K);
                            error = true;
                            break;
                        }
                        if (X == "701")
                            errorSintactico = Validar_NombreAtributo(K, out errorSintactico);
                        if (errorSintactico == true)
                        {
                            Errores.validarNombreAtributo(texto, lineas, K);
                            error = true;
                            break;
                        }
                        if (X == "702")
                            errorSintactico = Validar_ExistirAtributo(K, out errorSintactico, ref numeroTablaChecker);
                        if (errorSintactico == true)
                        {
                            Errores.validarExistirAtributo(texto, lineas, K, ref tablas, numeroTablaChecker);
                            error = true;
                            break;
                        }
                        if (X == "703")
                            errorSintactico = Validar_DupRestriccion(K, out errorSintactico);
                        if (errorSintactico == true)
                        {
                            Errores.validarDupRestriccion(texto, lineas, K);
                            error = true;
                            break;
                        }
                        if (X == "704")
                            errorSintactico = Validar_AtributoNoValido(K, out errorSintactico, ref numeroTablaChecker);
                        if (errorSintactico == true)
                        {
                            Errores.validarAtributoNoValido(texto, lineas, K, ref tablas, numeroTablaChecker);
                            error = true;
                            break;
                        }
                        if (X == "705")
                        {
                            // Validación de cantidad de atributos
                            errorSintactico = Validar_CantidadAtributos(K, out errorSintactico, ref numeroTablaChecker, apunN);

                            // Si pasa la validación de cantidad, validar tipos de datos
                            if (!errorSintactico)
                            {
                                //// 1. Obtener nombre de la tabla destino (debes tener esta variable)
                                //string nombreTablaDestino = tokens[tokens.IndexOf("INTO") + 1];

                                //// 2. Obtener los valores del INSERT (debes tener esta lista)
                                //List<string> valoresInsert = tokens
                                //    .Skip(tokens.IndexOf("VALUES") + 1)
                                //    .TakeWhile(t => t != ")")
                                //    .Where(t => t != "(" && t != ",")
                                //    .ToList();
                                //valoresInsert.RemoveAll(v => v.Trim() == "'");

                                //// 3. Validar tipos y longitud
                                //bool validacionTipo = Validar_TipoDatoEnInsercion(
                                //    nombreTablaDestino,
                                //    valoresInsert,
                                //    out (string error, int linea, string atributo) errorInfo
                                //);

                                //if (!validacionTipo)
                                //{
                                //    Errores.validarTipoDatoInsert(texto, lineas, errorInfo.atributo, errorInfo.error);
                                //    error = true;
                                //    break;
                                //}
                            }
                            else
                            {
                                Errores.validarCantidadAtributos(texto, lineas);
                                error = true;
                                break;
                            }
                        }
                        if (X == "706")
                            errorSintactico = Validar_ExistirTabla(K, out errorSintactico, ref numeroTablaChecker);
                        if (errorSintactico == true)
                        {
                            Errores.validarExistirTabla(texto, lineas);
                            error = true;
                            break;
                        }
                        //if (X == "707")
                        //    errorSintactico = Validar_CantidadBytes(out errorSintactico, numeroTablaChecker, apunN, tokens[apun-2]);
                        //if (errorSintactico == true)
                        //{
                        //    Errores.validarCantidadBytes(texto, lineas);
                        //    error = true;
                        //    break;
                        //}
                        if (X == "708")
                            errorSintactico = Validar_TablaContAtrib(out errorSintactico, numeroTablaCheckerRef, K);
                        if (errorSintactico == true)
                        {
                            Errores.validarAtributoNoValido(texto, lineas, K, ref tablas, numeroTablaCheckerRef);
                            error = true;
                            break;
                        }
                        if (X == "709")
                        {
                            errorSintactico = Validar_ExistirTablaParaRef(K, out errorSintactico, ref numeroTablaCheckerRef);
                        }
                        if (errorSintactico == true)
                        {
                            Errores.validarExistirTabla(texto, lineas);
                            error = true;
                            break;
                        }
                        if(X == "800")
                        {
                            List<string> ambiguos = ObtenerAtributosAmbiguos();

                            if (ambiguos.Any())
                            {
                                if (tokens[apun - 2] != "(")
                                {
                                    foreach (var atributo in ambiguos)
                                    {
                                        Errores.validarAmbiguedad(texto, atributo, lineas);
                                        error = true;
                                        break;
                                    }
                                }


                                
                            }



                            List<string> atributosNoEncontrados = nombrePerteneceATabla();

                            if (atributosNoEncontrados.Any())
                            {
                                foreach (var atributo in atributosNoEncontrados)
                                {
                                    Errores.validarNombreEnTabla(texto,atributo,lineas);
                                    error = true;
                                    break;
                                }
                            }
                            // NUEVA REGLA
                            // Se ocupa que K si esta en una linea diferente a la 1, cheque con el atributo de 3 o 2 tokens atras, si son iguales
                            // todo esta altoke, si son diferentes, errrror.
                            if (lineas != 1 && BuscarAtributoPorLinea(K) && tokens[apun-2] == "SELECT")
                            {
                                if (tokens[apun - 5] != K)
                                {
                                    Errores.ValidarAtributoScInvalido(texto, lineas, K, tokens[apun-5]);
                                    error = true;
                                    break;
                                }
                            }
                            



                        }
                        if(X == "801")
                        {
                            if (tokens[apun - 2] == ".")
                            {
                                bool errrrror = ValidarAtributoEnTabla(tokens[apun-3]+"."+K);
                                if (errrrror == false)
                                {
                                    Errores.validarIdentificadorInvalido(texto, lineas, K);
                                    error = true;
                                    break;
                                }
                            }

                            List<string> erroresWhereINv = ObtenerAtributosWhereInvalidosConAlias();

                                                      
                            if (erroresWhereINv.Any())
                            {
                                foreach (var atributo in erroresWhereINv)
                                {
                                    Errores.validarTablaNoValidaAlias(texto, atributo, lineas);
                                    error = true;
                                    break;
                                }
                            }


                        }
                        if (X == "803")
                        {
                            string tablaMala = "";
                            bool errorWrok = Validar_NombresTablasEnConsulta(out tablaMala, ref listaFrom);
                            if (errorWrok == false)
                            {
                                error = true;
                                Errores.validarTablaNoValida(texto, lineas, tablaMala);
                                break;
                            }

                            string identificadorInvalido;
                            bool errorAlex = Validar_IdentificadorEnWhere(out identificadorInvalido, ref listaWhere, ref tablas);
                            if (errorAlex == false)
                            {
                                error = true;
                                Errores.validarIdentificadorInvalido(texto, lineas, identificadorInvalido);
                                break;
                            }

                        }
                        if (X == "808") // Código para validación de subconsultas
                        {
                            (string errorMsg, int linea) errorInfo;
                            bool valido = Validar_TipoDatoEnComparacion(
                                out errorInfo,
                                ref listaWhere,
                                ref atributos
                            );

                            if (!valido)
                            {
                                error = true;

                                // errorInfo.error == "Error de tipo en la comparación: MNOMBRE no es del mismo tipo..."
                                var match = System.Text.RegularExpressions.Regex.Match(
                                    errorInfo.errorMsg,
                                    @":\s*(\w+)"
                                );
                                string atributoInvalido = match.Success
                                    ? match.Groups[1].Value
                                    : errorInfo.errorMsg; // fallback

                                Errores.validarAtributoSubconsultaInvalido(
                                    txtError: texto,
                                    lineas: errorInfo.linea,
                                    atributo: atributoInvalido
                                );
                                break;
                            }
                            bool errorsisimo = Validar_TipoDatoEnComparacionDos(out errorInfo, ref listaWhere, ref atributos);
                            if (!errorsisimo)
                            {
                                error = true;

                                // errorInfo.error == "Error de tipo en la comparación: MNOMBRE no es del mismo tipo..."
                                var match = System.Text.RegularExpressions.Regex.Match(
                                    errorInfo.errorMsg,
                                    @":\s*(\w+)"
                                );
                                string atributoInvalido = match.Success
                                    ? match.Groups[1].Value
                                    : errorInfo.errorMsg; // fallback

                                Errores.validarAtributoSubconsultaInvalido(
                                    txtError: texto,
                                    lineas: errorInfo.linea,
                                    atributo: atributoInvalido
                                );
                                break;
                            }

                        }
                        //apunN++;
                    }
                    else
                    {
                        K = tokens[apun];
                        if (apunN < tokensConN.Count())
                            KparaN = tokensConN[apunN];
                        if (KparaN != "\n")
                        {
                            if (!Regex.IsMatch(X, @"^[32]\d{2}$") || X == "199")
                            {
                                if (X == ConvertirToken(K))
                                {

                                    if(apun == 19)
                                    {

                                    }
                                    if (apun < tokens2.Count - 1)
                                        if (primeravez != true)
                                        {
                                            if (apun == 93)
                                            {

                                            }
                                            apun++;
                                            tokens.RemoveAt(tokens.Count - 1);
                                            actual = Analizador(ennt, dgvLex, txtError, dgvTabla, dgvAtributos, dgvRestriccion);
                                            while (actual == "")
                                            {                                               
                                                dondevoy++;
                                                actual = Analizador(ennt, dgvLex, txtError, dgvTabla, dgvAtributos, dgvRestriccion);
                                            }
                                            dondevoy = dondevoy + 1;
                                            tokensConN.Add(actual);
                                            tokens.Add(actual);
                                            if (valorAntesN == true)
                                            {
                                                tokensConN.Add("\n");
                                                valorAntesN = false;
                                            }
                                        }
                                        else
                                        {
                                            tokensConN.Add(tokens[0]);
                                            apun++;
                                            tokens.RemoveAt(tokens.Count - 1);
                                            actual = Analizador(ennt, dgvLex, txtError, dgvTabla, dgvAtributos, dgvRestriccion);
                                            dondevoy = dondevoy + 1;
                                            actual = Analizador(ennt, dgvLex, txtError, dgvTabla, dgvAtributos, dgvRestriccion);
                                            dondevoy = dondevoy + 1;
                                            tokensConN.Add(actual);
                                            tokens.Add(actual);
                                            primeravez = false;
                                        }
                                    else
                                    {
                                        apun++;
                                        tokens.RemoveAt(tokens.Count - 1);
                                    }
                                    //tokens.RemoveAt(tokens.Count - 1);
                                    //if (apun < tokens2.Count)
                                    //    tokens.Add(tokens2[apun]);

                                    tokens.Add("$");
                                    //tokens = Analizador(ennt, dgvLex, txtError, dgvTabla, dgvAtributos, dgvRestriccion);

                                    
                                    
                                    //tokens = Analizador(ennt, dgvLex, txtError, dgvTabla, dgvAtributos, dgvRestriccion);
                                    apunN++;
                                }
                                else
                                {
                                    error = true;
                                    string palFaltante = tablaSimbolos.FirstOrDefault(z => z.Value.ToString() == X).Key ?? "Identificador";
                                    if (reservadas.IsMatch(palFaltante))
                                        Errores.ErrorPalabraReservada(texto, lineas);
                                    else if (constantesTL.IsMatch(palFaltante))
                                        Errores.ErrorConstante(texto, lineas);
                                    else if (palFaltante == "Identificador")
                                        Errores.ErrorMaestro(texto, lineas, palFaltante);
                                    else if (constantes.IsMatch(palFaltante))
                                        Errores.ErrorConstante(texto, lineas);
                                    else if (relacionales.IsMatch(palFaltante))
                                        Errores.ErrorOperadorRelacional(texto, lineas);
                                    else if (acumuladorComillas1 % 2 != 0)
                                        Errores.ErroresComillas(texto, acumuladorComillas1, lineas);
                                    else if (acumuladorComillas3 % 2 != 0)
                                        Errores.ErroresComillas(texto, acumuladorComillas3, lineas);
                                    else if (operadores.IsMatch(palFaltante))
                                        Errores.ErrorMaestro(texto, lineas, palFaltante);
                                    else if (acumuladorParentesisAbierto % 2 != 0)
                                        Errores.ErrorParentesisDDL(texto, lineas);
                                    else
                                        Errores.ErrorSintactico(texto, lineas);
                                    break;
                                }
                            }
                            else
                            {
                                if (tokens[apun] == "INSERT" && tokens[apun + 1] == "INTO")
                                {
                                    string nombreTabla = tokens[apun + 2];
                                    var tabla = tablas.FirstOrDefault(t => t.nombreTabla == nombreTabla);
                                    if (tabla.Equals(default)) continue;

                                    // Extraer valores entre VALUES (...) o hasta ;
                                    List<string> valores = new List<string>();
                                    int j = apun + 5; // Saltar "INSERT INTO tabla VALUES ("
                                    while (j < tokens.Count && tokens[j] != ")")
                                    {
                                        if (tokens[j] != "," && tokens[j] != "(")
                                            valores.Add(tokens[j].Trim('\''));
                                        j++;
                                    }

                                    // Almacenar los datos
                                    if (!datosTablas.ContainsKey(tabla.noTabla))
                                        datosTablas[tabla.noTabla] = new List<List<string>>();
                                    datosTablas[tabla.noTabla].Add(valores);
                                }

                                if (K == "INSERT")
                                {
                                    string nombreTabla = tokens[apun + 2];
                                    List<string> valores = tokens
                                        .Skip(apun+5) // Saltar hasta después de "("
                                        .TakeWhile(t => t != ")")
                                        .Where(t => t != ",")
                                        .ToList();
                                    valores.RemoveAll(v => v.Trim() == "'");

                                    bool errorSemantico;
                                    if (!ValidarLlaveForaneaInsert(nombreTabla, valores, texto, lineas, out errorSemantico))
                                    {
                                        error = true;
                                    }
                                }
                                string produccion = TablaSintac[EncontrarIndiceX(X), EncontrarIndiceK(ConvertirToken(K))];
                                if (produccion != null)
                                {
                                    if (produccion != "99")
                                    {
                                        produccion.Split(' ').Reverse().ToList().ForEach(prod => pila.Push(prod));
                                    }

                                }
                                else
                                {
                                    error = true;
                                    if (reservadas.IsMatch(tokens[apun - 1]) && tokens[apun].StartsWith("'") && tokens[apun].EndsWith("'"))
                                    {
                                        Errores.ErrorIdentificador(texto, lineas);
                                        break;
                                    }
                                    else if ((relacionales.IsMatch(tokens[apun - 1]) || (relacionales.IsMatch(tokens[apun - 2])) && (!tokens[apun].StartsWith("'") && !tokens[apun].EndsWith("'"))))
                                    {
                                        Errores.ErrorConstante(texto, lineas);
                                        break;
                                    }
                                    else if (tokens[apun - 1] == "," && (constantes.IsMatch(tokens[apun - 3]) || constantesTL.IsMatch(tokens[apun - 2])))
                                    {
                                        if (tokens[apun] == ")")
                                        {
                                            Errores.ErrorConstante(texto, lineas);
                                            break;
                                        }
                                    }
                                    else if (constantes.IsMatch(tokens[apun - 2]) && constantes.IsMatch(tokens[apun + 1]))
                                    {
                                        Errores.ErrorConstante(texto, lineas);
                                        break;
                                    }
                                    else if (constantesTL.IsMatch(tokens[apun - 1]) && constantesTL.IsMatch(tokens[apun]))
                                    {
                                        Errores.ErroresComillas(texto, acumuladorComillas2, lineas);
                                        break;
                                    }
                                    else if (tokens[apun] == "(")
                                    {
                                        if (!reservadas.IsMatch(tokens[apun - 1]))
                                        {
                                            Errores.ErrorPalabraReservada(texto, lineas);
                                            break;
                                        }
                                    }
                                    else if (Errores.ErroresComillas(texto, acumuladorComillas1, lineas))
                                    {
                                        texto.Text = "Error 2:205: Linea " + (lineas - 1) + " Se esperaba Delimitador";
                                        break;
                                    }
                                    else if (Errores.ErroresComillas(texto, acumuladorComillas3, lineas))
                                    {
                                        texto.Text = "Error 2:205: Linea " + (lineas - 1) + " Se esperaba Delimitador";
                                        break;
                                    }
                                    else if (tokens[apun - 1] == "NULL")
                                    {
                                        if (tokens[apun] != "," || tokens[apun] != ";")
                                        {
                                            Errores.ErroresComillas(texto, acumuladorComillas2, lineas);
                                            break;
                                        }
                                    }
                                    else if (delimitadores.IsMatch(tokens[apun]) && !relacionales.IsMatch(tokens[apun - 1]))
                                    {
                                        Errores.ErrorOperadorRelacional(texto, lineas);
                                        break;
                                    }
                                    else if (reservadas.IsMatch(tokens[apun - 1]))
                                    {
                                        if (reservadas.IsMatch(tokens[apun]))
                                        {
                                            Errores.ErrorIdentificador(texto, lineas);
                                            break;
                                        }
                                    }
                                    else if (tokens[apun - 1] == ",")
                                    {
                                        if (4.ToString() != ConvertirToken(tokens[apun]))
                                        {
                                            Errores.ErrorIdentificador(texto, lineas);
                                            break;
                                        }
                                    }
                                    else if (relacionales.IsMatch(tokens[apun]))
                                    {
                                        if (apun + 2 >= tokens.Count) // ADELANTE
                                        {
                                            Errores.ErrorIdentificador(texto, lineas);
                                            break;
                                        }
                                        if (4.ToString() != ConvertirToken(tokens[apun + 1])) // ADELANTE
                                        {
                                            Errores.ErrorIdentificador(texto, lineas);
                                            break;
                                        }
                                    }
                                    else if (operadores.IsMatch(tokens[apun]) || relacionales.IsMatch(tokens[apun])) // ADELANTE
                                    {
                                        if (apun + 2 >= tokens.Count) // ADELANTE
                                        {
                                            Errores.ErrorIdentificador(texto, lineas);
                                            break;
                                        }
                                    }
                                    else if (acumuladorParentesisAbierto % 2 != 0)
                                    {
                                        Errores.ErrorParentesisDDL(texto, lineas);
                                    }
                                    else if (4.ToString() == ConvertirToken(tokens[apun]) && delimitadores.IsMatch(tokens[apun - 1]))
                                    {
                                        Errores.ErrorPalabraReservada(texto, lineas);
                                        break;
                                    }
                                    else if (tokens[apun] == "INTO")
                                    {
                                        if (tokens[apun - 1] != "SELECT")
                                        {
                                            Errores.ErrorPalabraReservada(texto, lineas);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Errores.ErrorSintactico(texto, lineas);

                                        break;
                                    }
                                }

                            }
                        }

                        else
                        {
                            lineas++;
                            apunN++;
                            pila.Push(X);
                        }
                    }

                        equis = X;
                }
                while (equis != "199");
                // Validación adicional para INSERT después del análisis sintáctico
                

                if (error == false)
                {
                    Errores.SinError(texto, lineas);
                }
                pila.Clear();
                return error;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return true;
            }
        }

        List<(int noTabla, string nombreTabla, int cantidadAtributos, int cantidadRestricciones)> tablas = new List<(int, string, int, int)>();
        List<(int noTabla, int noAtributo, string nombreAtributo, string tipo, int longitud, int noNull, int noAtributoTabla)> atributos = new List<(int, int, string, string, int, int, int)>();
        List<(int noTabla, int noRestriccion, int Tipo, string nombreRestriccion, int atributoAsociado, int Tabla, int atributo)> restricciones = new List<(int, int, int, string, int, int, int)>();
        
        List<(string tabla, string alias, int linea)> listaFrom = new List<(string tabla, string alias, int linea)>();
        List<(string tabla, string atributo, int linea)> listaSelect = new List<(string tabla, string atributo, int linea)>();
        List<(string tabla, string atributo, string tipo, int linea)> listaWhere = new List<(string tabla, string atributo, string tipo, int linea)>();

        public bool BuscarAtributoPorLinea(string atributoSc)
        {
            // Buscar en listaSelect y listaWhere, asegurando que la línea no sea 1
            bool encontrado = listaSelect.Any(s => s.atributo == atributoSc && s.linea != 1) ||
                             listaWhere.Any(w => w.atributo == atributoSc && w.linea != 1);

            // Verificar si el atributo existe en la lista de atributos
            bool atributoValido = atributos.Any(a => a.nombreAtributo.Equals(atributoSc, StringComparison.OrdinalIgnoreCase));

            // Retornar true solo si se encuentra en listaSelect o listaWhere con línea != 1 y es un atributo válido
            return encontrado && atributoValido;
        }

        public bool ValidarAtributoEnTabla(string atributoCalificado)
        {
            // Separar tabla y atributo
            if (!atributoCalificado.Contains('.')) return false;

            var partes = atributoCalificado.Split('.');
            if (partes.Length != 2) return false;

            string nombreTabla = partes[0];
            string nombreAtributo = partes[1];

            // 1. Verificar si la tabla está en listaFrom
            bool tablaEnFrom = listaFrom.Any(f => f.tabla == nombreTabla);
            if (!tablaEnFrom)
            {
                var fromCoincidente = listaFrom.FirstOrDefault(t =>
                        t.alias.Equals(nombreTabla, StringComparison.OrdinalIgnoreCase));

                if (fromCoincidente != default)
                {
                    // Verificar si el atributo existe en la tabla
                    var tabla2 = tablas.FirstOrDefault(t => t.nombreTabla.Equals(fromCoincidente.tabla,
                                                StringComparison.OrdinalIgnoreCase));
                    bool atributoExiste2 = atributos.Any(a =>
                        a.noTabla == tabla2.noTabla &&
                        a.nombreAtributo.Equals(nombreAtributo, StringComparison.OrdinalIgnoreCase));

                    if (!atributoExiste2)
                    {
                        return false;
                    }
                    else
                        return true;
                }
            }

            // 2. Obtener noTabla
            var tabla = tablas.FirstOrDefault(t => t.nombreTabla == nombreTabla);
            if (tabla == default) return false;

            // 3. Verificar si el atributo pertenece a esa tabla
            bool atributoExiste = atributos.Any(a =>
                a.noTabla == tabla.noTabla &&
                a.nombreAtributo == nombreAtributo);

            return atributoExiste;
        }
        
        private bool VerificarValorExiste(int noTablaRef, string nombreAtributoRef, string valor, int tabla)
        {
            string tipoDato = "";
            if (!datosTablas.ContainsKey(tabla))
                return false;


            
            var atributosTabla = atributos
                .Where(a => a.noTabla == noTablaRef)
                .OrderBy(a => a.noAtributoTabla)
                .ToList();
            int indiceAttr = atributosTabla.FindIndex(a => a.nombreAtributo == nombreAtributoRef);
            if (indiceAttr == -1)
                return false;

            if (constantes.IsMatch(valor))
                tipoDato = "CHAR";
            else if (constantesTL.IsMatch(valor))
                tipoDato = "NUMERIC";
            if (atributosTabla[indiceAttr].tipo == tipoDato)
            {
                if (atributosTabla[indiceAttr].longitud == valor.Length - 2)
                {
                    
                    return true;
                    
                }
                else
                    return false;

            }
            else
                return false;



        }

        public bool ValidarLlaveForaneaInsert(
    string nombreTabla,
    List<string> valores,
    TextBox txtError,
    int linea,
    out bool errorSemantico)
        {
            errorSemantico = false;

            // Obtener la tabla destino
            var tabla = tablas.FirstOrDefault(t => t.nombreTabla == nombreTabla);
            if (tabla.Equals(default))
            {
                txtError.Text = $"Error: La tabla '{nombreTabla}' no existe.";
                errorSemantico = true;
                return false;
            }

            // Obtener atributos en orden
            var attrs = atributos
                .Where(a => a.noTabla == tabla.noTabla)
                .OrderBy(a => a.noAtributoTabla)
                .ToList();

            if (attrs.Count != valores.Count)
            {
                Errores.validarCantidadAtributos(txtError, linea);
                errorSemantico = true;
                return false;
            }

            // Mapear valores a atributos
            var valoresPorAtributo = new Dictionary<string, string>();
            for (int i = 0; i < attrs.Count; i++)
                valoresPorAtributo[attrs[i].nombreAtributo] = valores[i];

            // Obtener restricciones de llave foránea
            var fks = restricciones
                .Where(r => r.noTabla == tabla.noTabla && r.Tipo == 2) // 2 = llave foránea
                .ToList();

            foreach (var fk in fks)
            {
                var attrLocal = atributos.FirstOrDefault(a => a.noAtributo == fk.atributoAsociado);
                if (attrLocal.Equals(default)) continue;

                string valor = valoresPorAtributo[attrLocal.nombreAtributo];
                var tablaRef = tablas.FirstOrDefault(t => t.noTabla == fk.Tabla);
                var attrRef = atributos.FirstOrDefault(a => a.noAtributo == fk.atributo);

                if (tablaRef.Equals(default) || attrRef.Equals(default)) continue;

                // Verificar si el valor existe
                if (!VerificarValorExiste(tablaRef.noTabla, attrRef.nombreAtributo, valor, tabla.noTabla))
                {
                    txtError.Text = $"ERROR: La Sentencia INSERT está en conflicto con la restricción de Llave Foránea '{fk.nombreRestriccion}'. " +
                                    $"El conflicto ocurre en la BD 'INSCRITOS', tabla '{tablaRef.nombreTabla}', atributo '{attrRef.nombreAtributo}'.";
                    txtError.BackColor = Color.FromArgb(255, 137, 137);
                    errorSemantico = true;
                    return false;
                }
            }

            return true;
        }
        public List<string> nombrePerteneceATabla()
        {
            List<string> errores = new List<string>();

            foreach (var (tablaZ, atributoCompleto, linea) in listaSelect)
            {
                var partes = atributoCompleto.Split('.');

                if (partes.Length == 2)
                {
                    // Caso: alias.atributo
                    string alias = partes[0];
                    string nombreAtributo = partes[1];

                    // Buscar a qué tabla corresponde el alias
                    var fromMatch = listaFrom.FirstOrDefault(f => f.alias == alias);
                    if (fromMatch == default)
                    {
                        errores.Add($"Línea {linea}: Alias '{alias}' no encontrado en FROM.");
                        continue;
                    }

                    // Verificar si esa tabla existe
                    var tabla = tablas.FirstOrDefault(t => t.nombreTabla == fromMatch.tabla);
                    if (tabla == default)
                    {
                        errores.Add($"Línea {linea}: Tabla '{fromMatch.tabla}' referida por alias '{alias}' no encontrada.");
                        continue;
                    }

                    // Verificar si el atributo está en esa tabla
                    bool existe = atributos.Any(a =>
                        a.noTabla == tabla.noTabla &&
                        a.nombreAtributo == nombreAtributo);

                    if (!existe)
                        errores.Add($"Línea {linea}: El atributo '{nombreAtributo}' no se encuentra en la tabla '{tabla.nombreTabla}' referida como '{alias}'.");
                }
                else if (partes.Length == 1)
                {
                    // Caso: solo nombre de atributo (sin alias)
                    string nombreAtributo = partes[0];

                    // Verificar si el atributo existe en alguna de las tablas del FROM
                    bool existe = listaFrom.Any(from =>
                    {
                        var tabla = tablas.FirstOrDefault(t => t.nombreTabla == from.tabla);
                        if (tabla == default) return false;

                        return atributos.Any(a =>
                            a.noTabla == tabla.noTabla &&
                            a.nombreAtributo == nombreAtributo);
                    });

                    if (!existe)
                        errores.Add($"Línea {linea}: El atributo '{nombreAtributo}' no se encontró en ninguna tabla");
                }
                else
                {
                    errores.Add($"Línea {linea}: El atributo '{atributoCompleto}' tiene un formato inválido.");
                }
            }

            return errores;
        }
        public bool Validar_TipoDatoEnInsercion(
    string nombreTabla,
    List<string> valores,
    out (string error, int linea, string atributo) errorInfo)
        {
            errorInfo = ("", -1, "");

            // Buscar la tabla
            var tabla = tablas.FirstOrDefault(t => t.nombreTabla == nombreTabla);
            if (tabla.Equals(default))
            {
                errorInfo = ("La tabla no existe", -1, nombreTabla);
                return false;
            }

            // Obtener los atributos de la tabla en orden
            var atributosTabla = atributos
                .Where(a => a.noTabla == tabla.noTabla)
                .OrderBy(a => a.noAtributoTabla)
                .ToList();

            // Verificar cantidad de valores vs atributos
            if (valores.Count != atributosTabla.Count)
            {
                errorInfo = ("La cantidad de valores no coincide con los atributos", -1, "");
                return false;
            }

            for (int i = 0; i < valores.Count; i++)
            {
                string valor = valores[i];
                var atributo = atributosTabla[i];

                // Validar tipo de dato
                if (atributo.tipo == "CHAR")
                {
                    // Debe ser una cadena entre comillas
                    if (!constantes.IsMatch(valor))
                    {
                        errorInfo = (
                            $"El valor '{valor}' no es una cadena válida para el atributo {atributo.nombreAtributo}",
                            -1,
                            atributo.nombreAtributo
                        );
                        return false;
                    }

                    // Validar longitud
                    string valorSinComillas = valor.Trim('\'');
                    if (valorSinComillas.Length > atributo.longitud)
                    {
                        errorInfo = (
                            $"Longitud excedida para '{atributo.nombreAtributo}'. Máximo: {atributo.longitud}",
                            -1,
                            atributo.nombreAtributo
                        );
                        return false;
                    }
                }
                else if (atributo.tipo == "NUMERIC")
                {
                    // Debe ser un número sin comillas
                    if (!constantesTL.IsMatch(valor))
                    {
                        errorInfo = (
                            $"El valor '{valor}' no es numérico para el atributo {atributo.nombreAtributo}",
                            -1,
                            atributo.nombreAtributo
                        );
                        return false;
                    }
                }
            }

            return true;
        }
        public string ObtenerPalabraEnIndice(RichTextBox richTextBox, int indice)
        {
            // Separar el texto por espacios en blanco para obtener palabras
            var palabras = richTextBox.Text.Split(new[] { ' ', '\t', '\n', '\r' },
                                                   StringSplitOptions.RemoveEmptyEntries);

            // Verificar si el índice está dentro del rango de palabras
            if (indice >= 0 && indice < palabras.Length)
            {
                return palabras[indice];
            }
            else
            {
                // Si el índice está fuera del rango, podrías manejarlo como desees,
                // por ejemplo, lanzando una excepción o devolviendo un valor por defecto.
                throw new IndexOutOfRangeException("El índice está fuera del rango de palabras.");
            }
        }
        public List<string> ObtenerAtributosAmbiguos()
        {
            List<string> atributosAmbiguos = new List<string>();

            // Agrupamos los atributos por línea (subconsulta)
            var atributosPorLinea = listaSelect
                .Where(s => !s.atributo.Contains('.')) // solo atributos sin alias
                .GroupBy(s => s.linea);

            foreach (var grupo in atributosPorLinea)
            {
                int linea = grupo.Key;

                foreach (var (tablaZ, atributoCompleto, _) in grupo)
                {
                    string nombreAtributo = atributoCompleto;
                    int cantidadApariciones = 0;

                    // Solo considerar las tablas del mismo contexto (misma línea)
                    var fromEnLinea = listaFrom.Where(f => f.linea == linea);

                    foreach (var from in fromEnLinea)
                    {
                        var tabla = tablas.FirstOrDefault(t => t.nombreTabla == from.tabla);
                        if (tabla == default) continue;

                        // Verificar duplicidad solo dentro de la tabla especificada en tablaZ
                        if (string.IsNullOrEmpty(tablaZ) || from.tabla == tablaZ)
                        {
                            bool existe = atributos.Any(a =>
                                a.noTabla == tabla.noTabla &&
                                a.nombreAtributo == nombreAtributo);

                            if (existe)
                                cantidadApariciones++;
                        }
                    }

                    if (cantidadApariciones > 1 && !atributosAmbiguos.Contains(nombreAtributo))
                        atributosAmbiguos.Add(nombreAtributo);
                }
            }

            return atributosAmbiguos;
        }
        public List<string> ObtenerAtributosWhereInvalidos()
        {
            List<string> atributosInvalidos = new List<string>();

            foreach (var (tablaWhere, atributoWhere, tipo, linea) in listaWhere)
            {
                // Buscar la tabla por nombre
                var tabla = tablas.FirstOrDefault(t => t.nombreTabla == tablaWhere);
                if (tabla == default)
                {
                    atributosInvalidos.Add($"3:311 Línea {linea}: La tabla '{tablaWhere}' no existe.");
                    continue;
                }

                // Verificar si el atributo está en esa tabla
                bool existe = atributos.Any(a =>
                    a.noTabla == tabla.noTabla &&
                    a.nombreAtributo == atributoWhere);

                if (!existe)
                {
                    atributosInvalidos.Add($"3:311 Línea {linea}: El atributo '{atributoWhere}' no se encuentra en la tabla '{tablaWhere}'.");
                }
            }

            return atributosInvalidos;
        }
        public List<string> ObtenerAtributosWhereInvalidosConAlias()
        {
            List<string> errores = new List<string>();

            foreach (var (tablaWhere, atributoWhere, tipo, linea) in listaWhere)
            {
                // Buscar si tablaWhere es un alias
                var fromItem = listaFrom.FirstOrDefault(f => f.alias == tablaWhere || f.tabla == tablaWhere);
                if (fromItem == default)
                {
                    errores.Add($"Línea {linea}: No se encontró la tabla o alias '{tablaWhere}' en la cláusula FROM.");
                    continue;
                }

                // Buscar la tabla real en la lista de tablas
                var tablaReal = tablas.FirstOrDefault(t => t.nombreTabla == fromItem.tabla);
                if (tablaReal == default)
                {
                    errores.Add($"3:312 Línea {linea}: La tabla '{fromItem.tabla}' no existe en la base de datos.");
                    continue;
                }

                // Validar que el atributo exista en la tabla real
                bool existe = atributos.Any(a =>
                    a.noTabla == tablaReal.noTabla &&
                    a.nombreAtributo == atributoWhere);

                if (!existe)
                {
                    errores.Add($"3:312 Línea {linea}: El atributo '{atributoWhere}' no existe en la tabla '{tablaReal.nombreTabla}' referida como '{tablaWhere}'.");
                }
            }

            return errores;
        }
        public bool Validar_NombresTablasEnConsulta(out string nombreTabla,
            ref List<(string tabla, string alias, int linea)> listaFrom)
        {
            bool salida = false;
            nombreTabla = null;
            foreach (var tablaInfo in listaFrom)
            {
                nombreTabla = tablaInfo.tabla;
                string nombreTablaTemp = nombreTabla;
                int lineaActual = tablaInfo.linea;

                bool tablaExiste = tablas.Any(t => t.nombreTabla.Equals(nombreTablaTemp,
                                        StringComparison.OrdinalIgnoreCase));

                salida = tablaExiste;
                if (salida == false)
                {
                    break;
                }
            }
            return salida;
        }
        public bool Validar_TipoDatoEnComparacion(out (string error, int linea) errorInfo, ref List<(string tabla, string atributo, string tipo, int linea)> listaWhere, ref List<(int noTabla, int noAtributo, string nombreAtributo, string tipo, int longitud, int noNull, int noAtributoTabla)> atributos)
        {
            errorInfo = ("", -1);

            for (int i = 0; i < listaWhere.Count; i++)
            {
                var izquierda = listaWhere[i];
                var derecha = listaWhere[i];
                if (i+1< listaWhere.Count)
                {
                    derecha = listaWhere[i+1];
                }
                if (izquierda.tipo == "atributo" && derecha.tipo != "")
                {
                    var atributoIzq = atributos.FirstOrDefault(a => a.nombreAtributo == izquierda.atributo);
                    if (izquierda.tipo != atributoIzq.tipo)
                    {
                        errorInfo = ($"Error de tipo en la comparación: {izquierda.atributo} no es del mismo tipo que el valor a la derecha.", izquierda.linea);
                        return false;
                    }
                }
            }

            return true;
        }
        public bool Validar_TipoDatoEnComparacionDos(out (string error, int linea) errorInfo, ref List<(string tabla, string atributo, string tipo, int linea)> listaWhere, ref List<(int noTabla, int noAtributo, string nombreAtributo, string tipo, int longitud, int noNull, int noAtributoTabla)> atributos)
        {
            errorInfo = ("", -1);

            for (int i = 0; i < listaWhere.Count; i++)
            {
                var izquierda = listaWhere[i];
                var derecha = listaWhere[i];
                if (i + 1 < listaWhere.Count)
                {
                    derecha = listaWhere[i + 1];
                }
                var atributoIzq = atributos.FirstOrDefault(a => a.nombreAtributo == izquierda.atributo);
                if (derecha.tipo != "" && izquierda.tipo != "")
                {
                    if (izquierda.tipo != atributoIzq.tipo)
                    {
                        errorInfo = ($"Error de tipo en la comparación: {izquierda.atributo} no es del mismo tipo que el valor a la derecha.", izquierda.linea);
                        return false;
                    }

                }
            }

            return true;
        }


        public bool Validar_IdentificadorEnWhere(
out string identificadorInvalido,
ref List<(string tabla, string atributo, string tipo, int linea)> listaWhere,
ref List<(int noTabla, string nombreTabla, int cantidadAtributos, int cantidadRestricciones)> tablas)
        {
            identificadorInvalido = null;

            foreach (var condicion in listaWhere)
            {
                // Solo validamos identificadores que usan notación tabla.atributo
                if (condicion.tabla != "")
                {
                    string nombreTabla = condicion.tabla;
                    string nombreAtributo = condicion.atributo;
                    int linea = condicion.linea;

                    // Verificar si la tabla existe
                    bool tablaExiste = tablas.Any(t => t.nombreTabla.Equals(nombreTabla,
                                            StringComparison.OrdinalIgnoreCase));

                    if (!tablaExiste)
                    {
                        var fromCoincidente = listaFrom.FirstOrDefault(t =>
                        t.alias.Equals(nombreTabla, StringComparison.OrdinalIgnoreCase));

                        if (fromCoincidente != default)
                        {
                            // Verificar si el atributo existe en la tabla
                            var tabla2 = tablas.FirstOrDefault(t => t.nombreTabla.Equals(fromCoincidente.tabla,
                                                        StringComparison.OrdinalIgnoreCase));
                            bool atributoExiste2 = atributos.Any(a =>
                                a.noTabla == tabla2.noTabla &&
                                a.nombreAtributo.Equals(nombreAtributo, StringComparison.OrdinalIgnoreCase));

                            if (!atributoExiste2)
                            {
                                identificadorInvalido = $"{nombreTabla}.{nombreAtributo}";
                                return false;
                            }
                            //if (!tablaExiste2)
                            //{
                            //    identificadorInvalido = $"{nombreTabla}.{nombreAtributo}";
                            //    return false;
                            //}
                            else
                                continue;
                        }

                        
                    }

                    // Verificar si el atributo existe en la tabla
                    var tabla = tablas.FirstOrDefault(t => t.nombreTabla.Equals(nombreTabla,
                                                StringComparison.OrdinalIgnoreCase));
                    bool atributoExiste = atributos.Any(a =>
                        a.noTabla == tabla.noTabla &&
                        a.nombreAtributo.Equals(nombreAtributo, StringComparison.OrdinalIgnoreCase));

                    if (!atributoExiste)
                    {
                        identificadorInvalido = $"{nombreTabla}.{nombreAtributo}";
                        return false;
                    }
                }
            }

            return true;
        }
        #region Utilidades
        public bool Validar_NombreTabla(string nombreTabla, out bool salida, ref int numTabChecker)
        {
            // Contar cuántas veces aparece el nombre en la lista
            int count = tablas.Count(t => t.nombreTabla == nombreTabla);

            // Buscar la primera coincidencia y actualizar numTabChecker
            var registro = tablas.FirstOrDefault(t => t.nombreTabla == nombreTabla);
            if (!registro.Equals(default))
            {
                numTabChecker = registro.noTabla;
            }

            salida = count > 1; // Si aparece más de una vez, es verdadero
            return salida;
        }
        public bool Validar_TablaContAtrib(out bool salida, int numTabCheck, string nombreAtributo)
        {
            int i = -1; // Valor por defecto si no se encuentra

            // Iterar sobre la lista de atributos
            foreach (var atributo in atributos)
            {
                if (atributo.noTabla == numTabCheck && atributo.nombreAtributo == nombreAtributo)
                {
                    i = atributo.noAtributo - 1;
                }
            }

            while (i >= 0 && i < atributos.Count && atributos[i].noTabla == numTabCheck)
            {
                if (atributos[i].nombreAtributo == nombreAtributo)
                    return salida = false;
                i++;
            }
            return salida = true;
        }
        public bool Validar_NombreAtributo(string nombreAtributo, out bool salida)
        {
            // ENTRO D# 1era VEZ
            salida = false;
            int encontradoNoTabla = 0;
            for (int i = 0; i < atributos.Count; i++) // 1
            {
                if (atributos[i].nombreAtributo == nombreAtributo) //SIMON
                {
                    encontradoNoTabla = atributos[i].noTabla; // 1
                    int preI = i; // 0
                    i++; // 1
                    while (i < atributos.Count && atributos[i].noTabla == encontradoNoTabla)
                    {
                        if (atributos[i].nombreAtributo == nombreAtributo)
                        {
                            salida = true; // error
                            return salida;
                        }
                        else
                            i++;
                        if (i > atributos.Count)
                            break;
                    }
                    i = preI;
                }
            }
            return salida;
            
        }
        public bool Validar_ExistirAtributo(string nombreAtributo, out bool salida, ref int numTabCheck)
        {
            int i = -1; // Valor por defecto si no se encuentra

            // Iterar sobre la lista de atributos
            foreach (var atributo in atributos)
            {
                if (atributo.noTabla == numTabCheck && atributo.nombreAtributo == nombreAtributo)
                {
                    i = atributo.noAtributo - 1;
                }
            }

            while ( i >= 0 && i < atributos.Count && atributos[i].noTabla == numTabCheck)
            {
                if (atributos[i].nombreAtributo == nombreAtributo)
                    return salida = false;
                i++;
            }
            return salida = true;
        }
        public bool Validar_DupRestriccion(string nombreRestriccion, out bool salida)
        {
            int count = restricciones.Count(t => t.nombreRestriccion == nombreRestriccion);

            salida = count > 1; // Si aparece más de una vez, es verdadero

            return salida;
        }
        public bool Validar_AtributoNoValido(string nombreAtributo, out bool salida, ref int numTabCheck)
        {
            int i = 0;
            while (i < atributos.Count && atributos[i].noTabla == numTabCheck)
            {
                if (atributos[i].nombreAtributo == nombreAtributo)
                    return salida = false;
                i++;
            }
            return salida = true;
        }
        public bool Validar_CantidadAtributos(string nombreAtributo, out bool salida, ref int numTabCheck, int indice)
        {
            salida = false;
            int numTablaBusqueda = numTabCheck;
            List<string> tempTokens = new List<string>();

            // Validar que el índice sea válido
            if (indice < 0 || indice >= tokens.Count)
                return false;

            // Recorrer hacia atrás desde el índice dado
            for (int i = indice - 2; i >= 0; i--)
            {
                if (tokens[i] == "(")
                    break; // Detenerse si encuentra "("

                tempTokens.Insert(0, tokens[i]); // Insertar al inicio para mantener el orden
            }

            // Contar elementos reales en la lista
            int count = tempTokens.Count(t => t == ",") + 1;

            // Buscar la tabla correspondiente en la lista 'tablas'
            var tabla = tablas.FirstOrDefault(t => t.noTabla == numTablaBusqueda);

            // Verificar si la tabla fue encontrada
            if (tabla.Equals(default))
                return false;

            // Comparar el número de atributos
            if (count == tabla.cantidadAtributos)
            {
                return salida = false;
            }
            return true;
        }
        public bool Validar_ExistirTabla(string nombreTabla, out bool salida, ref int numTabChecker)
        {
            // Contar cuántas veces aparece el nombre en la lista
            int count = tablas.Count(t => t.nombreTabla == nombreTabla);

            // Buscar la primera coincidencia y actualizar numTabChecker
            var registro = tablas.FirstOrDefault(t => t.nombreTabla == nombreTabla);
            if (!registro.Equals(default))
            {
                numTabChecker = registro.noTabla;
            }

            salida = count != 1; // Si aparece más de una vez, es verdadero
            return salida;
        }
        public bool Validar_ExistirTablaParaRef(string nombreTabla, out bool salida, ref int numTabChecker)
        {
            // Contar cuántas veces aparece el nombre en la lista
            int count = tablas.Count(t => t.nombreTabla == nombreTabla);

            // Buscar la primera coincidencia y actualizar numTabChecker
            var registro = tablas.FirstOrDefault(t => t.nombreTabla == nombreTabla);
            if (!registro.Equals(default))
            {
                numTabChecker = registro.noTabla;
            }

            salida = count != 1; // Si aparece más de una vez, es verdadero
            return salida;
        }
        public bool Validar_CantidadBytes(out bool salida, int numTabChecker, int indice, string nombreAtributo)
        {
            salida = false;
            List<string> resultado = new List<string>();

            // Validar que el índice sea válido
            if (indice < 0 || indice >= tokens.Count)
                return salida = false;

            int i = indice;

            // 1. Ir hacia atrás hasta encontrar "("
            while (i >= 0 && tokens[i] != "(")
            {
                if (tokens[i] != "," && tokens[i] != ")") // Omitir comas
                    resultado.Insert(0, tokens[i]); // Insertar al inicio para mantener el orden
                i--;
            }

            // 2. Ir hacia adelante desde la coma (si estaba en una coma al inicio)
            i = indice + 1;
            while (i < tokens.Count && tokens[i] != ")" && tokens[i - 1] != ")")
            {
                if (tokens[i] != ",") // Omitir comas
                    resultado.Add(tokens[i]); // Insertar al final para mantener el orden
                i++;
            }

            // Eliminar todas las comillas simples ("'") de la lista antes de retornar
            resultado.RemoveAll(item => item == "'");

            // Buscar el índice del "nombreAtributo" dentro de "resultado"
            int indiceAtributo = resultado.IndexOf(nombreAtributo) + 1; // +1 porque los noAtributo suelen empezar en 1

            if (indiceAtributo > 0) // Si se encontró (Index +1 sería > 0)
            {
                var atributoEncontrado = atributos.FirstOrDefault(a =>
                    a.noTabla == numTabChecker &&
                    a.noAtributoTabla == indiceAtributo);

                if (atributoEncontrado != default)
                {
                    Console.WriteLine($"Atributo encontrado en tabla {numTabChecker}: " +
                                     $"Nombre={atributoEncontrado.nombreAtributo}, " +
                                     $"Tipo={atributoEncontrado.tipo}, " +
                                     $"Longitud={atributoEncontrado.longitud}");

                    // Nueva validación de longitud
                    int longitudNombreAtributo = resultado[indiceAtributo-1].Length - 2;
                    if (longitudNombreAtributo <= atributoEncontrado.longitud)
                    {
                        salida = false;
                    }
                    else
                    {
                        salida = true;
                    }
                }
            }
            return salida;
        }

        public void MostrarDgv(DataGridView dgvLex, string token, int linea)
        {
            tablaSimbolos.TryGetValue(token, out int valor);
            if (reservadas.IsMatch(token))
            {
                dgvLex.Rows.Add(contador, linea, token, 1, valor);
            }
            else if (delimitadores.IsMatch(token))
            {
                dgvLex.Rows.Add(contador, linea, token, 5, valor);
            }
            else if (operadores.IsMatch(token))
            {
                dgvLex.Rows.Add(contador, linea, token, 7, valor);
            }
            else if (token.Contains('~'))
            {
                dgvLex.Rows.Add(contador, linea, "CONSTANTE", 6, valorConstante);
                //dgvCons.Rows.Add(contador, token.Remove(token.Length - 1), 62, valorConstante);
                valorConstante++;
            }
            else if (constantesTL.IsMatch(token))
            {
                dgvLex.Rows.Add(contador, linea, token, 6, valorConstante);
                //if (Regex.IsMatch(token, @"^\d+$"))
                    //dgvCons.Rows.Add(contador, token, 61, valorConstante);
                if (Regex.IsMatch(token, @"^[a-zA-Z0-9]+$"))
                    //dgvCons.Rows.Add(contador, token, 62, valorConstante);
                    valorConstante++;
            }
            else if (relacionales.IsMatch(token))
            {
                dgvLex.Rows.Add(contador, linea, token, 8, valor);
            }
            else
            {
                bool encontrado = false;

                for (int i = 0; i < dgvLex.Rows.Count; i++)
                {
                    if (dgvLex.Rows[i].Cells[2].Value != null && dgvLex.Rows[i].Cells[2].Value.ToString() == token)
                    {
                        if (dgvLex.Rows[i].Cells[4].Value != null)
                        {
                            dgvLex.Rows.Add(contador, linea, token, 4, dgvLex.Rows[i].Cells[4].Value);
                        }
                        encontrado = true;
                        break;
                    }
                }
                if (encontrado ==  false)
                {
                    dgvLex.Rows.Add(contador, linea, token, 4, valorIdentificador);
                    //dgvIden.Rows.Add(token, valorIdentificador, linea);
                    valorIdentificador++;
                    encontrado = false;
                }
            }
            contador++;
        }
        public string ConvertirToken(string token)
        {
            if (tablaSimbolos.TryGetValue(token, out int valor))
            {
                if (valor.ToString().StartsWith("8")) return 8.ToString();
                return valor.ToString();
            }
            else
            {
                if (constantes.IsMatch(token)) return 62.ToString();
                else if (constantesTL.IsMatch(token)) return 61.ToString();
                return 4.ToString();
            }
        }

        public int EncontrarIndiceK(string token)
        {
            if (token.StartsWith("8")) return 1;
            else if (token == "10") return 2;
            else if (token == "11") return 3;
            else if (token == "12") return 4;
            else if (token == "13") return 5;
            else if (token == "14") return 6;
            else if (token == "15") return 7;
            else if (token == "16") return 8;
            else if (token == "18") return 9;
            else if (token == "19") return 10;
            else if (token == "20") return 11;
            else if (token == "22") return 12;
            else if (token == "24") return 13;
            else if (token == "25") return 14;
            else if (token == "26") return 15;
            else if (token == "27") return 16;
            else if (token == "50") return 17;
            else if (token == "51") return 18;
            else if (token == "52") return 19;
            else if (token == "53") return 20;
            else if (token == "54") return 21;
            else if (token == "61") return 22;
            else if (token == "62") return 23;
            else if (token == "72") return 24;
            else if (token == "199") return 25;
            else if (token == "30") return 26;
            else return 0;
        }
        public int EncontrarIndiceX(string regla)
        {
            if (regla == "200") return 0;
            else if (regla == "201") return 1;
            else if (regla == "202") return 2;
            else if (regla == "203") return 3;
            else if (regla == "204") return 4;
            else if (regla == "205") return 5;
            else if (regla == "206") return 6;
            else if (regla == "207") return 7;
            else if (regla == "208") return 8;
            else if (regla == "209") return 9;
            else if (regla == "210") return 10;
            else if (regla == "211") return 11;
            else if (regla == "212") return 12;
            else if (regla == "213") return 13;
            else if (regla == "214") return 14;
            else if (regla == "215") return 15;
            else if (regla == "300") return 16;
            else if (regla == "301") return 17;
            else if (regla == "302") return 18;
            else if (regla == "303") return 19;
            else if (regla == "304") return 20;
            else if (regla == "305") return 21;
            else if (regla == "306") return 22;
            else if (regla == "307") return 23;
            else if (regla == "308") return 24;
            else if (regla == "309") return 25;
            else if (regla == "310") return 26;
            else if (regla == "311") return 27;
            else if (regla == "312") return 28;
            else if (regla == "313") return 29;
            else if (regla == "314") return 30;
            else if (regla == "315") return 31;
            else if (regla == "316") return 32;
            else if (regla == "317") return 33;
            else if (regla == "318") return 34;
            else if (regla == "319") return 35;
            else if (regla == "320") return 36;
            else if (regla == "321") return 37;
            else return -1;
        }
        #endregion
    }

}