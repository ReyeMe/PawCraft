namespace PawCraft.Rendering
{
    using PawCraft.Level;
    using SharpGL;
    using SharpGL.SceneGraph.Core;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Grid lines
    /// </summary>
    internal class GridLines : SceneElement, IRenderable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridLines"/> class
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        public GridLines(OpenGL gl)
        {
            List<BufferedVertex> buffer = new List<BufferedVertex>();
            float half = (LevelData.MapDimensionSize + 1) / 2.0f;
            
            buffer.Add(new BufferedVertex
            {
                X = (float)half,
                Y = -(float)LevelData.MapDimensionSize * 100.0f,
                Z = 0.0f,
                Color = Color.LightGreen
            });
            
            buffer.Add(new BufferedVertex
            {
                X = (float)half,
                Y = (float)LevelData.MapDimensionSize * 100.0f,
                Z = 0.0f,
                Color = Color.LightGreen
            });
            
            buffer.Add(new BufferedVertex
            {
                X = -(float)LevelData.MapDimensionSize * 100.0f,
                Y = (float)half,
                Z = 0.0f,
                Color = Color.Red
            });
            
            buffer.Add(new BufferedVertex
            {
                X = (float)LevelData.MapDimensionSize * 100.0f,
                Y = (float)half,
                Z = 0.0f,
                Color = Color.Red
            });
            
            for (int x = 0; x <= LevelData.MapDimensionSize; x++)
            {
                buffer.Add(new BufferedVertex
                {
                    X = (float)x,
                    Y = 0.0f,
                    Z = 0.0f,
                    Color = Color.DimGray
                });
            
                buffer.Add(new BufferedVertex
                {
                    X = (float)x,
                    Y = (float)LevelData.MapDimensionSize,
                    Z = 0.0f,
                    Color = Color.DimGray
                });
            }
            
            for (int y = 0; y <= LevelData.MapDimensionSize; y++)
            {
                buffer.Add(new BufferedVertex
                {
                    X = 0.0f,
                    Y = (float)y,
                    Z = 0.0f,
                    Color = Color.DimGray
                });
            
                buffer.Add(new BufferedVertex
                {
                    X = (float)LevelData.MapDimensionSize,
                    Y = (float)y,
                    Z = 0.0f,
                    Color = Color.DimGray
                });
            }
            
            // Setup VBO
            uint[] result = new uint[1];
            gl.GenBuffers(1, result);
            this.Id = result[0];
            
            this.Count = buffer.Count;
            float[] floatBuffer = buffer.SelectMany(vertex => vertex.ToArray()).ToArray();
            this.Length = floatBuffer.Length;
            this.Size = sizeof(float) * 6;
            
            GCHandle vertsHandle = GCHandle.Alloc(floatBuffer, GCHandleType.Pinned);
            IntPtr vertsPtr = vertsHandle.AddrOfPinnedObject();
            
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, this.Id);
            gl.BufferData(OpenGL.GL_ARRAY_BUFFER, this.Size * this.Length, vertsPtr, OpenGL.GL_STATIC_DRAW);
            gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, 0);
            gl.VertexPointer(3, OpenGL.GL_FLOAT, 0, IntPtr.Zero);
            gl.EnableClientState(OpenGL.GL_VERTEX_ARRAY);
            
            vertsHandle.Free();
            this.Visible = true;
        }

        /// <summary>
        /// Gets number of objects
        /// </summary>
        private int Count { get; }

        /// <summary>
        /// Gets buffer identifier
        /// </summary>
        private uint Id { get; }

        /// <summary>
        /// Gets buffer length
        /// </summary>
        private int Length { get; }

        /// <summary>
        /// Gets type size
        /// </summary>
        private int Size { get; }

        /// <summary>
        /// Gets or sets visibility
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Render grid lines
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        /// <param name="renderMode">Current render mode</param>
        public void Render(OpenGL gl, RenderMode renderMode)
        {
            if (renderMode != RenderMode.HitTest && this.Visible)
            {
                gl.Disable(OpenGL.GL_LIGHTING);
                gl.Disable(OpenGL.GL_TEXTURE_2D);
                gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, this.Id);
            
                gl.VertexPointer(3, OpenGL.GL_FLOAT, this.Size, IntPtr.Zero);
                gl.ColorPointer(3, OpenGL.GL_FLOAT, this.Size, new IntPtr(this.Size / 2));
            
                gl.EnableClientState(OpenGL.GL_VERTEX_ARRAY);
                gl.EnableClientState(OpenGL.GL_COLOR_ARRAY);
                gl.DrawArrays(OpenGL.GL_LINES, 0, this.Count);
            
                gl.DisableClientState(OpenGL.GL_VERTEX_ARRAY);
                gl.DisableClientState(OpenGL.GL_COLOR_ARRAY);
                gl.BindBuffer(OpenGL.GL_ARRAY_BUFFER, 0);
            }
        }

        /// <summary>
        /// Line vertex
        /// </summary>
        public struct BufferedVertex
        {
            /// <summary>
            /// Vertex color
            /// </summary>
            public Color Color;

            /// <summary>
            /// X position component
            /// </summary>
            public float X;

            /// <summary>
            /// Y position component
            /// </summary>
            public float Y;

            /// <summary>
            /// Z position component
            /// </summary>
            public float Z;

            /// <summary>
            /// Convert to array
            /// </summary>
            /// <returns>Float array</returns>
            public float[] ToArray()
            {
                return new[] {
                        this.X,
                        this.Y,
                        this.Z,
                        this.Color.R / (float)byte.MaxValue,
                        this.Color.G / (float)byte.MaxValue,
                        this.Color.B / (float)byte.MaxValue
                    };
            }
        }
    }
}