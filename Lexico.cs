using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;

namespace Sintaxis_1
{
    public class Lexico : Token, IDisposable
    {
        int lines;
        const int F = -1;
        const int E = -2;
        StreamReader archivo;
        protected StreamWriter log;
        protected StreamWriter ensamblador;
        readonly int[,] TRAND = {
            {  0,  1,  2, 33,  1, 12, 14,  8,  9, 10, 11, 23, 16, 16, 18, 20, 21, 26, 25, 27, 29, 32, 34,  0,  F, 33  },
            {  F,  1,  1,  F,  1,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  2,  3,  5,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  E,  E,  4,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E  },
            {  F,  F,  4,  F,  5,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  E,  E,  7,  E,  E,  6,  6,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E  },
            {  E,  E,  7,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E  },
            {  F,  F,  7,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F, 13,  F,  F,  F,  F,  F, 13,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F, 13,  F,  F,  F,  F, 13,  F,  F,  F,  F,  F,  F, 15,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 17,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 19,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 19,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 22,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 24,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 24,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 24,  F,  F,  F,  F,  F,  F, 24,  F,  F,  F,  F,  F,  F,  F  },
            { 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 27, 28, 27, 27, 27, 27,  E, 27  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30  },
            {  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E,  E, 31,  E,  E,  E,  E,  E  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F, 32,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F  },
            {  F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F, 17, 36,  F,  F,  F,  F,  F,  F,  F,  F,  F, 35,  F,  F,  F  },
            { 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35, 35,  0, 35, 35  },
            { 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 37, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36  },
            { 36, 36, 36, 36, 36, 36, 35, 36, 36, 36, 36, 36, 37, 36, 36, 36, 36, 36, 36, 36, 36, 36,  0, 36, 36, 36  }
        };
        public Lexico()
        {
            log = new StreamWriter("./main.log");
            ensamblador = new StreamWriter("./main.asm");

            log.AutoFlush = true;
            ensamblador.AutoFlush = true;

            if (File.Exists("./main.cpp"))
            {
                lines = 1;
                archivo = new StreamReader("./main.cpp");
            }
            else
            {
                throw new Error("File main.cpp doesn´t exists", log);
            }
        }

        public Lexico(string archivo)
        {

            if (!(Path.GetExtension(archivo) == ".cpp"))
            {
                throw new Error("File doesn´t have correct extension .cpp");
            }

            string archivoName = Path.GetFileNameWithoutExtension(archivo);

            log = new StreamWriter("./" + archivoName + ".log")
            {
                AutoFlush = true
            };

            if (!File.Exists(archivo))
            {
                throw new Error("File " + archivo + " doesn´t exist", log);
            }

            ensamblador = new StreamWriter("./" + archivoName + ".asm")
            {
                AutoFlush = true
            };

            lines = 1;
            this.archivo = new StreamReader("./" + archivo);
        }

        public void Dispose()
        {
            archivo.Close();
            log.Close();
            ensamblador.Close();
        }

        private int Column(char c)
        {
            if (c == '\n')
            {
                return 23;
            }
            else if (EndOfFile())
            {
                return 24;
            }
            else if (char.IsWhiteSpace(c))
            {
                return 0;
            }
            else if (char.ToLower(c) == 'e')
            {
                return 4;
            }
            else if (char.IsLetter(c))
            {
                return 1;
            }
            else if (char.IsDigit(c))
            {
                return 2;
            }
            else if (c == '.')
            {
                return 3;
            }
            else if (c == '+')
            {
                return 5;
            }
            else if (c == '-')
            {
                return 6;
            }
            else if (c == ';')
            {
                return 7;
            }
            else if (c == '{')
            {
                return 8;
            }
            else if (c == '}')
            {
                return 9;
            }
            else if (c == '?')
            {
                return 10;
            }
            else if (c == '=')
            {
                return 11;
            }
            else if (c == '*')
            {
                return 12;
            }
            else if (c == '%')
            {
                return 13;
            }
            else if (c == '&')
            {
                return 14;
            }
            else if (c == '|')
            {
                return 15;
            }
            else if (c == '!')
            {
                return 16;
            }
            else if (c == '<')
            {
                return 17;
            }
            else if (c == '>')
            {
                return 18;
            }
            else if (c == '"')
            {
                return 19;
            }
            else if (c == '\'')
            {
                return 20;
            }
            else if (c == '#')
            {
                return 21;
            }
            else if (c == '/')
            {
                return 22;
            }
            return 25;
        }

        private void Classify(int state)
        {
            switch (state)
            {
                case 1: setClasification(Tipos.Indentificador); break;
                case 2: setClasification(Tipos.Numero); break;
                case 8: setClasification(Tipos.FinSentencia); break;
                case 9: setClasification(Tipos.InicioBloque); break;
                case 10: setClasification(Tipos.FinBloque); break;
                case 11: setClasification(Tipos.OperadorTernario); break;
                case 12:
                case 14: setClasification(Tipos.OperadorTermino); break;
                case 13: setClasification(Tipos.IncrementoTermino); break;
                case 15: setClasification(Tipos.Puntero); break;
                case 16:
                case 34: setClasification(Tipos.OperadorFactor); break;
                case 17: setClasification(Tipos.IncrementoFactor); break;
                case 18:
                case 20:
                case 29:
                case 32:
                case 33: setClasification(Tipos.Caracter); break;
                case 19:
                case 21: setClasification(Tipos.OperadorLogico); break;
                case 22:
                case 24:
                case 25:
                case 26: setClasification(Tipos.OperadorRelacional); break;
                case 23: setClasification(Tipos.Asignacion); break;
                case 27: setClasification(Tipos.Cadena); break;
            }
        }
        public void NextToken()
        {
            char c;
            string buffer = "";
            int state = 0;

            while (state >= 0)
            {

                c = (char)archivo.Peek();

                state = TRAND[state, Column(c)];
                Classify(state);

                if (state >= 0)
                {
                    archivo.Read();

                    if (c == '\n')
                    {
                        lines++;
                    }

                    if (state > 0)
                    {
                        buffer += c;
                    }
                    else
                    {
                        buffer = "";
                    }
                }
            }

            if (state == E)
            {
                string message;

                if (getClasification() == Tipos.Numero)
                {
                    message = "Lexical, a digit is missing";
                }
                else if (getClasification() == Tipos.Cadena)
                {
                    message = "Lexical, unclosed string";
                }
                else if (getClasification() == Tipos.Caracter)
                {
                    message = "Lexical, invalid character";
                }
                else
                {
                    message = "Lexical, Unclosed comment";
                }

                throw new Error(message, log, lines);
            }

            setContent(buffer);
            log.WriteLine(buffer + " ---- " + getClasification());
        }

        public void GetAllTokens()
        {
            while (!archivo.EndOfStream)
            {
                NextToken();
            }
        }

        public bool EndOfFile()
        {
            return archivo.EndOfStream;
        }
    }
}

/*

    EXPRESIÓN REGULAR
    Es un método formal el cual a través de una secuencia de 
    carácteres define un patrón de búsqueda.

    a) Reglas BNF
    b) Reglas BNF extendidas
    c) Operaciones aplicadas al lenguaje

        Operaciones Aplicadas al Lenguaje (OAF)

        1. Concactenación simple. (.)
        2. Concatenación exponencial. (^)
        3. Cerradura de Kleene. (*)
        4. Cerradura positiva. (+)
        5. Cerradura Epsilon. (?)
        6. Operador  (|)
        7. Parentesis, agrupación. ()

        L = {A, B, C, D, ..., Z, a, b, c, d, ... , z}
        D = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}

        1. L.D, LD
        2. L^3 = LLL, D^5 = DDDDD
        3. L* = Cero o más
        4. L+ = Una o más
        5. L? = Cero o una vez (Opcional)
        6. L | D = Letra o Digíto
        7. (LD)L? = Letra seguido de dígito y una letra opcional

        Producción Gramátical

        Clasificación del token -> Expresion regular

        Identificador -> L(L|D)*
        Número -> D+(.D+)?(E(+|-)?D+)?
        Fin de Sentencia -> ;
        Inicio de Bloque -> {
        Fin de Bloque -> }
        Operador Ternario -> ?
        Operador de Término -> +|-
        Operador de Factor -> *|/|%
        Incremento de Término -> (+|-)((+|-)|=)
        Incremento de Factor -> (*|/|%)=
        Operador Lógico -> &&||||!
        Operador Relacional -> >=?|<(>|=)?|==|!=
        Puntero -> ->
        Asignación -> =
        Cadena -> C*
        Caracter -> 'C'|#D*|Lambda

    AUTÓMATA
    Modelo matemático que representa una expresión regular a través
    de una grafo que consiste en un conjunto de estados bien definidos, 
    un estado inicial, un alfabeto de entrada y una función de transición.

*/