namespace PawCraft.Tools
{
    using PawCraft.Level;
    using PawCraft.Rendering;
    using PawCraft.ToolsApi;
    using PawCraft.Utils.Types;
    using SharpGL;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// Entity placing tool
    /// </summary>
    [ToolDialog(Type = typeof(EntityToolDialog))]
    public class EntityTool : ToolBase
    {
        /// <summary>
        /// Gets or sets currently selected entity
        /// </summary>
        public Level.EntityData.EntityType SelectedEntity { get; set; }

        /// <summary>
        /// Gets or sets texture atlas
        /// </summary>
        public TextureHandler TextureAtlas { get; set; }

        /// <summary>
        /// Gets or sets entity container
        /// </summary>
        public EntitiesContainer Container { get; set; }

        /// <summary>
        /// Apply tool to the target tile
        /// </summary>
        /// <param name="targetTile">X and Y location of the tile</param>
        /// <param name="level">Level data</param>
        public override void Apply(Point targetTile, LevelData level)
        {
            if (this.SelectedEntity != EntityData.EntityType.Empty)
            {
                float tileMiddleDepth = (float)level.GetTileVerticeHeights(targetTile.X, targetTile.Y).Sum() / 4.0f;

                level.Entities = new List<EntityData>(level.Entities).Concat(
                    new[] {
                        new EntityData
                        {
                            Type = this.SelectedEntity,
                            X = (short)targetTile.X,
                            Y = (short)targetTile.Y
                        }
                    }).ToArray();

                this.Container.Refresh();
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
            if (this.SelectedEntity != EntityData.EntityType.Empty)
            {

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
            if (this.SelectedEntity != EntityData.EntityType.Empty)
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
        }
    }
}
