using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sintaxis_1
{
    public class Sintaxis : Lexico
    {
        public Sintaxis() : base()
        {
            NextToken();
        }

        public Sintaxis(string name) : base(name)
        {
            NextToken();
        }
        public void match(string content)
        {
            if (content == getContent())
            {
                NextToken();
            }
            else
            {
                throw new Error("Sintaxis: se espera un " + content);
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