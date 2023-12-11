namespace PawCraft.Tools
{
    using PawCraft.Level;
    using PawCraft.ToolsApi;
    using PawCraft.Utils.Types;
    using SharpGL.SceneGraph;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using Color = System.Drawing.Color;
    using SaturnColor = PawCraft.Utils.Types.Color;

    /// <summary>
    /// Entity tool dialog controls
    /// </summary>
    public partial class LightToolDialog : ToolDialogBase<LightTool>
    {
        /// <summary>
        /// Light tool isntance
        /// </summary>
        private readonly LightTool tool;

        /// <summary>
        /// Initializes a new instance of the <see cref="LightToolDialog"/> class
        /// </summary>
        public LightToolDialog(LightTool tool) : base(tool)
        {
            this.Load += (sender, e) =>
            {
                // Load level defaults
                Level.LevelData level = ((PawCraftMainWindow)((Form)this.Parent).MdiParent).ViewModel.LevelData;
                this.tool.SunDirection = FxVector.ToVertex(level.Light.Direction);
                this.tool.SunColor = level.Light.Color;

                // Set control defaults
                this.colorPanel.BackColor = Color.FromArgb(this.tool.SunColor.Red, this.tool.SunColor.Green, this.tool.SunColor.Blue);
                float angle = this.tool.SunDirection.GetAngleTo(new Vertex(0.0f, 0.0f, 1.0f));
                this.pitchAngle.Value = (decimal)Math.Max(
                    Math.Min(
                        180.0 - (angle * (180.0 / Math.PI)),
                        (double)this.pitchAngle.Maximum),
                    (double)this.pitchAngle.Minimum);
            };

            this.tool = tool;
            this.InitializeComponent();
        }

        /// <summary>
        /// Get angle between line and Y axis.
        /// </summary>
        /// <param name="firstPoint">Start point of the line</param>
        /// <param name="secondPoint">End point of the line</param>
        /// <returns>Angle in radians</returns>
        private static double GetAngle(Vertex firstPoint, Vertex secondPoint)
        {
            if (firstPoint.X.IsSame(secondPoint.X) && secondPoint.Y.IsSame(firstPoint.Y))
            {
                return 0.0;
            }

            float width = secondPoint.X - firstPoint.X;
            float height = secondPoint.Y - firstPoint.Y;
            float atan = 0.0f;

            if (width.IsNotZero())
            {
                atan = (float)Math.Atan(height / width);
            }
            else if (height.IsLess(0.0f))
            {
                return (float)(Math.PI / 2.0);
            }
            else if (height.IsGreater(0.0f))
            {
                return (float)((Math.PI / 2.0) + Math.PI);
            }

            if (width.IsLessOrSame(0.0f) || height.IsLessOrSame(0.0f))
            {
                atan += (float)Math.PI;
            }

            if (width.IsGreaterOrSame(0.0f) && height.IsLessOrSame(0.0f))
            {
                atan -= (float)Math.PI;
            }

            if (atan.IsLessOrSame(0.0f))
            {
                atan += (float)(Math.PI * 2.0);
            }

            float rawAngle = (float)(atan % (Math.PI * 2.0));

            // Hotfix
            if (rawAngle.IsSame((float)(Math.PI / 2.0)) && secondPoint.Y.IsLessOrSame(firstPoint.Y))
            {
                rawAngle += (float)Math.PI;
            }

            return (Math.PI * 2.0) - rawAngle;
        }

        /// <summary>
        /// Get vertex color
        /// </summary>
        /// <param name="vertices">All vertices</param>
        /// <param name="lightDir">Light direction</param>
        /// <param name="lightColor">Light color</param>
        /// <param name="x">Current X location</param>
        /// <param name="y">Current Y location</param>
        /// <param name="normal">Vertex normal</param>
        /// <returns>Vertex color</returns>
        private static SaturnColor GetColor(Vertex[,] vertices, Vertex lightDir, SaturnColor lightColor, int x, int y, out Vertex normal)
        {
            Vertex current = vertices[x, y];
            List<Vertex> cross = new List<Vertex>();
            Vertex? n1 = null;
            Vertex? n2 = null;
            Vertex? n3 = null;
            Vertex? n4 = null;

            // Add in positive direction
            if (x + 1 < vertices.GetLength(0))
            {
                Vertex vector = vertices[x + 1, y] - current;
                vector.Normalize();
                n1 = vector;
            }

            if (y + 1 < vertices.GetLength(1))
            {
                Vertex vector = vertices[x, y + 1] - current;
                vector.Normalize();
                n2 = vector;
            }

            // Add in negative direction
            if (x - 1 >= 0)
            {
                Vertex vector = vertices[x - 1, y] - current;
                vector.Normalize();
                n3 = vector;
            }

            if (y - 1 >= 0)
            {
                Vertex vector = vertices[x, y - 1] - current;
                vector.Normalize();
                n4 = vector;
            }

            // Create cross products
            if (n1.HasValue && n2.HasValue)
            {
                cross.Add(n1.Value.VectorProduct(n2.Value));
            }

            if (n2.HasValue && n3.HasValue)
            {
                cross.Add(n2.Value.VectorProduct(n3.Value));
            }

            if (n3.HasValue && n4.HasValue)
            {
                cross.Add(n3.Value.VectorProduct(n4.Value));
            }

            if (n4.HasValue && n1.HasValue)
            {
                cross.Add(n4.Value.VectorProduct(n1.Value));
            }

            // Normalize result
            if (cross.Any())
            {
                // Rotate vector if upside down
                Vertex total = new Vertex();

                foreach (Vertex crossVertex in cross)
                {
                    Vertex newCross = new Vertex(crossVertex);
                    newCross *= newCross.ScalarProduct(new Vertex(0.0f, 0.0f, 1.0f));
                    newCross.Normalize();

                    total += newCross;
                }

                total /= cross.Count;
                total.Normalize();
                total *= total.ScalarProduct(new Vertex(0.0f, 0.0f, 1.0f));

                if (((float)total.Magnitude()).IsZero())
                {
                    total = new Vertex(0.0f, 0.0f, 1.0f);
                }

                // Calculate light normal
                float strength = Math.Max(Math.Min(-total.ScalarProduct(lightDir), 1.0f), (Math.Abs(new Vertex(0.0f, 0.0f, 1.0f).ScalarProduct(lightDir)) / 3.0f));

                total.Normalize();
                normal = total;
                return SaturnColor.FromRgb(
                    (lightColor.Red * strength) / (float)byte.MaxValue,
                    (lightColor.Green * strength) / (float)byte.MaxValue,
                    (lightColor.Blue * strength) / (float)byte.MaxValue);
            }

            normal = new Vertex { Z = 1.0f };
            return SaturnColor.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }

        /// <summary>
        /// Compute light of the map
        /// </summary>
        /// <param name="sender">Button control</param>
        /// <param name="e">Empty event</param>
        private void ComputeClick(object sender, EventArgs e)
        {
            Level.LevelData level = ((PawCraftMainWindow)((Form)this.Parent).MdiParent).ViewModel.LevelData;

            // Create light settings
            LevelLight light = new LevelLight
            {
                Direction = FxVector.FromVertex(this.tool.SunDirection),
                Color = this.tool.SunColor
            };

            // Create heights
            Vertex[,] vertices = new Vertex[LevelData.MapDimensionSize + 1, LevelData.MapDimensionSize + 1];

            for (int y = 0; y < LevelData.MapDimensionSize; y++)
            {
                for (int x = 0; x < LevelData.MapDimensionSize; x++)
                {
                    float[] heights = level.GetTileVerticeHeights(x, y);
                    vertices[x, y] = new Vertex(x, y, heights[0]);
                    vertices[x, y + 1] = new Vertex(x, y + 1.0f, heights[1]);
                    vertices[x + 1, y + 1] = new Vertex(x + 1.0f, y + 1.0f, heights[2]);
                    vertices[x + 1, y] = new Vertex(x + 1.0f, y, heights[3]);
                }
            }

            // Calculate shading
            List<Gourad> shading = new List<Gourad>();
            List<FxVector> normals = new List<FxVector>();

            for (int y = 0; y < LevelData.MapDimensionSize; y++)
            {
                for (int x = 0; x < LevelData.MapDimensionSize; x++)
                {
                    Vertex v0;
                    Vertex v1;
                    Vertex v2;
                    Vertex v3;

                    List<SaturnColor> saturnColors = new List<SaturnColor>
                    {
                        LightToolDialog.GetColor(vertices, this.tool.SunDirection, this.tool.SunColor, x, y, out v0),
                        LightToolDialog.GetColor(vertices, this.tool.SunDirection, this.tool.SunColor, x, y + 1, out v1),
                        LightToolDialog.GetColor(vertices, this.tool.SunDirection, this.tool.SunColor, x + 1, y + 1, out v2),
                        LightToolDialog.GetColor(vertices, this.tool.SunDirection, this.tool.SunColor, x + 1, y, out v3)
                    };

                    shading.Add(Gourad.FromColors(saturnColors.ToArray()));

                    // Calculate plane normal
                    Vertex planeNormal = (v0 + v1 + v2 + v3) / 4.0f;
                    planeNormal.Normalize();
                    normals.Add(FxVector.FromVertex(planeNormal));
                }
            }

            // save result
            ((PawCraftMainWindow)((Form)this.Parent).MdiParent).ViewModel.SetLevelLight(light, shading.ToArray(), normals.ToArray());
        }

        /// <summary>
        /// Update angle when mouse clicks and moves
        /// </summary>
        /// <param name="sender">Picture box</param>
        /// <param name="e">Mouse event</param>
        private void DirectionPickerMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point center = new Point(
                     ((PictureBox)sender).ClientRectangle.X + (((PictureBox)sender).ClientRectangle.Width / 2),
                     ((PictureBox)sender).ClientRectangle.Y + (((PictureBox)sender).ClientRectangle.Height / 2));

                Point mouse = ((PictureBox)sender).PointToClient(Control.MousePosition);
                Point vector = new Point(mouse.X - center.X, mouse.Y - center.Y);
                Vertex vertex = new Vertex(vector.X, -vector.Y, 0.0f);
                vertex.Normalize();

                float pitch = (float)(Math.PI / 2) - this.tool.SunDirection.GetAngleTo(new Vertex(0.0f, 0.0f, 1.0f));
                float yaw = (float)LightToolDialog.GetAngle(new Vertex(), vertex);

                this.tool.SunDirection = new Vertex(1.0f, 0.0f, 0.0f)
                    .RotateAroundAxis(pitch, new Vertex(0.0f, 1.0f, 0.0f))
                    .RotateAroundAxis(yaw, new Vertex(0.0f, 0.0f, 1.0f));

                ((PictureBox)sender).Invalidate();
            }
        }

        /// <summary>
        /// Paint direction picker
        /// </summary>
        /// <param name="sender">Picture box</param>
        /// <param name="e">Draw arguments</param>
        private void DirectionPickerPaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Pen foreground = new Pen(Color.OrangeRed, 1);
            Brush foregroundBrush = new SolidBrush(Color.White);
            Pen outline = new Pen(Color.DarkGray, 2);
            Pen outline2 = new Pen(Color.LightGray, 1);
            Brush background = new SolidBrush(Color.Black);
            Rectangle rectangle = new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
            Rectangle rectangle2 = new Rectangle(e.ClipRectangle.X + 1, e.ClipRectangle.Y + 1, e.ClipRectangle.Width - 3, e.ClipRectangle.Height - 3);
            int radius = (e.ClipRectangle.Width / 2);
            Point center = new Point(e.ClipRectangle.X + (e.ClipRectangle.Width / 2), e.ClipRectangle.Y + (e.ClipRectangle.Height / 2));

            e.Graphics.FillEllipse(background, rectangle);
            e.Graphics.FillRectangle(foregroundBrush, new Rectangle(e.ClipRectangle.X + (e.ClipRectangle.Width / 2) - 1, e.ClipRectangle.Y + (e.ClipRectangle.Height / 2) - 1, 3, 3));

            double angle = LightToolDialog.GetAngle(new Vertex(), this.tool.SunDirection);
            Point outer = new Point(center.X + (int)(radius * Math.Cos(angle)), center.X + (int)(radius * Math.Sin(angle)));

            e.Graphics.DrawLine(foreground, center, outer);

            e.Graphics.DrawEllipse(outline, rectangle);
            e.Graphics.DrawEllipse(outline2, rectangle2);

            outline.Dispose();
            outline2.Dispose();
            background.Dispose();
            foregroundBrush.Dispose();
            foreground.Dispose();
        }

        /// <summary>
        /// Pick light color
        /// </summary>
        /// <param name="sender">Pick button</param>
        /// <param name="e">Empty event</param>
        private void PickColor(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = this.colorPanel.BackColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                SaturnColor color = SaturnColor.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                this.colorPanel.BackColor = Color.FromArgb(color.Red, color.Green, color.Blue);
                this.tool.SunColor = color;
            }
        }

        /// <summary>
        /// Pitch changed
        /// </summary>
        /// <param name="sender">Number box</param>
        /// <param name="e">Empty event</param>
        private void PitchChanged(object sender, EventArgs e)
        {
            float pitch = -(float)((int)this.pitchAngle.Value * (Math.PI / 180.0));
            float yaw = (float)LightToolDialog.GetAngle(new Vertex(), this.tool.SunDirection);

            this.tool.SunDirection = new Vertex(1.0f, 0.0f, 0.0f)
                .RotateAroundAxis(pitch, new Vertex(0.0f, 1.0f, 0.0f))
                .RotateAroundAxis(yaw, new Vertex(0.0f, 0.0f, 1.0f));

            this.pictureBox1.Invalidate();
        }
    }
}