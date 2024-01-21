namespace PawCraft
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Main program class
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Settings.LoadSettings();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PawCraftMainWindow());

            Settings.SaveSettings();
        }
    }
}