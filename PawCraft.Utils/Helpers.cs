namespace PawCraft.Utils
{
    using System;

    /// <summary>
    /// Help methods
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Fixed point base value
        /// </summary>
        private const double FixedPoint = 65536.0;

        /// <summary>
        /// From fixed point
        /// </summary>
        /// <param name="value">Fixed point value</param>
        /// <returns>Double value</returns>
        public static double FromFixed(this int value)
        {
            return value / Helpers.FixedPoint;
        }

        /// <summary>
        /// Convert to degrees
        /// </summary>
        /// <param name="value">Radians value</param>
        /// <returns>Degrees value</returns>
        public static double FromRadians(this double value)
        {
            return value * (180.0 / Math.PI);
        }

        /// <summary>
        /// To fixed point
        /// </summary>
        /// <param name="value">Integer value</param>
        /// <returns>Fixed point value</returns>
        public static int ToFixed(this int value)
        {
            return (int)(value * Helpers.FixedPoint);
        }

        /// <summary>
        /// To fixed point
        /// </summary>
        /// <param name="value">Double value</param>
        /// <returns>Fixed point value</returns>
        public static int ToFixed(this double value)
        {
            return (int)(value * Helpers.FixedPoint);
        }

        /// <summary>
        /// To fixed point
        /// </summary>
        /// <param name="value">Float value</param>
        /// <returns>Fixed point value</returns>
        public static int ToFixed(this float value)
        {
            return (int)(value * Helpers.FixedPoint);
        }

        /// <summary>
        /// Convert to radians
        /// </summary>
        /// <param name="value">Degrees value</param>
        /// <returns>Radians value</returns>
        public static double ToRadians(this double value)
        {
            return value * (Math.PI / 180.0);
        }
    }
}