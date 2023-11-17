namespace PawCraft.Tools
{
    using PawCraft.Level;
    using PawCraft.ToolsApi;
    using SharpGL;
    using SharpGL.SceneGraph;
    using System;
    using System.Drawing;
    using Color = Utils.Types.Color;

    /// <summary>
    /// Env light tool
    /// </summary>
    [ToolDialog(Type = typeof(LightToolDialog))]
    public class LightTool : ToolBase
    {
        /// <summary>
        /// Initializes a new isntance of the <see cref="LightTool"/> class
        /// </summary>
        public LightTool()
        {
            Vertex defaultDirection = new Vertex(1.0f, 1.0f, -1.0f);
            defaultDirection.Normalize();
            this.SunDirection = defaultDirection;
        }

        /// <summary>
        /// Gets or sets env color
        /// </summary>
        public Color SunColor { get; set; }

        /// <summary>
        /// Gets or sets direction in which the sun is shining
        /// </summary>
        public Vertex SunDirection { get; set; }

        /// <summary>
        /// Apply tool to the target tile
        /// </summary>
        /// <param name="targetTile">X and Y location of the tile</param>
        /// <param name="level">Level data</param>
        public override void Apply(Point targetTile, LevelData level)
        {
            // Do nothing
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
            // Do nothing
        }

        /// <summary>
        /// Draw tool in 3d view
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        /// <param name="targetTile">X and Y location of the tile</param>
        /// <param name="level">Level data</param>
        public override void Draw3D(OpenGL gl, Point targetTile, LevelData level)
        {
            // Do nothing
        }

        /// <summary>
        /// Draw tool in 3D view while tool is active
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        /// <param name="level">Level data</param>
        public override void Draw3DContinuous(SharpGL.OpenGL gl, LevelData level)
        {
            Vertex center = new Vertex(
                (LevelData.MapDimensionSize + 1) / 2.0f,
                (LevelData.MapDimensionSize + 1) / 2.0f,
                5.0f);

            gl.Begin(OpenGL.GL_LINES);

            gl.Color((byte)0, byte.MaxValue, (byte)(byte.MaxValue / 2));
            gl.Vertex(center + (this.SunDirection * 0.3f));

            gl.Color((byte)0, byte.MaxValue, (byte)(byte.MaxValue / 2));
            gl.Vertex(center + (this.SunDirection * 2.0f));

            gl.End();

            // Draw sphere
            LightTool.DrawSphere(gl, center, 0.3, 4, 8);
        }

        /// <summary>
        /// Draw 3D sphere
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        /// <param name="center">Center point</param>
        /// <param name="radius">Sphere radius</param>
        /// <param name="lats">Latitude lines</param>
        /// <param name="longs">Longditude lines</param>
        private static void DrawSphere(OpenGL gl, Vertex center, double radius, int lats, int longs)
        {
            for (int i = 0; i <= lats; i++)
            {
                double lat0 = Math.PI * (-0.5 + (double)(i - 1) / lats);
                double z0 = Math.Sin(lat0);
                double zr0 = Math.Cos(lat0);

                double lat1 = Math.PI * (-0.5 + (double)i / lats);
                double z1 = Math.Sin(lat1);
                double zr1 = Math.Cos(lat1);

                gl.Begin(OpenGL.GL_QUAD_STRIP);

                for (int j = 0; j <= longs; j++)
                {
                    double lng = 2.0 * Math.PI * (double)(j - 1) / longs;
                    double x = Math.Cos(lng);
                    double y = Math.Sin(lng);

                    gl.Color((byte)0, byte.MaxValue, (byte)0);
                    gl.Vertex(center.X + (radius * x * zr0), center.Y + (radius * y * zr0), center.Z + (radius * z0));

                    gl.Color((byte)0, byte.MaxValue, (byte)0);
                    gl.Vertex(center.X + (radius * x * zr1), center.Y + (radius * y * zr1), center.Z + (radius * z1));
                }

                gl.End();

                gl.Begin(OpenGL.GL_LINES);

                for (int j = 0; j <= longs; j++)
                {
                    double lng = 2.0 * Math.PI * (double)(j - 1) / longs;
                    double x = Math.Cos(lng);
                    double y = Math.Sin(lng);

                    gl.Color(byte.MaxValue, byte.MaxValue, (byte)0);
                    gl.Vertex(center.X + (radius * x * zr0), center.Y + (radius * y * zr0), center.Z + (radius * z0));

                    gl.Color(byte.MaxValue, byte.MaxValue, (byte)0);
                    gl.Vertex(center.X + (radius * x * zr1), center.Y + (radius * y * zr1), center.Z + (radius * z1));
                }

                gl.End();
            }
        }
    }
}