namespace PawCraft.Level
{
    using PawCraft.Utils.Types;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Level light data
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 2)]
    public struct LevelLight
    {
        /// <summary>
        /// Light X direction
        /// </summary>
        [FieldOffset(0)]
        public int X;

        /// <summary>
        /// Light Y direction
        /// </summary>
        [FieldOffset(4)]
        public int Y;

        /// <summary>
        /// Light Z direction
        /// </summary>
        [FieldOffset(8)]
        public int Z;

        /// <summary>
        /// Light color
        /// </summary>
        [FieldOffset(12)]
        public Color Color;

        /// <summary>
        /// Reserved padding
        /// </summary>
        [FieldOffset(14)]
        public short Reserved;
    }
}
