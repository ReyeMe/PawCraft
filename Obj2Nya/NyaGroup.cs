namespace Obj2Nya
{
    using PawCraft.Utils.Serializer;
    using PawCraft.Utils.Types;

    /// <summary>
    /// Mesh group
    /// </summary>
    public class NyaGroup
    {
        /// <summary>
        /// Gets number of meshes in group
        /// </summary>
        [FieldOrder(0)]
        public int MeshCount { get; set; }

        /// <summary>
        /// Gets or sets meshes in group
        /// </summary>
        [ArraySizeDynamic("MeshCount")]
        [FieldOrder(2)]
        public NyaMesh[] Meshes { get; set; } = new NyaMesh[0];

        /// <summary>
        /// Gets number of textures in the file
        /// </summary>
        [FieldOrder(1)]
        public int TextureCount { get; set; }

        /// <summary>
        /// Gets or sets textures in the file
        /// </summary>
        [ArraySizeDynamic("TextureCount")]
        [FieldOrder(3)]
        public NyaTexture[] Textures { get; set; } = new NyaTexture[0];
    }

    /// <summary>
    /// Mesh group
    /// </summary>
    public class NyaSmoothGroup
    {
        /// <summary>
        /// Gets number of meshes in group
        /// </summary>
        [FieldOrder(0)]
        public int MeshCount { get; set; }

        /// <summary>
        /// Gets or sets meshes in group
        /// </summary>
        [ArraySizeDynamic("MeshCount")]
        [FieldOrder(2)]
        public NyaSmoothMesh[] Meshes { get; set; } = new NyaSmoothMesh[0];

        /// <summary>
        /// Gets number of textures in the file
        /// </summary>
        [FieldOrder(1)]
        public int TextureCount { get; set; }

        /// <summary>
        /// Gets or sets textures in the file
        /// </summary>
        [ArraySizeDynamic("TextureCount")]
        [FieldOrder(3)]
        public NyaTexture[] Textures { get; set; } = new NyaTexture[0];
    }
}