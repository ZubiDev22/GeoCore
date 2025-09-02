using System;
using System.Globalization;

namespace GeoCore.Utilidades
{
    public static class ValidationUtils
    {
        // Validación de cadena: mayúsculas si no es nula o vacía
        public static string ToUpperSafe(this string texto)
        {
            return string.IsNullOrEmpty(texto) ? texto : texto.ToUpper();
        }

        // Validación de cadena: capitalizar primera letra
        public static string CapitalizeFirst(this string texto)
        {
            if (string.IsNullOrEmpty(texto)) return texto;
            return char.ToUpper(texto[0]) + texto.Substring(1);
        }

        // Validación de número: es par
        public static bool IsEven(this int numero)
        {
            return numero % 2 == 0;
        }

        // Validación de división segura
        public static decimal SafeDivide(decimal a, decimal b)
        {
            return b != 0 ? a / b : throw new DivideByZeroException("No se puede dividir entre 0");
        }

        // Validación de fecha: formato dd/MM/yyyy
        public static bool IsValidDateFormat(string fecha)
        {
            return DateTime.TryParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        // Cálculo de antigüedad en años
        public static int GetYearsSince(string fecha)
        {
            if (!IsValidDateFormat(fecha)) return -1;
            var date = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var now = DateTime.Now;
            return now.Year - date.Year - (now.DayOfYear < date.DayOfYear ? 1 : 0);
        }
    }
}
