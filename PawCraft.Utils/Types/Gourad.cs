namespace PawCraft.Utils.Types
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Gourad color
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Gourad
    {
        /// <summary>
        /// Create gourad from single color
        /// </summary>
        /// <param name="color">Color data</param>
        /// <returns>Gourad color</returns>
        public static Gourad FromColor(Color color)
        {
            return new Gourad
            {
                Colors = Enumerable.Repeat(color, 4).ToArray()
            };
        }

        /// <summary>
        /// Create gourad from colors
        /// </summary>
        /// <param name="colors">Color collection (must contain 4 colors)</param>
        /// <returns>Grourad entry</returns>
        /// <exception cref="NotSupportedException">When parameters does not contain 4 colors</exception>
        public static Gourad FromColors(params Color[] colors)
        {
            if (colors == null || colors.Length != 4)
            {
                throw new NotSupportedException("Color parameters must contain 4 colors");
            }

            return new Gourad
            {
                Colors = colors
            };
        }

        /// <summary>
        /// Gourad color
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        [FieldOffset(0)]
        public Color[] Colors;
    }
}