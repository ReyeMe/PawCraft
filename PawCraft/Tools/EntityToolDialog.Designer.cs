namespace PawCraft.Tools
{
    partial class EntityToolDialog
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
            this.currentEntity = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // currentEntity
            // 
            this.currentEntity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currentEntity.FormattingEnabled = true;
            this.currentEntity.Location = new System.Drawing.Point(3, 3);
            this.currentEntity.Name = "currentEntity";
            this.currentEntity.Size = new System.Drawing.Size(176, 21);
            this.currentEntity.TabIndex = 1;
            // 
            // EntityToolDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.currentEntity);
            this.Name = "EntityToolDialog";
            this.Size = new System.Drawing.Size(182, 63);
            this.Load += new System.EventHandler(this.EntityToolDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox currentEntity;
    }
}
