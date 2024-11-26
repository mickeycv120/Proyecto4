/* 
//SECTION - Requerimientos
    //* 1) Concatenaciones
    //* 2) Inicializar una variable desde la declaración
    //* 3) Evaluar las expresiones matemáticas
    //* 4) Levantar si en el Console.ReadLine() no ingresan números
    //* 5) Modificar la variable con el resto de operadores (Incremento de factor y término)
    //* 6) Hacer que funcione el else



//!SECTION

 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sintaxis_1
{
    public class Lenguaje : Sintaxis
    {
        Stack<float> s;
        List<Variable> l;
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
        // ? Cerradura epsilon
        //Programa  -> Librerias? Variables? Main
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

        //Librerias -> using ListaLibrerias; Librerias?
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

        //Variables -> tipo_dato Lista_identificadores; Variables?
        private void Variables()
        {
            Variable.TipoDato t = Variable.TipoDato.Char;

            switch (getContenido())
            {
                case "int": t = Variable.TipoDato.Int; break;
                case "float": t = Variable.TipoDato.Float; break;
            }

            match(Tipos.TipoDato);
            ListaIdentificadores(t);
            match(";");

            if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
        }

        //ListaLibrerias -> identificador (.ListaLibrerias)?
        private void ListaLibrerias()
        {
            match(Tipos.Identificador);

            if (getContenido() == ".")
            {
                match(".");
                ListaLibrerias();
            }
        }

        // ListaIdentificadores -> identificador (=Expresion)? (,ListaIdentificadores)?
        private void ListaIdentificadores(Variable.TipoDato t)
        {
            if (l.Find(variable => variable.getNombre() == getContenido()) != null)
            {
                /* throw new Error("Sintaxis: la variable " + getContenido() + " ya existe", log, linea, column); */
                throw new Error("Sintaxis: la variable " + getContenido() + " ya existe", log, linea);
            }

            l.Add(new Variable(t, getContenido()));
            match(Tipos.Identificador);


            if (getContenido() == "=")
            {
                match("=");
                Expresion();
            }

            if (getContenido() == ",")
            {
                match(",");
                ListaIdentificadores(t);
            }
        }
        // BloqueInstrucciones -> { listaIntrucciones? }
        private void BloqueInstrucciones(bool excecute)
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones(excecute);
            }
            match("}");
        }
        // ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool excecute)
        {
            Instruccion(excecute);

            if (getContenido() != "}")
            {
                ListaInstrucciones(excecute);
            }
        }

        // Instruccion -> console | If | While | do | For | Variables | Asignación
        private void Instruccion(bool excecute)
        {
            if (getContenido() == "Console")
            {
                console(excecute);
            }
            else if (getContenido() == "if")
            {
                If(excecute);
            }
            else if (getContenido() == "while")
            {
                While();
            }
            else if (getContenido() == "do")
            {
                Do();
            }
            else if (getContenido() == "for")
            {
                For();
            }
            else if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
            else
            {
                Asignacion();
                match(";");
            }
        }
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
        private void Asignacion()
        {
            Variable? v = l.Find(variable => variable.getNombre() == getContenido());

            if (v == null)
            {
                /* throw new Error("Sintaxis: la variable " + getContenido() + " no está definida ", logger, line, column); */
                throw new Error("Sintaxis: la variable " + getContenido() + " no está definida ", log, linea);
            }
            //Console.Write(getContenido() + " = ");
            match(Tipos.Identificador);

            if (getContenido() == "=")
            {
                match("=");

                if (getContenido() == "Console")
                {
                    int read;

                    match("Console");
                    match(".");

                    if (getContenido() == "Read")
                    {
                        read = 1;
                        match("Read");
                    }
                    else
                    {
                        read = 2;
                        match("ReadLine");
                    }

                    match("(");
                    match(")");

                    switch (read)
                    {
                        case 1: Console.Read(); break;
                        case 2: Console.ReadLine(); break;
                    }
                }
                else
                {
                    Expresion();
                }
            }
            else
            {
                string currentContent = getContenido();

                if (getClasificacion() == Tipos.IncrementoTermino)
                {
                    match(Tipos.IncrementoTermino);

                    if (currentContent != "++" && currentContent != "--")
                    {
                        Expresion();
                    }
                }
                else
                {
                    match(Tipos.IncrementoFactor);
                    Expresion();
                }
            }


            float r = s.Pop();
            v.setValor(r);
            //displayStack();
        }
        // If -> if (Condicion) bloqueInstrucciones | instruccion
        // (else bloqueInstrucciones | instruccion)?
        private void If(bool excecute2)
        {
            match("if");
            match("(");
            bool execute = Condicion() && excecute2;
            Console.WriteLine(execute);
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
                    BloqueInstrucciones(execute);
                }
                else
                {
                    Instruccion(execute);
                }
            }
        }
        // Condicion -> Expresion operadorRelacional Expresion
        private bool Condicion()
        {
            float v1 = s.Pop();
            Expresion();
            match(Tipos.OperadorRelacional);
            String operador = getContenido();
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
        // While -> while(Condicion) bloqueInstrucciones | instruccion
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
        // Do -> do 
        // bloqueInstrucciones | intruccion 
        // while(Condicion);
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
        // For -> for(Asignacion; Condicion; Asignación) 
        // BloqueInstrucciones | Intruccion
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
        // Console -> Console.(WriteLine|Write) (cadena concatenaciones?);
        private void console(bool excecute)
        {
            bool console = false;
            string content = "";

            match("Console");
            match(".");

            switch (getContenido())
            {
                case "Write":
                    console = true;
                    match("Write");
                    break;
                default:
                    match("WriteLine");
                    break;
            }

            match("(");

            if (getContenido() != ")")
            {
                content += getContenido();

                if (getClasificacion() == Tipos.Cadena)
                {
                    match(Tipos.Cadena);
                }
                else
                {
                    match(Tipos.Identificador);
                }
            }
            else if (console)
            {
                if (getClasificacion() == Tipos.Cadena)
                {
                    match(Tipos.Cadena);
                }
                else
                {
                    match(Tipos.Identificador);
                }
            }

            match(")");
            match(";");

            content = content.Replace("\"", "").Replace("\\n", "\n");
            switch (console)
            {
                case true: Console.Write(content); break;
                case false: Console.WriteLine(content); break;
            }
        }
        // Main -> static void Main(string[] args) BloqueInstrucciones 
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
        // Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        // MasTermino -> (OperadorTermino Termino)?
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
        // Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        // PorFactor -> (OperadorFactor Factor)?
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
                    case "%": s.Push(n2 % n1); break;
                }
            }
        }
        // Factor -> numero | identificador | (Expresion)
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

        // Concatenaciones -> (Identificador | Cadena) (+ Concatenaciones) ?
        private void Concatenaciones()
        {
            if (getClasificacion() == Tipos.Identificador)
            {
                match(Tipos.Identificador);
            }
            else
            {
                match(Tipos.Cadena);
            }

            if (getContenido() == "+")
            {
                match("+");
                Concatenaciones();
            }
        }
    }
}