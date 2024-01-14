namespace PawCraft.Level
{
    using PawCraft.Utils.Serializer;
    using PawCraft.Entities;
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
        /// Gets or sets X location
        /// </summary>
        [FieldOrder(1)]
        public ushort X;

        /// <summary>
        /// Gets or sets Y location
        /// </summary>
        [FieldOrder(2)]
        public ushort Y;

        /// <summary>
        /// Entity direction in radians
        /// </summary>
        [FieldOrder(3)]
        public int Direction;

        /// <summary>
        /// Reserved entity data
        /// </summary>
        [ArraySizeStatic(16)]
        [FieldOrder(3)]
        public byte[] Reserved = new byte[16];

        /// <summary>
        /// Reset reserved values
        /// </summary>
        internal void ResetReservedValues()
        {
            this.Reserved = new byte[16];
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
            /// Model entity
            /// </summary>
            [EntityProperty(typeof(ModelProperties))]
            Model,
        }
    }
}