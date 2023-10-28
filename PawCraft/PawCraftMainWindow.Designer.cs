namespace PawCraft
{
    partial class PawCraftMainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PawCraftMainWindow));
            this.editorMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewShading = new System.Windows.Forms.ToolStripMenuItem();
            this.viewHeightmap = new System.Windows.Forms.ToolStripMenuItem();
            this.viewTextured = new System.Windows.Forms.ToolStripMenuItem();
            this.viewTexturedShading = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keyboardBindingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editorTools = new System.Windows.Forms.ToolStrip();
            this.pointerTool = new System.Windows.Forms.ToolStripButton();
            this.raiseTool = new System.Windows.Forms.ToolStripButton();
            this.digTool = new System.Windows.Forms.ToolStripButton();
            this.paintTool = new System.Windows.Forms.ToolStripButton();
            this.entityTool = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.editorMenu.SuspendLayout();
            this.editorTools.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // editorMenu
            // 
            this.editorMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.editorMenu.Location = new System.Drawing.Point(0, 0);
            this.editorMenu.Name = "editorMenu";
            this.editorMenu.Size = new System.Drawing.Size(800, 24);
            this.editorMenu.TabIndex = 0;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator1,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.CreateNewLevel);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenLevel);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveLevel);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsLevel);
            // 
            // dToolStripMenuItem
            // 
            this.dToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewShading});
            this.dToolStripMenuItem.Name = "dToolStripMenuItem";
            this.dToolStripMenuItem.Size = new System.Drawing.Size(33, 20);
            this.dToolStripMenuItem.Text = "&3D";
            // 
            // viewShading
            // 
            this.viewShading.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewHeightmap,
            this.viewTextured,
            this.viewTexturedShading});
            this.viewShading.Name = "viewShading";
            this.viewShading.Size = new System.Drawing.Size(180, 22);
            this.viewShading.Text = "&View";
            // 
            // viewHeightmap
            // 
            this.viewHeightmap.CheckOnClick = true;
            this.viewHeightmap.Name = "viewHeightmap";
            this.viewHeightmap.Size = new System.Drawing.Size(184, 22);
            this.viewHeightmap.Tag = "0";
            this.viewHeightmap.Text = "&Heightmap";
            this.viewHeightmap.Click += new System.EventHandler(this.ViewShadingModeChanged);
            // 
            // viewTextured
            // 
            this.viewTextured.CheckOnClick = true;
            this.viewTextured.Name = "viewTextured";
            this.viewTextured.Size = new System.Drawing.Size(184, 22);
            this.viewTextured.Tag = "1";
            this.viewTextured.Text = "&Textured";
            this.viewTextured.Click += new System.EventHandler(this.ViewShadingModeChanged);
            // 
            // viewTexturedShading
            // 
            this.viewTexturedShading.Checked = true;
            this.viewTexturedShading.CheckOnClick = true;
            this.viewTexturedShading.CheckState = System.Windows.Forms.CheckState.Checked;
            this.viewTexturedShading.Name = "viewTexturedShading";
            this.viewTexturedShading.Size = new System.Drawing.Size(184, 22);
            this.viewTexturedShading.Tag = "2";
            this.viewTexturedShading.Text = "Textured and &Shaded";
            this.viewTexturedShading.Click += new System.EventHandler(this.ViewShadingModeChanged);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keyboardBindingToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // keyboardBindingToolStripMenuItem
            // 
            this.keyboardBindingToolStripMenuItem.Name = "keyboardBindingToolStripMenuItem";
            this.keyboardBindingToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.keyboardBindingToolStripMenuItem.Text = "&View help";
            this.keyboardBindingToolStripMenuItem.Click += new System.EventHandler(this.ViewHelp);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutBtnClick);
            // 
            // editorTools
            // 
            this.editorTools.CanOverflow = false;
            this.editorTools.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.editorTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pointerTool,
            this.raiseTool,
            this.digTool,
            this.paintTool,
            this.entityTool});
            this.editorTools.Location = new System.Drawing.Point(0, 24);
            this.editorTools.Name = "editorTools";
            this.editorTools.Size = new System.Drawing.Size(800, 25);
            this.editorTools.TabIndex = 1;
            // 
            // pointerTool
            // 
            this.pointerTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pointerTool.Image = global::PawCraft.Properties.Resources.PointerIco;
            this.pointerTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pointerTool.Name = "pointerTool";
            this.pointerTool.Size = new System.Drawing.Size(23, 22);
            this.pointerTool.Text = "Pointer";
            this.pointerTool.Click += new System.EventHandler(this.SetTool);
            // 
            // raiseTool
            // 
            this.raiseTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.raiseTool.Image = global::PawCraft.Properties.Resources.RaiseIco;
            this.raiseTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.raiseTool.Name = "raiseTool";
            this.raiseTool.Size = new System.Drawing.Size(23, 22);
            this.raiseTool.Text = "Raise";
            this.raiseTool.Click += new System.EventHandler(this.SetTool);
            // 
            // digTool
            // 
            this.digTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.digTool.Image = global::PawCraft.Properties.Resources.DigIco;
            this.digTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.digTool.Name = "digTool";
            this.digTool.Size = new System.Drawing.Size(23, 22);
            this.digTool.Text = "Dig";
            this.digTool.Click += new System.EventHandler(this.SetTool);
            // 
            // paintTool
            // 
            this.paintTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.paintTool.Image = global::PawCraft.Properties.Resources.PaintIco;
            this.paintTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.paintTool.Name = "paintTool";
            this.paintTool.Size = new System.Drawing.Size(23, 22);
            this.paintTool.Text = "Paint";
            this.paintTool.Click += new System.EventHandler(this.SetTool);
            // 
            // entityTool
            // 
            this.entityTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.entityTool.Image = global::PawCraft.Properties.Resources.EntityIco;
            this.entityTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.entityTool.Name = "entityTool";
            this.entityTool.Size = new System.Drawing.Size(23, 22);
            this.entityTool.Text = "Entities";
            this.entityTool.Click += new System.EventHandler(this.SetTool);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStatusLabel
            // 
            this.toolStatusLabel.Name = "toolStatusLabel";
            this.toolStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // PawCraftMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.editorTools);
            this.Controls.Add(this.editorMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.editorMenu;
            this.Name = "PawCraftMainWindow";
            this.Text = "PawCraft";
            this.Load += new System.EventHandler(this.PawCraftLoad);
            this.editorMenu.ResumeLayout(false);
            this.editorMenu.PerformLayout();
            this.editorTools.ResumeLayout(false);
            this.editorTools.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip editorMenu;
        private System.Windows.Forms.ToolStrip editorTools;
        private System.Windows.Forms.ToolStripButton raiseTool;
        private System.Windows.Forms.ToolStripButton digTool;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton paintTool;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton pointerTool;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keyboardBindingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewHeightmap;
        private System.Windows.Forms.ToolStripMenuItem viewTextured;
        private System.Windows.Forms.ToolStripMenuItem viewTexturedShading;
        private System.Windows.Forms.ToolStripMenuItem viewShading;
        private System.Windows.Forms.ToolStripButton entityTool;
    }
}

