namespace PawCraft.Tools
{
    using PawCraft.Level;
    using PawCraft.ToolsApi;
    using SharpGL;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Input;

    /// <summary>
    /// Paint texture onto terrain
    /// </summary>
    [ToolDialog(Type = typeof(PaintToolDialog))]
    public class PaintTool : AreaToolBase
    {
        /// <summary>
        /// Are we in rotation mode?
        /// </summary>
        private bool rotationMode = false;

        /// <summary>
        /// Texture index to paint
        /// </summary>
        [DisplayName("Texture")]
        [Range(0, 255)]
        public int TextureIndex { get; set; }

        /// <summary>
        /// Apply texture to the tile
        /// </summary>
        /// <param name="targetTile">Target tile coordinates</param>
        /// <param name="level">Level data</param>
        public override void Apply(Point targetTile, LevelData level)
        {
            if (this.rotationMode)
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

        /// <summary>
        /// Draw tool in 2D view
        /// </summary>
        /// <param name="gr">Bitmap graphics</param>
        /// <param name="targetTile">X and Y location of the tile</param>
        /// <param name="bitmapScale">by how much to scale X and Y to get real bitmap coordinates</param>
        /// <param name="level">Level data</param>
        public override void Draw2D(Graphics gr, Point targetTile, int bitmapScale, LevelData level)
        {
            if (this.rotationMode)
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

        /// <summary>
        /// Draw tool in 3d view
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        /// <param name="targetTile">X and Y location of the tile</param>
        /// <param name="level">Level data</param>
        public override void Draw3D(OpenGL gl, Point targetTile, LevelData level)
        {
            if (this.rotationMode)
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

        /// <summary>
        /// Keyboard key state changed
        /// </summary>
        /// <param name="keyCode">Pressed keys</param>
        /// <param name="state">Key state</param>
        /// <returns><see langword="true"/> if keyboard was handled</returns>
        public override bool OnKeyChanged(Keys keyCode, KeyStates state)
        {
            if (keyCode.HasFlag(Keys.ControlKey))
            {
                this.rotationMode = state == KeyStates.Down;

                if (this.rotationMode)
                {
                    this.OnToolStatusTextChanged("Release [CTRL] to return to paint mode.");
                }
                else
                {
                    this.OnToolStatusTextChanged("Hold [CTRL] to enter rotation mode.");
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Tool is starting
        /// </summary>
        public override void Starting()
        {
            this.OnToolStatusTextChanged("Hold [CTRL] to enter rotation mode.");
        }

        /// <summary>
        /// Apply tool to target tile
        /// </summary>
        /// <param name="targetTile">Target tile to apply tool to</param>
        /// <param name="pickedTile">Picked tile (ussualy in middle of the area picker)</param>
        /// <param name="levelData">Level data</param>
        protected override void ApplyToTile(Point targetTile, Point pickedTile, LevelData levelData)
        {
            levelData.TileData[LevelData.GeTileArrayIndex(targetTile.X, targetTile.Y)].TextureIndex = (byte)this.TextureIndex;
        }
    }
}