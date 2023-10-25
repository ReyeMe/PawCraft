namespace PawCraft
{
    using System;
    using System.Linq;

    /// <summary>
    /// Main view model
    /// </summary>
    public class ViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class
        /// </summary>
        public ViewModel()
        {
            this.LevelData = new Level.LevelData
            {
                TileData = new Level.TileData[Level.LevelData.MapDimensionSize * Level.LevelData.MapDimensionSize],
                EntityData = Enumerable.Range(0, byte.MaxValue + 1).Select(value => new Level.EntityData { Reserved3 = new byte[12] }).ToArray(),
                Identifier = new[] { (byte)'U', (byte)'T', (byte)'E', Level.LevelData.VersionNumber }
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
            this.LevelData = Level.LevelData.Open(filename);
        }

        /// <summary>
        /// Gets current level file
        /// </summary>
        public string CurrentLevelFile { get; private set; }

        /// <summary>
        /// Gets level data
        /// </summary>
        public Level.LevelData LevelData { get; }

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
    }
}