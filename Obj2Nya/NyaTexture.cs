namespace Obj2Nya
{
    using PawCraft.Utils.Serializer;
    using PawCraft.Utils.Types;
    using System.Collections.Generic;

    /// <summary>
    /// Catgirl texture
    /// </summary>
    public class NyaTexture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NyaTexture" class
        /// </summary>
        public NyaTexture() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NyaTexture" class
        /// </summary>
        /// <param name="bitmap">Bitmap data</param>
        public NyaTexture(System.Drawing.Bitmap bitmap) : this()
        {
            this.Width = (short)bitmap.Width;
            this.Height = (short)bitmap.Height;
            List<Color> colors = new List<Color>();

            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    System.Drawing.Color color = bitmap.GetPixel(x, y);

                    if (color.A < 0x80)
                    {
                        colors.Add(Color.TransparentColor);
                    }
                    else
                    {
                        colors.Add(Color.FromRgb(color.R, color.G, color.B));
                    }
                }
            }

            this.Data = colors.ToArray();
        }

        /// <summary>
        /// GEts image width
        /// </summary>
        [FieldOrder(0)]
        public short Width { get; set; }

        /// <summary>
        /// Gets image height
        /// </summary>
        [FieldOrder(1)]
        public short Height { get; set; }

        /// <summary>
        /// Gets data length
        /// </summary>
        public int DataLength => this.Width * this.Height;

        /// <summary>
        /// Gets or sets image data
        /// </summary>
        [ArraySizeDynamic("DataLength")]
        [FieldOrder(2)]
        public Color[] Data { get; set; }
    }
}
