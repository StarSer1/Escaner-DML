using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class Parser
{
    private readonly Regex delimitadores;
    private readonly Regex reservadas;
    private readonly Regex constantes;
    private readonly Regex constantesTL;

    // Usamos colecciones concurrentes para seguridad en hilos
    public readonly ConcurrentBag<(string, string, int)> listaSelect = new ConcurrentBag<(string, string, int)>();
    public readonly ConcurrentBag<(string, string, int)> listaFrom = new ConcurrentBag<(string, string, int)>();
    public readonly ConcurrentBag<(string, string, string, int)> listaWhere = new ConcurrentBag<(string, string, string, int)>();

    public Parser(Regex delimitadores, Regex reservadas, Regex constantes, Regex constantesTL)
    {
        this.delimitadores = delimitadores;
        this.reservadas = reservadas;
        this.constantes = constantes;
        this.constantesTL = constantesTL;
    }

    public void LlenadoSelectsParallel(List<string> tokens)
    {
        // Preprocesar tokens: eliminar comillas simples y clonar
        var tokens2 = new List<string>(tokens);
        tokens2.RemoveAll(t => t == "'");

        // Variables compartidas para calcular la línea; no son perfectas en paralelismo
        var lineNumbers = new int[tokens2.Count];
        int linea = 1;
        for (int i = 0; i < tokens2.Count; i++)
        {
            lineNumbers[i] = linea;
            if (tokens2[i] == "\n") linea++;
        }

        // Flags por índice: estados de SELECT, FROM, WHERE
        var isSelectRegion = new bool[tokens2.Count];
        var isFromRegion = new bool[tokens2.Count];
        var isWhereRegion = new bool[tokens2.Count];

        bool sel = false, frm = false, whr = false;
        for (int i = 0; i < tokens2.Count; i++)
        {
            if (tokens2[i] == "SELECT") { sel = true; frm = whr = false; }
            else if (tokens2[i] == "FROM") { frm = true; sel = whr = false; }
            else if (tokens2[i] == "WHERE") { whr = true; sel = frm = false; }

            isSelectRegion[i] = sel;
            isFromRegion[i] = frm;
            isWhereRegion[i] = whr;
        }

        // Ejecución en paralelo
        Parallel.For(0, tokens2.Count, i =>
        {
            try
            {
                if (tokens2[i] == "\n") return; // solo incrementa línea

                int ln = lineNumbers[i];

                // WHERE
                if (isWhereRegion[i])
                {
                    if (i + 1 < tokens2.Count && tokens2[i + 1] == ".")
                    {
                        // X . Y
                        if (i - 1 >= 0 && tokens2[i - 1] != "=" && i + 3 < tokens2.Count && tokens2[i + 3] != "IN")
                        {
                            string tipo;
                            if (constantes.IsMatch(tokens2[i + 4])) tipo = "CHAR";
                            else if (constantesTL.IsMatch(tokens2[i + 4])) tipo = "NUMERIC";
                            else tipo = "atributo";
                            listaWhere.Add((tokens2[i], tokens2[i + 2], tipo, ln));
                        }
                    }
                    else if (i + 1 < tokens2.Count && tokens2[i + 1] == "=")
                    {
                        if (i - 1 >= 0 && tokens2[i - 1] != "=")
                        {
                            string tipo;
                            if (constantes.IsMatch(tokens2[i + 2])) tipo = "CHAR";
                            else if (constantesTL.IsMatch(tokens2[i + 2])) tipo = "NUMERIC";
                            else tipo = "atributo";
                            listaWhere.Add(("", tokens2[i], tipo, ln));
                        }
                    }
                }

                // FROM
                if (isFromRegion[i] && !delimitadores.IsMatch(tokens2[i]))
                {
                    string tempAlias = "";
                    if (i + 1 < tokens2.Count
                        && !delimitadores.IsMatch(tokens2[i + 1])
                        && !reservadas.IsMatch(tokens2[i + 1])
                        && tokens2[i + 1] != "\n")
                    {
                        tempAlias = tokens2[i + 1];
                    }
                    listaFrom.Add((tokens2[i], tempAlias, ln));
                }

                // SELECT
                if (isSelectRegion[i] && tokens2[i] != "DISTINCT" && !delimitadores.IsMatch(tokens2[i]))
                {
                    if (i + 1 < tokens2.Count && tokens2[i + 1] == ".")
                    {
                        listaSelect.Add((tokens2[i], tokens2[i + 2], ln));
                    }
                    else
                    {
                        listaSelect.Add(("", tokens2[i], ln));
                    }
                }
            }
            catch
            {
                // Manejo silencioso de errores
            }
        });
    }
}
