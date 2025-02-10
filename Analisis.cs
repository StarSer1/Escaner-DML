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
        Regex constantes = new Regex(@"\b\d+\b");

        List<string> tokens = new List<string>();

        public List<string> Analizador(RichTextBox texto)
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
                    if (sigoRelacional)
                    {
                        if (relacionales.IsMatch(c) && c != " ")
                        {
                            cadena += c;
                            tokens.Add(cadena);
                            cadena = "";
                            c = "";
                        }
                        sigoRelacional = false;
                    }
                    if (c != " " && !relacionales.IsMatch(c) && !constantes.IsMatch(c))
                    {
                        if (c != "\n")
                            cadena += c;
                    }
                    if (relacionales.IsMatch(c))
                    {
                        if (c != " ")
                        {
                            tokens.Add(cadena);
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
                                cadena = "";
                            }
                            else if (constantes.IsMatch(siguienteChar.ToString()))
                            {
                                cadena += c;
                            }
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
            return tokens;
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


/*if (i + 1 < texto.TextLength)
{
    char siguienteChar = texto.Text[i + 1];

    //si el siguiente es espacio en blanco
    if (siguienteChar.ToString() == " ")
    {
        cadena = "";
    }
    else if (constantes.IsMatch(siguienteChar.ToString()))
    {
        cadena += c;
    }
    else
    {
        tokens.Add(cadena);
        cadena = "";
    }
}*/