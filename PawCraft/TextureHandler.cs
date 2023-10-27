namespace PawCraft
{
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
        /// Loaded textures
        /// </summary>
        private readonly List<Texture> textures = new List<Texture>();

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
                        Texture texture = new Texture { Name = Path.GetFileName(textureFile) };
                        texture.Create(gl, bmp);
                        texture.Bind(gl);
                        gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
                        gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);
                        gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST_MIPMAP_NEAREST);
                        gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);
                        gl.GenerateMipmapEXT(OpenGL.GL_TEXTURE_2D);
                        this.textures.Add(texture);
                    }

                    counter++;

                    if (counter > byte.MaxValue)
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
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
        /// Get number of available textures
        /// </summary>
        /// <returns>Number of textures</returns>
        public int GetTextureCount()
        {
            return this.textures.Count;
        }
    }
}