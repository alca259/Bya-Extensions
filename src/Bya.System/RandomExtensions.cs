namespace System
{
    /// <summary>
    /// Extensiones de random
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Siguiente doble entre rango
        /// </summary>
        /// <param name="random"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}
