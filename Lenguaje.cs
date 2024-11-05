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
            logger.WriteLine("Constructor lenguaje");
        }

        public Lenguaje(string name) : base(name)
        {
            logger.WriteLine("Constructor lenguaje");
        }

        public void Libreria()
        {
            match("#");
            match("include");
            match("<");
            match(Tipos.Indentificador);
            match(">");
        }


    }
}