namespace PawCraft
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using PawCraft.Properties;
    using PawCraft.Utils;
    using SharpGL;
    using SharpGL.SceneGraph.Assets;

    /// <summary>
    /// Texture handler
    /// </summary>
    public class TextureHandler : IDisposable
    {
        /// <summary>
        /// Entities textures folder
        /// </summary>
        private static readonly string entityTextureFolder = Path.Combine(Path.GetDirectoryName(typeof(TextureHandler).Assembly.Location), @"Assets\Icons");

        /// <summary>
        /// Textures folder
        /// </summary>
        private static readonly string textureFolder = Path.Combine(Path.GetDirectoryName(typeof(TextureHandler).Assembly.Location), @"Assets\Textures");

        /// <summary>
        /// Loaded entity textures
        /// </summary>
        private readonly List<GlTexture> entityTextures = new List<GlTexture>();

        /// <summary>
        /// Loaded textures
        /// </summary>
        private readonly List<GlTexture> textures = new List<GlTexture>();

        /// <summary>
        /// Initializes a new isntance of the <see cref="TextureHandler"/> class
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        public TextureHandler(OpenGL gl)
        {
            this.Gl = gl;

            try
            {
                foreach (GlTexture texture in this.textures)
                {
                    texture.Destroy(gl);
                }

                this.textures.Clear();

                if (!Directory.Exists(TextureHandler.textureFolder))
                {
                    Directory.CreateDirectory(TextureHandler.textureFolder);
                }

                int counter = 0;

                foreach (string textureFile in Directory.EnumerateFiles(TextureHandler.textureFolder, "*.tga").OrderBy(file => file))
                {
                    this.textures.Add(new GlTexture(textureFile, Path.GetFileNameWithoutExtension(textureFile), gl));

                    counter++;

                    if (counter > byte.MaxValue)
                    {
                        return;
                    }
                }

                // Dummy entity icon
                this.entityTextures.Add(new GlTexture(Resources.EntityIco, "dummy", gl));

                // Entity icons
                foreach (string textureFile in Directory.EnumerateFiles(TextureHandler.entityTextureFolder, "*.png").OrderBy(file => file))
                {
                    this.entityTextures.Add(new GlTexture(textureFile, Path.GetFileNameWithoutExtension(textureFile), gl));
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
        /// <param name="name">Texture name</param>
        /// <returns>Loaded texture</returns>
        public GlTexture GetEntityTexture(string name)
        {
            GlTexture found = this.entityTextures.FirstOrDefault(texture => texture.Name == name);

            if (found == null)
            {
                found = this.entityTextures.First();
            }

            return found;
        }

        /// <summary>
        /// Get loaded texture
        /// </summary>
        /// <param name="index">Texture index</param>
        /// <returns>Loaded texture</returns>
        public GlTexture GetTexture(int index)
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