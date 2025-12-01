using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Factura
{
    // Excepción personalizada para indicar que la secuencia de NCF se ha agotado
    public class NCFAgotadoException : Exception
    {
        // Constructor por defecto con un mensaje predeterminado
        public NCFAgotadoException() : base("La secuencia de NCF se ha agotado. Debe solicitar una nueva autorización a la DGII.")
        {
        }

        // Constructor que permite especificar un mensaje personalizado
        public NCFAgotadoException(string mensaje) : base(mensaje)
        {
        }
    }
}
