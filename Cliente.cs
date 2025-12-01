using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Factura
{
    // Clase para representar un cliente
    public class Cliente
    {
        public string Nombre { get; set; }
        public string RncCedula { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public TipoIdentificacion TipoId { get; set; }

        // Constructor
        public Cliente(string nombre, string rncCedula, TipoIdentificacion tipoId, string direccion = "", string telefono = "")
        {
            Nombre = nombre;
            RncCedula = rncCedula;
            TipoId = tipoId;
            Direccion = direccion;
            Telefono = telefono;
        }

    }
    // Enum para el tipo de identificación del cliente
    public enum TipoIdentificacion
    {
        RNC,    // 9 dígitos
        Cedula  // 11 dígitos
    }
}
