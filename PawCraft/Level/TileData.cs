﻿namespace PawCraft.Level
{
    using PawCraft.Utils.Serializer;

    /// <summary>
    /// Tile data
    /// </summary>
    public struct TileData
    {
        /// <summary>
        /// Depth and rotation are present in a single byte
        /// </summary>
        [FieldOrder(0)]
        public byte DepthAndRotationAndMirror;

        /// <summary>
        /// Offset to the entity data list
        /// </summary>
        [FieldOrder(2)]
        public ushort Dummy;

        /// <summary>
        /// Index of a texture to use
        /// </summary>
        [FieldOrder(1)]
        public byte TextureIndex;

        /// <summary>
        /// Texture rotation state
        /// </summary>
        public enum RotationState : byte
        {
            /// <summary>
            /// Do not rotate
            /// </summary>
            Rotate0 = 0,

            /// <summary>
            /// Rotate by 90 degrees
            /// </summary>
            Rotate90 = 1,

            /// <summary>
            /// Rotate by 180 degrees
            /// </summary>
            Rotate180 = 2,

            /// <summary>
            /// Rotate by 270 degrees
            /// </summary>
            Rotate270 = 3
        }

        /// <summary>
        /// Gets or sets depth of the tile (depth has 5 bits)
        /// </summary>
        public byte Depth
        {
            get
            {
                return (byte)(this.DepthAndRotationAndMirror & 0x3F);
            }

            set
            {
                this.DepthAndRotationAndMirror = (byte)((this.DepthAndRotationAndMirror & 0xE0) | (value & 0x1F));
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating whether texture is mirrored
        /// </summary>
        public bool MirrorTexture
        {
            get
            {
                return (this.DepthAndRotationAndMirror & 0x10) != 0;
            }

            set
            {
                if (value)
                {
                    this.DepthAndRotationAndMirror |= 0x10;
                }
                else
                {
                    this.DepthAndRotationAndMirror &= 0xEF;
                }
            }
        }

        /// <summary>
        /// Gets or sets rotation of the texture on the tile
        /// </summary>
        public TileData.RotationState RotateTexture
        {
            get
            {
                return (TileData.RotationState)((byte)(this.DepthAndRotationAndMirror & 0xC0) >> 6);
            }

            set
            {
                this.DepthAndRotationAndMirror = (byte)((this.DepthAndRotationAndMirror & 0x3F) | ((byte)value << 6));
            }
        }
    }
}