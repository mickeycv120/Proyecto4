/* 
//SECTION - Requerimientos
    //* 1) Indicar en el error léxico o sintáctico el número de línea y carácter de error
    //* 2) En el log colocar el nombre del archivo a compilar, la fecha y la hora:
           Ejemplo:
           Programa: prueba.cpp
           Fecha: 11/11/2024
           Hora: 3:25 p.m
    //* 3) Agregar el resto de asignaciones:
        ID = Expresion
        ID++
        ID--
        ID IncrementoTermino Expresion
        ID IncrementoFactor Expresion
        ID = Console.Read()
        ID = Console.ReadLine()
    //* 4) Emular el Console.Write() y Console.WriteLine()
    //* 5) Emular Console.Read() y Conesole.ReadLine()

//!SECTION

 */


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
        int linea;
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
            { 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 37, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36, 36,  E, 36  },
            { 36, 36, 36, 36, 36, 36, 35, 36, 36, 36, 36, 36, 37, 36, 36, 36, 36, 36, 36, 36, 36, 36,  0, 36,  E, 36  }
        };
        public Lexico()
        {
            log = new StreamWriter("./main.log");
            ensamblador = new StreamWriter("./main.asm");

            log.AutoFlush = true;
            ensamblador.AutoFlush = true;

            if (File.Exists("./main.cpp"))
            {
                linea = 1;
                archivo = new StreamReader("./main.cpp");
            }
            else
            {
                throw new Error("File main.cpp no existe", log);
            }

            string fecha = DateTime.Now.ToString("dd/MM/yyyy");
            string hora = DateTime.Now.ToString("hh:mm tt");
            log.WriteLine($"Fecha: {fecha} \nHora: {hora} \nArchivo: {archivo}");
        }

        public Lexico(string archivo)
        {
            if (!(Path.GetExtension(archivo) == ".cpp"))
            {
                throw new Error("Archivo no tiene la extensión .cpp", log, linea);
            }

            string archivoName = Path.GetFileNameWithoutExtension(archivo);

            log = new StreamWriter("./" + archivoName + ".log")
            {
                AutoFlush = true
            };

            if (!File.Exists(archivo))
            {
                throw new Error("Archivo " + archivo + " no existe", log);
            }

            ensamblador = new StreamWriter("./" + archivoName + ".asm")
            {
                AutoFlush = true
            };

            linea = 1;
            this.archivo = new StreamReader("./" + archivo);

            string fecha = DateTime.Now.ToString("dd/MM/yyyy");
            string hora = DateTime.Now.ToString("hh:mm tt");
            log.WriteLine($"Fecha: {fecha} \nHora: {hora} \nArchivo: {archivo}");
        }

        public void Dispose()
        {
            archivo.Close();
            log.Close();
            ensamblador.Close();
        }

        private int Column(char c)
        {
            return c switch
            {
                _ when char.IsWhiteSpace(c) => 0,
                _ when char.IsDigit(c) => 2,
                '.' => 3,
                'e' or 'E' => 4,
                _ when char.IsLetter(c) => 1,
                '+' => 5,
                '-' => 6,
                ';' => 7,
                '{' => 8,
                '}' => 9,
                '?' => 10,
                '=' => 11,
                '*' => 12,
                '%' => 13,
                '&' => 14,
                '|' => 15,
                '!' => 16,
                '<' => 17,
                '>' => 18,
                '"' => 19,
                '\'' => 20,
                '#' => 21,
                '/' => 22,
                _ when c == '\n' => 23,
                _ when finArchivo() => 24,
                _ => 25
            };

            /*  //ANCHOR - IF en orden
            
            if (char.IsWhiteSpace(c))
            {
                return 0;
            }
            else if (char.IsLetter(c))
            {
                return 1;
            }
            else if (char.IsDigit(c))
            {
                return 2;
            } */
            /* else if (c == '.')
            {
                return 3;
            }
            else if (char.ToLower(c) == 'e')
            {
                return 4;
            }
            else if(c=='+'){
                return 5;
            }
            else if(c=='-'){
                return 6;
            }
            else if(c==';'){
                return 7;
            }
            else if(c=='{'){
                return 8;
            }
            else if(c=='}'){
                return 9;
            }

 */
        }

        private void Clasifica(int estado)
        {
            switch (estado)
            {
                case 1: setClasificacion(Tipos.Identificador); break;
                case 2: setClasificacion(Tipos.Numero); break;
                case 8: setClasificacion(Tipos.FinSentencia); break;
                case 9: setClasificacion(Tipos.InicioBloque); break;
                case 10: setClasificacion(Tipos.FinBloque); break;
                case 11: setClasificacion(Tipos.OperadorTernario); break;
                case 12: setClasificacion(Tipos.OperadorTermino); break;
                case 13: setClasificacion(Tipos.OperadorTermino); break;
                case 14: setClasificacion(Tipos.OperadorTermino); break;
                case 15: setClasificacion(Tipos.Puntero); break;
                case 16: setClasificacion(Tipos.OperadorFactor); break;
                case 17: setClasificacion(Tipos.IncrementoFactor); break;
                case 18: setClasificacion(Tipos.Caracter); break;
                case 19: setClasificacion(Tipos.OperadorLogico); break;
                case 20: setClasificacion(Tipos.Caracter); break;
                case 21: setClasificacion(Tipos.OperadorLogico); break;
                case 22: setClasificacion(Tipos.OperadorRelacional); break;
                case 23: setClasificacion(Tipos.Asignacion); break;
                case 24: setClasificacion(Tipos.OperadorRelacional); break;
                case 25: setClasificacion(Tipos.OperadorRelacional); break;
                case 26: setClasificacion(Tipos.OperadorRelacional); break;
                case 27: setClasificacion(Tipos.Cadena); break;
                case 29: setClasificacion(Tipos.Caracter); break;
                case 32: setClasificacion(Tipos.Caracter); break;
                case 33: setClasificacion(Tipos.Caracter); break;
                case 34: setClasificacion(Tipos.OperadorFactor); break;
            }
        }
        public void NextToken()
        {
            char c;
            string buffer = "";
            int estado = 0;

            while (estado >= 0)
            {
                if (estado == 0)
                {
                    buffer = "";
                }

                c = (char)archivo.Peek();
                estado = TRAND[estado, Column(c)];
                Clasifica(estado);

                if (estado >= 0)
                {
                    archivo.Read();
                    if (c == '\n')
                    {
                        linea++;
                    }

                    if (c == '/' && (char)archivo.Peek() == '/')
                    {
                        while (c != '\n' && !finArchivo())
                        {
                            archivo.Read();
                            c = (char)archivo.Peek();
                        }

                        estado = 0;
                        buffer = "";
                        continue;
                    }

                    if (estado > 0)
                    {
                        buffer += c;
                    }
                }
            }

            if (estado == E)
            {
                string message;

                if (getClasificacion() == Tipos.Numero)
                {
                    message = "Léxico, se espera un dígito";
                }
                else if (getClasificacion() == Tipos.Cadena)
                {
                    message = "Léxico, se espera a que se cierre la cadena";
                }
                else if (getClasificacion() == Tipos.Caracter)
                {
                    message = "Léxico, caracter invalido";
                }
                else
                {
                    message = "No se ha cerrado el comentario";
                }

                throw new Error(message, log, linea);
            }

            setContent(buffer);

            if (getClasificacion() == Tipos.Identificador)
            {
                switch (getContent())
                {
                    case "char":
                    case "int":
                    case "float":
                        setClasificacion(Tipos.TipoDato);
                        break;
                    case "if":
                    case "else":
                    case "do":
                    case "while":
                    case "for":
                        setClasificacion(Tipos.PalabraReservada);
                        break;
                    default:
                        break;
                }
            }
            if (!finArchivo())
            {
                log.WriteLine(buffer + " = " + getClasificacion());
            }
        }

        public void GetAllTokens()
        {
            while (!archivo.EndOfStream)
            {
                NextToken();
            }
        }

        public bool finArchivo()
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