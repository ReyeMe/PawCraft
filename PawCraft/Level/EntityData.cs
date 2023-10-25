namespace PawCraft.Level
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Entity data
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct EntityData
    {
        /// <summary>
        /// Entity type
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        [FieldOffset(0)]
        public EntityType Type;

        /// <summary>
        /// Entity placement (depth offset, mirror flag and rotation)
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        [FieldOffset(1)]
        public byte Placement;

        /// <summary>
        /// Reserved for future use
        /// </summary>
        [FieldOffset(2)]
        public byte Reserved1;

        /// <summary>
        /// Reserved for future use
        /// </summary>
        [FieldOffset(3)]
        public byte Reserved2;

        /// <summary>
        /// 10 bytes reserved for future use
        /// </summary>
        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] Reserved3;

        /// <summary>
        /// Gets or sets depth offset from the tile (offset has 5 bits)
        /// </summary>
        public byte DepthOffset
        {
            get
            {
                return (byte)(this.Placement & 0x3F);
            }

            set
            {
                this.Placement = (byte)((this.Placement & 0xE0) | (value & 0x1F));
            }
        }


        /// <summary>
        /// Gets or sets a flag indicating whether entitiy is mirrored
        /// </summary>
        public bool Mirror
        {
            get
            {
                return (this.Placement & 0x10) != 0;
            }

            set
            {
                if (value)
                {
                    this.Placement |= 0x10;
                }
                else
                {
                    this.Placement &= 0xEF;
                }
            }
        }

        /// <summary>
        /// Gets or sets rotation of the entity on the tile
        /// </summary>
        public TileData.RotationState Rotate
        {
            get
            {
                return (TileData.RotationState)((byte)this.Placement & 0xC0 >> 6);
            }

            set
            {
                this.Placement = (byte)((this.Placement & 0x3F) | ((byte)value << 6));
            }
        }
        /// <summary>
        /// Type of the entity in this block
        /// </summary>
        public enum EntityType : byte
        {
            Empty = 0,
        }
    }
}