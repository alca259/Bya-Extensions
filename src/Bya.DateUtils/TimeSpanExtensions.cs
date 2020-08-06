namespace System
{
    /// <summary>
    /// Extensions de timespan
    /// </summary>
    public static class TimeSpanExtensions
    {
        public static readonly DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0);

        /// <summary>
        /// Retorna el timespan en base a la fecha y hora actual
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public static TimeSpan Now(this TimeSpan _) => TimeSpan.FromSeconds(DateTime.Now.Subtract(UnixTime).TotalSeconds);

        /// <summary>
        /// Retorna el timespan en base a la fecha de hoy
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public static TimeSpan Today(this TimeSpan _) => TimeSpan.FromSeconds(DateTime.Today.Subtract(UnixTime).TotalSeconds);

        /// <summary>
        /// Retorna el timespan en base a la fecha y hora suministradas por parámetro
        /// </summary>
        /// <param name="_"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static TimeSpan Convert(this TimeSpan _, DateTime date) => TimeSpan.FromSeconds(date.Subtract(UnixTime).TotalSeconds);
    }
}
