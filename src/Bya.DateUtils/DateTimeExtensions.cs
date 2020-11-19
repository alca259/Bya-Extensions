using System.Globalization;

namespace System
{
    /// <summary>
    /// Extensiones de DateTime
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Randomizer
        /// <summary>
        /// Randomiza desde la hora hasta los milisegundos para esta fecha
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime RandomizeFromHours(this DateTime dt)
        {
            var rnd = new Random();
            return dt.AddHours(rnd.Next(-2, 5)).RandomizeFromMinutes();
        }

        /// <summary>
        /// Randomiza desde los minutos hasta los milisegundos para esta fecha
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime RandomizeFromMinutes(this DateTime dt)
        {
            var rnd = new Random();
            return dt.AddMinutes(rnd.Next(-2, 5)).RandomizeFromSeconds();
        }

        /// <summary>
        /// Randomiza desde los segundos hasta los milisegundos para esta fecha
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime RandomizeFromSeconds(this DateTime dt)
        {
            var rnd = new Random();
            return dt.AddSeconds(rnd.Next(0, 59)).RandomizeFromMilliseconds();
        }

        /// <summary>
        /// Randomiza los milisegundos para esta fecha
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime RandomizeFromMilliseconds(this DateTime dt)
        {
            var rnd = new Random();
            return dt.AddMilliseconds(rnd.Next(0, 999));
        }
        #endregion

        #region Setters
        /// <summary>
        /// Establece el año para esta fecha
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static DateTime SetYear(this DateTime dt, int newValue = 0)
        {
            var current = dt.Year;
            return dt.AddYears(-current).AddYears(newValue);
        }

        /// <summary>
        /// Establece el mes para esta fecha
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static DateTime SetMonth(this DateTime dt, int newValue = 0)
        {
            var current = dt.Month;
            return dt.AddMonths(-current).AddMonths(newValue);
        }

        /// <summary>
        /// Establece el día para esta fecha
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static DateTime SetDay(this DateTime dt, int newValue = 0)
        {
            var current = dt.Day;
            return dt.AddDays(-current).AddDays(newValue);
        }

        /// <summary>
        /// Establece la hora para esta fecha
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static DateTime SetHour(this DateTime dt, int newValue = 0)
        {
            var current = dt.Hour;
            return dt.AddHours(-current).AddHours(newValue);
        }

        /// <summary>
        /// Establece el minuto para esta fecha
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static DateTime SetMinute(this DateTime dt, int newValue = 0)
        {
            var current = dt.Minute;
            return dt.AddMinutes(-current).AddMinutes(newValue);
        }

        /// <summary>
        /// Establece el segundo para esta fecha
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static DateTime SetSecond(this DateTime dt, int newValue = 0)
        {
            var current = dt.Second;
            return dt.AddSeconds(-current).AddSeconds(newValue);
        }

        /// <summary>
        /// Establece el milisegundo para esta fecha
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static DateTime SetMillisecond(this DateTime dt, int newValue = 0)
        {
            var current = dt.Millisecond;
            return dt.AddMilliseconds(-current).AddMilliseconds(newValue);
        }
        #endregion

        #region Week utils
        /// <summary>
        /// Obtiene el día de inicio de la semana actual
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek">Día en el que empieza la semana</param>
        /// <returns></returns>
        public static DateTime GetStartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Obtiene el día de fin de la semana actual
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetEndOfWeek(this DateTime dt)
        {
            return dt.GetStartOfWeek().AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        /// <summary>
        /// Obtiene el día de inicio de la semana siguiente
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek">Día en el que empieza la semana</param>
        /// <returns></returns>
        public static DateTime GetNextStartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            return dt.GetStartOfWeek(startOfWeek).AddDays(7);
        }

        /// <summary>
        /// Obtiene el día de fin de la semana siguiente
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetNextEndOfWeek(this DateTime dt)
        {
            return dt.GetStartOfWeek().AddDays(7 + 6).AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        /// <summary>
        /// Obtiene el siguiente día que sea
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime GetNextWeekday(this DateTime dt, DayOfWeek day = DayOfWeek.Monday)
        {
            int daysToAdd = ((int)day - (int)dt.DayOfWeek + 7) % 7;
            return dt.AddDays(daysToAdd);
        }

        /// <summary>
        /// This presumes that weeks start with Monday.
        /// Week 1 is the 1st week of the year with a Thursday in it.
        /// </summary>
        public static int GetIso8601WeekOfYear(this DateTime time, CalendarWeekRule calendarWeekRule = CalendarWeekRule.FirstFourDayWeek, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, calendarWeekRule, firstDayOfWeek);
        }
        #endregion

        #region Month utils
        /// <summary>
        /// Obtiene el primer día del mes actual
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetStartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        /// <summary>
        /// Obtiene el último día del mes actual
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetEndOfMonth(this DateTime dt)
        {
            var daysInMonth = DateTime.DaysInMonth(dt.Year, dt.Month);
            return new DateTime(dt.Year, dt.Month, daysInMonth, 23, 59, 59);
        }

        /// <summary>
        /// Obtiene la diferencia de meses entre dos fechas sin importar el día
        /// </summary>
        /// <param name="dtEnd">Fecha de fin</param>
        /// <param name="dtStart">Fecha de inicio</param>
        /// <param name="sumOne">Suma un mes al finalizar el cálculo</param>
        /// <returns></returns>
        public static int GetMonthDiff(this DateTime dtEnd, DateTime dtStart, bool sumOne = false)
        {
            var months = Math.Abs(dtEnd.Month - dtStart.Month + 12 * (dtEnd.Year - dtStart.Year));
            if (sumOne)
            {
                months++;
            }
            return months;
        }
        #endregion

        #region Year utils
        /// <summary>
        /// Obtiene el primer día del año actual
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetStartOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }

        /// <summary>
        /// Obtiene el último día del año actual
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetEndOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 12, 31, 23, 59, 59);
        }
        #endregion

        #region Date equals
        /// <summary>
        /// Devuelve si una fecha tiene valor y es mayor que 1900
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsValid(this DateTime? dt, int year = 1900)
        {
            if (!dt.HasValue) return false;
            return dt.Value.IsValid(year);
        }

        /// <summary>
        /// Devuelve si una fecha es mayor que 1900
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsValid(this DateTime dt, int year = 1900)
        {
            return dt > new DateTime(year, 1, 1);
        }

        /// <summary>
        /// Compara dos fechas Año, Mes y Día
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool EqualsDate(this DateTime dt, DateTime other)
        {
            return dt.Year == other.Year && dt.Month == other.Month && dt.Day == other.Day;
        }

        /// <summary>
        /// Compara dos fechas solamente hora minuto y segundo
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool EqualsHourMinuteSecond(this DateTime dt, DateTime other)
        {
            return dt.Hour == other.Hour && dt.Minute == other.Minute && dt.Second == other.Second;
        }

        /// <summary>
        /// Obtiene el número de días para este año dependiendo de si es o no bisiesto
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int NaturalDaysInYear(this DateTime date)
        {
            return DateTime.IsLeapYear(date.Year) ? 366 : 365;
        }

        /// <summary>
        /// Devuelve la edad actual
        /// </summary>
        /// <param name="dateOfBirth"></param>
        /// <returns></returns>
        public static int Age(this DateTime dateOfBirth)
        {
            if (DateTime.Today.Month < dateOfBirth.Month ||
            DateTime.Today.Month == dateOfBirth.Month &&
             DateTime.Today.Day < dateOfBirth.Day)
            {
                return DateTime.Today.Year - dateOfBirth.Year - 1;
            }
            else
                return DateTime.Today.Year - dateOfBirth.Year;
        }
        #endregion
    }
}
