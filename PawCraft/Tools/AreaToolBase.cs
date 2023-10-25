namespace PawCraft.Tools
{
    using PawCraft.Level;
    using SharpGL;
    using SharpGL.SceneGraph;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// Area tool base
    /// </summary>
    public abstract class AreaToolBase : ToolBase
    {
        /// <summary>
        /// Gets or sets tool radius
        /// </summary>
        [DisplayName("Radius")]
        [Range(1, 70)]
        public int Radius { get; set; } = 1;

        /// <summary>
        /// Apply tool to the target tile
        /// </summary>
        /// <param name="targetTile">X and Y location of the tile</param>
        /// <param name="level">Level data</param>
        public override void Apply(Point targetTile, LevelData level)
        {
            for (int x = 0; x < LevelData.MapDimensionSize; x++)
            {
                for (int y = 0; y < LevelData.MapDimensionSize; y++)
                {
                    if (Math.Ceiling(new Vertex(targetTile.X - x, targetTile.Y - y, 0.0f).Magnitude()) < this.Radius + float.Epsilon)
                    {
                        this.ApplyToTile(new Point(x, y), targetTile, level);
                    }
                }
            }
        }

        /// <summary>
        /// Draw tool in 2D view
        /// </summary>
        /// <param name="gr">Bitmap graphics</param>
        /// <param name="targetTile">X and Y location of the tile</param>
        /// <param name="bitmapScale">by how much to scale X and Y to get real bitmap coordinates</param>
        /// <param name="level">Level data</param>
        public override void Draw2D(Graphics gr, Point targetTile, int bitmapScale, LevelData level)
        {
            int halfScale = (bitmapScale / 2);
            int grX = (targetTile.X * bitmapScale) + halfScale;
            int grY = (targetTile.Y * bitmapScale) + halfScale;
            int toolSize = this.Radius * bitmapScale * 2;

            gr.DrawLine(ToolBase.ToolPen, grX - halfScale, grY, grX + halfScale, grY);
            gr.DrawLine(ToolBase.ToolPen, grX, grY - halfScale, grX, grY + halfScale);
            gr.DrawEllipse(ToolBase.ToolPen, grX - (toolSize / 2), grY - (toolSize / 2), toolSize, toolSize);
        }

        /// <summary>
        /// Draw tool in 3d view
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        /// <param name="targetTile">X and Y location of the tile</param>
        /// <param name="level">Level data</param>
        public override void Draw3D(OpenGL gl, Point targetTile, LevelData level)
        {
            double maxDepth = 2.0f;
            float tileMiddleDepth = (float)level.GetTileVerticeHeights(targetTile.X, targetTile.Y).Sum() / 4.0f;

            gl.LineWidth(2.0f);
            gl.Begin(OpenGL.GL_LINE_STRIP);
            gl.Color(1.0f, 0.3f, 0.0f);
            gl.Vertex(targetTile.X + 0.5f, targetTile.Y + 0.5f, tileMiddleDepth);
            gl.Color(1.0f, 0.3f, 0.0f);
            gl.Vertex(targetTile.X + 0.5f, targetTile.Y + 0.5f, maxDepth + 1.0f);
            gl.End();

            gl.Begin(OpenGL.GL_LINE_STRIP);

            for (int i = 0; i <= 360; i+=20)
            {
                float angle = i * (float)(Math.PI / 180.0f);
                float x = targetTile.X + 0.5f + (this.Radius * (float)Math.Cos(angle));
                float y = targetTile.Y + 0.5f + (this.Radius * (float)Math.Sin(angle));

                gl.Color(1.0f, 0.3f, 0.0f);
                gl.Vertex(x, y, tileMiddleDepth + 0.5f);
            }

            gl.End();
            gl.LineWidth(1.0f);
        }

        /// <summary>
        /// Apply tool to target tile
        /// </summary>
        /// <param name="targetTile">Target tile to apply tool to</param>
        /// <param name="pickedTile">Picked tile (ussualy in middle of the area picker)</param>
        /// <param name="levelData">Level data</param>
        protected abstract void ApplyToTile(Point targetTile, Point pickedTile, LevelData levelData);
    }
}