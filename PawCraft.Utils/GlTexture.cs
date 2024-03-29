﻿namespace PawCraft.Utils
{
    using System.Drawing;
    using SharpGL;
    using SharpGL.SceneGraph.Assets;

    /// <summary>
    /// Custom texture class
    /// </summary>
    public class GlTexture : Texture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlTexture"/> class
        /// </summary>
        /// <param name="file">Image file</param>
        /// <param name="name">Texture name</param>
        /// <param name="gl">OpenGL isntance</param>
        public GlTexture(string file, string name, OpenGL gl) : base()
        {
            this.Name = name;

            using (Bitmap result = GlTexture.GetBitmap(file))
            {
                this.CreateFromBitmap(result, gl);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlTexture"/> class
        /// </summary>
        /// <param name="bitmap">Image data</param>
        /// <param name="name">Texture name</param>
        /// <param name="gl">OpenGL isntance</param>
        public GlTexture(Bitmap bitmap, string name, OpenGL gl) : base()
        {
            this.Name = name;
            this.CreateFromBitmap(bitmap, gl);
        }

        /// <summary>
        /// Gets or sets texture height
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets texture width
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets bitmap image from file
        /// </summary>
        /// <param name="file">File path</param>
        /// <returns>Bitmap image</returns>
        public static Bitmap GetBitmap(string file) => PawCraft.Utils.Helpers.GetBitmap(file);

        /// <summary>
        /// Create texture from bitmap data
        /// </summary>
        /// <param name="bitmap">Bitmap data</param>
        /// <param name="gl">OpenGL isntance</param>
        private void CreateFromBitmap(Bitmap bitmap, OpenGL gl)
        {
            this.Width = bitmap.Width;
            this.Height = bitmap.Height;
            this.Create(gl, bitmap);
            this.Bind(gl);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_REPEAT);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_NEAREST_MIPMAP_NEAREST);
            gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_NEAREST);
            gl.GenerateMipmapEXT(OpenGL.GL_TEXTURE_2D);
        }
    }
}