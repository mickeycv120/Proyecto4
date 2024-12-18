using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Sintaxis_1
{
    public class Token
    {
        public enum Tipos
        {
            Identificador,
            Numero,
            Caracter,
            FinSentencia,
            InicioBloque,
            FinBloque,
            OperadorTermino,
            OperadorTernario,
            OperadorFactor,
            IncrementoTermino,
            IncrementoFactor,
            Flecha,
            Asignacion,
            OperadorRelacional,
            OperadorLogico,
            Puntero,
            Cadena,
            TipoDato,
            PalabraReservada
        }
        private string content;
        private Tipos clasification;
        public Token()
        {
            content = "";
            clasification = Tipos.Caracter;
        }

        public void setContent(string content)
        {
            this.content = content;
        }

        public void setClasificacion(Tipos clasification)
        {
            this.clasification = clasification;
        }

        public string getContenido()
        {
            return content;
        }

        public Tipos getClasificacion()
        {
            return clasification;
        }
    }
}