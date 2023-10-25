namespace PawCraft
{
    using PawCraft.Tools;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// Main editor window
    /// </summary>
    public partial class PawCraftMainWindow : Form
    {
        /// <summary>
        /// Current active tool
        /// </summary>
        private ToolBase activeEditorTool;

        /// <summary>
        /// Current tool window position
        /// </summary>
        private Point toolWindowPosition = Point.Empty;

        /// <summary>
        /// Application view model
        /// </summary>
        private ViewModel viewModel;

        /// <summary>
        /// Window loaded
        /// </summary>
        private readonly bool loaded;

        /// <summary>
        /// Initializes a new instancve of the <see cref="PawCraftMainWindow"/> class
        /// </summary>
        public PawCraftMainWindow()
        {
            this.InitializeComponent();
            this.RestoreSize();
            this.loaded = true;

            // Create MDI windows
            this.WorldView = new WorldViewWindow { MdiParent = this };
            this.TerrainView = new TerrainViewWindow { MdiParent = this };

            // Set new level data
            this.CreateNewLevel(null, null);

            // Show the windows
            this.WorldView.Show();
            this.TerrainView.Show();
        }

        /// <summary>
        /// Gets current active tool
        /// </summary>
        public ToolBase ActiveEditorTool
        {
            get
            {
                return this.activeEditorTool;
            }

            set
            {
                Dictionary<string, object> values = this.activeEditorTool?.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(prop => prop.CanWrite && prop.CanRead)
                    .ToDictionary(prop => prop.Name, prop => prop.GetValue(this.activeEditorTool, null));

                this.activeEditorTool = value;

                if (this.PropertyWindow != null)
                {
                    this.toolWindowPosition = this.PropertyWindow.Location;
                    this.PropertyWindow.Close();
                    this.PropertyWindow = null;
                }

                if (value != null)
                {
                    if (values != null)
                    {
                        this.activeEditorTool.GetType()
                            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(prop => prop.CanWrite && prop.CanRead && values.ContainsKey(prop.Name) && prop.PropertyType.IsAssignableFrom(values[prop.Name].GetType()))
                            .ToList()
                            .ForEach(prop => prop.SetValue(this.activeEditorTool, values[prop.Name]));
                    }

                    this.PropertyWindow = new ToolPropertyDialog(value) { MdiParent = this };

                    if (this.toolWindowPosition != Point.Empty)
                    {
                        this.PropertyWindow.Location = this.toolWindowPosition;
                    }

                    this.PropertyWindow.Show();
                }
            }
        }

        /// <summary>
        /// Gets current property window
        /// </summary>
        public ContainedForm PropertyWindow { get; private set; } = null;

        /// <summary>
        /// Gets terrain view
        /// </summary>
        public ContainedForm TerrainView { get; }

        /// <summary>
        /// Application view model
        /// </summary>
        public ViewModel ViewModel
        {
            get
            {
                return this.viewModel;
            }

            private set
            {
                this.viewModel = value;
                ((WorldViewWindow)this.WorldView).ReloadTileData();
            }
        }

        /// <summary>
        /// Gets world view
        /// </summary>
        public ContainedForm WorldView { get; }

        /// <summary>
        /// Show about window
        /// </summary>
        /// <param name="sender">Tool strip button control</param>
        /// <param name="e">Empty event</param>
        private void AboutBtnClick(object sender, EventArgs e)
        {
            new About().ShowDialog(this);
        }

        /// <summary>
        /// Create new level
        /// </summary>
        /// <param name="sender">Menu button</param>
        /// <param name="e">Empty event</param>
        private void CreateNewLevel(object sender, EventArgs e)
        {
            this.ViewModel = new ViewModel();
        }

        /// <summary>
        /// Open level file
        /// </summary>
        /// <param name="sender">menu button</param>
        /// <param name="e">Empty event</param>
        private void OpenLevel(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Utenyaa level file|*.UTE",
                AddExtension = true,
                CheckPathExists = true,
                CheckFileExists = true,
                RestoreDirectory = true,
                ValidateNames = true
            };

            if (dialog.ShowDialog() == DialogResult.OK && File.Exists(dialog.FileName))
            {
                try
                {
                    this.ViewModel = new ViewModel(dialog.FileName);
                }
                catch (Exception ex)
                {
                    this.ShowErrorMessage("Could not open the file!", ex);
                }
            }
        }

        /// <summary>
        /// Save as new level
        /// </summary>
        /// <param name="sender">Menu button</param>
        /// <param name="e">Empty event</param>
        private void SaveAsLevel(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Utenyaa level file|*.UTE",
                OverwritePrompt = true,
                AddExtension = true,
                CheckPathExists = true,
                RestoreDirectory = true,
                ValidateNames = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.ViewModel.Save(dialog.FileName);
                }
                catch (Exception ex)
                {
                    this.ShowErrorMessage("Could not save the file!", ex);
                }
            }
        }

        /// <summary>
        /// Save level
        /// </summary>
        /// <param name="sender">Menu button</param>
        /// <param name="e">Empty event</param>
        private void SaveLevel(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ViewModel.CurrentLevelFile))
            {
                this.SaveAsLevel(sender, e);
                return;
            }

            try
            {
                this.ViewModel.Save();
            }
            catch (Exception ex)
            {
                this.ShowErrorMessage("Could not save the file!", ex);
            }
        }

        /// <summary>
        /// Error message box
        /// </summary>
        /// <param name="message">Main messge</param>
        /// <param name="exception">Exception</param>
        private void ShowErrorMessage(string message, Exception exception)
        {
            if (exception == null)
            {
                MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(this, message + "\n" +  exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Set current tool
        /// </summary>
        /// <param name="sender">Toolbar button</param>
        /// <param name="e">Empty event</param>
        private void SetTool(object sender, EventArgs e)
        {
            if (sender is ToolStripItem control)
            {
                switch (control.Name.ToLower())
                {
                    case "painttool":
                        this.ActiveEditorTool = new PaintTool();
                        break;

                    case "digtool":
                        this.ActiveEditorTool = new DigTool();
                        break;

                    case "raisetool":
                        this.ActiveEditorTool = new RaiseTool();
                        break;

                    default:
                        this.ActiveEditorTool = null;
                        break;
                }
            }
        }


        /// <summary>
        /// Location changed
        /// </summary>
        /// <param name="e">Empty event</param>
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            this.SaveSize();
        }

        /// <summary>
        /// Window resized
        /// </summary>
        /// <param name="e">Empty event</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.SaveSize();
        }

        /// <summary>
        /// Size changed
        /// </summary>
        /// <param name="e">Empty event</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.SaveSize();
        }

        /// <summary>
        /// Gets setting name prefix
        /// </summary>
        /// <returns>Setting name prefix</returns>
        private string GetSettingPrefix()
        {
            return string.Format("{0}.{1}_", this.GetType().Namespace, "MainWindow");
        }

        /// <summary>
        /// Restore window size
        /// </summary>
        private void RestoreSize()
        {
            string prefix = this.GetSettingPrefix();

            if (Settings.GetValue(prefix + "State", out int state) && state != (int)FormWindowState.Normal)
            {
                if (Settings.GetValue(prefix + "X", out int x) &&
                    Settings.GetValue(prefix + "Y", out int y))
                {
                    this.StartPosition = FormStartPosition.Manual;
                    this.Location = new System.Drawing.Point(x, y);
                }

                this.WindowState = (FormWindowState)state;
            }
            else if (Settings.GetValue(prefix + "X", out int x) &&
                Settings.GetValue(prefix + "Y", out int y) &&
                Settings.GetValue(prefix + "W", out int w) &&
                Settings.GetValue(prefix + "H", out int h))
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(x, y);
                this.Size = new System.Drawing.Size(w, h);
            }
        }

        /// <summary>
        /// Save window size
        /// </summary>
        private void SaveSize()
        {
            if (this.loaded)
            {
                string prefix = this.GetSettingPrefix();
                Settings.SetValue(prefix + "State", (int)this.WindowState);
                Settings.SetValue(prefix + "X", this.Location.X);
                Settings.SetValue(prefix + "Y", this.Location.Y);
                Settings.SetValue(prefix + "W", this.Size.Width);
                Settings.SetValue(prefix + "H", this.Size.Height);
            }
        }
    }
}