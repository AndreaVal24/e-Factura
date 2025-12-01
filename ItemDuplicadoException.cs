using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Factura
{
    // Excepcion personalizada para indicar que un item ya existe en la factura
    public class ItemDuplicadoException : Exception
    {
        // Constructor por defecto con un mensaje predeterminado
        public ItemDuplicadoException() : base("El producto ya existe en la factura. Use la opción de modificar cantidad.")
        {
        }
        // Constructor que permite especificar un mensaje personalizado
        public ItemDuplicadoException(string mensaje) : base(mensaje)
        {
        }
    }
}
