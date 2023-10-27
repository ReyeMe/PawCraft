namespace PawCraft.Tools
{
    partial class PaintToolDialog
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textureScroll = new System.Windows.Forms.VScrollBar();
            this.textureView = new SharpGL.SceneControl();
            this.currentTexture = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.textureView)).BeginInit();
            this.SuspendLayout();
            // 
            // textureScroll
            // 
            this.textureScroll.Location = new System.Drawing.Point(122, 30);
            this.textureScroll.Name = "textureScroll";
            this.textureScroll.Size = new System.Drawing.Size(17, 244);
            this.textureScroll.TabIndex = 2;
            this.textureScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ViewScroll);
            // 
            // textureView
            // 
            this.textureView.DrawFPS = false;
            this.textureView.Location = new System.Drawing.Point(0, 30);
            this.textureView.Name = "textureView";
            this.textureView.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.textureView.Padding = new System.Windows.Forms.Padding(5);
            this.textureView.RenderContextType = SharpGL.RenderContextType.FBO;
            this.textureView.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.textureView.Size = new System.Drawing.Size(122, 244);
            this.textureView.TabIndex = 1;
            this.textureView.Click += new System.EventHandler(this.MouseViewClick);
            // 
            // currentTexture
            // 
            this.currentTexture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currentTexture.FormattingEnabled = true;
            this.currentTexture.Location = new System.Drawing.Point(3, 3);
            this.currentTexture.Name = "currentTexture";
            this.currentTexture.Size = new System.Drawing.Size(133, 21);
            this.currentTexture.TabIndex = 0;
            this.currentTexture.SelectedIndexChanged += new System.EventHandler(this.TextureSelectionChanged);
            // 
            // PaintToolDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textureScroll);
            this.Controls.Add(this.textureView);
            this.Controls.Add(this.currentTexture);
            this.Name = "PaintToolDialog";
            this.Size = new System.Drawing.Size(139, 274);
            ((System.ComponentModel.ISupportInitialize)(this.textureView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox currentTexture;
        private SharpGL.SceneControl textureView;
        private System.Windows.Forms.VScrollBar textureScroll;
    }
}
