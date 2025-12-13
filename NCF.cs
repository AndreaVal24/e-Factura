using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Factura
{
    //TODO Clase para representar un numero de comprobante fiscal (NCF)
    public class NCF
    {
        public TipoNCF Tipo { get; set; }
        public long Secuencia { get; set; }
        public DateTime FechaEmision { get; set; }
        public string NumeroCompleto => $"{GetPrefijoTipo()}{Secuencia:D8}";

        // Constructor
        public NCF(TipoNCF tipo, long secuencia)
        {
            Tipo = tipo;
            Secuencia = secuencia;
            FechaEmision = DateTime.Now;
        }

        //TODO Metodo privado para obtener el prefijo segun el tipo de NCF
        private string GetPrefijoTipo()
        {
            return Tipo switch
            {
                TipoNCF.CreditoFiscal => "B01",
                TipoNCF.ConsumidorFinal => "B02",
                TipoNCF.Gubernamental => "B14",
                _ => "B02"
            };
        }
    }
    // Enum para el tipo de NCF
    public enum TipoNCF
    {
        CreditoFiscal,      // B01 - Para empresas con RNC
        ConsumidorFinal,    // B02 - Para consumidores finales
        Gubernamental       // B14 - Para entidades gubernamentales
    }
}
