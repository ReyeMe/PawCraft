namespace PawCraft
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.aboutTitle = new System.Windows.Forms.Label();
            this.smug = new System.Windows.Forms.PictureBox();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.aboutPanel = new System.Windows.Forms.Panel();
            this.link = new System.Windows.Forms.LinkLabel();
            this.date = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.smug)).BeginInit();
            this.aboutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // aboutTitle
            // 
            this.aboutTitle.Font = new System.Drawing.Font("Arial Unicode MS", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutTitle.Location = new System.Drawing.Point(0, 0);
            this.aboutTitle.Name = "aboutTitle";
            this.aboutTitle.Size = new System.Drawing.Size(400, 32);
            this.aboutTitle.TabIndex = 0;
            this.aboutTitle.Text = "PawCraft level editor";
            this.aboutTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // smug
            // 
            this.smug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.smug.Image = global::PawCraft.Properties.Resources.CatGirl;
            this.smug.Location = new System.Drawing.Point(0, 60);
            this.smug.Margin = new System.Windows.Forms.Padding(0);
            this.smug.Name = "smug";
            this.smug.Size = new System.Drawing.Size(40, 40);
            this.smug.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.smug.TabIndex = 0;
            this.smug.TabStop = false;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(43, 74);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 2;
            this.cancelBtn.Text = "Close";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // aboutPanel
            // 
            this.aboutPanel.Controls.Add(this.smug);
            this.aboutPanel.Controls.Add(this.link);
            this.aboutPanel.Controls.Add(this.date);
            this.aboutPanel.Controls.Add(this.aboutTitle);
            this.aboutPanel.Controls.Add(this.cancelBtn);
            this.aboutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aboutPanel.Location = new System.Drawing.Point(0, 0);
            this.aboutPanel.MaximumSize = new System.Drawing.Size(400, 100);
            this.aboutPanel.MinimumSize = new System.Drawing.Size(400, 100);
            this.aboutPanel.Name = "aboutPanel";
            this.aboutPanel.Size = new System.Drawing.Size(400, 100);
            this.aboutPanel.TabIndex = 2;
            // 
            // link
            // 
            this.link.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.link.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.link.Location = new System.Drawing.Point(0, 32);
            this.link.Name = "link";
            this.link.Size = new System.Drawing.Size(400, 39);
            this.link.TabIndex = 4;
            this.link.Text = "Created by: ReyeMe\r\nArt by: Am25, ReyeMe, AnriFox";
            this.link.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkClicked);
            // 
            // date
            // 
            this.date.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.date.AutoSize = true;
            this.date.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.date.Location = new System.Drawing.Point(366, 84);
            this.date.Name = "date";
            this.date.Size = new System.Drawing.Size(31, 13);
            this.date.TabIndex = 3;
            this.date.Text = "2024";
            // 
            // About
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(400, 100);
            this.Controls.Add(this.aboutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            ((System.ComponentModel.ISupportInitialize)(this.smug)).EndInit();
            this.aboutPanel.ResumeLayout(false);
            this.aboutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox smug;
        private System.Windows.Forms.Label aboutTitle;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Panel aboutPanel;
        private System.Windows.Forms.Label date;
        private System.Windows.Forms.LinkLabel link;
    }
}