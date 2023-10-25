namespace PawCraft.Tools
{
    using PawCraft.Level;
    using System.Drawing;

    /// <summary>
    /// Tool base class
    /// </summary>
    public abstract class ToolBase
    {
        /// <summary>
        /// Pen used to draw 2D tool DM
        /// </summary>
        protected static readonly Pen ToolPen = new Pen(Color.Yellow, 1.0f);

        /// <summary>
        /// Apply tool to the target tile
        /// </summary>
        /// <param name="targetTile">X and Y location of the tile</param>
        /// <param name="level">Level data</param>
        public abstract void Apply(Point targetTile, LevelData level);

        /// <summary>
        /// Draw tool in 2D view
        /// </summary>
        /// <param name="gr">Bitmap graphics</param>
        /// <param name="targetTile">X and Y location of the tile</param>
        /// <param name="bitmapScale">by how much to scale X and Y to get real bitmap coordinates</param>
        /// <param name="level">Level data</param>
        public abstract void Draw2D(Graphics gr, Point targetTile, int bitmapScale, LevelData level);

        /// <summary>
        /// Draw tool in 3d view
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        /// <param name="targetTile">X and Y location of the tile</param>
        /// <param name="level">Level data</param>
        public abstract void Draw3D(SharpGL.OpenGL gl, Point targetTile, LevelData level);
    }
}