namespace PawCraft
{
    using PawCraft.Level;
    using SharpGL;
    using SharpGL.SceneGraph;
    using SharpGL.SceneGraph.Cameras;
    using SharpGL.SceneGraph.Core;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;
    using System.Windows.Media.Media3D;
    using GL = SharpGL.OpenGL;

    /// <summary>
    /// World view window
    /// </summary>
    public partial class WorldViewWindow : ContainedForm
    {
        /// <summary>
        /// Tile map container
        /// </summary>
        private readonly SceneContainer tileContainer = new SceneContainer();

        /// <summary>
        /// Last mouse position
        /// </summary>
        private Point? lastMousePosition;

        /// <summary>
        /// Current tile
        /// </summary>
        private Point? tileLocation;

        /// <summary>
        /// Last tile
        /// </summary>
        private Point? lastTileLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldViewWindow"/> class
        /// </summary>
        public WorldViewWindow()
        {
            this.Shown += (sender, e) =>
            {
                ((LookAtCamera)this.glControl.Scene.CurrentCamera).Position = new Vertex(20.0f, 20.0f, 4.0f);
                ((LookAtCamera)this.glControl.Scene.CurrentCamera).Target = new Vertex(0.0f, 0.0f, 0.0f);

                TextureHandler.LoadTextures(this.glControl.OpenGL);
                this.glControl.Scene.SceneContainer.AddChild(new Rendering.GridLines(this.glControl.OpenGL));
                this.glControl.Scene.SceneContainer.AddChild(this.tileContainer);
            };

            this.InitializeComponent();
        }

        /// <summary>
        /// Reload tile data
        /// </summary>
        public void ReloadTileData()
        {
            this.tileContainer.Children.Clear();

            for (int x = 0; x < LevelData.MapDimensionSize; x++)
            {
                for (int y = 0; y < LevelData.MapDimensionSize; y++)
                {
                    this.tileContainer.Children.Add(new Rendering.Tile(((PawCraftMainWindow)this.MdiParent).ViewModel.LevelData, new Point(x, y)));
                }
            }
        }

        /// <summary>
        /// OpenGL initialized
        /// </summary>
        /// <param name="sender">GL control</param>
        /// <param name="e">Empty event</param>
        private void GlInitialized(object sender, EventArgs e)
        {
            this.glControl.OpenGL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            this.glControl.OpenGL.Enable(OpenGL.GL_BLEND);
            this.glControl.OpenGL.Enable(OpenGL.GL_ALPHA_TEST);
            this.glControl.OpenGL.AlphaFunc(OpenGL.GL_GREATER, 0.5f);
        }

        /// <summary>
        /// Mouse moved
        /// </summary>
        /// <param name="sender">GL control</param>
        /// <param name="e">Mouse event arguments</param>
        private void MouseMoved(object sender, MouseEventArgs e)
        {
            Point currentPosition = this.glControl.PointToClient(Control.MousePosition);
            Point lastPosition = this.lastMousePosition.HasValue ? this.lastMousePosition.Value : currentPosition;
            int deltaX = (currentPosition.X - lastPosition.X);
            int deltaY = (currentPosition.Y - lastPosition.Y);

            // Get current tile
            if (this.glControl.ClientRectangle.Contains(currentPosition) && ((PawCraftMainWindow)this.MdiParent).ActiveEditorTool != null)
            {
                List<Point> picked = this.glControl.Scene.DoHitTest(currentPosition.X, currentPosition.Y).OfType<Rendering.Tile>().Select(tile => tile.Location).Take(1).ToList();

                this.tileLocation = picked.Any() ? picked[0] : (Point?)null;
            }
            else
            {
                this.tileLocation = null;
            }

            // Apply tool
            if (this.tileLocation.HasValue &&
                (!this.lastTileLocation.HasValue || (this.lastTileLocation.Value.X != this.tileLocation.Value.X || this.lastTileLocation.Value.Y != this.tileLocation.Value.Y)))
            {
                this.ApplyTool(sender, e);
            }

            // Move camera
            if (Control.MouseButtons == MouseButtons.Middle)
            {
                LookAtCamera camera = this.glControl.Scene.CurrentCamera as LookAtCamera;

                /* apply the changes to pitch and yaw*/
                float yaw = deltaX / 10.0f;
                float pitch = deltaY / 10.0f;

                Vertex up = new Vertex(0.0f, 0.0f, 1.0f);
                Vertex current = camera.Target - camera.Position;

                current = current.RotateAroundAxis(yaw.ConvertToRadians(), up);

                Vertex side = current.VectorProduct(up);
                side.Normalize();
                Vertex newPitch = current.RotateAroundAxis(pitch.ConvertToRadians(), side);

                float angle = newPitch.GetAngleTo(up).ConvertToDegrees();

                if (angle < 160.0f && angle > 20.0f)
                {
                    camera.Target = camera.Position + newPitch;
                }
                else
                {
                    camera.Target = camera.Position + current;
                }
            }

            this.lastMousePosition = currentPosition;
        }

        /// <summary>
        /// Redraw scene
        /// </summary>
        /// <param name="sender">GL control</param>
        /// <param name="args">Draw event arguments</param>
        private void Redraw(object sender, RenderEventArgs args)
        {
            if (this.tileLocation != null)
            {
                ((PawCraftMainWindow)this.MdiParent).ActiveEditorTool?.Draw3D(
                    this.glControl.OpenGL,
                    this.tileLocation.Value,
                    ((PawCraftMainWindow)this.MdiParent).ViewModel.LevelData);
            }

            if (this.tileLocation.HasValue)
            {
                this.Text = string.Format("3D [{0}, {1}]", this.tileLocation.Value.X, this.tileLocation.Value.Y);
            }
            else
            {
                this.Text = "3D";
            }
        }

        /// <summary>
        /// Apply tool
        /// </summary>
        /// <param name="sender">GL control</param>
        /// <param name="e">Mouse event</param>
        private void ApplyTool(object sender, MouseEventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.Left && this.tileLocation.HasValue)
            {
                this.lastTileLocation = this.tileLocation;
                ((PawCraftMainWindow)this.MdiParent).ActiveEditorTool?.Apply(
                    this.tileLocation.Value,
                    ((PawCraftMainWindow)this.MdiParent).ViewModel.LevelData);
            }
        }
    }
}