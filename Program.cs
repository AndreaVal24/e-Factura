using System;


namespace e_Factura
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Instancia del menu de acciones
            MenuAcciones acciones = new MenuAcciones();

            // Instancia del menu de acciones
            ItemFactura itemfactura; 

            // Bucle del menu principal
            while (true)
            {
                Console.Clear();
                Colores.Titulo("===== MENÚ PRINCIPAL =====");

                Colores.Opcion("1. Crear nueva factura");
                Colores.Opcion("2. Agregar productos a la factura actual");
                Colores.Opcion("3. Aplicar descuento");
                Colores.Opcion("4. Calcular totales");
                Colores.Opcion("5. Generar NCF");
                Colores.Opcion("6. Guardar factura");
                Colores.Opcion("7. Buscar factura");
                Colores.Opcion("8. Reporte del día");
                Colores.Opcion("9. Salir");

                Console.Write("\nSeleccione una opción: ");
                string entrada = Console.ReadLine();

                // Validar entrada numerica
                if (!int.TryParse(entrada, out int opcion))
                {
                    Colores.Error("Debe ingresar un número.");
                    Console.ReadLine();
                    continue;
                }

                // Ejecuta la opcion seleccionada
                try
                {
                    switch (opcion)
                    {
                        case 1:
                             acciones.CrearFactura();
                            break;

                        case 2:
                                acciones.AgregarProducto();
                            break;

                        case 3:
                                acciones.AplicarDescuento();
                            break;

                        case 4:
                                acciones.CalcularTotales();
                            break;

                        case 5:
                                acciones.GenerarNCF();
                            break;

                        case 6:
                                acciones.GuardarFactura();
                            break;

                        case 7:
                            var encontrada = acciones.BuscarFactura();
                            if (encontrada != null)
                            {
                                Colores.Exito("\n✓ Factura encontrada:");
                                // Mostrar información básica
                                Console.WriteLine($"  Número: #{encontrada.NumeroFactura}");
                                Console.WriteLine($"  Cliente: {encontrada.Cliente.Nombre}");
                                Console.WriteLine($"  Fecha: {encontrada.Fecha:dd/MM/yyyy HH:mm}");
                                Console.WriteLine($"  Total: {encontrada.Total:C}");
                                Console.WriteLine($"  Productos: {encontrada.Items.Count}");
                                
                                if (encontrada.NCF != null)
                                    Console.WriteLine($"  NCF: {encontrada.NCF.NumeroCompleto}");
                            }
                            break;

                        case 8:
                            acciones.ReporteDelDia();
                            break;

                        case 9:
                            return;

                        default:
                            Colores.Error("Opción fuera de rango.");
                            break;
                    }
                }
                //exception general por si algo falla
                catch (Exception ex)
                {
                    Colores.Error($"ERROR: {ex.Message}");
                }

                Console.WriteLine("\nPresione ENTER para continuar...");
                Console.ReadLine();
            }
        }
    }
}


