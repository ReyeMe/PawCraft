namespace PawCraft.Rendering
{
    using SharpGL.SceneGraph.Core;

    /// <summary>
    /// Custom bounding volume
    /// </summary>
    public interface ICustomVolumeBound
    {
        /// <summary>
        /// Gets bounding volume
        /// </summary>
        IRenderable BoundingVolume { get; }
    }
}