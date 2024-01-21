namespace PawCraft.Level
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using PawCraft.Utils.Serializer;
    using PawCraft.Utils.Types;

    /// <summary>
    /// Level map data
    /// </summary>
    public class LevelData
    {
        /// <summary>
        /// Size of both map dimensions
        /// </summary>
        public const int MapDimensionSize = 20;

        /// <summary>
        /// Current version number
        /// </summary>
        public const byte VersionNumber = 0;

        /// <summary>
        /// Level entities
        /// </summary>
        [ArraySizeDynamic("EntityCount")]
        [FieldOrder(6)]
        public EntityData[] Entities;

        /// <summary>
        /// Number of entities in the level
        /// </summary>
        [FieldOrder(5)]
        public int EntityCount;

        /// <summary>
        /// Entity data table
        /// </summary>
        [ArraySizeStatic(400)]
        [FieldOrder(3)]
        public Gourad[] Gourad;

        /// <summary>
        /// File identifier
        /// </summary>
        [ArraySizeStatic(4)]
        [FieldOrder(0)]
        public byte[] Identifier;

        /// <summary>
        /// Level light
        /// </summary>
        [FieldOrder(2)]
        public LevelLight Light;

        /// <summary>
        /// Normal vectors
        /// </summary>
        [ArraySizeStatic(400)]
        [FieldOrder(4)]
        public FxVector[] Normals;

        /// <summary>
        /// Tile data
        /// </summary>
        [ArraySizeStatic(400)]
        [FieldOrder(1)]
        public TileData[] TileData;

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

                return (LevelData)CustomMarshal.MarshalAsObject(stream, typeof(LevelData));
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
        /// Write to a file file
        /// </summary>
        /// <param name="filename">Path and full name of the file with extension</param>
        public void WriteToFile(string filename)
        {
            this.Identifier = new byte[] { (byte)'U', (byte)'T', (byte)'E', LevelData.VersionNumber };

            this.EntityCount = this.Entities.Length;
            List<byte> data = new List<byte>(CustomMarshal.MarshalAsBytes(this));

            using (FileStream stream = File.Open(filename, FileMode.Create))
            {
                stream.Write(data.ToArray(), 0, data.Count);
            }
        }
    }
}