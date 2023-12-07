namespace PawCraft.Tools
{
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
        private readonly EntityTool tool;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityToolDialog"/> class
        /// </summary>
        public EntityToolDialog(EntityTool tool) : base(tool)
        {
            this.tool = tool;
            this.InitializeComponent();
        }

        private void EntityToolDialog_Load(object sender, EventArgs e)
        {

            tool.TextureAtlas = ((WorldViewWindow)((PawCraftMainWindow)((Form)this.Parent).MdiParent).WorldView).TextureAtlas;
            tool.Container = ((WorldViewWindow)((PawCraftMainWindow)((Form)this.Parent).MdiParent).WorldView).EntityContainer;

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

            this.currentEntity.SelectedIndex = (int)tool.SelectedEntity;

            this.currentEntity.SelectedIndexChanged += (senderx, ex) =>
            {
                tool.SelectedEntity = (Level.EntityData.EntityType)this.currentEntity.SelectedIndex;
            };
        }
    }
}