namespace PawCraft.ToolsApi
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// Tool dialog base
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    [DesignerCategory("UserControl")]
    [DefaultEvent("Load")]
    public class ToolDialogBase<Tool> : UserControl, IToolDialog where Tool : class
    {
        /// <summary>
        /// Initializes a new isntance of the <see cref="ToolDialogBase{Tool}"/> class
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if called outside of designer</exception>
        public ToolDialogBase()
        {
            // Do nothing
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolDialogBase"/> class
        /// </summary>
        /// <param name="tool">Tool instance</param>
        public ToolDialogBase(Tool tool)
        {
            this.ActiveTool = tool;
        }

        /// <summary>
        /// Gets currently active tool instance
        /// </summary>
        protected Tool ActiveTool { get; private set; }

        /// <summary>
        /// Tool is closing
        /// </summary>
        public virtual void OnClose()
        {
            // Do nothing
        }

        /// <summary>
        /// Tool is opening
        /// </summary>
        public virtual void OnShown()
        {
            // Do nothing
        }
    }
}