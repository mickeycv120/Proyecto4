using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Sintaxis_1
{
    public class Error : Exception
    {
        public Error(string mensaje) : base("Error " + mensaje) { }
        public Error(string mensaje, StreamWriter log) : base("Error " + mensaje)
        {
            log.WriteLine("Error " + mensaje);
        }

        public Error(string mensaje, StreamWriter log, int linea) : base("Error: " + mensaje + " en la linea " + linea)
        {
            log.WriteLine("Error: " + mensaje + " on linea " + linea);
        }
    }
}