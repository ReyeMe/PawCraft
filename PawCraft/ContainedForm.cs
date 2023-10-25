namespace PawCraft
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Contained window
    /// </summary>
    public class ContainedForm : Form
    {
        /// <summary>
        /// Hide close button flag
        /// </summary>
        private const int CP_NOCLOSE_BUTTON = 0x200;

        /// <summary>
        /// Window loaded
        /// </summary>
        private bool loaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainedForm"/> class
        /// </summary>
        public ContainedForm()
        {
            this.StartPosition = FormStartPosition.Manual;

            this.Shown += (sender, e) =>
            {
                this.RestoreSize();
                this.loaded = true;
            };
        }

        /// <summary>
        /// Gets creation parameters
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams parameters = base.CreateParams;
                parameters.ClassStyle |= ContainedForm.CP_NOCLOSE_BUTTON;
                return parameters;
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
            return string.Format("{0}.{1}_", this.GetType().Namespace, this.GetType().Name);
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
                Settings.GetValue(prefix + "Y", out int y))
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(x, y);

                if (Settings.GetValue(prefix + "W", out int w) &&
                    Settings.GetValue(prefix + "H", out int h))
                {
                    this.Size = new System.Drawing.Size(w, h);
                }
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

                if (this.MaximizeBox || this.MinimizeBox)
                {
                    Settings.SetValue(prefix + "State", (int)this.WindowState);
                }

                Settings.SetValue(prefix + "X", this.Location.X);
                Settings.SetValue(prefix + "Y", this.Location.Y);

                if (this.FormBorderStyle != FormBorderStyle.FixedDialog)
                {
                    Settings.SetValue(prefix + "W", this.Size.Width);
                    Settings.SetValue(prefix + "H", this.Size.Height);
                }
            }
        }
    }
}