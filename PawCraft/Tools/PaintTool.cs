namespace PawCraft.Tools
{
    using PawCraft.Level;
    using SharpGL;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;
    using System.Linq;
    using System.Reflection.Emit;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// Paint texture onto terrain
    /// </summary>
    public class PaintTool : AreaToolBase
    {
        /// <summary>
        /// Texture index to paint
        /// </summary>
        [DisplayName("Texture")]
        [Range(0,255)]
        public int TextureIndex { get; set; }

        /// <summary>
        /// Apply texture to the tile
        /// </summary>
        /// <param name="targetTile">Target tile coordinates</param>
        /// <param name="level">Level data</param>
        public override void Apply(Point targetTile, LevelData level)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                int tileIndex = LevelData.GeTileArrayIndex(targetTile.X, targetTile.Y);
                
                if (level.TileData[tileIndex].TextureIndex == this.TextureIndex)
                {
                    int rot = (int)level.TileData[tileIndex].RotateTexture + 1;
                    level.TileData[tileIndex].RotateTexture = (TileData.RotationState)(rot >= 4 ? 0 : rot);
                }
                else
                {
                    this.ApplyToTile(targetTile, targetTile, level);
                }
            }
            else
            {
                base.Apply(targetTile, level);
            }
        }

        public override void Draw2D(Graphics gr, Point targetTile, int bitmapScale, LevelData level)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                int halfScale = (bitmapScale / 2);
                int grX = (targetTile.X * bitmapScale) + halfScale;
                int grY = (targetTile.Y * bitmapScale) + halfScale;

                gr.DrawLine(ToolBase.ToolPen, grX - halfScale, grY, grX + halfScale, grY);
                gr.DrawLine(ToolBase.ToolPen, grX, grY - halfScale, grX, grY + halfScale);
            }
            else
            {
                base.Draw2D(gr, targetTile, bitmapScale, level);
            }
        }

        public override void Draw3D(OpenGL gl, Point targetTile, LevelData level)
        {
            if (Control.ModifierKeys == Keys.Control)
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
                gl.LineWidth(1.0f);
            }
            else
            {
                base.Draw3D(gl, targetTile, level);
            }
        }

        protected override void ApplyToTile(Point targetTile, Point pickedTile, LevelData levelData)
        {
            levelData.TileData[LevelData.GeTileArrayIndex(targetTile.X, targetTile.Y)].TextureIndex = (byte)this.TextureIndex;
        }
    }
}
