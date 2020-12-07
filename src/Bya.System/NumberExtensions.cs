namespace System
{
    /// <summary>
    /// Extensiones de números
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// Parse decimal value to hours
        /// </summary>
        /// <param name="value">Must be hours</param>
        /// <param name="days"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        public static void ParseToTime(this decimal value, out int days, out int hours, out int minutes, out int seconds)
        {
            var time = TimeSpan.FromHours(Convert.ToDouble(value));
            days = time.Days;
            hours = time.Hours;
            minutes = time.Minutes;
            seconds = time.Seconds;
        }

        /// <summary>
        /// Parse double value to hours
        /// </summary>
        /// <param name="value">Must be hours</param>
        /// <param name="days"></param>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <param name="seconds"></param>
        public static void ParseToTime(this double value, out int days, out int hours, out int minutes, out int seconds)
        {
            var time = TimeSpan.FromHours(value);
            days = time.Days;
            hours = time.Hours;
            minutes = time.Minutes;
            seconds = time.Seconds;
        }

        /// <summary>
        /// Parse double value to string
        /// </summary>
        /// <param name="value">Must be hours</param>
        /// <param name="showDays">Muestra los días o los suma a las horas</param>
        /// <param name="showSeconds">Muestra los segundos</param>
        public static string ParseTimeToReadableHuman(this double value, bool showDays = true, bool showSeconds = true)
        {
            var isNegative = value < 0.0;

            var time = TimeSpan.FromHours(Math.Abs(value));

            string result;
            if (time.Days > 0)
            {
                if (showDays)
                {
                    result = $"{time.Days}d {time.Hours}h {time.Minutes}m";
                }
                else
                {
                    var hours = (time.Days * 24) + time.Hours;
                    result = $"{hours}h {time.Minutes}m";
                }
            }
            else
            {
                result = $"{time.Hours}h {time.Minutes}m";
            }

            if (showSeconds)
            {
                result += $" {time.Seconds}s";
            }

            if (isNegative)
            {
                result = $"-{result}";
            }

            return result;
        }


        /// <summary>
        /// Securiza la división, asegurando que el denominador no sea 0
        /// </summary>
        /// <param name="Numerator">The numerator.</param>
        /// <param name="Denominator">The denominator.</param>
        /// <returns></returns>
        public static decimal SafeDivision(this decimal Numerator, decimal Denominator)
        {
            return Denominator.EqualsEpsilon(0) ? 0 : Numerator / Denominator;
        }

        /// <summary>
        /// Intenta igualar recortando
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool EqualsEpsilon(this double a, double b, double epsilon = 0.0000001)
        {
            double absA = Math.Abs(a);
            double absB = Math.Abs(b);
            double diff = Math.Abs(a - b);

            if (a == b)
            {
                // shortcut, handles infinities
                return true;
            }
            else if (a == 0 || b == 0 || diff < double.Epsilon)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < epsilon;
            }
            else
            {
                // use relative error
                return diff / (absA + absB) < epsilon;
            }
        }

        /// <summary>
        /// Intenta igualar recortando
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool EqualsEpsilon(this decimal a, decimal b, decimal epsilon = 0.0000001M)
        {
            decimal absA = Math.Abs(a);
            decimal absB = Math.Abs(b);
            decimal diff = Math.Abs(a - b);

            if (a == b)
            {
                // shortcut, handles infinities
                return true;
            }
            else if (a == 0 || b == 0 || diff < Convert.ToDecimal(double.Epsilon))
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < epsilon;
            }
            else
            {
                // use relative error
                return diff / (absA + absB) < epsilon;
            }
        }

        /// <summary>
        /// Convierte un cantidad de un tamaño a otro
        /// </summary>
        /// <param name="units">Cantidad a convertir</param>
        /// <param name="currentUnit">Unidad de origen</param>
        /// <param name="destinationUnit">Unidad a la que se quiere convertir</param>
        /// <returns>Cantidad convertida</returns>
        public static double ConvertToByteUnit(this double units, ByteSizes currentUnit, ByteSizes destinationUnit)
        {
            int diff = (int)destinationUnit - (int)currentUnit;
            bool negative = diff < 0;
            diff = Math.Abs(diff);

            if (diff == 0) return units;

            if (negative)
            {
                return units * Math.Pow(1024L, diff);
            }

            return units / Math.Pow(1024L, diff);
        }

        /// <summary>
        /// Convierte un cantidad de un tamaño a otro
        /// </summary>
        /// <param name="units">Cantidad a convertir</param>
        /// <param name="currentUnit">Unidad de origen</param>
        /// <param name="destinationUnit">Unidad a la que se quiere convertir</param>
        /// <returns>Cantidad convertida</returns>
        public static double ConvertToByteUnit(this long units, ByteSizes currentUnit, ByteSizes destinationUnit)
        {
            var unitsConverted = Convert.ToDouble(units);
            return unitsConverted.ConvertToByteUnit(currentUnit, destinationUnit);
        }

        /// <summary>
        /// Convierte un cantidad de un tamaño a otro
        /// </summary>
        /// <param name="units">Cantidad a convertir</param>
        /// <param name="currentUnit">Unidad de origen</param>
        /// <param name="destinationUnit">Unidad a la que se quiere convertir</param>
        /// <returns>Cantidad convertida</returns>
        public static double ConvertToByteUnit(this int units, ByteSizes currentUnit, ByteSizes destinationUnit)
        {
            var unitsConverted = Convert.ToDouble(units);
            return unitsConverted.ConvertToByteUnit(currentUnit, destinationUnit);
        }

        /// <summary>
        /// Convierte un cantidad de un tamaño a otro
        /// </summary>
        /// <param name="units">Cantidad a convertir</param>
        /// <param name="currentUnit">Unidad de origen</param>
        /// <param name="destinationUnit">Unidad a la que se quiere convertir</param>
        /// <returns>Cantidad convertida</returns>
        public static double ConvertToByteUnit(this decimal units, ByteSizes currentUnit, ByteSizes destinationUnit)
        {
            var unitsConverted = Convert.ToDouble(units);
            return unitsConverted.ConvertToByteUnit(currentUnit, destinationUnit);
        }

        /// <summary>
        /// Convierte un cantidad de un tamaño a otro
        /// </summary>
        /// <param name="units">Cantidad a convertir</param>
        /// <param name="currentUnit">Unidad de origen</param>
        /// <param name="destinationUnit">Unidad a la que se quiere convertir</param>
        /// <returns>Cantidad convertida</returns>
        public static double ConvertToByteUnit(this float units, ByteSizes currentUnit, ByteSizes destinationUnit)
        {
            var unitsConverted = Convert.ToDouble(units);
            return unitsConverted.ConvertToByteUnit(currentUnit, destinationUnit);
        }
    }

    /// <summary>
    /// Tamaños de conversión
    /// </summary>
    public enum ByteSizes
    {
        /// <summary>
        /// Bytes
        /// </summary>
        Bytes = 0,
        /// <summary>
        /// Kilobytes
        /// </summary>
        Kilobytes = 1,
        /// <summary>
        /// Megabytes
        /// </summary>
        Megabytes = 2,
        /// <summary>
        /// Gigabytes
        /// </summary>
        Gigabytes = 3,
        /// <summary>
        /// Terabytes
        /// </summary>
        Terabytes = 4,
        /// <summary>
        /// Petabytes
        /// </summary>
        Petabytes = 5,
        /// <summary>
        /// Exabytes
        /// </summary>
        Exabytes = 6,
        /// <summary>
        /// Zetabytes
        /// </summary>
        Zetabytes = 7,
        /// <summary>
        /// Yotabytes
        /// </summary>
        Yotabytes = 8,
        /// <summary>
        /// Xentabytes
        /// </summary>
        Xentabytes = 9
    }
}
