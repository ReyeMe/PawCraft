namespace PawCraft.Tools
{
    using System;
    using System.Drawing;
    using PawCraft.Level;
    using PawCraft.ToolsApi;

    /// <summary>
    /// Dig a hole
    /// </summary>
    public class DigTool : AreaToolBase
    {
        /// <summary>
        /// Apply tool to target tile
        /// </summary>
        /// <param name="targetTile">Target tile to apply tool to</param>
        /// <param name="pickedTile">Picked tile (ussualy in middle of the area picker)</param>
        /// <param name="levelData">Level data</param>
        protected override void ApplyToTile(Point targetTile, Point pickedTile, LevelData levelData)
        {
            int current = levelData[targetTile.X, targetTile.Y].Depth - 1;
            levelData.TileData[LevelData.GeTileArrayIndex(targetTile.X, targetTile.Y)].Depth = (byte)Math.Max(Math.Min(current, 31), 0);
        }
    }
}