namespace PawCraft.Rendering
{
    using System;
    using System.Windows.Forms;
    using SharpGL;
    using SharpGL.SceneGraph;
    using SharpGL.SceneGraph.Cameras;

    /// <summary>
    /// The SceneControl is an OpenGLControl that contains and draws a Scene object.
    /// </summary>
    [System.Drawing.ToolboxBitmap(typeof(SceneControl), "SharpGL.png")]
    public class LevelSceneControl : SceneControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=""/>
        /// </summary>
        public LevelSceneControl()
        {
            this.InitializeComponent();

            this.OpenGLInitialized += (sender, e) =>
            {
                this.Scene = new LevelScene();
                SharpGL.SceneGraph.Helpers.SceneHelper.InitialiseModelingScene(this.Scene);
                this.Scene.CreateInContext(this.OpenGL);

                this.Scene.RenderBoundingVolumes = false;

                ((LookAtCamera)this.Scene.CurrentCamera).Far = 400.0f;

                this.MouseWheel += (control, mousewheel) =>
                {
                    LookAtCamera current = this.Scene.CurrentCamera as LookAtCamera;

                    if (current != null)
                    {
                        float delta = mousewheel.Delta * (1.0f / 120.0f);
                        Vertex look = current.Target - current.Position;
                        look.Normalize();

                        current.Position = current.Position + (look * delta);
                        current.Target = current.Position + look;
                    }
                };

                this.Scene.SceneContainer.Children.Clear();

                this.OpenGL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
                this.OpenGL.Enable(OpenGL.GL_BLEND);
                this.OpenGL.Enable(OpenGL.GL_ALPHA_TEST);
                this.OpenGL.AlphaFunc(OpenGL.GL_GREATER, 0.5f);
            };
        }

        /// <summary>
        /// Draw scene
        /// </summary>
        /// <param name="e">Paint arguments</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            this.stopwatch.Restart();
            this.OpenGL.MakeCurrent();

            this.Scene.Draw();
            this.DoOpenGLDraw(new RenderEventArgs(e.Graphics));

            if (this.DrawFPS)
            {
                this.OpenGL.DrawText(5, 5, 1f, 0f, 0f, "Courier New", 12f, $"Draw Time: {frameTime:0.0000} ms ~ {1000.0 / frameTime:0.0} FPS");
                this.OpenGL.Flush();
            }

            IntPtr hdc = e.Graphics.GetHdc();
            this.OpenGL.Blit(hdc);
            e.Graphics.ReleaseHdc(hdc);

            this.DoGDIDraw(new RenderEventArgs(e.Graphics));

            this.stopwatch.Stop();
            this.frameTime = this.stopwatch.Elapsed.TotalMilliseconds;
        }

        /// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
        {
            //
            // OpenGLCtrl
            //
            this.Name = "OpenGLCtrl";
        }
    }
}