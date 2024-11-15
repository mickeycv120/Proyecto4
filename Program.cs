using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sintaxis_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                using Lenguaje L = new(@"prueba.cpp");
                L.Programa();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error: " + e.Message);

            }

        }
    }
}