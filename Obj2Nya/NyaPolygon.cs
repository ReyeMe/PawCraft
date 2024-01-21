namespace Obj2Nya
{
    using PawCraft.Utils.Serializer;
    using PawCraft.Utils.Types;

    /// <summary>
    /// Catgirl polygon
    /// </summary>
    public class NyaPolygon
    {
        /// <summary>
        /// Gets or sets polygon normal
        /// </summary>
        [FieldOrder(0)]
        public FxVector Normal { get; set; } = new FxVector();

        /// <summary>
        /// Gets or sets Point indicies
        /// </summary>
        [ArraySizeStatic(4)]
        [FieldOrder(1)]
        public short[] Vertices { get; set; } = new short[4];
    }
}