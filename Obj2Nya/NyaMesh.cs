namespace Obj2Nya
{
    using PawCraft.Utils.Serializer;
    using PawCraft.Utils.Types;

    /// <summary>
    /// Mesh data
    /// </summary>
    public class NyaMesh
    {
        /// <summary>
        /// Gets or sets face flags
        /// </summary>
        [ArraySizeDynamic("PolygonCount")]
        [FieldOrder(4)]
        public NyaFaceFlags[] FaceFlags { get; set; } = new NyaFaceFlags[0];

        /// <summary>
        /// Gets number of points
        /// </summary>
        [FieldOrder(0)]
        public int PointCount { get; set; }

        /// <summary>
        /// Gets or sets mesh points
        /// </summary>
        [ArraySizeDynamic("PointCount")]
        [FieldOrder(2)]
        public FxVector[] Points { get; set; } = new FxVector[0];

        /// <summary>
        /// Gets number of polygons
        /// </summary>
        [FieldOrder(1)]
        public int PolygonCount { get; set; }

        /// <summary>
        /// Gets or sets mesh polygon
        /// </summary>
        [ArraySizeDynamic("PolygonCount")]
        [FieldOrder(3)]
        public NyaPolygon[] Polygons { get; set; } = new NyaPolygon[0];
    }
}