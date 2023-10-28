namespace PawCraft.Tools
{
    using PawCraft.ToolsApi;

    /// <summary>
    /// Entity tool dialog controls
    /// </summary>
    public partial class EntityToolDialog : ToolDialogBase<EntityTool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityToolDialog"/> class
        /// </summary>
        public EntityToolDialog(EntityTool tool) : base(tool)
        {
            this.InitializeComponent();
        }
    }
}