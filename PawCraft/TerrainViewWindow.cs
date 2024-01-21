namespace PawCraft
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Linq;
    using System.Windows.Forms;
    using PawCraft.Level;

    /// <summary>
    /// Terrain view
    /// </summary>
    public partial class TerrainViewWindow : ContainedForm
    {
        /// <summary>
        /// Scale of the preview
        /// </summary>
        private const int PreviewScale = 10;

        /// <summary>
        /// Window title
        /// </summary>
        private const string Title = "Terrain";

        /// <summary>
        /// Drawing pens
        /// </summary>
        private static readonly Brush[] colors = Enumerable.Range(0, 32)
            .Select(color =>
            {
                return (byte)(((float)byte.MaxValue / 31.0f) * color);
            })
            .Select(color => new SolidBrush(Color.FromArgb(byte.MaxValue, color, color, color)))
            .ToArray();

        /// <summary>
        /// Last edited tile
        /// </summary>
        private Point? lastEditedTile;

        /// <summary>
        /// Initializes a new instance of the <see cref="TerrainViewWindow"/> class
        /// </summary>
        public TerrainViewWindow()
        {
            this.InitializeComponent();
            this.ClientSize = new System.Drawing.Size(200, 200);
        }

        /// <summary>
        /// Render heightmap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            LevelData level = ((PawCraftMainWindow)this.MdiParent).ViewModel.LevelData;
            Graphics grp = e.Graphics;
            grp.PageUnit = GraphicsUnit.Pixel;

            // Begin container
            GraphicsContainer containerState = grp.BeginContainer();

            // Set scaling
            for (int x = 0; x < LevelData.MapDimensionSize; x++)
            {
                for (int y = 0; y < LevelData.MapDimensionSize; y++)
                {
                    grp.FillRectangle(
                        TerrainViewWindow.colors[level[x, y].Depth],
                        x * TerrainViewWindow.PreviewScale,
                        y * TerrainViewWindow.PreviewScale,
                        TerrainViewWindow.PreviewScale,
                        TerrainViewWindow.PreviewScale);
                }
            }

            // Mouse position
            Point mouse = this.PointToClient(Control.MousePosition);

            if (this.ClientRectangle.Contains(mouse))
            {
                int x = (mouse.X - (mouse.X % TerrainViewWindow.PreviewScale)) / TerrainViewWindow.PreviewScale;
                int y = (mouse.Y - (mouse.Y % TerrainViewWindow.PreviewScale)) / TerrainViewWindow.PreviewScale;

                if (((PawCraftMainWindow)this.MdiParent).ActiveEditorTool != null)
                {
                    ((PawCraftMainWindow)this.MdiParent).ActiveEditorTool.Draw2D(grp, new Point(x, y), TerrainViewWindow.PreviewScale, ((PawCraftMainWindow)this.MdiParent).ViewModel.LevelData);
                }

                this.Text = string.Format("{0} [{1},{2}-{3}]", TerrainViewWindow.Title, x, y, level[x, y].Depth);
            }
            else
            {
                this.Text = TerrainViewWindow.Title;
            }

            // end container
            grp.EndContainer(containerState);
        }

        /// <summary>
        /// Apply tool to data
        /// </summary>
        /// <param name="sender">Window form</param>
        /// <param name="e">Empty event</param>
        private void ApplyTool(object sender, EventArgs e)
        {
            Point mouse = this.PointToClient(Control.MousePosition);

            if (this.ClientRectangle.Contains(mouse) && Control.MouseButtons == MouseButtons.Left)
            {
                int x = (mouse.X - (mouse.X % TerrainViewWindow.PreviewScale)) / TerrainViewWindow.PreviewScale;
                int y = (mouse.Y - (mouse.Y % TerrainViewWindow.PreviewScale)) / TerrainViewWindow.PreviewScale;

                if (((PawCraftMainWindow)this.MdiParent).ActiveEditorTool != null)
                {
                    Point editedTile = new Point(x, y);
                    ((PawCraftMainWindow)this.MdiParent).ActiveEditorTool.Apply(editedTile, ((PawCraftMainWindow)this.MdiParent).ViewModel.LevelData);
                    this.lastEditedTile = editedTile;
                }
            }
        }

        /// <summary>
        /// Editing stopped
        /// </summary>
        /// <param name="sender">GL control</param>
        /// <param name="e">Mouse event</param>
        private void EditingEnd(object sender, MouseEventArgs e)
        {
            this.ApplyTool(sender, e);
            this.lastEditedTile = null;
        }

        /// <summary>
        /// Editing started
        /// </summary>
        /// <param name="sender">GL control</param>
        /// <param name="e">Mouse event</param>
        private void EditingStart(object sender, MouseEventArgs e)
        {
            this.lastEditedTile = null;
        }

        /// <summary>
        /// Refresh window
        /// </summary>
        /// <param name="sender">Timer control</param>
        /// <param name="e">Empty event</param>
        private void RefreshWindow(object sender, System.EventArgs e)
        {
            this.Invalidate();
        }

        /// <summary>
        /// Mouse is moving around
        /// </summary>
        /// <param name="sender">View window</param>
        /// <param name="e">Mouse event</param>
        private void ViewMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Point mouse = this.PointToClient(Control.MousePosition);

            if (this.ClientRectangle.Contains(mouse) && Control.MouseButtons == MouseButtons.Left)
            {
                int x = (mouse.X - (mouse.X % TerrainViewWindow.PreviewScale)) / TerrainViewWindow.PreviewScale;
                int y = (mouse.Y - (mouse.Y % TerrainViewWindow.PreviewScale)) / TerrainViewWindow.PreviewScale;

                if (((PawCraftMainWindow)this.MdiParent).ActiveEditorTool != null)
                {
                    Point editedTile = new Point(x, y);

                    if (this.lastEditedTile == null || (this.lastEditedTile.Value.X - editedTile.X != 0 || this.lastEditedTile.Value.Y - editedTile.Y != 0))
                    {
                        this.ApplyTool(sender, e);
                    }
                }
            }
        }
    }
}