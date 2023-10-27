namespace PawCraft
{
    using System.Drawing;

    partial class HelpWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.webReadme = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webReadme
            // 
            this.webReadme.AllowNavigation = false;
            this.webReadme.AllowWebBrowserDrop = false;
            this.webReadme.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webReadme.IsWebBrowserContextMenuEnabled = false;
            this.webReadme.Location = new System.Drawing.Point(0, 0);
            this.webReadme.MinimumSize = new System.Drawing.Size(20, 20);
            this.webReadme.Name = "webReadme";
            this.webReadme.ScrollBarsEnabled = false;
            this.webReadme.Size = new System.Drawing.Size(581, 357);
            this.webReadme.TabIndex = 0;
            this.webReadme.WebBrowserShortcutsEnabled = false;
            // 
            // HelpWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 357);
            this.Controls.Add(this.webReadme);
            this.Name = "HelpWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Help";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webReadme;
    }
}