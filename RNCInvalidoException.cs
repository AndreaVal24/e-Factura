using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Factura
{
    // Excepcion personalizada para indicar que el RNC/Cedula es inválido
    public class RNCInvalidoException : Exception
    {
        // Constructor por defecto con un mensaje predeterminado
        public RNCInvalidoException() : base("El RNC/Cédula proporcionado no es válido.\nFormato Cédula: XXX-XXXXXXX-X.\nFormato RNC: X-XX-XXXXX-X. ")
        {
        }
        // Constructor que permite especificar un mensaje personalizado
        public RNCInvalidoException(string mensaje) : base(mensaje)
        {
        }
    }
}
