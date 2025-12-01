using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Factura
{
    //Colores para que se vea bonitis
    public static class Colores
    {
        public static void Titulo(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(texto);
            Console.ResetColor();
        }

        public static void Opcion(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(texto);
            Console.ResetColor();
        }

        public static void Error(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(texto);
            Console.ResetColor();
        }

        public static void Exito(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(texto);
            Console.ResetColor();
        }

        public static void Info(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(texto);
            Console.ResetColor();
        }
    }
}
