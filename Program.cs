using System;


namespace e_Factura
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Instancia del menu de acciones
            MenuAcciones acciones = new MenuAcciones();
            Factura facturaActual = null; // Factura en curso de la clase factura

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
                            facturaActual = acciones.CrearFactura();
                            break;

                        case 2:
                            if (facturaActual == null)
                                Colores.Error("Debe crear una factura primero.");
                            else
                                acciones.AgregarProducto(facturaActual);
                            break;

                        case 3:
                            if (facturaActual == null)
                                Colores.Error("Debe crear una factura primero.");
                            else
                                acciones.AplicarDescuento(facturaActual);
                            break;

                        case 4:
                            if (facturaActual == null)
                                Colores.Error("Debe crear una factura primero.");
                            else
                                acciones.CalcularTotales(facturaActual);
                            break;

                        case 5:
                            if (facturaActual == null)
                                Colores.Error("Debe crear una factura primero.");
                            else
                                acciones.GenerarNCF(facturaActual);
                            break;

                        case 6:
                            if (facturaActual == null)
                                Colores.Error("Debe crear una factura primero.");
                            else
                                acciones.GuardarFactura(facturaActual);
                            facturaActual = null;
                            break;

                        case 7:
                            var encontrada = acciones.BuscarFactura();
                            if (encontrada == null)
                                Colores.Error("No existe esa factura.");
                            else
                            {
                                Colores.Exito("Factura encontrada:");
                                acciones.CalcularTotales(encontrada);
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


