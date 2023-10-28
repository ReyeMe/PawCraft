namespace PawCraft.Tools
{
    using PawCraft.Level;
    using PawCraft.ToolsApi;
    using SharpGL;
    using System.Drawing;

    /// <summary>
    /// Entity placing tool
    /// </summary>
    [ToolDialog(Type = typeof(EntityToolDialog))]
    public class EntityTool : ToolBase
    {
        public override void Apply(Point targetTile, LevelData level)
        {
        }

        public override void Draw2D(Graphics gr, Point targetTile, int bitmapScale, LevelData level)
        {
        }

        public override void Draw3D(OpenGL gl, Point targetTile, LevelData level)
        {
        }
    }
}
