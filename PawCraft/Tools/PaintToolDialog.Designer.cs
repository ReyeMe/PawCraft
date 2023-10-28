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
            this.radiusValue = new System.Windows.Forms.HScrollBar();
            this.valueLabel = new System.Windows.Forms.Label();
            this.radiusLabel = new System.Windows.Forms.Label();
            this.textureScroll = new System.Windows.Forms.VScrollBar();
            this.textureView = new SharpGL.SceneControl();
            this.currentTexture = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.textureView)).BeginInit();
            this.SuspendLayout();
            // 
            // radiusValue
            // 
            this.radiusValue.LargeChange = 1;
            this.radiusValue.Location = new System.Drawing.Point(46, 31);
            this.radiusValue.Maximum = 20;
            this.radiusValue.Minimum = 1;
            this.radiusValue.Name = "radiusValue";
            this.radiusValue.ScaleScrollBarForDpiChange = false;
            this.radiusValue.Size = new System.Drawing.Size(115, 17);
            this.radiusValue.TabIndex = 6;
            this.radiusValue.Value = 1;
            this.radiusValue.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(161, 33);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(13, 13);
            this.valueLabel.TabIndex = 5;
            this.valueLabel.Text = "1";
            // 
            // radiusLabel
            // 
            this.radiusLabel.AutoSize = true;
            this.radiusLabel.Location = new System.Drawing.Point(3, 33);
            this.radiusLabel.Name = "radiusLabel";
            this.radiusLabel.Size = new System.Drawing.Size(40, 13);
            this.radiusLabel.TabIndex = 4;
            this.radiusLabel.Text = "Radius";
            // 
            // textureScroll
            // 
            this.textureScroll.Location = new System.Drawing.Point(165, 51);
            this.textureScroll.Name = "textureScroll";
            this.textureScroll.Size = new System.Drawing.Size(17, 223);
            this.textureScroll.TabIndex = 2;
            this.textureScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ViewScroll);
            // 
            // textureView
            // 
            this.textureView.DrawFPS = false;
            this.textureView.Location = new System.Drawing.Point(0, 51);
            this.textureView.Name = "textureView";
            this.textureView.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.textureView.Padding = new System.Windows.Forms.Padding(5);
            this.textureView.RenderContextType = SharpGL.RenderContextType.FBO;
            this.textureView.RenderTrigger = SharpGL.RenderTrigger.TimerBased;
            this.textureView.Size = new System.Drawing.Size(165, 223);
            this.textureView.TabIndex = 1;
            this.textureView.Click += new System.EventHandler(this.MouseViewClick);
            // 
            // currentTexture
            // 
            this.currentTexture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currentTexture.FormattingEnabled = true;
            this.currentTexture.Location = new System.Drawing.Point(3, 3);
            this.currentTexture.Name = "currentTexture";
            this.currentTexture.Size = new System.Drawing.Size(176, 21);
            this.currentTexture.TabIndex = 0;
            this.currentTexture.SelectedIndexChanged += new System.EventHandler(this.TextureSelectionChanged);
            // 
            // PaintToolDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radiusValue);
            this.Controls.Add(this.valueLabel);
            this.Controls.Add(this.radiusLabel);
            this.Controls.Add(this.textureScroll);
            this.Controls.Add(this.textureView);
            this.Controls.Add(this.currentTexture);
            this.Name = "PaintToolDialog";
            this.Size = new System.Drawing.Size(182, 274);
            ((System.ComponentModel.ISupportInitialize)(this.textureView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox currentTexture;
        private SharpGL.SceneControl textureView;
        private System.Windows.Forms.VScrollBar textureScroll;
        private System.Windows.Forms.Label radiusLabel;
        private System.Windows.Forms.Label valueLabel;
        private System.Windows.Forms.HScrollBar radiusValue;
    }
}
