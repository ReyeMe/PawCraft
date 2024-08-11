namespace PawCraft
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Eventing.Reader;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using Obj2Nya;
    using PawCraft.Rendering;
    using PawCraft.Utils;
    using PawCraft.Utils.Serializer;
    using PawCraft.Utils.Types;
    using SharpGL;
    using SharpGL.SceneGraph;
    using SharpGL.SceneGraph.Core;

    /// <summary>
    /// Model handler
    /// </summary>
    public class EntityModelHandler
    {
        /// <summary>
        /// Entities textures folder
        /// </summary>
        private static readonly string entityModelFolder = Path.Combine(Path.GetDirectoryName(typeof(TextureHandler).Assembly.Location), @"Assets\Models");

        /// <summary>
        /// All available models
        /// </summary>
        private static readonly IEnumerable<string> models;

        /// <summary>
        /// All available entities
        /// </summary>
        private readonly List<EntityModel> entities;

        /// <summary>
        /// OpenGL isntance
        /// </summary>
        private readonly OpenGL gl;

        /// <summary>
        /// Initializes static members of the <see cref="EntityModelHandler"/> class
        /// </summary>
        static EntityModelHandler()
        {
            EntityModelHandler.models = Directory.EnumerateFiles(EntityModelHandler.entityModelFolder, "*.nya").OrderBy(file => file);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityModelHandler"/> class
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        public EntityModelHandler(OpenGL gl)
        {
            this.gl = gl;
            this.entities = new List<EntityModel>();

            // Entity icons
            foreach (string modelFile in EntityModelHandler.models)
            {
                this.entities.Add(new EntityModel(modelFile, Path.GetFileNameWithoutExtension(modelFile), gl));
            }
        }

        /// <summary>
        /// Renderable entity interface
        /// </summary>
        public interface IRenderableEntity
        {
            /// <summary>
            /// Render entity
            /// </summary>
            /// <param name="gl">OpenGL instnance</param>
            /// <param name="lightDir">Light direction</param>
            /// <param name="selected">Entity selected</param>
            /// <param name="renderMode">Render mode</param>
            void Render(OpenGL gl, Vertex lightDir, bool selected, RenderMode renderMode);
        }

        /// <summary>
        /// Get model name
        /// </summary>
        /// <param name="index">Model index</param>
        /// <returns>Get model name by index</returns>
        public static string GetModelName(int index)
        {
            string name = EntityModelHandler.models.ElementAtOrDefault(index);

            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            else
            {
                return Path.GetFileNameWithoutExtension(name);
            }
        }

        /// <summary>
        /// Get names of all loaded entities
        /// </summary>
        /// <returns>Model names</returns>
        public static IEnumerable<string> GetNames()
        {
            return EntityModelHandler.models.Select(file => Path.GetFileNameWithoutExtension(file));
        }

        /// <summary>
        /// Get entity model
        /// </summary>
        /// <param name="name">Entity name</param>
        /// <returns>Renderable model</returns>
        public IRenderableEntity GetModel(string name)
        {
            return this.entities.FirstOrDefault(entity => entity.Name == name);
        }

        /// <summary>
        /// Entity model
        /// </summary>
        private class EntityModel : IRenderableEntity
        {
            /// <summary>
            /// Texture coordinates
            /// </summary>
            private static readonly List<float[]> textureCoords = new List<float[]>
            {
                new float[] { 0.0f, 1.0f },
                new float[] { 1.0f, 1.0f },
                new float[] { 1.0f, 0.0f },
                new float[] { 0.0f, 0.0f },
            };

            /// <summary>
            /// Bounding cube
            /// </summary>
            private readonly EntityBoundingVolume cube;

            /// <summary>
            /// Model textures
            /// </summary>
            private readonly List<GlTexture> textures = new List<GlTexture>();

            /// <summary>
            /// Initializes a new instance of the <see cref="EntityModel"/> class
            /// </summary>
            /// <param name="file">Model file</param>
            /// <param name="name">Entity name</param>
            /// <param name="gl">OpenGL isntance</param>
            public EntityModel(string file, string name, OpenGL gl)
            {
                this.Name = name;
                int type;

                // Load model
                using (Stream stream = File.OpenRead(file))
                {
                    type = (int)CustomMarshal.MarshalAsObject(stream, typeof(int));
                    stream.Seek(0, SeekOrigin.Begin);

                    if (type == 1)
                    {
                        this.Mesh = (NyaSmoothGroup)CustomMarshal.MarshalAsObject(stream, typeof(NyaSmoothGroup));
                    }
                    else
                    {
                        this.Mesh = (NyaGroup)CustomMarshal.MarshalAsObject(stream, typeof(NyaGroup));
                    }
                }

                // Load bounding box
                this.cube = new EntityBoundingVolume();
                IEnumerable<FxVector> points = type == 0 ? ((NyaGroup)this.Mesh).Meshes.SelectMany(mesh => mesh.Points) : ((NyaSmoothGroup)this.Mesh).Meshes.SelectMany(mesh => mesh.Points);
                this.cube.FromVertices(points.Select(point => FxVector.ToVertex(point)));

                // Load textures
                IEnumerable<NyaTexture> textures = type == 0 ? ((NyaGroup)this.Mesh).Textures : ((NyaSmoothGroup)this.Mesh).Textures;

                foreach (NyaTexture tex in textures)
                {
                    using (Bitmap bitmap = new Bitmap(tex.Width, tex.Height))
                    {
                        int counter = 0;

                        for (int y = 0; y < tex.Height; y++)
                        {
                            for (int x = 0; x < tex.Width; x++)
                            {
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(tex.Data[counter].Red, tex.Data[counter].Green, tex.Data[counter].Blue));
                                counter++;
                            }
                        }

                        this.textures.Add(new GlTexture(bitmap, string.Empty, gl));
                    }
                }
            }

            /// <summary>
            /// Gets raw entity mesh
            /// </summary>
            public object Mesh { get; }

            /// <summary>
            /// Gets entity namea
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Render entity
            /// </summary>
            /// <param name="gl">OpenGL instnance</param>
            /// <param name="lightDir">Light direction</param>
            /// <param name="selected">Entity selected</param>
            /// <param name="renderMode">Render mode</param>
            public void Render(OpenGL gl, Vertex lightDir, bool selected, RenderMode renderMode)
            {
                if (this.Mesh is NyaSmoothGroup smooth)
                {
                    foreach (NyaSmoothMesh mesh in smooth.Meshes)
                    {
                        for (int face = 0; face < mesh.PolygonCount; face++)
                        {
                            float[] color = new[] { 1.0f, 1.0f, 1.0f };

                            if (renderMode != RenderMode.HitTest)
                            {
                                if (mesh.FaceFlags[face].HasTexture)
                                {
                                    gl.Enable(OpenGL.GL_TEXTURE_2D);
                                    this.textures[mesh.FaceFlags[face].TextureId].Bind(gl);
                                }
                                else
                                {
                                    color = new[]
                                    {
                                    mesh.FaceFlags[face].BaseColor.Red / (float)byte.MaxValue,
                                    mesh.FaceFlags[face].BaseColor.Green / (float)byte.MaxValue,
                                    mesh.FaceFlags[face].BaseColor.Blue / (float)byte.MaxValue
                                };
                                }

                            }
                            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                            gl.Enable(OpenGL.GL_BLEND);
                            gl.Begin(OpenGL.GL_QUADS);

                            for (int point = 0; point < 4; point++)
                            {
                                FxVector vertexFxNormal = mesh.Normals[mesh.Polygons[face].Vertices[point]];
                                Vertex vertexNormal = FxVector.ToVertex(vertexFxNormal);
                                float strength = Math.Max(Math.Min(-vertexNormal.ScalarProduct(lightDir), 1.0f), (Math.Abs(new Vertex(0.0f, 0.0f, 1.0f).ScalarProduct(lightDir)) / 3.0f));

                                color = new[]
                                {
                                    color[0] * strength,
                                    color[1] * strength,
                                    color[2] * strength
                                };

                                gl.Color(color);
                                gl.TexCoord(EntityModel.textureCoords[point]);
                                gl.Normal(FxVector.ToArray(vertexFxNormal));
                                gl.Vertex(FxVector.ToArray(mesh.Points[mesh.Polygons[face].Vertices[point]]));
                            }

                            gl.End();

                            if (mesh.FaceFlags[face].HasTexture)
                            {
                                gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
                            }
                        }
                    }
                }
                else if (this.Mesh is NyaGroup group)
                {
                    foreach (NyaMesh mesh in group.Meshes)
                    {
                        for (int face = 0; face < mesh.PolygonCount; face++)
                        {
                            float[] color = new[] { 1.0f, 1.0f, 1.0f };
                            Vertex normal = FxVector.ToVertex(mesh.Polygons[face].Normal);

                            if (renderMode != RenderMode.HitTest)
                            {
                                if (mesh.FaceFlags[face].HasTexture)
                                {
                                    gl.Enable(OpenGL.GL_TEXTURE_2D);
                                    this.textures[mesh.FaceFlags[face].TextureId].Bind(gl);
                                }
                                else
                                {
                                    color = new[]
                                    {
                                    mesh.FaceFlags[face].BaseColor.Red / (float)byte.MaxValue,
                                    mesh.FaceFlags[face].BaseColor.Green / (float)byte.MaxValue,
                                    mesh.FaceFlags[face].BaseColor.Blue / (float)byte.MaxValue
                                };
                                }

                                float strength = Math.Max(Math.Min(-normal.ScalarProduct(lightDir), 1.0f), (Math.Abs(new Vertex(0.0f, 0.0f, 1.0f).ScalarProduct(lightDir)) / 3.0f));

                                color = new[]
                                {
                                color[0] * strength,
                                color[1] * strength,
                                color[2] * strength
                            };
                            }
                            gl.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                            gl.Enable(OpenGL.GL_BLEND);
                            gl.Begin(OpenGL.GL_QUADS);

                            for (int point = 0; point < 4; point++)
                            {
                                gl.Color(color);
                                gl.TexCoord(EntityModel.textureCoords[point]);
                                gl.Normal(normal);
                                gl.Vertex(FxVector.ToArray(mesh.Points[mesh.Polygons[face].Vertices[point]]));
                            }

                            gl.End();

                            if (mesh.FaceFlags[face].HasTexture)
                            {
                                gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
                            }
                        }
                    }
                }

                if (selected && renderMode != RenderMode.HitTest)
                {
                    this.cube.Render(gl, renderMode);
                }
            }
        }
    }
}