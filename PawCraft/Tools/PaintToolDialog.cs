namespace PawCraft.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Forms;
    using PawCraft.Rendering;
    using PawCraft.ToolsApi;
    using SharpGL;
    using SharpGL.Enumerations;
    using SharpGL.SceneGraph;
    using SharpGL.SceneGraph.Assets;
    using SharpGL.SceneGraph.Cameras;
    using SharpGL.SceneGraph.Core;
    using SharpGL.SceneGraph.Effects;

    /// <summary>
    /// Paint tool dialog
    /// </summary>
    public partial class PaintToolDialog : ToolDialogBase<PaintTool>
    {
        /// <summary>
        /// Paint tool instance
        /// </summary>
        private readonly PaintTool toolInstance;

        /// <summary>
        /// Last picked texture tile
        /// </summary>
        private TextureTile lastPickedTile;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintToolDialog"/> class
        /// </summary>
        /// <param name="tool">Paint tool</param>
        public PaintToolDialog(PaintTool tool) : base(tool)
        {
            this.toolInstance = tool;
            this.InitializeComponent();
        }

        /// <summary>
        /// Dispose of the texture atlas
        /// </summary>
        public override void OnClose()
        {
            ((TextureViewScene)this.textureView.Scene).TextureAtlas.Dispose();
        }

        /// <summary>
        /// Tool has opened
        /// </summary>
        public override void OnShown()
        {
            this.textureView.Scene = new TextureViewScene(this.textureView.OpenGL, this.textureView.Size, this.textureView.Padding);
            int select = this.toolInstance?.TextureIndex ?? 0;
            this.radiusValue.Value = this.ActiveTool.Radius;

            for (int texture = 0; texture < ((TextureViewScene)this.textureView.Scene).TextureAtlas.GetTextureCount(); texture++)
            {
                Texture glTexture = ((TextureViewScene)this.textureView.Scene).TextureAtlas.GetTexture(texture);
                this.currentTexture.Items.Add(glTexture.Name);
            }

            this.currentTexture.SelectedIndex = select;
            TextureTile lastTile = this.textureView.Scene.SceneContainer.Children
                .OfType<TextureTile>()
                .OrderByDescending(tile => tile.Location.Y)
                .FirstOrDefault();

            if (lastTile != null)
            {
                int tileSelectorHeight = lastTile.Location.Y + lastTile.Size + this.textureView.Padding.Top + (this.textureView.Padding.Bottom * 2);
                this.textureScroll.Maximum = Math.Max(tileSelectorHeight - this.textureView.Size.Height, 0);
            }
        }

        /// <summary>
        /// Pick texture tile
        /// </summary>
        /// <param name="sender">GL control</param>
        /// <param name="e">Empty event</param>
        private void MouseViewClick(object sender, EventArgs e)
        {
            Point mouse = this.textureView.PointToClient(Control.MousePosition);

            if (this.textureView.ClientRectangle.Contains(mouse))
            {
                int index = (this.textureView.Scene.DoHitTest(mouse.X, mouse.Y).FirstOrDefault() as TextureTile)?.Index ?? 0;
                this.currentTexture.SelectedIndex = index;
            }
        }

        /// <summary>
        /// Selected texture changed
        /// </summary>
        /// <param name="sender">Combo box</param>
        /// <param name="e">Empty event</param>
        private void TextureSelectionChanged(object sender, EventArgs e)
        {
            if (this.lastPickedTile != null)
            {
                this.lastPickedTile.IsSelected = false;
            }

            this.lastPickedTile = this.textureView.Scene.SceneContainer.Children.OfType<TextureTile>().ElementAtOrDefault(this.currentTexture.SelectedIndex);

            if (this.lastPickedTile == null)
            {
                this.lastPickedTile = this.textureView.Scene.SceneContainer.Children.OfType<TextureTile>().FirstOrDefault();
            }

            if (this.lastPickedTile != null)
            {
                this.lastPickedTile.IsSelected = true;
                this.toolInstance.TextureIndex = this.lastPickedTile.Index;
            }
        }

        /// <summary>
        /// Value changed
        /// </summary>
        /// <param name="sender">Track bar</param>
        /// <param name="e"></param>
        private void ValueChanged(object sender, EventArgs e)
        {
            this.ActiveTool.Radius = this.radiusValue.Value;
            this.valueLabel.Text = this.radiusValue.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// View scrolling
        /// </summary>
        /// <param name="sender">Scroll bar</param>
        /// <param name="e">Scroll event</param>
        private void ViewScroll(object sender, ScrollEventArgs e)
        {
            foreach (TextureTile item in this.textureView.Scene.SceneContainer.Children.OfType<TextureTile>())
            {
                item.VerticalOffset = e.NewValue;
            }
        }

        /// <summary>
        /// Texture tile
        /// </summary>
        private class TextureTile : SceneElement, IRenderable
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
            /// Initializes a new instance of the <see cref="TextureTile"/> class
            /// </summary>
            /// <param name="index">Texture index</param>
            /// <param name="location">Tile location</param>
            /// <param name="size">Texture tile size</param>
            /// <param name="textures">Texture atlas</param>
            public TextureTile(int index, Point location, int size, TextureHandler textures)
            {
                this.Index = index;
                this.Size = size;
                this.Location = location;
                this.Textures = textures;
            }

            /// <summary>
            /// Gets texture index
            /// </summary>
            public int Index { get; }

            /// <summary>
            /// Gets or sets a value indicating whether texture is selected
            /// </summary>
            public bool IsSelected { get; set; }

            /// <summary>
            /// Gets tile location
            /// </summary>
            public Point Location { get; }

            /// <summary>
            /// Gets texture tile size
            /// </summary>
            public int Size { get; }

            /// <summary>
            /// Gets texture atlas
            /// </summary>
            public TextureHandler Textures { get; }

            /// <summary>
            /// Gets or sets vertical offset
            /// </summary>
            public int VerticalOffset { get; set; }

            /// <summary>
            /// Render texture tile
            /// </summary>
            /// <param name="gl">OpenGl instance</param>
            /// <param name="renderMode">Render mode</param>
            public void Render(OpenGL gl, RenderMode renderMode)
            {
                if (renderMode == RenderMode.HitTest)
                {
                    gl.Begin(OpenGL.GL_QUADS);

                    foreach (int[] texCoord in TextureTile.uv)
                    {
                        gl.Color(1.0f, 1.0f, 1.0f);
                        gl.Vertex(this.Location.X + (texCoord[0] * this.Size), this.Location.Y + (texCoord[1] * this.Size) - this.VerticalOffset, 0);
                    }

                    gl.End();
                }
                else
                {
                    gl.Disable(OpenGL.GL_DEPTH);
                    gl.Disable(OpenGL.GL_LIGHTING);
                    gl.Enable(OpenGL.GL_TEXTURE);
                    this.Textures.GetTexture(this.Index)?.Push(gl);

                    gl.Begin(OpenGL.GL_QUADS);

                    foreach (int[] texCoord in TextureTile.uv)
                    {
                        gl.Color(1.0f, 1.0f, 1.0f);
                        gl.TexCoord(texCoord[0], texCoord[1]);
                        gl.Vertex(this.Location.X + (texCoord[0] * this.Size), this.Location.Y + (texCoord[1] * this.Size) - this.VerticalOffset, 0);
                    }

                    gl.End();

                    this.Textures.GetTexture(this.Index)?.Pop(gl);

                    if (this.IsSelected)
                    {
                        int outlineSize = this.Size + 4;

                        gl.Begin(OpenGL.GL_LINE_LOOP);

                        foreach (int[] texCoord in TextureTile.uv)
                        {
                            gl.Color(0.0f, 1.0f, 0.0f);
                            gl.Vertex(this.Location.X + (texCoord[0] * outlineSize) - 2, this.Location.Y + (texCoord[1] * outlineSize) - this.VerticalOffset - 2, 0);
                        }

                        gl.End();
                    }
                }
            }
        }

        /// <summary>
        /// Texture view scene
        /// </summary>
        private class TextureViewScene : LevelScene
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TextureViewScene"/> class
            /// </summary>
            /// <param name="gl">OpenGL isntance</param>
            /// <param name="windowSize">Window view size</param>
            /// <param name="padding">Item padding</param>
            public TextureViewScene(OpenGL gl, Size windowSize, Padding padding)
            {
                this.TextureAtlas = new TextureHandler(gl);
                this.SceneContainer.Children.Clear();
                this.CurrentCamera = new OrthographicCamera
                {
                    Position = new Vertex(),
                    Top = 0.0,
                    Left = 0.0,
                    Right = windowSize.Width,
                    Bottom = windowSize.Height
                };

                OpenGLAttributesEffect openGLAttributesEffect = new OpenGLAttributesEffect { Name = "Scene Attributes" };
                openGLAttributesEffect.EnableAttributes.EnableDepthTest = false;
                openGLAttributesEffect.EnableAttributes.EnableNormalize = false;
                openGLAttributesEffect.EnableAttributes.EnableLighting = false;
                openGLAttributesEffect.EnableAttributes.EnableTexture2D = true;
                openGLAttributesEffect.EnableAttributes.EnableBlend = false;
                openGLAttributesEffect.ColorBufferAttributes.BlendingSourceFactor = BlendingSourceFactor.SourceAlpha;
                openGLAttributesEffect.ColorBufferAttributes.BlendingDestinationFactor = BlendingDestinationFactor.OneMinusSourceAlpha;
                openGLAttributesEffect.LightingAttributes.TwoSided = true;
                this.SceneContainer.AddEffect(openGLAttributesEffect);

                int step = windowSize.Width / 4;
                int textureSize = ((windowSize.Width / 4) - (padding.Left + padding.Right));

                int x = 0;
                int line = padding.Top;

                for (int texture = 0; texture < this.TextureAtlas.GetTextureCount(); texture++)
                {
                    if (texture != 0 && texture % 4 == 0)
                    {
                        x =0;
                        line += padding.Top + textureSize + padding.Bottom;
                    }

                    this.SceneContainer.AddChild(
                        new TextureTile(
                            texture,
                            new Point(x + padding.Left, line),
                            textureSize,
                            this.TextureAtlas));

                    x += step;
                }

                this.CreateInContext(gl);
            }

            /// <summary>
            /// Gets texture atlas
            /// </summary>
            public TextureHandler TextureAtlas { get; }
        }
    }
}