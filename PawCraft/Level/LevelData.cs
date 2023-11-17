namespace PawCraft.Level
{
    using PawCraft.Utils.Types;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Level map data
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct LevelData
    {
        /// <summary>
        /// Current version number
        /// </summary>
        public const byte VersionNumber = 0;

        /// <summary>
        /// Size of both map dimensions
        /// </summary>
        public const int MapDimensionSize = 20;

        /// <summary>
        /// File identifier
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        [FieldOffset(0)]
        public byte[] Identifier;

        /// <summary>
        /// Tile data
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 400)]
        [FieldOffset(4)]
        public TileData[] TileData;

        /// <summary>
        /// Level light
        /// </summary>
        [FieldOffset(1604)]
        public LevelLight Light;

        /// <summary>
        /// Entity data table
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 400)]
        [FieldOffset(1620)]
        public Gourad[] Gourad;

        /// <summary>
        /// Gets or sets tile data of specific tile
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Tile data</returns>
        public TileData this[int x, int y]
        {
            get
            {
                return this.TileData[LevelData.GeTileArrayIndex(x, y)];
            }

            set
            {
                this.TileData[LevelData.GeTileArrayIndex(x, y)] = value;
            }
        }

        /// <summary>
        /// Get depth of tile vertices
        /// <para>Order: [0,0] [0,1] [1,1] [1,0]</para>
        /// </summary>
        /// <param name="x">Tile X coordinate</param>
        /// <param name="y">Tile Y coordinate</param>
        /// <returns>Tile vertex heights</returns>
        public float[] GetTileVerticeHeights(int x, int y)
        {
            List<float> points = new List<float>();

            for (int px = 0; px < 2; px++)
            {
                int ax1 = Math.Max(Math.Min(x + px - 1, LevelData.MapDimensionSize - 1), 0);
                int ax2 = Math.Max(Math.Min(ax1 + 1, LevelData.MapDimensionSize - 1), 0);

                for (int py = 0; py < 2; py++)
                {
                    int ay1 = Math.Max(Math.Min(y + py - 1, LevelData.MapDimensionSize - 1), 0);
                    int ay2 = Math.Max(Math.Min(ay1 + 1, LevelData.MapDimensionSize - 1), 0);

                    points.Add(new[] { this[ax1, ay1].Depth, this[ax1, ay2].Depth, this[ax2, ay2].Depth, this[ax2, ay1].Depth }.Select(depth => depth / 16.0f).Sum() / 4.0f);
                }
            }

            return points.ToArray();
        }

        /// <summary>
        /// Get index in tile array
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <returns><c>-1</c> if out of range</returns>
        public static int GeTileArrayIndex(int x, int y)
        {
            if (x < 0 || y < 0 || y >= LevelData.MapDimensionSize || x >= LevelData.MapDimensionSize)
            {
                return -1;
            }

            return x + (y * LevelData.MapDimensionSize);
        }

        /// <summary>
        /// Write to a file file
        /// </summary>
        /// <param name="filename">Path and full name of the file with extension</param>
        public void WriteToFile(string filename)
        {
            this.Identifier = new byte[] { (byte)'U', (byte)'T', (byte)'E', LevelData.VersionNumber };

            int length = Marshal.SizeOf(this);
            IntPtr ptr = Marshal.AllocHGlobal(length);
            byte[] myBuffer = new byte[length];

            Marshal.StructureToPtr(this, ptr, true);
            Marshal.Copy(ptr, myBuffer, 0, length);
            Marshal.FreeHGlobal(ptr);

            using (FileStream stream = File.Open(filename, FileMode.Create))
            {
                stream.Write(myBuffer, 0, length);
            }
        }

        /// <summary>
        /// Open file
        /// </summary>
        /// <param name="filename">Path and full name of the file with extension</param>
        /// <returns>Level data</returns>
        /// <exception cref="FileLoadException">Unknown file format</exception>
        /// <exception cref="EndOfStreamException">Unexpected EOF</exception>
        public static LevelData Open(string filename)
        {
            using (FileStream stream = File.Open(filename, FileMode.Open))
            {
                // Check header of the file
                byte[] identifier = new byte[4];

                if (stream.Read(identifier, 0, identifier.Length) != identifier.Length ||
                    !(identifier[0] == 'U' && identifier[1] == 'T' && identifier[2] == 'E' && identifier[3] == LevelData.VersionNumber))
                {
                    throw new FileLoadException("Unknown format!");
                }

                // Seek back to start
                stream.Seek(0, SeekOrigin.Begin);

                // Attemt to read file
                byte[] bytes = new byte[Marshal.SizeOf(typeof(LevelData))];

                if (stream.Read(bytes, 0, bytes.Length) != bytes.Length)
                {
                    throw new EndOfStreamException("File is damaged!");
                }

                GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                LevelData theStructure = (LevelData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(LevelData));
                handle.Free();

                return theStructure;
            }
        }
    }
}