using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Factura
{
    //TODO Excepción personalizada para indicar que ya se ha asignado un NCF a una factura y no seguir generando más
    public class NCFAsignadoException : Exception
    {
        
      
       
        // Constructor por defecto con un mensaje predeterminado
        public NCFAsignadoException() : base("La Factura en progreso ya se le ha asignado un NCF.")
        {
        }

        // Constructor que permite especificar un mensaje personalizado
        public NCFAsignadoException(string mensaje) : base(mensaje)
        {
        }
      
    }
}
