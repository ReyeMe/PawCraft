namespace PawCraft.Tools
{
    using PawCraft.Entities;
    using PawCraft.ToolsApi;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// Entity tool dialog controls
    /// </summary>
    public partial class EntityToolDialog : ToolDialogBase<EntityTool>
    {
        /// <summary>
        /// Tool instance
        /// </summary>
        private readonly EntityTool tool;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityToolDialog"/> class
        /// </summary>
        public EntityToolDialog(EntityTool tool) : base(tool)
        {
            this.tool = tool;
            this.InitializeComponent();
        }

        /// <summary>
        /// Dialog was loaded
        /// </summary>
        /// <param name="sender">Dialog instance</param>
        /// <param name="e">Empty event</param>
        private void EntityToolDialogLoad(object sender, EventArgs e)
        {
            this.tool.TextureAtlas = ((WorldViewWindow)((PawCraftMainWindow)((Form)this.Parent).MdiParent).WorldView).TextureAtlas;
            this.tool.Container = ((WorldViewWindow)((PawCraftMainWindow)((Form)this.Parent).MdiParent).WorldView).EntityContainer;

            foreach (object option in Enum.GetValues(typeof(Level.EntityData.EntityType)))
            {
                DisplayAttribute display = option.GetType().GetField(option.ToString()).GetCustomAttribute<DisplayAttribute>();

                if (display == null)
                {
                    this.currentEntity.Items.Add(option.ToString());
                }
                else
                {
                    this.currentEntity.Items.Add(display.Name);
                }
            }

            this.currentEntity.SelectedIndex = (int)this.tool.SelectedEntity.Type;

            this.currentEntity.SelectedIndexChanged += (senderx, ex) =>
            {
                Level.EntityData.EntityType newType = (Level.EntityData.EntityType)(this.currentEntity.SelectedIndex + 1);

                if (newType != this.tool.SelectedEntity.Type)
                {
                    this.tool.SelectedEntity.ResetReservedValues();
                }

                this.tool.SelectedEntity.Type = (Level.EntityData.EntityType)this.currentEntity.SelectedIndex;
                this.ReloadEntityParameters();
            };
        }

        /// <summary>
        /// Reload entity parameters
        /// </summary>
        private void ReloadEntityParameters()
        {
            Entities.EntityPropertyAttribute properties = this.tool.SelectedEntity.Type.GetType().GetField(this.tool.SelectedEntity.Type.ToString()).GetCustomAttribute<Entities.EntityPropertyAttribute>();

            if (properties != null)
            {
                this.properties.SelectedObject = Activator.CreateInstance(properties.Dialog, new[] { this.tool.SelectedEntity });
            }
            else
            {
                this.properties.SelectedObject = new BaseEntityProperties(this.tool.SelectedEntity);
            }
        }
    }
}