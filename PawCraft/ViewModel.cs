namespace PawCraft
{
    using PawCraft.Level;
    using PawCraft.Utils.Types;
    using System;
    using System.Linq;

    /// <summary>
    /// Main view model
    /// </summary>
    public class ViewModel
    {
        private Level.LevelData data;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class
        /// </summary>
        public ViewModel()
        {
            this.data = new Level.LevelData
            {
                TileData = Enumerable.Range(0, 400).Select(value => new TileData()).ToArray(),
                Gourad = Enumerable.Range(0, 400).Select(value => Gourad.FromColor(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue))).ToArray(),
                Light = new Level.LevelLight { Color = Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue), Direction = new FxVector { Z = 65536 } },
                Identifier = new[] { (byte)'U', (byte)'T', (byte)'E', Level.LevelData.VersionNumber },
                Normals = Enumerable.Range(0, 400).Select(value => new FxVector() { Z = 65536 }).ToArray(),
                Entities = new EntityData[0]
            };

            this.CurrentLevelFile = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class
        /// </summary>
        /// <param name="filename">Map file to open</param>
        public ViewModel(string filename)
        {
            this.CurrentLevelFile = filename;
            this.data = Level.LevelData.Open(filename);

            for (int i = 0; i < this.data.Entities.Length; i++)
            {
                if (this.data.Entities[i].X >= LevelData.MapDimensionSize)
                {
                    this.data.Entities[i].X = (ushort)Math.Min((int)this.data.Entities[i].X, LevelData.MapDimensionSize);
                }

                if (this.data.Entities[i].Y >= LevelData.MapDimensionSize)
                {
                    this.data.Entities[i].Y = (ushort)Math.Min((int)this.data.Entities[i].X, LevelData.MapDimensionSize);
                }
            }
        }

        /// <summary>
        /// Gets current level file
        /// </summary>
        public string CurrentLevelFile { get; private set; }

        /// <summary>
        /// Gets level data
        /// </summary>
        public Level.LevelData LevelData
        {
            get
            {
                return this.data;
            }
        }

        /// <summary>
        /// Save level data
        /// </summary>
        /// <param name="filename">Path and full name of the file with extension</param>
        /// <exception cref="ArgumentNullException">File name is empty</exception>
        public void Save(string filename = null)
        {
            string targetFile = filename;

            if (string.IsNullOrWhiteSpace(filename))
            {
                targetFile = this.CurrentLevelFile;

                if (string.IsNullOrWhiteSpace(this.CurrentLevelFile))
                {
                    throw new ArgumentNullException("File name cannot be empty!");
                }
            }

            this.CurrentLevelFile = targetFile;
            this.LevelData.WriteToFile(targetFile);
        }

        /// <summary>
        /// Set level light data
        /// </summary>
        /// <param name="levelLight">Level light</param>
        /// <param name="lightTable">Light table</param>
        /// <param name="normals">Quad normals</param>
        public void SetLevelLight(Level.LevelLight levelLight, Gourad[] lightTable, FxVector[] normals)
        {
            if (lightTable.Length != 400 || normals.Length != 400)
            {
                throw new NotSupportedException("There must be 400 entries in the shading table!");
            }

            this.data.Normals = normals;
            this.data.Light = levelLight;
            this.data.Gourad = lightTable;
        }
    }
}