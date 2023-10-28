namespace PawCraft
{
    partial class WorldViewWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorldViewWindow));
            this.glControl = new PawCraft.Rendering.LevelSceneControl();
            ((System.ComponentModel.ISupportInitialize)(this.glControl)).BeginInit();
            this.SuspendLayout();
            // 
            // glControl
            // 
            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.DrawFPS = false;
            this.glControl.FrameRate = 30;
            this.glControl.Location = new System.Drawing.Point(0, 0);
            this.glControl.Name = "glControl";
            this.glControl.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.glControl.RenderContextType = SharpGL.RenderContextType.NativeWindow;
            this.glControl.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.glControl.Size = new System.Drawing.Size(394, 261);
            this.glControl.TabIndex = 0;
            this.glControl.OpenGLInitialized += new System.EventHandler(this.GlInitialized);
            this.glControl.OpenGLDraw += new SharpGL.RenderEventHandler(this.Redraw);
            this.glControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ApplyTool);
            this.glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMoved);
            // 
            // WorldViewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 261);
            this.Controls.Add(this.glControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WorldViewWindow";
            this.Text = "3D";
            ((System.ComponentModel.ISupportInitialize)(this.glControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Rendering.LevelSceneControl glControl;
    }
}