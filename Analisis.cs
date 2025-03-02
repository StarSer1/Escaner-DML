﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        Regex constantes = new Regex(@"\b\d+\b");

        Stack<string> pila = new Stack<string>();
        string[,] TablaSintac =
        {
//             4     8             10           11    12    13    14    15    50    51    53    54    61    62    72    99
            { null, null, "10 301 11 306 310", null, null, null, null, null, null, null, null, null, null, null, null, null}, // 300
            { "302", null, null, null, null, null, null, null, null, null, null, null, null, null, "72", null }, // 301
            { "304 303", null, null, null, null, null, null, null, null, null , null, null, null, null, null, null}, // 302 
            { null, null, null, "99", null, null, null, null, "50 302", null, null, null, null, null, null,  "99"}, // 303
            { "4 305", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null }, // 304
            { null , "99", null, "99", null, "99", "99", "99", "99", "51 4", "99", null, null, null, null, "99" }, // 305
            { "308 307", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null}, // 306
            {  null, null, null, null, "99", null, null, null, "50 306", null, "99", null, null, null, null, "99"},
            { "4 309", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null},
            { "4", null, null, null, "99", null, null, null, "99", null, "99", null, null, null, null, "99"},
            { null, null, null, null, "12 311", null, null, null, null, null, "99", null, null, null, null, "99" },
            { "313 312", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, "317 311", "317 311", null, null, "99", null, null, null, null, "99" },
            { "304 314", null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
            { null, "315 316", null, null, null, "13 52 300 53", null, null, null, null, null, null, null, null, null, null },
            { null, "8", null, null, null, null, null, null, null, null, null, null, null, null, null, null },
            { "304", null, null, null, null, null, null, null, null, null, null, "54 318 54", "319", null, null, null },
            { null, null, null, null, null, null, "14", "15", null, null, null, null, null, null, null, null },
            { null, null, null, null, null, null, null, null, null, null, null, null, null, "62", null, null },
            { null, null, null, null, null, null, null, null, null, null, null, null, "61", null, null, null }
        };
                                                                                                                                            
        int contador = 1;
        int valorIdentificador = 401;
        int valorConstante = 600;
        int acumuladorParentesisAbierto = 0;
        int acumuladorComillas = 0;
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
            {"", 99 }
        };
        List<string> tokens = new List<string>();
        public List<string> Analizador(RichTextBox texto, DataGridView dgvCons, DataGridView dgvIden, DataGridView dgvLex, TextBox txtError)
        {
            string cadena = "";
            int linea = 1;
            texto.Text = texto.Text.ToUpper() + " ";
            bool comillas = false;
            bool sigo = false;
            bool sigoRelacional = false;
            for (int i = 0; i<texto.TextLength; i++)
            {
                string c = texto.Text[i].ToString();
                if (!delimitadores.IsMatch(c))
                {
                    if (sigoRelacional)
                    {
                        if (relacionales.IsMatch(c) && c != " ")
                        {
                            cadena += c;
                            tokens.Add(cadena);
                            if (cadena != "")
                                MostrarDgv(dgvCons, dgvIden, dgvLex, tokens.Last(), linea);
                            cadena = "";
                            c = "";
                        }
                        sigoRelacional = false;
                    }
                    if (c != " " && !relacionales.IsMatch(c) && !constantes.IsMatch(c) || comillas == true)
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
                                MostrarDgv(dgvCons, dgvIden, dgvLex, tokens.Last(), linea);
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
                                        MostrarDgv(dgvCons, dgvIden, dgvLex, tokens.Last(), linea);
                                    cadena = "";
                                }
                            }
                        }
                    }
                    if (constantes.IsMatch(c))
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
                                    MostrarDgv(dgvCons, dgvIden, dgvLex, tokens.Last(), linea);
                                cadena = "";
                            }
                            else if (constantes.IsMatch(siguienteChar.ToString()) || char.IsLetter(siguienteChar))
                            {
                                cadena += c;
                            }
                            else if (delimitadores.IsMatch(siguienteChar.ToString()))
                            {
                                cadena += c;
                            }
                        }
                    }
                    if ((c == " " || c == "\n") && comillas == false)
                    {
                        if (empezoComilla == false)
                        {
                            if (reservadas.IsMatch(cadena))
                            {
                                tokens.Add(cadena);
                                tokens.Add(c);
                                if (cadena != "")
                                    MostrarDgv(dgvCons, dgvIden, dgvLex, tokens.Last(), linea);
                                cadena = "";
                            }
                            else if (tokens.Count != 0)
                            {
                                if (reservadas.IsMatch(tokens.Last()) && cadena != "")
                                {
                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvCons, dgvIden, dgvLex, tokens.Last(), linea);
                                    sigo = true;
                                    cadena = "";

                                }
                                if (relacionales.IsMatch(tokens.Last()) || relacionales.IsMatch(cadena))
                                {
                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvCons, dgvIden, dgvLex, tokens.Last(), linea);
                                    cadena = "";

                                }
                                if (delimitadores.IsMatch(tokens.Last()))
                                {
                                    if (c == "(")
                                        acumuladorParentesisAbierto++;
                                    else if (c == ")")
                                        acumuladorParentesisAbierto--;

                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvCons, dgvIden, dgvLex, tokens.Last(), linea);
                                    cadena = "";
                                }
                                else
                                {
                                    tokens.Add(cadena);
                                    if (cadena != "")
                                        MostrarDgv(dgvCons, dgvIden, dgvLex, tokens.Last(), linea);
                                    cadena = "";
                                }
                            }
                        }
                        else
                        {
                            cadena += " ";
                        }
                    }
                }
                else 
                {
                    if ("'’‘".Contains(c))
                    {
                        empezoComilla = !empezoComilla;
                    }
                    if (c == "(")
                        acumuladorParentesisAbierto++;
                    else if (c == ")")
                        acumuladorParentesisAbierto--;

                    if (c == "'" || c == "’" || c == "‘")
                        acumuladorComillas++;


                    if (comillas == false && Regex.IsMatch(c, @"['’‘]")) comillas = true;
                    else if (cadena != "" && comillas == true)
                    {
                        tokens.Add(cadena);
                        MostrarDgv(dgvCons, dgvIden, dgvLex, tokens.Last() + "~", linea);
                        comillas = false;
                    }
                    else if (cadena != "" && (c == ")" || c == ",") && constantes.IsMatch(cadena))
                    {
                        tokens.Add(cadena);
                        MostrarDgv(dgvCons, dgvIden, dgvLex, tokens.Last() + "~", linea);
                        tokens.Add(c);
                        MostrarDgv(dgvCons, dgvIden, dgvLex, c, linea);
                    }
                    else
                    {                       
                        tokens.Add(cadena);
                        if(tokens.Last() != "")
                            MostrarDgv(dgvCons, dgvIden, dgvLex, tokens.Last(), linea);
                        tokens.Add(c);
                        if (c != "")
                            MostrarDgv(dgvCons, dgvIden, dgvLex, c, linea);
                    }
                    cadena = "";



                }
                //tokens.RemoveAll(item => string.IsNullOrEmpty(item));
                if (c == "\n")
                    linea++;
            }
            Errores.ErrorParentesis(dgvCons, dgvIden, dgvLex, txtError, acumuladorParentesisAbierto);
            Errores.ErroresComillas(dgvCons, dgvIden, dgvLex, txtError, acumuladorComillas);
            tokens = RemoverDuplicadosVacios(tokens);
            return tokens;
        }

        public void Sintaxis(List<string> tokens)
        {
            pila.Push("$");
            pila.Push("300");
            tokens.Add("$");
            int apun = 0;
            string equis;
            do
            {
                string X = pila.Pop();
                string K = tokens[apun];
                if (!X.StartsWith("3") || X == "$")
                {
                    if (X == ConvertirToken(K))
                        apun++;
                    else
                    {
                        MessageBox.Show("ERROR: {Cuando X y K son Terminales, pero X!=K. Tomar el valor de X}");
                        //ERROR: {Cuando X y K son Terminales, pero X!=K. Tomar el valor de X}
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
                        MessageBox.Show("ERROR: {Cuando TS[X,K]=Celda Vacía. Tomar el valor de los primeros}");
                        // ERROR: {Cuando TS[X,K]=Celda Vacía. Tomar el valor de los primeros} 
                    }
                }
                equis = X;
            }
            while (equis != "$");
        }
        public void MostrarDgv(DataGridView dgvCons, DataGridView dgvIden, DataGridView dgvLex, string token, int linea)
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
                dgvCons.Rows.Add(contador, token.Remove(token.Length - 1), 62, valorConstante);
                valorConstante++;
            }
            else if (constantes.IsMatch(token))
            {
                dgvLex.Rows.Add(contador, linea, token, 6, valorConstante);
                if (Regex.IsMatch(token, @"^\d+$"))
                    dgvCons.Rows.Add(contador, token, 61, valorConstante);
                else if (Regex.IsMatch(token, @"^[a-zA-Z0-9]+$"))
                    dgvCons.Rows.Add(contador, token, 62, valorConstante);
                valorConstante++;
            }
            else if (relacionales.IsMatch(token))
            {
                dgvLex.Rows.Add(contador, linea, token, 8, valor);
            }
            else
            {
                bool encontrado = false;
                // Recorrer el array por si lo encuentra
                for (int i = 0; i < dgvIden.Rows.Count; i++)
                {
                    if (dgvIden.Rows[i].Cells[0].Value != null && dgvIden.Rows[i].Cells[0].Value.ToString() == token)
                    {
                        if (dgvIden.Rows[i].Cells[2].Value != null)
                        {
                            dgvIden.Rows[i].Cells[2].Value += ", "+linea.ToString();
                        }
                        encontrado = true;
                    }
                }

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
                    dgvIden.Rows.Add(token, valorIdentificador, linea);
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
                return valor.ToString();
            }
            else
            {
                return 4.ToString();
            }
        }

        public int EncontrarIndiceK(string token)
        {
            if (token == "8") return 1;
            else if (token == "10") return 2;
            else if (token == "11") return 3;
            else if (token == "12") return 4;
            else if (token == "13") return 5;
            else if (token == "14") return 6;
            else if (token == "15") return 7;
            else if (token == "50") return 8;
            else if (token == "51") return 9;
            else if (token == "53") return 10;
            else if (token == "54") return 11;
            else if (token == "61") return 12;
            else if (token == "62") return 13;
            else if (token == "72") return 14;
            else if (token == "99") return 15;
            else return 0;
        }
        public int EncontrarIndiceX(string regla)
        {
            if (regla == "300") return 0;
            else if (regla == "301") return 1;
            else if (regla == "302") return 2;
            else if (regla == "303") return 3;
            else if (regla == "304") return 4;
            else if (regla == "305") return 5;
            else if (regla == "306") return 6;
            else if (regla == "307") return 7;
            else if (regla == "308") return 8;
            else if (regla == "309") return 9;
            else if (regla == "310") return 10;
            else if (regla == "311") return 11;
            else if (regla == "312") return 12;
            else if (regla == "313") return 13;
            else if (regla == "314") return 14;
            else if (regla == "315") return 15;
            else if (regla == "316") return 16;
            else if (regla == "317") return 17;
            else if (regla == "318") return 18;
            else if (regla == "319") return 19;
            else return -1;
        }
        public List<string> RemoverDuplicadosVacios(List<string> lista)
        {
            List<string> nuevaLista = new List<string>();
            bool ultimoFueVacio = false;

            foreach (string item in lista)
            {
                if (item == "")
                {
                    if (!ultimoFueVacio)
                    {
                        nuevaLista.Add(item);
                        ultimoFueVacio = true;
                    }
                }
                else
                {
                    nuevaLista.Add(item);
                    ultimoFueVacio = false;
                }
            }

            return nuevaLista;
        }
    }

}