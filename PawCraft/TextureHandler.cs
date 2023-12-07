namespace PawCraft
{
    using PawCraft.Properties;
    using SharpGL;
    using SharpGL.SceneGraph.Assets;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Texture handler
    /// </summary>
    public class TextureHandler : IDisposable
    {
        /// <summary>
        /// Textures folder
        /// </summary>
        private static readonly string textureFolder = Path.Combine(Path.GetDirectoryName(typeof(TextureHandler).Assembly.Location), @"Assets\Textures");

        /// <summary>
        /// Entities textures folder
        /// </summary>
        private static readonly string entityTextureFolder = Path.Combine(Path.GetDirectoryName(typeof(TextureHandler).Assembly.Location), @"Assets\Icons");

        /// <summary>
        /// Loaded textures
        /// </summary>
        private readonly List<Texture> textures = new List<Texture>();

        /// <summary>
        /// Loaded entity textures
        /// </summary>
        private readonly List<Texture> entityTextures = new List<Texture>();

        /// <summary>
        /// Initializes a new isntance of the <see cref="TextureHandler"/> class
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        public TextureHandler(OpenGL gl)
        {
            this.Gl = gl;

            try
            {
                foreach (Texture texture in this.textures)
                {
                    texture.Destroy(gl);
                }

                this.textures.Clear();

                if (!Directory.Exists(TextureHandler.textureFolder))
                {
                    Directory.CreateDirectory(TextureHandler.textureFolder);
                }

                int counter = 0;

                foreach (string textureFile in Directory.EnumerateFiles(TextureHandler.textureFolder, "*.bmp").OrderBy(file => file))
                {
                    using (Bitmap bmp = new Bitmap(textureFile))
                    {
                        Texture texture = TextureHandler.LoadFromBitmap(bmp, gl, Path.GetFileNameWithoutExtension(textureFile));
                        this.textures.Add(texture);
                    }

                    counter++;

                    if (counter > byte.MaxValue)
                    {
                        return;
                    }
                }

                Texture dummyTexture = TextureHandler.LoadFromBitmap(Resources.EntityIco, gl, "dummy");
                this.entityTextures.Add(dummyTexture);

                foreach (string textureFile in Directory.EnumerateFiles(TextureHandler.entityTextureFolder, "*.png").OrderBy(file => file))
                {
                    using (Bitmap bmp = new Bitmap(textureFile))
                    {
                        Texture texture = TextureHandler.LoadFromBitmap(bmp, gl, Path.GetFileNameWithoutExtension(textureFile));
                        this.entityTextures.Add(texture);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        /// <summary>
        /// Load texture
        /// </summary>
        /// <param name="bitmap">Texture bitmap</param>
        /// <param name="gl">OpenGL instance</param>
        /// <param name="name">Texture name</param>
        /// <returns>Loaded texture</returns>
        private static Texture LoadFromBitmap(Bitmap bitmap, OpenGL gl, string name)
        {
            CustomTexture texture = new CustomTexture { Name = name, Width = bitmap.Width, Height = bitmap.Height };
            texture.Create(gl, bitmap);
            texture.Bind(gl);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST_MIPMAP_NEAREST);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);
            gl.GenerateMipmapEXT(OpenGL.GL_TEXTURE_2D);
            return texture;
        }

        /// <summary>
        /// Gets current OpenGL instance
        /// </summary>
        public OpenGL Gl { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this object was disposed of
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Dispose of the resources used by this instance
        /// </summary>
        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                foreach (Texture texture in this.textures)
                {
                    texture.Destroy(this.Gl);
                }

                this.textures.Clear();
            }
        }

        /// <summary>
        /// Get loaded texture
        /// </summary>
        /// <param name="index">Texture index</param>
        /// <returns>Loaded texture</returns>
        public Texture GetTexture(int index)
        {
            if (index >= 0 && index < this.textures.Count)
            {
                return this.textures[index];
            }

            return null;
        }

        /// <summary>
        /// Get loaded texture
        /// </summary>
        /// <param name="name">Texture name</param>
        /// <returns>Loaded texture</returns>
        public Texture GetEntityTexture(string name)
        {
            Texture found = this.entityTextures.FirstOrDefault(texture => texture.Name == name);

            if (found == null)
            {
                found = this.entityTextures.First();
            }

            return found;
        }

        /// <summary>
        /// Get number of available textures
        /// </summary>
        /// <returns>Number of textures</returns>
        public int GetTextureCount()
        {
            return this.textures.Count;
        }

        /// <summary>
        /// Custom texture class
        /// </summary>
        public class CustomTexture : Texture
        {
            /// <summary>
            /// Gets or sets texture width
            /// </summary>
            public int Width { get; set; }

            /// <summary>
            /// Gets or sets texture height
            /// </summary>
            public int Height { get; set; }
        }
    }
}