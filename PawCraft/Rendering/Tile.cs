namespace PawCraft.Rendering
{
    using PawCraft.Level;
    using PawCraft.Utils.Types;
    using SharpGL;
    using SharpGL.Enumerations;
    using SharpGL.SceneGraph;
    using SharpGL.SceneGraph.Assets;
    using SharpGL.SceneGraph.Core;
    using SharpGL.SceneGraph.Raytracing;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// Height map tile
    /// </summary>
    internal class Tile : SceneElement, IRenderable
    {
        /// <summary>
        /// UV layout
        /// </summary>
        private static readonly List<int[]> uv = new List<int[]>
        {
            new [] { 0, 0 },
            new [] { 0, 1 },
            new [] { 1, 1 },
            new [] { 1, 0 }
        };

        /// <summary>
        /// Initializes a new isntance of the <see cref="Tile"/> class
        /// </summary>
        /// <param name="location">Tile location</param>
        /// <param name="worldView">View window</param>
        internal Tile(Point location, WorldViewWindow worldView)
        {
            this.ParentWindow = worldView;
            this.Location = location;
        }

        /// <summary>
        /// Gets tile data
        /// </summary>
        public TileData Data
        {
            get
            {
                return ((PawCraftMainWindow)this.ParentWindow.MdiParent).ViewModel.LevelData[this.Location.X, this.Location.Y];
            }
        }

        /// <summary>
        /// Gets tile location
        /// </summary>
        public Point Location { get; }

        /// <summary>
        /// Gets parent window
        /// </summary>
        public WorldViewWindow ParentWindow { get; }

        /// <summary>
        /// Gets or sets tile shading
        /// </summary>
        public Gourad Shading
        {
            get
            {
                int location = LevelData.GeTileArrayIndex(this.Location.X, this.Location.Y);
                return ((PawCraftMainWindow)this.ParentWindow.MdiParent).ViewModel.LevelData.Gourad[location];
            }
        }

        /// <summary>
        /// Gets texture atlas
        /// </summary>
        public TextureHandler TextureAtlas
        {
            get
            {
                return this.ParentWindow.TextureAtlas;
            }
        }

        /// <summary>
        /// Gets vertices in global
        /// </summary>
        /// <returns>Vertices</returns>
        public IEnumerable<Vertex> GetVertices()
        {
            float[] depths = ((PawCraftMainWindow)this.ParentWindow.MdiParent).ViewModel.LevelData.GetTileVerticeHeights(this.Location.X, this.Location.Y);

            return new[]
            {
                new Vertex(this.Location.X, this.Location.Y, depths[0]),
                new Vertex(this.Location.X, this.Location.Y + 1, depths[1]),
                new Vertex(this.Location.X + 1, this.Location.Y + 1, depths[3]),
                new Vertex(this.Location.X + 1, this.Location.Y, depths[2]),
            };
        }

        /// <summary>
        /// Render object
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        /// <param name="renderMode">Rendering mode</param>
        public void Render(OpenGL gl, RenderMode renderMode)
        {
            TileData tile = this.Data;
            Texture texture = this.TextureAtlas.GetTexture(tile.TextureIndex);
            WorldViewWindow.ShadingMode shading = this.ParentWindow.CurrentShadingMode;

            List<Vertex> vertices = this.GetVertices().ToList();
            FxVector vector = ((PawCraftMainWindow)this.ParentWindow.MdiParent).ViewModel.LevelData.Normals[LevelData.GeTileArrayIndex(this.Location.X, this.Location.Y)];
            Vertex normalVector = FxVector.ToVertex(vector);

            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_LIGHT0);
            gl.Disable(OpenGL.GL_TEXTURE_2D);

            if (this.ParentWindow.ShowTileNormals)
            {
                Vertex center = new Vertex();

                for (int point = 0; point < vertices.Count; point++)
                {
                    center += vertices[point];
                }

                center /= 4.0f;

                gl.Begin(OpenGL.GL_LINES);

                gl.Color(0.0f, 1.0f, 0.3f);
                gl.Vertex(center);

                gl.Color(0.0f, 1.0f, 0.3f);
                gl.Vertex(center + normalVector);

                gl.End();
            }

            if (shading == WorldViewWindow.ShadingMode.Textured || shading == WorldViewWindow.ShadingMode.TexturedShaded)
            {
                if (texture != null)
                {
                    gl.Enable(OpenGL.GL_TEXTURE_2D);
                    texture.Push(gl);
                }
            }

            gl.DepthFunc(OpenGL.GL_LESS);

            gl.Begin(OpenGL.GL_QUADS);

            int uvIndex = 0;

            // Shading color
            Gourad gourad = this.Shading;

            // Get mirrored or not mirrored UV points
            List<int[]> uvs = (tile.MirrorTexture ? Tile.uv.Reverse<int[]>() : Tile.uv).ToList();

            // Shift accordint to rotation
            for (int rot = 0; rot < (int)tile.RotateTexture; rot++)
            {
                uvs = uvs.Skip(1).Concat(uvs.Take(1)).ToList();
            }

            for (int point = 0; point < vertices.Count; point++)
            {
                if (shading == WorldViewWindow.ShadingMode.Textured)
                {
                    gl.Color(1.0f, 1.0f, 1.0f);
                }
                else if (shading == WorldViewWindow.ShadingMode.Heightmap)
                {
                    float light = 0.5f + (0.5f * vertices[point].Z);
                    gl.Color(light, light, light);
                }
                else
                {
                    gl.Color(gourad.Colors[point].Red, gourad.Colors[point].Green, gourad.Colors[point].Blue);
                }

                gl.Normal(normalVector);
                gl.TexCoord(uvs[uvIndex][0], uvs[uvIndex][1]);
                gl.Vertex(vertices[point].X, vertices[point].Y, vertices[point].Z);

                if (++uvIndex >= 4)
                {
                    uvIndex = 0;
                }
            }

            gl.End();

            if (texture != null)
            {
                texture.Pop(gl);
                gl.Disable(OpenGL.GL_TEXTURE_2D);
            }
        }
    }
}