namespace PawCraft
{
    using Obj2Nya;
    using PawCraft.Utils;
    using PawCraft.Utils.Serializer;
    using PawCraft.Utils.Types;
    using SharpGL;
    using SharpGL.SceneGraph;
    using SharpGL.SceneGraph.Core;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Media;

    /// <summary>
    /// Model handler
    /// </summary>
    public class EntityModelHandler
    {
        /// <summary>
        /// OpenGL isntance
        /// </summary>
        private readonly OpenGL gl;

        /// <summary>
        /// All available entities
        /// </summary>
        private readonly List<EntityModel> entities;

        /// <summary>
        /// Entities textures folder
        /// </summary>
        private static readonly string entityModelFolder = Path.Combine(Path.GetDirectoryName(typeof(TextureHandler).Assembly.Location), @"Assets\Models");

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityModelHandler"/> class
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        public EntityModelHandler(OpenGL gl)
        {
            this.gl = gl;
            this.entities = new List<EntityModel>();

            // Entity icons
            foreach (string modelFile in Directory.EnumerateFiles(EntityModelHandler.entityModelFolder, "*.nya").OrderBy(file => file))
            {
                this.entities.Add(new EntityModel(modelFile, Path.GetFileNameWithoutExtension(modelFile), gl));
            }
        }

        /// <summary>
        /// Get entity model
        /// </summary>
        /// <param name="name">Entity name</param>
        /// <returns>Renderable model</returns>
        public IRenderable GetModel(string name)
        {
            return this.entities.FirstOrDefault(entity => entity.Name == name);
        }

        /// <summary>
        /// Entity model
        /// </summary>
        private class EntityModel : IRenderable
        {
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

                using (Stream stream = File.OpenRead(file))
                {
                    this.Mesh = (NyaGroup)CustomMarshal.MarshalAsObject(stream, typeof(NyaGroup));
                }

                foreach (NyaTexture tex in this.Mesh.Textures)
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
            /// Gets entity namea
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Gets raw entity mesh
            /// </summary>
            public NyaGroup Mesh { get; }

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
            /// Render entity
            /// </summary>
            /// <param name="gl">OpenGL instnance</param>
            /// <param name="renderMode">Render mode</param>
            public void Render(OpenGL gl, RenderMode renderMode)
            {
                foreach (NyaMesh mesh in this.Mesh.Meshes)
                {
                    for (int face = 0; face < mesh.PolygonCount; face++)
                    {
                        if (mesh.FaceFlags[face].HasTexture)
                        {
                            gl.Enable(OpenGL.GL_TEXTURE_2D);
                            this.textures[mesh.FaceFlags[face].TextureId].Bind(gl);
                            gl.Color(1.0f, 1.0f, 1.0f);
                        }
                        else
                        {
                            gl.Color(mesh.FaceFlags[face].BaseColor.Red, mesh.FaceFlags[face].BaseColor.Green, mesh.FaceFlags[face].BaseColor.Blue);
                        }

                        gl.Begin(OpenGL.GL_QUADS);

                        for (int point  = 0; point < 4; point++)
                        {
                            gl.TexCoord(EntityModel.textureCoords[point]);
                            gl.Normal(FxVector.ToFloatArray(mesh.Polygons[face].Normal));
                            gl.Vertex(FxVector.ToFloatArray(mesh.Points[mesh.Polygons[face].Vertices[point]]));
                        }

                        gl.End();

                        if (mesh.FaceFlags[face].HasTexture)
                        {
                            gl.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
                        }
                    }
                }
            }
        }
    }
}
