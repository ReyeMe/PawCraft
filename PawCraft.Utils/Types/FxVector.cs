namespace PawCraft.Utils.Types
{
    using PawCraft.Utils.Serializer;

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
        /// To fixed point conversion
        /// </summary>
        private const float Conversion = 65536.0f;

        /// <summary>
        /// Convert from float vector to fixed point vector
        /// </summary>
        /// <param name="data">Floating point vector</param>
        /// <returns>Fixed point vector</returns>
        public static FxVector FromFloatArray(float[] data)
        {
            return new FxVector
            {
                X = (int)(FxVector.Conversion * data[0]),
                Y = (int)(FxVector.Conversion * data[1]),
                Z = (int)(FxVector.Conversion * data[2]),
            };
        }

        /// <summary>
        /// Convert to float array from fixed point array
        /// </summary>
        /// <param name="vector">Fixed point vector</param>
        /// <returns>Floating point vector</returns>
        public static float[] ToFloatArray(FxVector vector)
        {
            return new float[]
            {
                vector.X / FxVector.Conversion,
                vector.Y / FxVector.Conversion,
                vector.Z / FxVector.Conversion,
            };
        }
    }
}