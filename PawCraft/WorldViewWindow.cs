namespace PawCraft
{
    using PawCraft.Level;
    using PawCraft.Rendering;
    using SharpGL;
    using SharpGL.SceneGraph;
    using SharpGL.SceneGraph.Cameras;
    using SharpGL.SceneGraph.Core;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using SWI = System.Windows.Input;

    /// <summary>
    /// World view window
    /// </summary>
    public partial class WorldViewWindow : ContainedForm
    {
        /// <summary>
        /// Movement keys
        /// </summary>
        private readonly Dictionary<Keys, bool> movementKeys = new Dictionary<Keys, bool>
        {
            { Keys.W, false },
            { Keys.A, false },
            { Keys.S, false },
            { Keys.D, false },
            { Keys.Q, false },
            { Keys.E, false },
            { Keys.ShiftKey, false },
        };

        /// <summary>
        /// Tile map container
        /// </summary>
        private readonly SceneContainer tileContainer = new SceneContainer();

        /// <summary>
        /// Last mouse position
        /// </summary>
        private Point? lastMousePosition;

        /// <summary>
        /// Last tile
        /// </summary>
        private Point? lastTileLocation;

        /// <summary>
        /// View is ready
        /// </summary>
        private bool ready = false;

        /// <summary>
        /// Current tile
        /// </summary>
        private Point? tileLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldViewWindow"/> class
        /// </summary>
        public WorldViewWindow()
        {
            this.Shown += (sender, e) =>
            {
                ((LookAtCamera)this.glControl.Scene.CurrentCamera).Position = new Vertex(20.0f, 20.0f, 4.0f);
                ((LookAtCamera)this.glControl.Scene.CurrentCamera).Target = new Vertex(0.0f, 0.0f, 0.0f);

                this.EntityContainer = new EntitiesContainer(this);
                this.TextureAtlas = new TextureHandler(this.glControl.OpenGL);
                this.glControl.Scene.SceneContainer.AddChild(new Rendering.GridLines(this.glControl.OpenGL));
                this.glControl.Scene.SceneContainer.AddChild(this.tileContainer);
                this.glControl.Scene.SceneContainer.AddChild(this.EntityContainer);
                this.ready = true;
                this.ReloadTileData();

            };

            this.FormClosing += (sender, e) =>
            {
                this.TextureAtlas.Dispose();
            };

            this.InitializeComponent();
        }

        /// <summary>
        /// Shading mode of the level scene
        /// </summary>
        public enum ShadingMode
        {
            /// <summary>
            /// Heightmap only
            /// </summary>
            Heightmap,

            /// <summary>
            /// Full bright textured
            /// </summary>
            Textured,

            /// <summary>
            /// Textured with shading
            /// </summary>
            TexturedShaded
        }

        /// <summary>
        /// Gets or sets active shading mode
        /// </summary>
        public ShadingMode CurrentShadingMode { get; set; } = ShadingMode.TexturedShaded;

        /// <summary>
        /// Gets entity container
        /// </summary>
        public EntitiesContainer EntityContainer { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show tile normals
        /// </summary>
        public bool ShowTileNormals { get; set; }

        /// <summary>
        /// Gets loaded textures
        /// </summary>
        public TextureHandler TextureAtlas { get; private set; }

        /// <summary>
        /// Handle keyboard input
        /// </summary>
        /// <param name="key">Changed key</param>
        /// <param name="state">Key state</param>
        /// <returns><see langword="true"/> if handled</returns>
        public bool HandleInput(Keys key, SWI.KeyStates state)
        {
            if (this.movementKeys.ContainsKey(key))
            {
                this.movementKeys[key] = state == SWI.KeyStates.Down;
                return true;
            }
            else if (key == Keys.L && this.ContainsFocus)
            {
                GridLines grid = this.glControl.Scene.SceneContainer.Children.OfType<GridLines>().FirstOrDefault();

                if (grid != null)
                {
                    grid.Visible = !grid.Visible;
                }
            }
            else if (key == Keys.F && this.ContainsFocus)
            {
                this.glControl.DrawFPS = !this.glControl.DrawFPS;
            }
            else if (key == Keys.N && this.ContainsFocus)
            {
                this.ShowTileNormals = !this.ShowTileNormals;
            }
            else if (key == Keys.Delete && this.ContainsFocus)
            {
                Entity selected = this.EntityContainer.Children.OfType<Entity>().FirstOrDefault(entity => entity.Selected);

                if (selected != null)
                {
                    selected.Level.Entities = selected.Level.Entities.Where(entity => entity != selected.Data).ToArray();
                    this.EntityContainer.Refresh();
                }
            }

            return false;
        }

        /// <summary>
        /// Reload tile data
        /// </summary>
        public void ReloadTileData()
        {
            if (this.ready)
            {
                this.EntityContainer.Refresh();
                this.tileContainer.Children.Clear();

                for (int x = 0; x < LevelData.MapDimensionSize; x++)
                {
                    for (int y = 0; y < LevelData.MapDimensionSize; y++)
                    {
                        this.tileContainer.Children.Add(
                            new Rendering.Tile(new Point(x, y), this));
                    }
                }
            }
        }

        /// <summary>
        /// Mouse clicked
        /// </summary>
        /// <param name="sender">GL control</param>
        /// <param name="e">Mouse event</param>
        private void MouseClicked(object sender, MouseEventArgs e)
        {
            if (Control.MouseButtons == MouseButtons.Left)
            {
                if (((PawCraftMainWindow)this.MdiParent).ActiveEditorTool != null)
                {
                    this.ApplyTool(sender, e);
                }
                else
                {
                    this.SelectEntity();
                }
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

                if (((PawCraftMainWindow)this.MdiParent).ActiveEditorTool != null)
                {
                    ((PawCraftMainWindow)this.MdiParent).ActiveEditorTool.Apply(
                        this.tileLocation.Value,
                        ((PawCraftMainWindow)this.MdiParent).ViewModel.LevelData);
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
        /// Handle keyboard movement
        /// </summary>
        private void HandleKeyboardMovement()
        {
            LookAtCamera camera = this.glControl.Scene.CurrentCamera as LookAtCamera;
            Vertex up = new Vertex(0.0f, 0.0f, 1.0f);
            Vertex current = camera.Target - camera.Position;
            Vertex side = current.VectorProduct(up);
            Vertex sideUp = current.VectorProduct(side);
            current.Normalize();
            side.Normalize();
            sideUp.Normalize();
            float speedMultiplier = this.movementKeys[Keys.ShiftKey] ? 2.0f : 1.0f;

            if (this.movementKeys[Keys.W])
            {
                current *= 0.3f * speedMultiplier;
            }
            else if (this.movementKeys[Keys.S])
            {
                current *= -0.3f * speedMultiplier;
            }
            else
            {
                current *= 0.0f;
            }

            if (this.movementKeys[Keys.A])
            {
                side *= -0.3f * speedMultiplier;
            }
            else if (this.movementKeys[Keys.D])
            {
                side *= 0.3f * speedMultiplier;
            }
            else
            {
                side *= 0.0f;
            }

            if (this.movementKeys[Keys.Q])
            {
                sideUp *= -0.3f * speedMultiplier;
            }
            else if (this.movementKeys[Keys.E])
            {
                sideUp *= 0.3f * speedMultiplier;
            }
            else
            {
                sideUp *= 0.0f;
            }

            camera.Target += current + side + sideUp;
            camera.Position += current + side + sideUp;
        }

        /// <summary>
        /// Select world entity
        /// </summary>
        private void SelectEntity()
        {
            Point currentPosition = this.glControl.PointToClient(Control.MousePosition);

            if (this.glControl.ClientRectangle.Contains(currentPosition) && ((PawCraftMainWindow)this.MdiParent).ActiveEditorTool == null)
            {
                List<SceneElement> element = this.glControl.Scene.DoHitTest(currentPosition.X, currentPosition.Y).ToList();
                Entity found = element.OfType<Entity>().FirstOrDefault();

                foreach (Entity entity in this.EntityContainer.Children)
                {
                    entity.Selected = found == entity;
                }
            }
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
                List<SceneElement> element = this.glControl.Scene.DoHitTest(currentPosition.X, currentPosition.Y).ToList();
                List<Point> picked = element.OfType<Rendering.Tile>().Select(tile => tile.Location).Take(1).ToList();
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
            if (this.ContainsFocus)
            {
                this.HandleKeyboardMovement();
            }

            ((PawCraftMainWindow)this.MdiParent).ActiveEditorTool?.Draw3DContinuous(
                this.glControl.OpenGL,
                ((PawCraftMainWindow)this.MdiParent).ViewModel.LevelData);

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
    }
}