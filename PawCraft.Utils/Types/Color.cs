namespace PawCraft.Utils.Types
{
    using PawCraft.Utils.Serializer;
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Color definition
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// RGB1555 color depth
        /// </summary>
        private const short ColorDepth = 0x1f;

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct
        /// </summary>
        /// <param name="red">Red color channel</param>
        /// <param name="green">Green color channel</param>
        /// <param name="blue">Blue color channel</param>
        public static Color FromRgb(float red, float green, float blue)
        {
            if (red < 0.0f || green < 0.0f || blue < 0.0f || red > 1.0f || green > 1.0f || blue > 1.0f)
            {
                throw new ArgumentException("Color channel value must be in range of 0.0f-1.0f!");
            }

            return Color.FromRgb((byte)(red * byte.MaxValue), (byte)(green * byte.MaxValue), (byte)(blue * byte.MaxValue));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Color"/> struct
        /// </summary>
        /// <param name="red">Red color channel</param>
        /// <param name="green">Green color channel</param>
        /// <param name="blue">Blue color channel</param>
        public static Color FromRgb(byte red, byte green, byte blue)
        {
            return new Color
            {
                Red = red,
                Green = green,
                Blue = blue,
                Transparent = false
            };
        }

        /// <summary>
        /// Transparent color
        /// </summary>
        public static readonly Color TransparentColor = new Color { ARGB = 0x0000 };

        /// <summary>
        /// Color data
        /// </summary>
        [FieldOrder(0)]
        public ushort ARGB;

        /// <summary>
        /// Gets or sets a transparent bit
        /// </summary>
        public bool Transparent
        {
            get
            {
                return (this.ARGB & 0x8000) == 0;
            }

            set
            {
                this.ARGB = value ? (ushort)(this.ARGB & 0x7fff) : (ushort)(this.ARGB | 0x8000);
            }
        }

        /// <summary>
        /// Gets or sets red channel
        /// </summary>
        public byte Blue
        {
            get
            {
                return (byte)((((this.ARGB & 0x7c00) >> 10) / (float)Color.ColorDepth) * byte.MaxValue);
            }

            set
            {
                byte channelValue = (byte)((byte)(Color.ColorDepth * (value / (float)byte.MaxValue)) & Color.ColorDepth);
                this.ARGB = (ushort)((this.ARGB & 0x83ff) | (channelValue << 10));
            }
        }

        /// <summary>
        /// Gets or sets green channel
        /// </summary>
        public byte Green
        {
            get
            {
                return (byte)((((this.ARGB & 0x3e0) >> 5) / (float)Color.ColorDepth) * byte.MaxValue);
            }

            set
            {
                byte channelValue = (byte)((byte)(Color.ColorDepth * (value / (float)byte.MaxValue)) & Color.ColorDepth);
                this.ARGB = (ushort)((this.ARGB & 0xfc1f) | (channelValue << 5));
            }
        }

        /// <summary>
        /// Gets or sets blue channel
        /// </summary>
        public byte Red
        {
            get
            {
                return (byte)(((this.ARGB & 0x1f) / (float)Color.ColorDepth) * byte.MaxValue);
            }

            set
            {
                byte channelValue = (byte)((byte)(Color.ColorDepth * (value / (float)byte.MaxValue)) & Color.ColorDepth);
                this.ARGB = (ushort)((this.ARGB & 0xffe0) | channelValue);
            }
        }
    }
}