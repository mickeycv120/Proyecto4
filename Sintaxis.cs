using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sintaxis_1
{
    public class Sintaxis : Lexico
    {

        private int linea;

        public Sintaxis() : base()
        {
            NextToken();
        }

        public Sintaxis(string nombre) : base(nombre)
        {
            NextToken();
        }
        public void match(string contenido)
        {
            if (getContenido() == contenido)
            {
                NextToken();
            }
            else
            {
                throw new Error($"Sintaxis: se espera un {contenido} en la linea");
            }
        }

        public void match(Tipos clasificacion)
        {
            if (clasificacion == getClasificacion())
            {
                NextToken();
            }
            else
            {
                throw new Error("Sintaxis: se espera un " + clasificacion);
            }
        }
    }
}