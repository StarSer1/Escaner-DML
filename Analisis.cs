using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Escaner_DML
{
    public class Analisis
    {
        Regex reservadas = new Regex(@"\b(SELECT|FROM|WHERE|IN|AND|OR|CREATE|TABLE|CHAR|NUMERIC|NOT|NULL|CONSTRAINT|KEY|PRIMARY|FOREIGN|REFERENCES|INSERT|INTO|VALUES)\b");
        Regex delimitadores = new Regex(@"[.,()']");
        Regex operadores = new Regex(@"[+\-*/]");
        Regex relacionales = new Regex(@"(>=|<=|>|<|=)");

        List<string> tokens = new List<string>();

        public void Analizador(RichTextBox texto)
        {
            string cadena = "";
            texto.Text = texto.Text.ToUpper() + " ";
            bool sigo = false;
            bool sigoRelacional = false;
            for (int i = 0; i<texto.TextLength; i++)
            {
                string c = texto.Text[i].ToString();
                if (!delimitadores.IsMatch(c))
                {
                    if (c != " ")
                    {
                        if (c != "\n")
                            cadena += c;
                    }
                    if (relacionales.IsMatch(c))
                    {
                        if (sigoRelacional)
                        {
                            tokens.Add(cadena);
                            cadena = "";
                            sigoRelacional = false;
                            InsertarEspacio(texto.Text, i + 1);
                        }
                        else
                        {
                            tokens.Add(cadena.Remove(cadena.Length - 1));
                            cadena = c;
                            sigoRelacional = true;
                        }
                    }
                    if (c == " " || c == "\n")
                    {
                        if (reservadas.IsMatch(cadena))
                        {
                            tokens.Add(cadena);
                            cadena = "";
                        }
                        else if (tokens.Count != 0)
                        {
                            if (reservadas.IsMatch(tokens.Last()) && cadena != "")
                            {
                                tokens.Add(cadena);
                                sigo = true;
                                cadena = "";

                            }
                            if (relacionales.IsMatch(tokens.Last()) || relacionales.IsMatch(cadena))
                            {
                                tokens.Add(cadena);
                                cadena = "";

                            }
                            if (delimitadores.IsMatch(tokens.Last()) && cadena != "")
                            {
                                tokens.Add(cadena);
                                cadena = "";
                            }
                            else
                            {

                            }
                        }
                    }
                }
                else
                {
                    tokens.Add(cadena);
                    cadena = "";
                    tokens.Add(c);
                }
                
            }
        }
        public string InsertarEspacio(string palabra, int indice)
        {
            string nuevaPalabra = "";
            int i = 0;
            while (i < palabra.Length)
            {
                if (i != indice)
                {
                    nuevaPalabra += palabra[i];
                    i++;
                }
                else
                {
                    nuevaPalabra += " "+ palabra[i];
                    i++;
                }
            }
            return nuevaPalabra;
        }
    }
}
