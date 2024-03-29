﻿namespace PawCraft.Level
{
    using PawCraft.Utils.Serializer;
    using PawCraft.Utils.Types;

    /// <summary>
    /// Level light data
    /// </summary>
    public struct LevelLight
    {
        /// <summary>
        /// Light color
        /// </summary>
        [FieldOrder(1)]
        public Color Color;

        /// <summary>
        /// Light direction
        /// </summary>
        [FieldOrder(0)]
        public FxVector Direction;

        /// <summary>
        /// Reserved padding
        /// </summary>
        [FieldOrder(2)]
        public short Reserved;
    }
}