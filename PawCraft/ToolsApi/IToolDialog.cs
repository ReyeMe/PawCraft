namespace PawCraft.ToolsApi
{
    /// <summary>
    /// Tool dialog interface
    /// </summary>
    public interface IToolDialog
    {
        /// <summary>
        /// Dialog is closing
        /// </summary>
        void OnClose();

        /// <summary>
        /// Dialog was shown
        /// </summary>
        void OnShown();
    }
}