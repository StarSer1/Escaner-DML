using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
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

        public Analisis(bool error)
        {
            this.errorActivado = error;
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
        string[,] TablaSintac =
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

        int contador = 1;
        int valorIdentificador = 401;
        int valorConstante = 600;
        int acumuladorParentesisAbierto = 0;
        int acumuladorComillas1 = 0;
        int acumuladorComillas2 = 0;
        int acumuladorComillas3 = 0;
        bool empezoComilla = false;
        Errores Errores = new Errores();

        Dictionary<string, int> tablaSimbolos = new Dictionary<string, int>
        {
            // Palabras Reservadas (1)
            { "SELECT", 10 }, { "FROM", 11 }, { "WHERE", 12 }, { "IN", 13 },
            { "AND", 14 }, { "OR", 15 }, { "CREATE", 16 }, { "TABLE", 17 },
            { "CHAR", 18 }, { "NUMERIC", 19 }, { "NOT", 20 }, { "NULL", 21 },
            { "CONSTRAINT", 22 }, { "KEY", 23 }, { "PRIMARY", 24 }, { "FOREIGN", 25 },
            { "REFERENCES", 26 }, { "INSERT", 27 }, { "INTO", 28 }, { "VALUES", 29 },

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
        public List<string> Analizador(RichTextBox texto, DataGridView dgvLex, TextBox txtError)
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
                if ( c == "\n")
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
                return tokens;
        }

        public void Sintaxis(List<string> tokens, TextBox texto)
        {
            try
            {
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
                List<string> tokensConN = tokens;
                tokens = tokens.Where(s => s != "\n").ToList();
                tokens.Add("$");
                int apun = 0;
                int apunN = 0;
                string equis;
                do
                {
                    string X = pila.Pop();
                    string K = tokens[apun];
                    if (apunN < tokensConN.Count())
                        KparaN = tokensConN[apunN];
                    if (KparaN != "\n")
                    {
                        if (!Regex.IsMatch(X, @"^[32]\d{2}$") || X == "199")
                        {
                            if (X == ConvertirToken(K))
                            {
                                apun++;
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
                                //if (reservadas.IsMatch(tokens[apun - 1]) && tokens[apun].StartsWith("'") && tokens[apun].EndsWith("'"))
                                //{
                                //    Errores.ErrorIdentificador(texto, lineas);
                                //    break;
                                //}
                                //else if ((relacionales.IsMatch(tokens[apun - 1]) || (relacionales.IsMatch(tokens[apun - 2])) && (!tokens[apun].StartsWith("'") && !tokens[apun].EndsWith("'"))))
                                //{
                                //    Errores.ErrorConstante(texto, lineas);
                                //    break;
                                //}
                                //else if (operadores.IsMatch(tokens[apun - 1]))
                                //{
                                //    Errores.ErrorOperador(texto, lineas);
                                //    break;
                                //}

                                //else if (4.ToString() == ConvertirToken(tokens[apun]) && !relacionales.IsMatch(tokens[apun - 1]))
                                //{
                                //    Errores.ErrorOperadorRelacional(texto, lineas);
                                //}
                                //else if (tokens[apun] == "(")
                                //{
                                //    if (!reservadas.IsMatch(tokens[apun - 1]))
                                //    {
                                //        Errores.ErrorPalabraReservada(texto, lineas);
                                //        break;
                                //    }
                                //}
                                //else if (4.ToString() == ConvertirToken(tokens[apun]))
                                //{
                                //    if (delimitadores.IsMatch(tokens[apun + 1]) && !reservadas.IsMatch(tokens[apun - 1]))
                                //    {
                                //        Errores.ErrorPalabraReservada(texto, lineas);
                                //        break;
                                //    }
                                //}
                                //else if (relacionales.IsMatch(tokens[apun]))
                                //{
                                //    if (4.ToString() != ConvertirToken(tokens[apun + 1]))
                                //    {
                                //        Errores.ErrorIdentificador(texto, lineas);
                                //        break;
                                //    }
                                //}
                                //else if (Errores.ErroresComillas(texto, acumuladorComillas, lineas))
                                //{
                                //    texto.Text = "Error 2:205: Linea " + (lineas - 1) + " Se esperaba Delimitador";
                                //}
                                //else
                                //{
                                //    Errores.ErrorSintactico(texto, lineas);
                                //    break;
                                //}
                            }
                        }
                        else
                        {
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
                                else if (tokens[apun-1] == "," && (constantes.IsMatch(tokens[apun-3]) || constantesTL.IsMatch(tokens[apun - 2])))
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
                                    if (apun + 2 >= tokens.Count)
                                    {
                                        Errores.ErrorIdentificador(texto, lineas);
                                        break;
                                    }
                                    if (4.ToString() != ConvertirToken(tokens[apun + 1]))
                                    {
                                        Errores.ErrorIdentificador(texto, lineas);
                                        break;
                                    }
                                }
                                else if (operadores.IsMatch(tokens[apun]) || relacionales.IsMatch(tokens[apun]))
                                {
                                    if (apun + 2 >= tokens.Count)
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
                    equis = X;
                }
                while (equis != "199");
                if (error == false)
                {
                    Errores.SinError(texto, lineas);
                }
                pila.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #region Utilidades
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