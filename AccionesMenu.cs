using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Factura
{
    public class MenuAcciones
   
    {
        private List<Factura> facturas = new List<Factura>();
        private int numeroFacturaActual = 1;

        // Secuencias NCF simuladas
        private long secuenciaB01 = 1;
        private long secuenciaB02 = 1;
        private long secuenciaB14 = 1;

        // =============================
        // 1. Crear nueva factura
        // =============================
        public Factura CrearFactura()
        {
            Colores.Titulo("\n--- CREAR NUEVA FACTURA ---");

            Console.Write("Nombre del cliente: ");
            string nombre = Console.ReadLine();

            Console.Write("RNC o Cédula: ");
            string identificacion = Console.ReadLine();

            // Validar tipo de identificación
            int tipoId;
            do
            {
                Console.Write("Tipo de identificación (1=RNC, 2=Cédula): ");
            } while (!int.TryParse(Console.ReadLine(), out tipoId) || (tipoId != 1 && tipoId != 2));

            TipoIdentificacion tipoIdentificacion =
                tipoId == 1 ? TipoIdentificacion.RNC : TipoIdentificacion.Cedula;

            // Validaciones
            if (tipoIdentificacion == TipoIdentificacion.RNC && !identificacion.ValidarRNC())
                throw new RNCInvalidoException();

            if (tipoIdentificacion == TipoIdentificacion.Cedula && !identificacion.ValidarCedula())
                throw new RNCInvalidoException();

            Cliente cliente = new Cliente(nombre, identificacion, tipoIdentificacion);

            Factura factura = new Factura(numeroFacturaActual++, cliente);
            facturas.Add(factura);

            Colores.Exito("Factura creada correctamente.");
            return factura;
        }

        // =============================
        // 2. Agregar productos
        // =============================
        public void AgregarProducto(Factura factura)
        {
            Colores.Titulo("\n--- AGREGAR PRODUCTO ---");

            Console.Write("Código: ");
            string codigo = Console.ReadLine();

            // Evitar items duplicados
            if (factura.Items.Any(i => i.Codigo == codigo))
                throw new ItemDuplicadoException();

            Console.Write("Descripción: ");
            string descripcion = Console.ReadLine();

            // Precio
            decimal precio;
            do
            {
                Console.Write("Precio: ");
            } while (!decimal.TryParse(Console.ReadLine(), out precio));

            // Cantidad
            int cantidad;
            do
            {
                Console.Write("Cantidad: ");
            } while (!int.TryParse(Console.ReadLine(), out cantidad));

            // ITBIS
            bool aplicaItbis = false;
            while (true)
            {
                Console.Write("Aplica ITBIS (s/n): ");
                string op = Console.ReadLine().ToLower();

                if (op == "s") { aplicaItbis = true; break; }
                if (op == "n") { aplicaItbis = false; break; }

                Colores.Error("Debe escribir 's' o 'n'.");
            }

            factura.Items.Add(new ItemFactura(codigo, descripcion, precio, cantidad, aplicaItbis));

            Colores.Exito("Producto agregado exitosamente.");
        }

        // =============================
        // 3. Aplicar descuento
        // =============================
        public void AplicarDescuento(Factura factura)
        {
            Colores.Titulo("\n--- APLICAR DESCUENTO ---");

            decimal descuento;
            do
            {
                Console.Write("Porcentaje de descuento: ");
            } while (!decimal.TryParse(Console.ReadLine(), out descuento));

            factura.PorcentajeDescuento = descuento;

            Colores.Exito("Descuento aplicado.");
        }

        // =============================
        // 4. Calcular totales
        // =============================
        public void CalcularTotales(Factura factura)
        {
            Colores.Titulo("\n--- TOTALES DE LA FACTURA ---");
            Console.WriteLine($"Subtotal: {factura.Subtotal:C}");
            Console.WriteLine($"ITBIS: {factura.TotalITBIS:C}");
            Console.WriteLine($"Descuento: {factura.Descuento:C}");
            Console.WriteLine($"TOTAL A PAGAR: {factura.Total:C}");
        }

        // =============================
        // 5. Generar NCF
        // =============================
        public void GenerarNCF(Factura factura)
        {
            Colores.Titulo("\n--- GENERAR NCF ---");

            int tipo;
            do
            {
                Console.Write("Tipo de NCF (1=B01, 2=B02, 3=B14): ");
            } while (!int.TryParse(Console.ReadLine(), out tipo) || tipo < 1 || tipo > 3);

            TipoNCF tipoNCF = tipo switch
            {
                1 => TipoNCF.CreditoFiscal,
                2 => TipoNCF.ConsumidorFinal,
                3 => TipoNCF.Gubernamental,
                _ => TipoNCF.ConsumidorFinal
            };

            long secuencia = tipoNCF switch
            {
                TipoNCF.CreditoFiscal => secuenciaB01++,
                TipoNCF.ConsumidorFinal => secuenciaB02++,
                TipoNCF.Gubernamental => secuenciaB14++,
                _ => 0
            };

            factura.NCF = new NCF(tipoNCF, secuencia);

            Colores.Exito($"NCF generado: {factura.NCF.NumeroCompleto}");
        }

        // =============================
        // 6. Guardar factura
        // =============================
        public void GuardarFactura(Factura factura)
        {
            Colores.Exito("\nFactura guardada correctamente (simulado).");
        }

        // =============================
        // 7. Buscar factura
        // =============================
        public Factura BuscarFactura()
        {
            Colores.Titulo("\n--- BUSCAR FACTURA ---");

            int num;
            while (true)
            {
                Console.Write("Ingrese número de factura: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out num))
                {
                    Colores.Error("Error: Debe ingresar solo números.");
                    continue; // vuelve a pedir
                }

                break; // entrada correcta → salir del while
            }

            var fac = facturas.FirstOrDefault(f => f.NumeroFactura == num);

            if (fac == null)
                Colores.Error("Factura no encontrada.");

            return fac;
        }

        // =============================
        // 8. Reporte del día
        // =============================
        public void ReporteDelDia()
        {
            Colores.Titulo("\n--- REPORTE DEL DÍA ---");

            var resumen = facturas
                .Where(f => f.Fecha.Date == DateTime.Now.Date)
                .Select(f => new
                {
                    f.NumeroFactura,
                    Cliente = f.Cliente.Nombre,
                    Total = f.Total
                })
                .ToList();

            if (resumen.Count == 0)
            {
                Colores.Info("No hay facturas en el día de hoy.");
                return;
            }

            foreach (var f in resumen)
                Console.WriteLine($"Factura {f.NumeroFactura} - Cliente: {f.Cliente} - Total: {f.Total:C}");
        }
    }
}
