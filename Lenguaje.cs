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

        public void Programa()
        {
            Librerias();
            Variables();
            Main();
        }

        private void Librerias()
        {
            match("using");
            ListaLibrerias();
            match(";");
            Librerias();
        }
        private void Variables()
        {

        }
        private void ListaLibrerias()
        {

        }
        private void ListaIdentificadores()
        {

        }
        private void BloqueInstrucciones()
        {

        }
        private void ListaInstrucciones()
        {

        }

        private void If()
        {

        }

        private void For()
        {

        }

        private void While()
        {

        }

        private void Do()
        {

        }

        private void Instruccion()
        {

        }

        private void Asignacion()
        {

        }

        private void Console()
        {

        }

        private void Main()
        {

        }
    }
}
/* 
SNT = Producciones = Invocar el metodo
ST  = Tokens (Contenido | Classification) = Invocar match

public () Programa  -> Librerias? Variables? Main
private (sin argumentos) Librerias -> using ListaLibrerias; Librerias?
private (sin argumentos) Variables -> tipo_dato Lista_identificadores; Variables?
private (sin argumentos) ListaLibrerias -> identificador (.ListaLibrerias)?
private (sin argumentos) ListaIdentificadores -> identificador (,ListaIdentificadores)?
private (sin argumentos) BloqueInstrucciones -> { listaIntrucciones? }
private (sin argumentos) ListaInstrucciones -> Instruccion ListaInstrucciones?

// * Instruccion -> Console | If | While | do | For | Variables | Asignación

//* Asignacion -> Identificador = Expresion;

//* If -> if (Condicion) bloqueInstrucciones | instruccion
     (else bloqueInstrucciones | instruccion)?

//* Condicion -> Expresion operadorRelacional Expresion

//* While -> while(Condicion) bloqueInstrucciones | instruccion
//* Do -> do 
        bloqueInstrucciones | intruccion 
      while(Condicion);

//* For -> for(Asignacion Condicion; Asignacion) 
       BloqueInstrucciones | Intruccion

//* Console -> Console.(WriteLine|Write) (cadena concatenaciones?);

//* Main      -> static void Main(string[] args) BloqueInstrucciones 

// Expresion -> Termino MasTermino
// MasTermino -> (OperadorTermino Termino)?
// Termino -> Factor PorFactor
// PorFactor -> (OperadorFactor Factor)?
// Factor -> numero | identificador | (Expresion)
 */
