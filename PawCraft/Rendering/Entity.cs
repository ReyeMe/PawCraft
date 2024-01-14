namespace PawCraft.Rendering
{
    using PawCraft.Level;
    using PawCraft.Utils;
    using PawCraft.Utils.Types;
    using SharpGL;
    using SharpGL.SceneGraph;
    using SharpGL.SceneGraph.Assets;
    using SharpGL.SceneGraph.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static PawCraft.EntityModelHandler;
    using static PawCraft.TextureHandler;

    public class Entity : SceneElement, IRenderable
    {
        /// <summary>
        /// Index to entity data array
        /// </summary>
        private readonly int entityDataIndex;

        /// <summary>
        /// Initializes a new isntance of the <see cref="Entity"/> class
        /// </summary>
        /// <param name="dataIndex">Entity data index</param>
        /// <param name="worldView">View window</param>
        public Entity(int dataIndex, WorldViewWindow worldView)
        {
            this.ParentWindow = worldView;
            this.entityDataIndex = dataIndex;
        }

        /// <summary>
        /// Gets parent window
        /// </summary>
        public WorldViewWindow ParentWindow { get; }

        /// <summary>
        /// Gets texture atlas
        /// </summary>
        public TextureHandler TextureAtlas
        {
            get
            {
                return this.ParentWindow.TextureAtlas;
            }
        }

        /// <summary>
        /// Gets entity atlas
        /// </summary>
        public EntityModelHandler EntityAtlas
        {
            get
            {
                return this.ParentWindow.EntityAtlas;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether entity is selected
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Gets level data
        /// </summary>
        public LevelData Level
        {
            get
            {
                return ((PawCraftMainWindow)this.ParentWindow.MdiParent).ViewModel.LevelData;
            }
        }

        /// <summary>
        /// Gets entity data
        /// </summary>
        public EntityData Data
        {
            get
            {
                if (this.entityDataIndex < this.Level.Entities.Length)
                {
                    return this.Level.Entities[this.entityDataIndex];
                }

                return new EntityData();
            }
        }

        /// <summary>
        /// Color iterator
        /// </summary>
        private static float colorIterator = 0.0f;

        /// <summary>
        /// Color iterator step
        /// </summary>
        private static float colorIteratorStep = 0.1f;

        /// <summary>
        /// Render object
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        /// <param name="renderMode">Rendering mode</param>
        public void Render(OpenGL gl, RenderMode renderMode)
        {
            // Do not allow selection when using any tool
            if (renderMode == RenderMode.HitTest && ((PawCraftMainWindow)this.ParentWindow.MdiParent).ActiveEditorTool != null)
            {
                return;
            }

            EntityData data = this.Data;

            // Select model
            IRenderableEntity model;

            if (data.Type == EntityData.EntityType.Model && EntityModelHandler.GetModelName(data.Reserved[1]) != null)
            {
                model = this.EntityAtlas.GetModel(EntityModelHandler.GetModelName(data.Reserved[1]));
            }
            else
            {
                model = this.EntityAtlas.GetModel(data.Type.ToString());
            }


            float height = this.Level.GetTileVerticeHeights(data.X, data.Y).Sum() / 4.0f;
            Vertex position = new Vertex(data.X + 0.5f, data.Y + 0.5f, height);

            if (model != null)
            {
                gl.PushMatrix();
                gl.Translate(position.X, position.Y, position.Z);
                gl.Rotate(data.Direction.FromFixed().FromRadians(), 0.0, 0.0, 1.0);
                model.Render(gl, FxVector.ToVertex(this.Level.Light.Direction), this.Selected, renderMode);
                gl.PopMatrix();

                gl.Disable(OpenGL.GL_TEXTURE_2D);

                return;
            }

            GlTexture tex = (GlTexture)this.TextureAtlas.GetEntityTexture(data.Type.ToString());
            float bitmapWidth = tex.Width / 16;
            float bitmapHeight = tex.Height / 16;

            // Set icon texture
            if (renderMode != RenderMode.HitTest)
            {
                gl.Enable(OpenGL.GL_TEXTURE_2D);
                tex.Bind(gl);
            }

            double[] projectionMatrix = new double[16];
            double[] modelViewMatrix = new double[16];
            int[] viewport = new int[4];
            gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);
            gl.GetDouble(OpenGL.GL_PROJECTION_MATRIX, projectionMatrix);
            gl.GetDouble(OpenGL.GL_MODELVIEW_MATRIX, modelViewMatrix);

            float[,] projection = new float[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    projection[i, j] = (float)projectionMatrix[(i * 4) + j];

            Vertex cameraRight = new Vertex(projection[0, 0], projection[1, 0], projection[2, 0]);
            cameraRight.Normalize();
            cameraRight = (cameraRight / 2.0f) * bitmapWidth;
            Vertex cameraUp = new Vertex(projection[0, 1], projection[1, 1], projection[2, 1]);
            cameraUp.Normalize();
            cameraUp = (cameraUp / 2.0f) * bitmapHeight;

            // Draw billboard sprite if on screen
            gl.Begin(OpenGL.GL_QUADS);
            gl.TexCoord(0, 1);
            gl.Color(1.0f, 1.0f, 1.0f);
            gl.Vertex(position - cameraRight);
            gl.TexCoord(1, 1);
            gl.Color(1.0f, 1.0f, 1.0f);
            gl.Vertex(position + cameraRight);
            gl.TexCoord(1, 0);
            gl.Color(1.0f, 1.0f, 1.0f);
            gl.Vertex(position + cameraRight + (cameraUp * 2.0f));
            gl.TexCoord(0, 0);
            gl.Color(1.0f, 1.0f, 1.0f);
            gl.Vertex(position - cameraRight + (cameraUp * 2.0f));
            gl.End();

            gl.Disable(OpenGL.GL_TEXTURE_2D);

            if (this.Selected)
            {
                Entity.colorIterator += Entity.colorIteratorStep;

                if (Entity.colorIterator > 1.0f || Entity.colorIterator < 0.0f)
                {
                    Entity.colorIterator = Math.Max(Math.Min(Entity.colorIterator, 1.0f), 0.0f);
                    Entity.colorIteratorStep *= -1.0f;
                }

                cameraRight *= 1.1f;
                cameraUp *= 1.1f;

                gl.LineWidth(2.0f);
                gl.Begin(OpenGL.GL_LINE_LOOP);
                gl.Color(1.0f - Entity.colorIterator, 1.0f, Entity.colorIterator);
                gl.Vertex(position - cameraRight);
                gl.Color(1.0f - Entity.colorIterator, 1.0f, Entity.colorIterator);
                gl.Vertex(position + cameraRight);
                gl.Color(1.0f - Entity.colorIterator, 1.0f, Entity.colorIterator);
                gl.Vertex(position + cameraRight + (cameraUp * 2.0f));
                gl.Color(1.0f - Entity.colorIterator, 1.0f, Entity.colorIterator);
                gl.Vertex(position - cameraRight + (cameraUp * 2.0f));
                gl.End();

                gl.LineWidth(1.0f);
            }
        }
    }
}
