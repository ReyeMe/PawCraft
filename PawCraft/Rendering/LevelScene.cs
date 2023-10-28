namespace PawCraft.Rendering
{
    using SharpGL;
    using SharpGL.SceneGraph.Cameras;
    using SharpGL.SceneGraph.Core;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Level scene
    /// </summary>
    public class LevelScene : SharpGL.SceneGraph.Scene
    {
        /// <summary>
        /// Do hit test
        /// </summary>
        /// <param name="x">Screen X coordinate</param>
        /// <param name="y">Screen Y coordinate</param>
        /// <returns>Hit elements</returns>
        public override IEnumerable<SceneElement> DoHitTest(int x, int y)
        {
            //  Create a result set.
            List<PickResult> resultSet = new List<PickResult>();

            //  Create a hitmap.
            Dictionary<uint, SceneElement> hitMap = new Dictionary<uint, SceneElement>();

            //	If we don't have a current camera, we cannot hit test.
            if (this.CurrentCamera == null)
            {
                return new List<SceneElement>();
            }

            //	Create an array that will be the viewport.
            int[] viewport = new int[4];

            //	Get the viewport, then convert the mouse point to an opengl point.
            this.CurrentOpenGLContext.GetInteger(OpenGL.GL_VIEWPORT, viewport);
            y = viewport[3] - y;

            //	Create a select buffer.
            uint[] selectBuffer = new uint[512];
            this.CurrentOpenGLContext.SelectBuffer(512, selectBuffer);

            //	Enter select mode.
            this.CurrentOpenGLContext.RenderMode(OpenGL.GL_SELECT);

            //	Initialise the names, and add the first name.
            this.CurrentOpenGLContext.InitNames();
            this.CurrentOpenGLContext.PushName(0);

            //	Push matrix, set up projection, then load matrix.
            this.CurrentOpenGLContext.MatrixMode(OpenGL.GL_PROJECTION);
            this.CurrentOpenGLContext.PushMatrix();
            this.CurrentOpenGLContext.LoadIdentity();
            this.CurrentOpenGLContext.PickMatrix(x, y, 4, 4, viewport);
            this.CurrentCamera.TransformProjectionMatrix(this.CurrentOpenGLContext);
            this.CurrentOpenGLContext.MatrixMode(OpenGL.GL_MODELVIEW);
            this.CurrentOpenGLContext.LoadIdentity();

            //  Create the name.
            uint currentName = 1;

            //  Render the root for hit testing.
            this.RenderElementForHitTest(this.SceneContainer, hitMap, ref currentName);

            //	Pop matrix and flush commands.
            this.CurrentOpenGLContext.MatrixMode(OpenGL.GL_PROJECTION);
            this.CurrentOpenGLContext.PopMatrix();
            this.CurrentOpenGLContext.MatrixMode(OpenGL.GL_MODELVIEW);
            this.CurrentOpenGLContext.Flush();

            //	End selection.
            int hits = this.CurrentOpenGLContext.RenderMode(OpenGL.GL_RENDER);
            uint posinarray = 0;

            //  Go through each name.
            for (int hit = 0; hit < hits; hit++)
            {
                uint nameCount = selectBuffer[posinarray++];
                uint min = selectBuffer[posinarray++];
                uint max = selectBuffer[posinarray++];

                if (nameCount == 0)
                    continue;

                //	Add each hit element to the result set to the array.
                for (int name = 0; name < nameCount; name++)
                {
                    uint hitName = selectBuffer[posinarray++];
                    resultSet.Add(new PickResult { Element = hitMap[hitName], MaximumZ = max, MinumumZ = min });
                }
            }

            //  Return the result set.
            return resultSet.OrderBy(result => result.MinumumZ).Select(result => result.Element);
        }

        //
        // Summary:
        //     This function draws all of the objects in the scene (i.e. every quadric in the
        //     quadrics arraylist etc).
        public override void Draw(Camera camera = null)
        {
            if (camera == null)
            {
                camera = CurrentCamera;
            }

            this.CurrentOpenGLContext.ClearColor(0.0f, 0.2f, 0.2f, 1.0f);
            camera?.Project(this.CurrentOpenGLContext);
            this.CurrentOpenGLContext.Clear(17664u);
            this.RenderElement(this.SceneContainer, RenderMode.Design);
            this.CurrentOpenGLContext.Flush();
        }

        /// <summary>
        /// Renders the element for hit test.
        /// </summary>
        /// <param name="sceneElement">The scene element.</param>
        /// <param name="hitMap">The hit map.</param>
        /// <param name="currentName">Current hit name.</param>
        private void RenderElementForHitTest(SceneElement sceneElement, Dictionary<uint, SceneElement> hitMap, ref uint currentName)
        {
            //  If the element is disabled, we're done.
            //  Also, never hit test the current camera.
            if (sceneElement.IsEnabled == false || sceneElement == this.CurrentCamera)
            {
                return;
            }

            //  Push each effect.
            foreach (var effect in sceneElement.Effects)
            {
                if (effect.IsEnabled)
                {
                    effect.Push(this.CurrentOpenGLContext, sceneElement);
                }
            }

            //  If the element has an object space, transform into it.
            if (sceneElement is IHasObjectSpace)
            {
                ((IHasObjectSpace)sceneElement).PushObjectSpace(this.CurrentOpenGLContext);
            }

            //  If the element is volume bound, render the volume.
            if (sceneElement is ICustomVolumeBound || sceneElement is IVolumeBound || sceneElement is IRenderable)
            {
                if (sceneElement is ICustomVolumeBound)
                {
                    //  Load and map the name.
                    this.CurrentOpenGLContext.LoadName(currentName);
                    hitMap[currentName] = sceneElement;

                    //  Render the bounding volume.
                    ((ICustomVolumeBound)sceneElement).BoundingVolume.Render(this.CurrentOpenGLContext, RenderMode.HitTest);

                    //  Increment the name.
                    currentName++;
                }
                else if (sceneElement is IVolumeBound)
                {
                    //  Load and map the name.
                    this.CurrentOpenGLContext.LoadName(currentName);
                    hitMap[currentName] = sceneElement;

                    //  Render the bounding volume.
                    ((IVolumeBound)sceneElement).BoundingVolume.Render(this.CurrentOpenGLContext, RenderMode.HitTest);

                    //  Increment the name.
                    currentName++;
                }
                else if (sceneElement is IRenderable)
                {
                    //  Load and map the name.
                    this.CurrentOpenGLContext.LoadName(currentName);
                    hitMap[currentName] = sceneElement;

                    //  Render the bounding volume.
                    ((IRenderable)sceneElement).Render(this.CurrentOpenGLContext, RenderMode.HitTest);

                    //  Increment the name.
                    currentName++;
                }
            }

            //  Recurse through the children.
            foreach (var childElement in sceneElement.Children)
            {
                this.RenderElementForHitTest(childElement, hitMap, ref currentName);
            }

            //  If the element has an object space, transform out of it.
            if (sceneElement is IHasObjectSpace)
            {
                ((IHasObjectSpace)sceneElement).PopObjectSpace(this.CurrentOpenGLContext);
            }

            //  Pop each effect.
            for (int i = sceneElement.Effects.Count - 1; i >= 0; i--)
            {
                if (sceneElement.Effects[i].IsEnabled)
                {
                    sceneElement.Effects[i].Pop(this.CurrentOpenGLContext, sceneElement);
                }
            }
        }

        /// <summary>
        /// Pick result
        /// </summary>
        private class PickResult
        {
            /// <summary>
            /// Picked element
            /// </summary>
            public SceneElement Element { get; set; }

            /// <summary>
            /// Buffer maximum position
            /// </summary>
            public uint MaximumZ { get; set; }

            /// <summary>
            /// Buffer minimum position
            /// </summary>
            public uint MinumumZ { get; set; }
        }
    }
}