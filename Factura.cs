using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Factura
{
    public class Factura
    {
        public int NumeroFactura { get; set; }
        public DateTime Fecha { get; set; }
        public Cliente Cliente { get; set; }// Clase Cliente
        public List<ItemFactura> Items { get; set; } // Lista de la clase items en la factura
        public NCF NCF { get; set; } // clase ncf
        public decimal PorcentajeDescuento => 10m;

        public decimal Descuento {  get; set; }
        //TODO Propiedades calculadas

        public decimal Subtotal => Items.Sum(i => i.Subtotal); // Suma de los subtotales de los items
        public decimal TotalITBIS => Items.Sum(i => i.ITBIS);// Suma del ITBIS de los items
        //public decimal Descuento => Subtotal * (PorcentajeDescuento / 100);// Calculo del descuento
        public decimal Total => Subtotal + TotalITBIS - Descuento;// Calculo del total de la factura

        // Constructor
        public Factura(int numeroFactura, Cliente cliente)
        {
            NumeroFactura = numeroFactura;
            Fecha = DateTime.Now;
            Cliente = cliente;
            Items = new List<ItemFactura>();
           // PorcentajeDescuento = 0;
        }
    }
}
