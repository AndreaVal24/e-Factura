using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Factura
{
    /*// Excepcion personalizada para indicar que un item ya existe en la factura
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
    }*/
    // Excepcion personalizada para indicar que un item ya existe en la factura
    public class ItemDuplicadoException : Exception
    {
        // Propiedad para almacenar el item duplicado
        public ItemFactura ItemDuplicado { get; private set; }

        // Constructor por defecto con un mensaje predeterminado
        public ItemDuplicadoException() : base("El producto ya existe en la factura. Use la opción de modificar cantidad.")
        {
        }

        // Constructor que recibe el item duplicado (NUEVO)
        public ItemDuplicadoException(ItemFactura itemDuplicado)
            : base($"El producto '{itemDuplicado.Descripcion}' ya existe en la factura.")
        {
            ItemDuplicado = itemDuplicado;
        }

        // Constructor que permite especificar un mensaje personalizado
        public ItemDuplicadoException(string mensaje) : base(mensaje)
        {
        }

        // Constructor completo con mensaje e item (NUEVO - OPCIONAL)
        public ItemDuplicadoException(string mensaje, ItemFactura itemDuplicado) : base(mensaje)
        {
            ItemDuplicado = itemDuplicado;
        }
    }
}
