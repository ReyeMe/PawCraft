namespace PawCraft.Rendering
{
    using PawCraft.Level;
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
    internal class Tile : SceneElement, IRenderable, IRayTracable, ICustomVolumeBound
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
        /// <param name="level">Level data</param>
        /// <param name="location">Tile location</param>
        /// <param name="textureAtlas">Texture atlas</param>
        internal Tile(LevelData level, Point location, WorldViewWindow worldView)
        {
            this.ParentWindow = worldView;
            this.Level = level;
            this.Location = location;
            this.BoundingVolume = new TileBoundingVolume(this);
        }

        /// <summary>
        /// Gets bounding volume
        /// </summary>
        public IRenderable BoundingVolume { get; }

        /// <summary>
        /// Gets tile data
        /// </summary>
        public TileData Data
        {
            get
            {
                return this.Level[this.Location.X, this.Location.Y];
            }
        }

        /// <summary>
        /// Gets level data
        /// </summary>
        public LevelData Level { get; }

        /// <summary>
        /// Gets tile location
        /// </summary>
        public Point Location { get; }

        /// <summary>
        /// Gets parent window
        /// </summary>
        public WorldViewWindow ParentWindow { get; }

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
            float[] depths = this.Level.GetTileVerticeHeights(this.Location.X, this.Location.Y);

            return new[]
            {
                new Vertex(this.Location.X, this.Location.Y, depths[0]),
                new Vertex(this.Location.X, this.Location.Y + 1, depths[1]),
                new Vertex(this.Location.X + 1, this.Location.Y + 1, depths[3]),
                new Vertex(this.Location.X + 1, this.Location.Y, depths[2]),
            };
        }

        /// <summary>
        /// Raytrace the object
        /// </summary>
        /// <param name="ray">Casted ray</param>
        /// <param name="scene">Rendered scene</param>
        /// <returns>The instesection</returns>
        public Intersection Raytrace(Ray ray, Scene scene)
        {
            //	This code came from jgt intersect_triangle code (search dogpile for it).
            Intersection intersect = new Intersection();
            List<Vertex> vertices = this.GetVertices().ToList();

            //	Find the point of intersection upon the plane, as a point 't' along
            //	the ray.
            Vertex point1OnPlane = vertices[0];
            Vertex point2OnPlane = vertices[1];
            Vertex point3OnPlane = vertices[2];
            Vertex midpointOpp1 = (point2OnPlane + point3OnPlane) / 2;
            Vertex midpointOpp2 = (point1OnPlane + point3OnPlane) / 2;
            Vertex midpointOpp3 = (point1OnPlane + point2OnPlane) / 2;

            Vertex planeNormal = new Vertex(0.0f, 0.0f, 1.0f);

            Vertex diff = point1OnPlane - ray.origin;
            float s1 = diff.ScalarProduct(planeNormal);
            float s2 = ray.direction.ScalarProduct(planeNormal);

            if (s2 != 0)
            {
                float t = s1 / s2;

                if (t >= 0)
                {
                    float denomintor = planeNormal.ScalarProduct(ray.direction);

                    if (Math.Abs(denomintor) > 0.00001f)
                    {
                        //	Now we can get the point of intersection.
                        Vertex vIntersect = ray.origin + (ray.direction * t);

                        //	Do my cool test.
                        Vertex vectorTo1 = vIntersect - point1OnPlane;
                        Vertex vectorTo2 = vIntersect - point2OnPlane;
                        Vertex vectorTo3 = vIntersect - point3OnPlane;
                        Vertex vectorMidTo1 = midpointOpp1 - point1OnPlane;
                        Vertex vectorMidTo2 = midpointOpp2 - point2OnPlane;
                        Vertex vectorMidTo3 = midpointOpp3 - point3OnPlane;

                        if (vectorTo1.Magnitude() < vectorMidTo1.Magnitude() &&
                            vectorTo2.Magnitude() < vectorMidTo2.Magnitude() &&
                            vectorTo3.Magnitude() < vectorMidTo3.Magnitude() &&
                            (intersect.closeness == -1 || t < intersect.closeness))
                        {
                            //	It's fucking intersection city man
                            intersect.point = vIntersect;
                            intersect.intersected = true;
                            intersect.normal = planeNormal;
                            intersect.closeness = t;
                            return intersect;
                        }
                    }
                }
            }

            return new Intersection();
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

            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_LIGHT0);
            gl.Disable(OpenGL.GL_TEXTURE_2D);

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
            List<Vertex> vertices = this.GetVertices().ToList();

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
                    // TODO: Directional light shading
                    float light = 0.5f + (0.5f * vertices[point].Z);
                    gl.Color(light, light, light);
                }

                gl.Normal(0.0f, 0.0f, 1.0f);
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

        /// <summary>
        /// Custom wall bounding volume
        /// </summary>
        private class TileBoundingVolume : IRenderable
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TileBoundingVolume"/> class
            /// </summary>
            /// <param name="tile">Tile instance</param>
            public TileBoundingVolume(Tile tile)
            {
                this.Tile = tile;
            }

            /// <summary>
            /// Tile instance
            /// </summary>
            public Tile Tile { get; }

            /// <summary>
            /// Render object
            /// </summary>
            /// <param name="gl">OpenGL instance</param>
            /// <param name="renderMode">Rendering mode</param>
            public void Render(OpenGL gl, RenderMode renderMode)
            {
                //  Push attributes, disable lighting.
                gl.PushAttrib(OpenGL.GL_CURRENT_BIT | OpenGL.GL_ENABLE_BIT | OpenGL.GL_LINE_BIT | OpenGL.GL_POLYGON_BIT);

                gl.Disable(OpenGL.GL_LIGHTING);
                gl.Disable(OpenGL.GL_TEXTURE_2D);
                gl.LineWidth(1.0f);
                gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, renderMode == RenderMode.HitTest ? (uint)PolygonMode.Filled : (uint)PolygonMode.Lines);

                IEnumerable<Vertex> points = this.Tile.GetVertices().ToList();

                if (renderMode != RenderMode.HitTest)
                {
                    gl.Begin(OpenGL.GL_LINE_LOOP);

                    foreach (Vertex point in points)
                    {
                        // Front line
                        gl.Color(0.0f, 1.0f, 0.3f);
                        gl.Vertex(point.X, point.Y, point.Z + 0.1f);
                    }

                    gl.End();
                }
                else
                {
                    gl.Color(1f, 1.0f, 1.0f, 1.0f);

                    gl.Begin(OpenGL.GL_QUADS);

                    foreach (Vertex point in points)
                    {
                        gl.Vertex(point.X, point.Y, point.Z);
                    }

                    gl.End();
                }

                //  Pop attributes.
                gl.PopAttrib();
            }
        }
    }
}