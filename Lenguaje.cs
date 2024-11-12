using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sintaxis_1
{
    public class Lenguaje : Sintaxis
    {
        public Lenguaje() : base()
        {
            log.WriteLine("Constructor lenguaje");
        }

        public Lenguaje(string name) : base(name)
        {
            log.WriteLine("Constructor lenguaje");
        }
        // ? Cerradura epsilon
        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            if (getContent() == "using")
            {
                Librerias();
            }

            if (getClasification() == Tipos.TipoDato)
            {
                Variables();
            }

            Main();
        }

        //Librerias -> using ListaLibrerias; Librerias?
        private void Librerias()
        {
            match("using");
            ListaLibrerias();
            match(";");

            if (getContent() == "using")
            {
                Librerias();
            }
        }

        //Variables -> tipo_dato Lista_identificadores; Variables?
        private void Variables()
        {
            match(Tipos.TipoDato);
            ListaIdentificadores();
            match(";");

            if (getClasification() == Tipos.TipoDato)
            {
                Variables();
            }
        }

        //ListaLibrerias -> identificador (.ListaLibrerias)?
        private void ListaLibrerias()
        {
            match(Tipos.Indentificador);

            if (getContent() == ".")
            {
                match(".");
                ListaLibrerias();
            }
        }

        // ListaIdentificadores -> identificador (,ListaIdentificadores)?
        private void ListaIdentificadores()
        {
            match(Tipos.Indentificador);

            if (getContent() == ",")
            {
                match(",");
                ListaIdentificadores();
            }
        }
        // BloqueInstrucciones -> { listaIntrucciones? }
        private void BloqueInstrucciones()
        {
            match("{");
            ListaInstrucciones();
            match("}");
        }
        // ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones()
        {
            Instruccion();
            if (getContent() != "}")
            {
                ListaInstrucciones();
            }
        }

        // Instruccion -> console | If | While | do | For | Variables | Asignación
        private void Instruccion()
        {
            if (getContent() == "Console")
            {
                console();
            }
            else if (getContent() == "if")
            {
                If();
            }
            else if (getContent() == "while")
            {
                While();
            }
            else if (getContent() == "do")
            {
                Do();
            }
            else if (getContent() == "for")
            {
                For();
            }
            else if (getClasification() == Tipos.TipoDato)
            {
                Variables();
            }
            else
            {
                Asignacion();
            }
        }
        // Asignacion -> Identificador = Expresion | ID ++ | ID -- 
        // ID Incremento Expresion;
        private void Asignacion()
        {
            match(Tipos.Indentificador);
            match("=");
            Expresion();
            /* match(";"); */
        }
        // If -> if (Condicion) bloqueInstrucciones | instruccion
        // (else bloqueInstrucciones | instruccion)?

        private void If()
        {
            match("if");
            match("(");
            Condicion();
            match(")");

            if (getContent() == "{")
            {
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
            match(";");

            if (getContent() == "else")
            {
                match("else");
                if (getContent() == "{")
                {
                    BloqueInstrucciones();
                }
                else
                {
                    Instruccion();
                }
                match(";");
                match("}");
            }
        }
        // Condicion -> Expresion operadorRelacional Expresion
        private void Condicion()
        {
            Expresion();
            match(Tipos.OperadorRelacional);
            Expresion();
        }
        // While -> while(Condicion) bloqueInstrucciones | instruccion
        private void While()
        {
            match("while");
            Condicion();
            if (getContent() == "{")
            {
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
        }
        // Do -> do 
        // bloqueInstrucciones | intruccion 
        // while(Condicion);
        private void Do()
        {
            match("do");
            if (getContent() == "{")
            {
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
            match("while");
            match("(");
            Condicion();
            match(")");
        }
        // For -> for(Asignacion; Condicion; Asignacion) 
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
            if (getContent() == "{")
            {
                BloqueInstrucciones();
            }
        }
        // Console -> Console.(WriteLine|Write) (cadena concatenaciones?);
        private void console()
        {
            match("(");
            Console.WriteLine(getContent());
            match(Tipos.Cadena);
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
            BloqueInstrucciones();
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
            if (getClasification() == Tipos.OperadorTermino)
            {
                match(Tipos.OperadorTermino);
                Termino();
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
            if (getClasification() == Tipos.OperadorFactor)
            {
                match(Tipos.OperadorFactor);
                Factor();
            }
        }
        // Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (getClasification() == Tipos.Numero)
            {
                match(Tipos.Numero);
            }
            else if (getClasification() == Tipos.Indentificador)
            {
                match(Tipos.Indentificador);
            }
            else
            {
                match("(");
                Expresion();
                match(")");
            }
        }
    }
}