namespace PawCraft.Level
{
    using PawCraft.Utils.Serializer;
    using PawCraft.Utils.Types;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Level light data
    /// </summary>
    public struct LevelLight
    {
        /// <summary>
        /// Light X direction
        /// </summary>
        [FieldOrder(0)]
        public int X;

        /// <summary>
        /// Light Y direction
        /// </summary>
        [FieldOrder(1)]
        public int Y;

        /// <summary>
        /// Light Z direction
        /// </summary>
        [FieldOrder(2)]
        public int Z;

        /// <summary>
        /// Light color
        /// </summary>
        [FieldOrder(3)]
        public Color Color;

        /// <summary>
        /// Reserved padding
        /// </summary>
        [FieldOrder(7)]
        public short Reserved;
    }
}
