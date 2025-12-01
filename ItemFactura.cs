using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Factura
{
    // Clase para representar un ítem en la factura
    public class ItemFactura
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public bool AplicaITBIS { get; set; } // Indica si el ítem está sujeto a ITBIS

        public decimal Subtotal => Precio * Cantidad;
        public decimal ITBIS => AplicaITBIS ? Subtotal * 0.18m : 0; // ITBIS del 18%
        public decimal Total => Subtotal + ITBIS;

        // Constructor
        public ItemFactura(string codigo, string descripcion, decimal precio, int cantidad, bool aplicaITBIS = true)
        {
            if (precio <= 0)
                throw new NCFAgotadoException("El precio debe ser mayor a cero.");

            if (cantidad <= 0)
                throw new NCFAgotadoException("La cantidad debe ser mayor a cero.");

            Codigo = codigo;
            Descripcion = descripcion;
            Precio = precio;
            Cantidad = cantidad;
            AplicaITBIS = aplicaITBIS;
        }
    }
}
