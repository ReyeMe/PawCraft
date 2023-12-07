namespace PawCraft.Level
{
    using PawCraft.Utils.Serializer;
    using PawCraft.Utils.Types;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Entity data
    /// </summary>
    public class EntityData
    {
        /// <summary>
        /// Entity type
        /// </summary>
        [FieldOrder(0)]
        public EntityType Type;

        /// <summary>
        /// Entity placement (depth offset, mirror flag and rotation)
        /// </summary>
        [FieldOrder(1)]
        public int Placement;

        /// <summary>
        /// Entity direction in radians
        /// </summary>
        [FieldOrder(2)]
        public int Direction;

        /// <summary>
        /// Reserved entity data
        /// </summary>
        [ArraySizeStatic(16)]
        [FieldOrder(3)]
        public byte[] Reserved = new byte[16];

        /// <summary>
        /// Gets or sets depth of the tile (depth has 5 bits)
        /// </summary>
        public short X
        {
            get
            {
                return (short)(this.Placement & 0x00FF);
            }

            set
            {
                this.Placement = (int)((this.Placement & 0xFF00) | (value & 0x00FF));
            }
        }

        /// <summary>
        /// Gets or sets depth of the tile (depth has 5 bits)
        /// </summary>
        public short Y
        {
            get
            {
                return (short)((this.Placement & 0xFF00) >> 8);
            }

            set
            {
                this.Placement = (int)((this.Placement & 0x00FF) | ((value & 0x00FF) << 8));
            }
        }

        /// <summary>
        /// Type of the entity in this block
        /// </summary>
        public enum EntityType : int
        {
            /// <summary>
            /// Empty entity
            /// </summary>
            Empty = 0,

            /// <summary>
            /// Player spawn entity
            /// </summary>
            [Display(Name = "Player spawn")]
            PlayerSpawn,

            /// <summary>
            /// Testing tree entity
            /// </summary>
            Tree,
        }
    }
}