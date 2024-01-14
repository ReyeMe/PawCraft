namespace PawCraft
{
    partial class EntityEditorWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityEditorWindow));
            this.currentEntity = new System.Windows.Forms.ComboBox();
            this.entityTypeLabel = new System.Windows.Forms.Label();
            this.properties = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // currentEntity
            // 
            this.currentEntity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentEntity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currentEntity.FormattingEnabled = true;
            this.currentEntity.Location = new System.Drawing.Point(68, 3);
            this.currentEntity.Name = "currentEntity";
            this.currentEntity.Size = new System.Drawing.Size(173, 21);
            this.currentEntity.TabIndex = 2;
            // 
            // entityTypeLabel
            // 
            this.entityTypeLabel.AutoSize = true;
            this.entityTypeLabel.Location = new System.Drawing.Point(6, 6);
            this.entityTypeLabel.Name = "entityTypeLabel";
            this.entityTypeLabel.Size = new System.Drawing.Size(56, 13);
            this.entityTypeLabel.TabIndex = 3;
            this.entityTypeLabel.Text = "Entity type";
            // 
            // properties
            // 
            this.properties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.properties.Location = new System.Drawing.Point(3, 30);
            this.properties.Name = "properties";
            this.properties.Size = new System.Drawing.Size(238, 188);
            this.properties.TabIndex = 4;
            // 
            // EntityEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 221);
            this.Controls.Add(this.properties);
            this.Controls.Add(this.entityTypeLabel);
            this.Controls.Add(this.currentEntity);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EntityEditorWindow";
            this.Text = "Edit entity";
            this.Load += new System.EventHandler(this.EntityEditorDialogLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox currentEntity;
        private System.Windows.Forms.Label entityTypeLabel;
        private System.Windows.Forms.PropertyGrid properties;
    }
}