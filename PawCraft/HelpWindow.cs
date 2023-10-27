namespace PawCraft
{
    using PawCraft.Properties;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;

    /// <summary>
    /// Help window class
    /// </summary>
    public partial class HelpWindow : Form
    {
        /// <summary>
        /// Initializes a new isntance of the <see cref="HelpWindow"/> class
        /// </summary>
        public HelpWindow()
        {
            this.Icon = SystemIcons.Question;
            this.InitializeComponent();
            this.ClientSize = new Size(590, 390);

            this.Shown += (sender, e) =>
            {
                this.MinimumSize = this.Size;

                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "PawCraft.Resources.Help.html";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        this.webReadme.DocumentText = reader.ReadToEnd();
                    }
                }
            };
        }
    }
}