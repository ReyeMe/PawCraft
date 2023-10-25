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
    internal static class TextureHandler
    {
        /// <summary>
        /// Textures folder
        /// </summary>
        private static readonly string textureFolder = Path.Combine(Path.GetDirectoryName(typeof(TextureHandler).Assembly.Location), @"Assets\Textures");

        /// <summary>
        /// Loaded textures
        /// </summary>
        private static List<Texture> textures = new List<Texture>();

        /// <summary>
        /// Get loaded texture
        /// </summary>
        /// <param name="index">Texture index</param>
        /// <returns>Loaded texture</returns>
        public static Texture GetTexture(int index)
        {
            if (index >= 0 && index < TextureHandler.textures.Count)
            {
                return TextureHandler.textures[index];
            }

            return null;
        }

        /// <summary>
        /// Load texture files
        /// </summary>
        /// <param name="gl">OpenGL isntance</param>
        internal static void LoadTextures(OpenGL gl)
        {
            try
            {
                foreach (Texture texture in TextureHandler.textures)
                {
                    texture.Destroy(gl);
                }

                TextureHandler.textures.Clear();

                if (!Directory.Exists(TextureHandler.textureFolder))
                {
                    Directory.CreateDirectory(TextureHandler.textureFolder);
                }

                foreach (string textureFile in Directory.EnumerateFiles(TextureHandler.textureFolder, "*.bmp").OrderBy(file => file))
                {
                    using (Bitmap  bmp = new Bitmap(textureFile))
                    {
                        Texture texture = new Texture();
                        texture.Create(gl, bmp);
                        texture.Bind(gl);
                        gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
                        gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);
                        gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST_MIPMAP_NEAREST);
                        gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);
                        gl.GenerateMipmapEXT(OpenGL.GL_TEXTURE_2D);
                        TextureHandler.textures.Add(texture);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}