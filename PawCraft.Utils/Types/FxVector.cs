namespace PawCraft.Utils.Types
{
    using PawCraft.Utils.Serializer;
    using PawCraft.Utils;
    using SharpGL.SceneGraph;

    /// <summary>
    /// Fixed point vector
    /// </summary>
    public struct FxVector
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        [FieldOrder(0)]
        public int X;

        /// <summary>
        /// Y coordinate
        /// </summary>
        [FieldOrder(1)]
        public int Y;

        /// <summary>
        /// Z coordinate
        /// </summary>
        [FieldOrder(2)]
        public int Z;

        /// <summary>
        /// Convert from float vector to fixed point vector
        /// </summary>
        /// <param name="data">Floating point vector</param>
        /// <returns>Fixed point vector</returns>
        public static FxVector FromArray(float[] data)
        {
            return new FxVector
            {
                X = data[0].ToFixed(),
                Y = data[1].ToFixed(),
                Z = data[2].ToFixed(),
            };
        }

        /// <summary>
        /// Gets float point vertex as fixed point vector
        /// </summary>
        /// <param name="vertex">Float point vertex</param>
        /// <returns>Fixed point vector</returns>
        public static FxVector FromVertex(Vertex vertex)
        {
            return FxVector.FromArray(new[] { vertex.X, vertex.Y, vertex.Z });
        }

        /// <summary>
        /// Convert to float array from fixed point array
        /// </summary>
        /// <param name="vector">Fixed point vector</param>
        /// <returns>Floating point vector</returns>
        public static float[] ToArray(FxVector vector)
        {
            return new float[]
            {
                (float)vector.X.FromFixed(),
                (float)vector.Y.FromFixed(),
                (float)vector.Z.FromFixed(),
            };
        }

        /// <summary>
        /// Gets fixed point vector as float point vertex
        /// </summary>
        /// <param name="vector">Fixed point vector</param>
        /// <returns>Float point vector</returns>
        public static Vertex ToVertex(FxVector vector)
        {
            float[] array = FxVector.ToArray(vector);
            return new Vertex(array[0], array[1], array[2]);
        }
    }
}