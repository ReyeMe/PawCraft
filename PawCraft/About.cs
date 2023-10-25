namespace PawCraft
{
    using System.Windows.Forms;

    /// <summary>
    /// About dialog
    /// </summary>
    public partial class About : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="About"/> class
        /// </summary>
        public About()
        {
            this.InitializeComponent();
            this.ClientSize = this.aboutPanel.Size;

            // We have to add links this way, because designer can't do it
            this.link.Links.Add(new LinkLabel.Link(12, 6, "https://reye.me"));
            this.link.Links.Add(new LinkLabel.Link(34, 6, "https://reye.me"));
            this.link.Links.Add(new LinkLabel.Link(28, 4, string.Empty));
        }

        /// <summary>
        /// Open website link
        /// </summary>
        /// <param name="sender">Link label</param>
        /// <param name="e">Link click event</param>
        private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link.LinkData is string url && !string.IsNullOrEmpty(url))
            {
                System.Diagnostics.Process.Start(url);
            }
        }
    }
}