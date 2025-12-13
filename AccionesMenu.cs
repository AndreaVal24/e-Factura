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
        public Factura facturaEnProceso { get; private set; }

        //TODO Secuencias NCF simuladas
        private long secuenciaB01 = 1;
        private long secuenciaB02 = 1;
        private long secuenciaB14 = 1;

        // =============================
        // 1. Crear nueva factura
        // =============================
        public Factura CrearFactura()
        {
            Colores.Titulo("\n--- CREAR NUEVA FACTURA ---");
            // ⚠️ Verificar si hay una factura en proceso sin guardar
            if (facturaEnProceso != null)
            {
                Colores.Error("\n⚠️ Ya existe una factura en proceso.");
                Console.WriteLine("¿Desea descartarla y crear una nueva? (s/n): ");
                string respuesta = Console.ReadLine()?.ToLower();

                if (respuesta != "s")
                {
                    Colores.Info("Operación cancelada. Continuando con la factura actual.");
                    return facturaEnProceso;
                }

                Colores.Info("Factura anterior descartada.");
            }

            Console.Write("Nombre del cliente: ");
            string nombre = Console.ReadLine();

            bool llevaComprobante = false;

            while (true)
            {
                Console.Write("¿Factura lleva Comprobante Fiscal? (s/n): ");
                string op = Console.ReadLine()?.ToLower();

                if (op == "s")
                {
                    llevaComprobante = true;
                    break;
                }
                else if (op == "n")
                {
                    llevaComprobante = false;
                    break;
                }
                else
                {
                    Colores.Error("Debe escribir 's' o 'n'.");
                }
            }

            Cliente cliente;

            string identificacion;
           
            // Si lleva comprobante, pedir RNC o Cédula
            if (llevaComprobante)
            {
                Console.Write("RNC o Cédula: ");
                 identificacion = Console.ReadLine();

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

                cliente = new Cliente(nombre, identificacion, tipoIdentificacion);
            }
            else
            {
                // Sin comprobante fiscal - cliente sin identificación
                cliente = new Cliente(nombre, null, TipoIdentificacion.Cedula);
                Colores.Info("Factura sin comprobante fiscal.");
            }

            Factura factura = new Factura(numeroFacturaActual++, cliente);
            facturaEnProceso = factura;

            Colores.Exito("Factura creada correctamente.");
            return factura;

        }

        // =============================
        // 2. Agregar productos
        // =============================
        public void AgregarProducto(Factura factura = null)
        {
            if (facturaEnProceso == null)
            {
                Colores.Error("\n❌ No hay factura en proceso.");
                Colores.Info("Debe crear una factura nueva primero (Opción 1).");
                return;
            }


            Colores.Titulo("\n--- AGREGAR PRODUCTO ---");

            try
            {
                Console.Write("Código: ");
                string codigo = Console.ReadLine();

                var itemExistente = facturaEnProceso.Items.FirstOrDefault(i => i.Codigo == codigo);

                // Si existe, lanzar la excepción
                if (itemExistente != null)
                    throw new ItemDuplicadoException(itemExistente);

                // Si no existe, continuar con el proceso normal de agregar
                Console.Write("Descripción: ");
                string descripcion = Console.ReadLine();

                // Precio
                decimal precio;
                do
                {
                    Console.Write("Precio: ");
                } while (!decimal.TryParse(Console.ReadLine(), out precio) || precio <= 0);

                // Cantidad
                int cantidad;
                do
                {
                    Console.Write("Cantidad: ");
                } while (!int.TryParse(Console.ReadLine(), out cantidad) || cantidad <= 0);

                // ITBIS
                bool aplicaItbis = false;
                while (true)
                {
                    Console.Write("Aplica ITBIS (s/n): ");
                    string op = Console.ReadLine()?.ToLower();

                    if (op == "s") { aplicaItbis = true; break; }
                    if (op == "n") { aplicaItbis = false; break; }

                    Colores.Error("Debe escribir 's' o 'n'.");
                }

                facturaEnProceso.Items.Add(new ItemFactura(codigo, descripcion, precio, cantidad, aplicaItbis));

                Colores.Exito("✓ Producto agregado exitosamente.");
            }
            catch (ItemDuplicadoException ex)
            {
                // Capturar la excepción y manejarla
                Colores.Error($"\n⚠️ {ex.Message}");

                // Obtener el item desde la excepción
                var itemExistente = ex.ItemDuplicado;

                if (itemExistente != null)
                {
                    Console.WriteLine($"\n   Producto: {itemExistente.Descripcion}");
                    Console.WriteLine($"   Código: {itemExistente.Codigo}");
                    Console.WriteLine($"   Cantidad actual: {itemExistente.Cantidad}");
                    Console.WriteLine($"   Precio actual: {itemExistente.Precio:C}");
                    Console.WriteLine($"   ITBIS actual: {(itemExistente.AplicaITBIS ? "Sí" : "No")}");

                    Console.WriteLine("\n¿Qué desea hacer?");
                    Console.WriteLine("1. Modificar el producto existente");
                    Console.WriteLine("2. Incrementar solo la cantidad");
                    Console.WriteLine("3. Cancelar y volver al menú");
                    Console.Write("\nSeleccione una opción: ");

                    string opcion = Console.ReadLine();

                    switch (opcion)
                    {
                        case "1":
                            ModificarProducto(itemExistente);
                            break;
                        case "2":
                            IncrementarCantidad(itemExistente);
                            break;
                        case "3":
                            Colores.Info("Operación cancelada.");
                            break;
                        default:
                            Colores.Error("Opción inválida. Operación cancelada.");
                            break;
                    }
                }
            }
        }
        // =============================
        // Método auxiliar: Modificar producto existente
        // =============================
        private void ModificarProducto(ItemFactura item)
        {
            Colores.Titulo("\n--- MODIFICAR PRODUCTO ---");
            Console.WriteLine($"Producto: {item.Descripcion} (Código: {item.Codigo})");
            Console.WriteLine("\nDeje en blanco para mantener el valor actual.\n");

            // Modificar Descripción
            Console.Write($"Nueva descripción [{item.Descripcion}]: ");
            string nuevaDescripcion = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(nuevaDescripcion))
                item.Descripcion = nuevaDescripcion;

            // Modificar Precio
            Console.Write($"Nuevo precio [{item.Precio:C}]: ");
            string precioInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(precioInput) && decimal.TryParse(precioInput, out decimal nuevoPrecio) && nuevoPrecio > 0)
                item.Precio = nuevoPrecio;

            // Modificar Cantidad
            Console.Write($"Nueva cantidad [{item.Cantidad}]: ");
            string cantidadInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(cantidadInput) && int.TryParse(cantidadInput, out int nuevaCantidad) && nuevaCantidad > 0)
                item.Cantidad = nuevaCantidad;

            // Modificar ITBIS
            Console.Write($"Aplica ITBIS (s/n) [Actual: {(item.AplicaITBIS ? "Sí" : "No")}]: ");
            string itbisInput = Console.ReadLine()?.ToLower();
            if (itbisInput == "s")
                item.AplicaITBIS = true;
            else if (itbisInput == "n")
                item.AplicaITBIS = false;

            Colores.Exito("\n✓ Producto modificado exitosamente.");
            Console.WriteLine($"\nResumen actualizado:");
            Console.WriteLine($"  Descripción: {item.Descripcion}");
            Console.WriteLine($"  Precio: {item.Precio:C}");
            Console.WriteLine($"  Cantidad: {item.Cantidad}");
            Console.WriteLine($"  ITBIS: {(item.AplicaITBIS ? "Sí" : "No")}");
            Console.WriteLine($"  Total: {(item.Precio * item.Cantidad):C}");
        }

        // =============================
        // Método auxiliar: Incrementar cantidad
        // =============================
        private void IncrementarCantidad(ItemFactura item)
        {
            Colores.Titulo("\n--- INCREMENTAR CANTIDAD ---");
            Console.WriteLine($"Producto: {item.Descripcion}");
            Console.WriteLine($"Cantidad actual: {item.Cantidad}");

            int cantidadAdicional;
            do
            {
                Console.Write("Cantidad a agregar: ");
            } while (!int.TryParse(Console.ReadLine(), out cantidadAdicional) || cantidadAdicional <= 0);

            int cantidadAnterior = item.Cantidad;
            item.Cantidad += cantidadAdicional;

            Colores.Exito($"\n✓ Cantidad actualizada: {cantidadAnterior} → {item.Cantidad}");
            Console.WriteLine($"Total del producto: {(item.Precio * item.Cantidad):C}");
        }

        // =============================
        // 3. Aplicar descuento
        // =============================
        public void AplicarDescuento()
        {
            if (facturaEnProceso == null)
            {
                Colores.Error("\n❌ No hay factura en proceso.");
                Colores.Info("Debe crear una factura nueva primero (Opción 1).");
                return;
            }

            Colores.Titulo("\n--- APLICAR DESCUENTO ---");

            //TODO Func<decimal, decimal> para aplicar descuento
            Func<decimal, decimal> aplicarDescuento = (subtotal) => subtotal * (facturaEnProceso.PorcentajeDescuento / 100);

          

            decimal montoDescuento = aplicarDescuento(facturaEnProceso.Subtotal);
           
            facturaEnProceso.Descuento = montoDescuento;
            Colores.Exito("Descuento aplicado.");
        }

        // =============================
        // 4. Calcular totales
        // =============================
        public void CalcularTotales()
        {
            if (facturaEnProceso == null)
            {
                Colores.Error("\n❌ No hay factura en proceso.");
                Colores.Info("Debe crear una factura nueva primero (Opción 1).");
                return;
            }
            // Mostrar NCF si existe
            if (facturaEnProceso.NCF != null)
                Console.WriteLine($"NCF: {facturaEnProceso.NCF.NumeroCompleto}");
            else
                Console.WriteLine("NCF: (No asignado)");

            // Mostrar RNC o Cédula del cliente
           if (!string.IsNullOrEmpty(facturaEnProceso.Cliente.RncCedula))
                Console.WriteLine($"RNC/Cédula: {facturaEnProceso.Cliente.RncCedula}");
           else
               Console.WriteLine("RNC/Cédula: (No aplica)");

            Console.WriteLine($"\nCliente: {facturaEnProceso.Cliente.Nombre}");

            // Mostrar productos de la factura
            Console.WriteLine(
                    "-  Descripcion  |  Cantidad  |  Precio  |  Subotal  |  ITBIS  |  Total  "
                );
            foreach (var item in facturaEnProceso.Items)
            {
                Console.WriteLine(
                    $"-  {item.Descripcion}  |  {item.Cantidad}  |  RD{item.Precio:C}  |  RD{(item.Precio * item.Cantidad):C} " +
                    $" |  RD{item.ITBIS:C}  |  RD{item.Total:C}"
                );
            }

            Console.WriteLine($"ITBIS: RD{facturaEnProceso.TotalITBIS:C}");
            Console.WriteLine($"Descuento: RD{facturaEnProceso.Descuento:C}");
            Console.WriteLine($"TOTAL A PAGAR: RD{facturaEnProceso.Total:C}");
        }

        // =============================
        // 5. Generar NCF
        // =============================
        public void GenerarNCF()
        {
            if (facturaEnProceso == null)
            {
                Colores.Error("\n❌ No hay factura en proceso.");
                Colores.Info("Debe crear una factura nueva primero (Opción 1).");
                return;
            }

            if (facturaEnProceso.NCF != null)
               throw new NCFAsignadoException();

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
            //TODO Generar NCF con formato secuencial: B0100000001.
            long secuencia = tipoNCF switch
            {
                TipoNCF.CreditoFiscal => secuenciaB01++,
                TipoNCF.ConsumidorFinal => secuenciaB02++,
                TipoNCF.Gubernamental => secuenciaB14++,
                _ => 0
            };

            facturaEnProceso.NCF = new NCF(tipoNCF, secuencia);

            Colores.Exito($"NCF generado: {facturaEnProceso.NCF.NumeroCompleto}");
        }

        // =============================
        // 6. Guardar factura
        // =============================
        public void GuardarFactura()
        {
            if (facturaEnProceso == null)
            {
                Colores.Error("\n❌ No hay factura en proceso para guardar.");
                return;
            }

            if (!facturaEnProceso.Items.Any())
            {
                Colores.Error("\n❌ No se puede guardar una factura sin productos.");
                return;
            }
            //TODO Action<Factura> para callback cuando se guarda
            Action<Factura> onFacturaGuardada = (f) =>
            {
                Colores.Exito($"\n✓ Factura #{f.NumeroFactura} guardada correctamente.");
                Colores.Info($"  Cliente: {f.Cliente.Nombre}");
                Colores.Info($"  Total: {f.Total:C}");
                Colores.Info($"  Productos: {f.Items.Count}");
                if (f.NCF != null)
                    Colores.Info($"  NCF: {f.NCF.NumeroCompleto}");

                Console.WriteLine($"  Guardado en: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            };

            facturas.Add(facturaEnProceso);

            // Ejecutar el callback Action
            onFacturaGuardada(facturaEnProceso);

            facturaEnProceso = null; // Limpiar

            Colores.Error("\n⚠️ Para trabajar con otra factura, cree una nueva (Opción 1).");
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

            //TODO uso de linq para filtrar y proyectar
            var resumen = facturas
                .Where(f => f.Fecha.Date == DateTime.Now.Date)
                .Select(f => new
                {
                    f.NumeroFactura,
                    Cliente = f.Cliente.Nombre,
                    f.Items.Count,
                    Total = f.Total
                })
                .ToList();

            if (resumen.Count == 0)
            {
                Colores.Info("No hay facturas en el día de hoy.");
                return;
            }

            foreach (var f in resumen)
                Console.WriteLine($"Factura {f.NumeroFactura} - Cliente: {f.Cliente} - Productos: {f.Count} - Total: {f.Total:C}");
        }
    }
}
