﻿namespace Obj2Nya
{
    using System.Collections.Generic;
    using PawCraft.Utils.Serializer;
    using PawCraft.Utils.Types;

    /// <summary>
    /// Catgirl texture
    /// </summary>
    public class NyaTexture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NyaTexture" class
        /// </summary>
        public NyaTexture()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NyaTexture" class
        /// </summary>
        /// <param name="name">Texture name</param>
        /// <param name="bitmap">Bitmap data</param>
        public NyaTexture(string name, System.Drawing.Bitmap bitmap) : this()
        {
            this.Name = name;
            this.Width = (ushort)bitmap.Width;
            this.Height = (ushort)bitmap.Height;
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
        /// Gets or sets image data
        /// </summary>
        [ArraySizeDynamic("DataLength")]
        [FieldOrder(2)]
        public Color[] Data { get; set; }

        /// <summary>
        /// Gets data length
        /// </summary>
        public int DataLength => this.Width * this.Height;

        /// <summary>
        /// Material name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets image height
        /// </summary>
        [FieldOrder(1)]
        public ushort Height { get; set; }

        /// <summary>
        /// Gets image width
        /// </summary>
        [FieldOrder(0)]
        public ushort Width { get; set; }
    }
}