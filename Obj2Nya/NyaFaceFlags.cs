namespace Obj2Nya
{
    using PawCraft.Utils.Serializer;
    using PawCraft.Utils.Types;

    /// <summary>
    /// Catgirl face flags
    /// </summary>
    public class NyaFaceFlags
    {
        /// <summary>
        /// Gets or sets base color
        /// </summary>
        [FieldOrder(1)]
        public Color BaseColor { get; set; } = new Color();

        /// <summary>
        /// Gets or sets flags
        /// </summary>
        [FieldOrder(0)]
        public short Flags { get; set; } = 0x7f;

        /// <summary>
        /// Gets or sets texture assigned to this face
        /// </summary>
        [FieldOrder(2)]
        public int TextureId { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether face has mesh effect
        /// </summary>
        public bool HasMeshEffect
        {
            get
            {
                return (this.Flags & 0x40) != 0;
            }

            set
            {
                this.Flags = (short)((this.Flags & 0xbf) | (value ? 0x40 : 0));
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating whether face has a texture
        /// </summary>
        public bool HasTexture
        {
            get
            {
                return (this.Flags & 0x80) != 0;
            }

            set
            {
                this.Flags =  (short)((this.Flags & 0x7f) | (value ? 0x80 : 0));
            }
        }
    }
}