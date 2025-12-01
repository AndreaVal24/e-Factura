using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Factura
{
    //debe ser estatic para usarse como extension
    public static class Extensiones
    {
     
            // Formatea RNC: 123456789 -> 1-23456789-0
            public static string FormatearRNC(this string rnc)
            {
                if (string.IsNullOrEmpty(rnc) || rnc.Length != 9)
                    return rnc;

                rnc = rnc.Replace("-", "");
                return $"{rnc.Substring(0, 1)}-{rnc.Substring(1, 8)}-{CalcularDigitoVerificadorRNC(rnc)}";
            }

            // Formatea Cédula: 12345678901 -> 123-4567890-1
            public static string FormatearCedula(this string cedula)
            {
                if (string.IsNullOrEmpty(cedula) || cedula.Length != 11)
                    return cedula;

                cedula = cedula.Replace("-", "");
                return $"{cedula.Substring(0, 3)}-{cedula.Substring(3, 7)}-{cedula.Substring(10, 1)}";
            }

            // Valida que el RNC tenga exactamente 9 dígitos
            public static bool ValidarRNC(this string rnc)
            {
                if (string.IsNullOrEmpty(rnc))
                    return false;

                string rncLimpio = rnc.Replace("-", "");
                return rncLimpio.Length == 9 && rncLimpio.All(char.IsDigit);
            }

            // Valida que la Cédula tenga exactamente 11 dígitos
            public static bool ValidarCedula(this string cedula)
            {
                if (string.IsNullOrEmpty(cedula))
                    return false;

                string cedulaLimpia = cedula.Replace("-", "");
                return cedulaLimpia.Length == 11 && cedulaLimpia.All(char.IsDigit);
            }

            // Cálculo básico de dígito verificador para RNC (simplificado)
            private static int CalcularDigitoVerificadorRNC(string rnc)
            {
                int suma = 0;
                int[] multiplicadores = { 7, 9, 8, 6, 5, 4, 3, 2, 1 };

                for (int i = 0; i < 9; i++)
                {
                    suma += int.Parse(rnc[i].ToString()) * multiplicadores[i];
                }

                int residuo = suma % 11;
                return residuo == 0 ? 0 : residuo == 1 ? 1 : 11 - residuo;
            }
        
    }
}
