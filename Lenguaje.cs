/* 
//SECTION - Requerimientos
    //* 1) Concatenaciones * 
    //* 2) Inicializar una variable desde la declaración * 
    //* 3) Evaluar las expresiones matemáticas *
    //* 4) Levantar si en el Console.ReadLine() no ingresan números *
    //* 5) Modificar la variable con el resto de operadores (Incremento de factor y término) *
    //* 6) Hacer que funcione el else *



//!SECTION

 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sintaxis_1
{
    //SECTION - Constructores
    public class Lenguaje : Sintaxis
    {
        //NOTE - No volver a tocar el Stack
        Stack<float> s; //
        List<Variable> l; //Para revisar la lista de variables ()
        public Lenguaje() : base()
        {
            s = new Stack<float>();
            l = new List<Variable>();
        }

        public Lenguaje(string name) : base(name)
        {
            s = new Stack<float>();
            l = new List<Variable>();
        }
        //!SECTION

        //SECTION - displayStack y displayList
        private void displayStack()
        {
            Console.WriteLine("Contenido del Stack");
            foreach (float elemento in s)
            {
                Console.WriteLine(elemento);
            }
        }

        private void displayList()
        {
            log.WriteLine("Lista de Variables");
            foreach (Variable elemento in l)
            {
                log.WriteLine($"{elemento.getNombre()} {elemento.getTipoDato()} {elemento.getValor()}");
            }
        }
        //!SECTION
        // ? Cerradura epsilon
        //Programa  -> Librerias? Variables? Main
        //SECTION - Programa
        public void Programa()
        {
            if (getContenido() == "using")
            {
                Librerias();
            }

            if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }

            Main();
            displayList();
        }
        //!SECTION


        //Librerias -> using ListaLibrerias; Librerias?
        //SECTION - Librerias
        private void Librerias()
        {
            match("using");
            ListaLibrerias();
            match(";");

            if (getContenido() == "using")
            {
                Librerias();
            }
        }
        //!SECTION

        //Variables -> tipo_dato Lista_identificadores; Variables?
        //SECTION - Variables
        private void Variables()
        {
            Variable.TipoDato t = Variable.TipoDato.Char;

            switch (getContenido())
            {
                case "int": t = Variable.TipoDato.Int; break;
                case "float": t = Variable.TipoDato.Float; break;
                case "string": t = Variable.TipoDato.String; break;
                case "bool": t = Variable.TipoDato.Boolean; break;
                case "char": t = Variable.TipoDato.Char; break;
            }

            match(Tipos.TipoDato);
            ListaIdentificadores(t);
            match(";");

            if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
        }
        //!SECTION

        //ListaLibrerias -> identificador (.ListaLibrerias)?
        //SECTION - ListaLibrerias
        private void ListaLibrerias()
        {
            match(Tipos.Identificador);

            if (getContenido() == ".")
            {
                match(".");
                ListaLibrerias();
            }
        }
        //!SECTION

        // ListaIdentificadores -> identificador (=Expresion)? (,ListaIdentificadores)?
        //SECTION - ListaIdentificadores
        private void ListaIdentificadores(Variable.TipoDato t)
        {
            if (l.Find(variable => variable.getNombre() == getContenido()) != null)
            {
                /* throw new Error("Sintaxis: la variable " + getContenido() + " ya existe", log, linea, column); */
                throw new Error("Sintaxis: la variable " + getContenido() + " ya existe", log, linea);
            }

            l.Add(new Variable(t, getContenido()));
            string variableActual = getContenido();
            //Variable a l.Find(variable => variable.getNombre() == variableActual);

            Console.WriteLine(variableActual);

            match(Tipos.Identificador);


            if (getContenido() == "=")
            {
                match("=");
                if (getContenido() == "Console")
                {
                    //int read;

                    match("Console");
                    match(".");

                    string opc = getContenido();
                    match(opc);

                    /* if (getContenido() == "Read")
                    {
                        read = 1;
                        match("Read");
                    }
                    else
                    {
                        read = 2;
                        match("ReadLine");
                    } */

                    match("(");
                    match(")");
                    string txt;
                    int num;

                    switch (opc)
                    {

                        // ? Buscar una forma de optimizar para no repetir el código innecesario

                        case "Read":
                            txt = Console.Read().ToString();
                            Variable? v = l.Find(variable => variable.getNombre() == variableActual);
                            v.setValor(v.getValor());

                            break;
                        case "ReadLine":
                            txt = Console.ReadLine();
                            if (!int.TryParse(txt, out num))
                            {
                                throw new Exception("Error: La entrada no es un número válido.");
                            }

                            Variable? m = l.Find(variable => variable.getNombre() == variableActual);
                            m.setValor(num);

                            break;
                        default:
                            throw new Exception("Argumento inválido");
                    }

                }
                else
                {
                    Expresion();
                    Variable? v = l.Find(variable => variable.getNombre() == variableActual);
                    v.setValor(s.Pop());
                }
            }

            if (getContenido() == ",")
            {
                match(",");
                ListaIdentificadores(t);
            }
        }
        //!SECTION

        // BloqueInstrucciones -> { listaIntrucciones? }
        //SECTION - BloqueInstrucciones
        private void BloqueInstrucciones(bool excecute)
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones(excecute);
            }
            match("}");
        }
        //!SECTION
        // ListaInstrucciones -> Instruccion ListaInstrucciones?
        //SECTION - ListaInstrucciones
        private void ListaInstrucciones(bool excecute)
        {
            Instruccion(excecute);

            if (getContenido() != "}")
            {
                ListaInstrucciones(excecute);
            }
        }
        //!SECTION

        // Instruccion -> console | If | While | do | For | Variables | Asignación
        //SECTION - Instruccion
        private void Instruccion(bool excecute)
        {

            //ANCHOR - PRUEBA EN SWITCH
            switch (getContenido())
            {
                case "Console":
                    console(excecute);
                    break;
                case "if":
                    If(excecute);
                    break;
                case "while":
                    While();
                    break;
                case "do":
                    Do();
                    break;
                case "for":
                    For();
                    break;
                default:
                    if (getClasificacion() == Tipos.TipoDato)
                    {
                        Variables();
                    }
                    else
                    {
                        Asignacion();
                        match(";");
                    }
                    break;
            }
        }
        //!SECTION
        /*
        Agregar el resto de asignaciones:
        ID = Expresion
        ID++
        ID--
        ID IncrementoTermino Expresion
        ID IncrementoFactor Expresion
        ID = Console.Read()
        ID = Console.ReadLine()
        */
        //SECTION - Asignacion
        private void Asignacion()
        {
            Variable? v = l.Find(variable => variable.getNombre() == getContenido());
            string variableActual = getContenido();
            match(Tipos.Identificador);

            if (v == null)
            {
                /* throw new Error("Sintaxis: la variable " + getContenido() + " no está definida ", logger, line, column); */
                throw new Error("Sintaxis: la variable " + getContenido() + " no está definida ", log, linea);
            }
            //Console.Write(getContenido() + " = ");


            /* if (getContenido() == "=")
            {
                match("=");

                if (getContenido() == "Console")
                {
                    //int read;

                    match("Console");
                    match(".");

                    string opc = getContenido();
                    match(opc);

                    match("(");
                    match(")");

                    switch (opc)
                    {
                        case "Read":
                            Console.Read();
                            break;
                        case "ReadLine":
                            Console.ReadLine();
                            break;
                        default:
                            throw new Exception("Argumento inválido");
                    }

                }
                else
                {
                    Expresion();
                    v.setValor(s.Pop());
                }
            }
            
            } */

            switch (getContenido())
            {
                case "=":
                    match("=");
                    if (getContenido() == "Console")
                    {
                        //int read;

                        match("Console");
                        match(".");

                        string opc = getContenido();
                        match(opc);

                        match("(");
                        match(")");

                        string input;
                        int value;

                        // ? Buscar una forma de optimizar para no repetir el código innecesario
                        //SECTION - Ideas
                        /*
                        1) Crear otro variable y utilizarlo al salir del switch //NOTE No funcionó 
                        
                         */

                        switch (opc)
                        {
                            case "Read":
                                input = Console.Read().ToString();
                                if (!int.TryParse(input, out value))
                                {
                                    throw new Exception("Error: La entrada no es un número válido.");
                                }
                                Variable? vr = l.Find(variable => variable.getNombre() == variableActual);
                                vr.setValor(value);

                                break;
                            case "ReadLine":
                                input = Console.ReadLine();
                                if (!int.TryParse(input, out value))
                                {
                                    throw new Exception("Error: La entrada no es un número válido.");
                                }

                                Variable? m = l.Find(variable => variable.getNombre() == variableActual);
                                m.setValor(value);
                                break;
                            default:
                                throw new Exception("Argumento inválido");
                        }

                    }
                    else
                    {
                        Expresion();
                        v.setValor(s.Pop());
                        displayList();
                    }
                    break;
                case "++":
                    match("++");
                    v.setValor(v.getValor() + 1);
                    displayList();
                    break;
                case "--":
                    match("--");
                    v.setValor(v.getValor() - 1);
                    break;
                case "+=":
                    match("+=");
                    Expresion();
                    v.setValor(v.getValor() + s.Pop());
                    break;
                case "-=":
                    match("-=");
                    Expresion();
                    v.setValor(v.getValor() - s.Pop());
                    break;
                case "*=":
                    match("*=");
                    Expresion();
                    v.setValor(v.getValor() * s.Pop());
                    break;
                case "/=":
                    match("/=");
                    Expresion();
                    v.setValor(v.getValor() / s.Pop());
                    break;
            }
        }
        //!SECTION
        // If -> if (Condicion) bloqueInstrucciones | instruccion
        // (else bloqueInstrucciones | instruccion)?
        //SECTION - If
        private void If(bool excecute2)
        {
            match("if");
            match("(");
            bool execute = Condicion() && excecute2;
            Console.WriteLine("\n" + execute);
            match(")");
            if (getContenido() == "{")
            {
                BloqueInstrucciones(execute);
            }
            else
            {
                Instruccion(execute);
            }

            if (getContenido() == "else")
            {
                match("else");

                if (getContenido() == "{")
                {
                    BloqueInstrucciones(!execute && excecute2);
                }
                else
                {
                    Instruccion(!execute && excecute2);
                }
            }
        }
        //!SECTION
        // Condicion -> Expresion operadorRelacional Expresion
        //SECTION - Condicion
        private bool Condicion()
        {
            Expresion();
            float v1 = s.Pop();
            String operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion();
            float v2 = s.Pop();

            switch (operador)
            {
                case ">":
                    return v1 > v2;
                case "<":
                    return v1 < v2;
                case ">=":
                    return v1 >= v2;
                case "<=":
                    return v1 <= v2;
                case "==":
                    return v1 == v2;
                default:
                    return v1 != v2;
            }
        }
        //!SECTION
        // While -> while(Condicion) bloqueInstrucciones | instruccion
        //SECTION - while
        private void While()
        {
            match("while");
            match("(");
            Condicion();
            match(")");

            if (getContenido() == "{")
            {
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
            }
        }
        //!SECTION
        // Do -> do 
        // bloqueInstrucciones | intruccion 
        // while(Condicion);
        //SECTION - Do
        private void Do()
        {
            match("do");

            if (getContenido() == "{")
            {
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
            }

            match("while");
            match("(");
            Condicion();
            match(")");
            match(";");
        }
        //!SECTION
        // For -> for(Asignacion; Condicion; Asignación) 
        // BloqueInstrucciones | Intruccion
        //SECTION - For
        private void For()
        {
            match("for");
            match("(");
            Asignacion();
            match(";");
            Condicion();
            match(";");
            Asignacion();
            match(")");

            if (getContenido() == "{")
            {
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
            }
        }
        //!SECTION
        // Console -> Console.(WriteLine|Write) (cadena concatenaciones?);
        //SECTION - Console
        //? Me da la impresión de que se puede optimizar - INVESTIGAR
        private void console(bool excecute)
        {
            bool console = false;
            bool isRead = false;
            string content = "";

            match("Console");
            match(".");

            switch (getContenido())
            {
                case "Write":
                    console = true;
                    match("Write");
                    break;
                case "Read":
                    isRead = true;
                    match("Read");
                    break;
                case "ReadLine":
                    isRead = true;
                    match("ReadLine");
                    break;
                default:
                    match("WriteLine");
                    break;
            }

            match("(");

            if (!isRead && getContenido() != ")")
            {

                if (getClasificacion() == Tipos.Cadena)
                {
                    if (excecute)
                    {
                        Console.Write(getContenido().ToString().Replace('"', ' '));
                    }
                    match(Tipos.Cadena);
                }
                else
                {
                    string nomV = getContenido();
                    match(Tipos.Identificador);
                    Variable v = l.Find(variable => variable.getNombre() == nomV);

                    if (v == null)
                    {
                        throw new Error("La variable no existe", log, linea);
                    }
                    if (excecute)
                    {
                        //? Por alguna razón sigue imprimiendo en float REVISAR
                        Console.Write(((int)v.getValor()).ToString());
                    }
                    //match(v.getValor().ToString());
                }
            }

            if (getContenido() == "+")
            {
                match("+");
                Concatenaciones();
            }

            match(")");
            match(";");

            if (isRead)
            {
                content = getContenido() == "ReadLine" ? Console.ReadLine() : ((char)Console.Read()).ToString();
            }


            if (!isRead && excecute)
            {
                switch (console)
                {
                    case true: Console.Write(content); break;
                    case false: Console.WriteLine(content); break;
                }
            }
        }
        //!SECTION
        // Main -> static void Main(string[] args) BloqueInstrucciones 
        //SECTION - Main
        private void Main()
        {
            match("static");
            match("void");
            match("Main");
            match("(");
            match("string");
            match("[");
            match("]");
            match("args");
            match(")");
            BloqueInstrucciones(true);
        }
        //!SECTION
        // Expresion -> Termino MasTermino
        //SECTION - Expresion
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        //!SECTION
        // MasTermino -> (OperadorTermino Termino)?
        //SECTION - MasTermino
        private void MasTermino()
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino();
                //Console.Write(operador + " ");

                float n1 = s.Pop();
                float n2 = s.Pop();

                switch (operador)
                {
                    case "+": s.Push(n2 + n1); break;
                    case "-": s.Push(n2 - n1); break;
                }

            }
        }
        //!SECTION
        // Termino -> Factor PorFactor
        //SECTION - Termino
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        //!SECTION
        // PorFactor -> (OperadorFactor Factor)?
        //SECTION - PorFactor
        private void PorFactor()
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor();

                //Console.Write(operador + " ");

                float n1 = s.Pop();
                float n2 = s.Pop();

                switch (operador)
                {
                    case "*": s.Push(n2 * n1); break;
                    case "/": s.Push(n2 / n1); break;
                    //ANCHOR - Prueba de Math.Floor
                    case "%": s.Push((float)Math.Floor(n2 % n1)); break;
                }
            }
        }
        //!SECTION
        // Factor -> numero | identificador | (Expresion)
        //SECTION - Factor
        private void Factor()
        {
            if (getClasificacion() == Tipos.Numero)
            {
                s.Push(float.Parse(getContenido()));
                //Console.Write(getContenido() + " ");
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                Variable? v = l.Find(variable => variable.getNombre() == getContenido());

                if (v == null)
                {
                    /* throw new Error("Sintaxis: la variable " + getContenido() + " no está definida ", log, linea, column); */
                    throw new Error("Sintaxis: la variable " + getContenido() + " no está definida ", log, linea);
                }

                s.Push(v.getValor());
                //Console.Write(getContenido() + " ");
                match(Tipos.Identificador);
            }
            else
            {
                match("(");
                Expresion();
                match(")");
            }
        }
        //!SECTION

        // Concatenaciones -> (Identificador | Cadena) (+ Concatenaciones) ?
        //SECTION - Concatenaciones
        private void Concatenaciones()
        {
            if (getClasificacion() == Tipos.Cadena)
            {
                Console.Write(getContenido().ToString().Replace('"', ' '));
                match(Tipos.Cadena);
            }
            else
            {
                string nomV = getContenido();
                match(Tipos.Identificador);

                Variable v = l.Find(variable => variable.getNombre() == nomV);
                if (v == null)
                {
                    throw new Error("La variable no existe", log, linea);
                }
                Console.Write(v.getValor().ToString().Replace('\"', ' '));
            }
            if (getContenido() == "+")
            {
                match("+");
                Concatenaciones();
            }
        }
        //!SECTION
    }
}