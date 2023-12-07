namespace PawCraft.Rendering
{
    using SharpGL.SceneGraph.Core;

    /// <summary>
    /// Entities container
    /// </summary>
    public class EntitiesContainer : SceneElement
    {
        /// <summary>
        /// Gets parent window
        /// </summary>
        public WorldViewWindow ParentWindow { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitiesContainer"/> class
        /// </summary>
        /// <param name="parentWindow">World view window</param>
        public EntitiesContainer(WorldViewWindow parentWindow)
        {
            this.ParentWindow = parentWindow;
        }

        /// <summary>
        /// Refresh entities
        /// </summary>
        public void Refresh()
        {
            this.Children.Clear();

            if (((PawCraftMainWindow)this.ParentWindow.MdiParent).ViewModel.LevelData.Entities != null)
            {
                int length = ((PawCraftMainWindow)this.ParentWindow.MdiParent).ViewModel.LevelData.Entities.Length;

                for (int i = 0; i < length; i++)
                {
                    this.AddChild(new Entity(i, this.ParentWindow));
                }
            }
        }
    }
}
