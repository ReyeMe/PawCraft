namespace PawCraft.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SharpGL;
    using SharpGL.SceneGraph;
    using SharpGL.SceneGraph.Core;

    internal class EntityBoundingVolume : IRenderable
    {
        /// <summary>
        /// Color iterator
        /// </summary>
        private static float colorIterator = 0.0f;

        /// <summary>
        /// Color iterator step
        /// </summary>
        private static float colorIteratorStep = 0.1f;

        private Vertex hhh;
        private Vertex hhl;
        private Vertex hlh;
        private Vertex hll;
        private Vertex lhh;
        private Vertex lhl;
        private Vertex llh;
        private Vertex lll;

        /// <summary>
        /// Creates the volume from vertices.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        public void FromVertices(IEnumerable<Vertex> vertices)
        {
            List<Vertex> list = vertices.ToList();
            if (list.Count >= 2)
            {
                lll = list[0];
                hll = list[0];
                lhl = list[0];
                llh = list[0];
                hhl = list[0];
                hlh = list[0];
                lhh = list[0];
                hhh = list[0];
                for (int i = 1; i < list.Count; i++)
                {
                    lll.X = Math.Min(lll.X, list[i].X);
                    lll.Y = Math.Min(lll.Y, list[i].Y);
                    lll.Z = Math.Min(lll.Z, list[i].Z);
                    hll.X = Math.Max(hll.X, list[i].X);
                    hll.Y = Math.Min(hll.Y, list[i].Y);
                    hll.Z = Math.Min(hll.Z, list[i].Z);
                    lhl.X = Math.Min(lhl.X, list[i].X);
                    lhl.Y = Math.Max(lhl.Y, list[i].Y);
                    lhl.Z = Math.Min(lhl.Z, list[i].Z);
                    llh.X = Math.Min(llh.X, list[i].X);
                    llh.Y = Math.Min(llh.Y, list[i].Y);
                    llh.Z = Math.Max(llh.Z, list[i].Z);
                    hhl.X = Math.Max(hhl.X, list[i].X);
                    hhl.Y = Math.Max(hhl.Y, list[i].Y);
                    hhl.Z = Math.Min(hhl.Z, list[i].Z);
                    hlh.X = Math.Max(hlh.X, list[i].X);
                    hlh.Y = Math.Min(hlh.Y, list[i].Y);
                    hlh.Z = Math.Max(hlh.Z, list[i].Z);
                    lhh.X = Math.Min(lhh.X, list[i].X);
                    lhh.Y = Math.Max(lhh.Y, list[i].Y);
                    lhh.Z = Math.Max(lhh.Z, list[i].Z);
                    hhh.X = Math.Max(hhh.X, list[i].X);
                    hhh.Y = Math.Max(hhh.Y, list[i].Y);
                    hhh.Z = Math.Max(hhh.Z, list[i].Z);
                }
            }
        }

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public void Render(OpenGL gl, RenderMode renderMode)
        {
            EntityBoundingVolume.colorIterator += EntityBoundingVolume.colorIteratorStep;

            if (EntityBoundingVolume.colorIterator > 1.0f || EntityBoundingVolume.colorIterator < 0.0f)
            {
                EntityBoundingVolume.colorIterator = Math.Max(Math.Min(EntityBoundingVolume.colorIterator, 1.0f), 0.0f);
                EntityBoundingVolume.colorIteratorStep *= -1.0f;
            }

            gl.PushAttrib(8205u);
            gl.Disable(2896u);
            gl.Disable(3553u);
            gl.LineWidth(2f);
            gl.Color(1.0f - EntityBoundingVolume.colorIterator, 1.0f, EntityBoundingVolume.colorIterator);
            gl.PolygonMode(1032u, (renderMode == RenderMode.HitTest) ? 6914u : 6913u);
            gl.Begin(7u);
            gl.Vertex(hhl);
            gl.Vertex(lhl);
            gl.Vertex(lhh);
            gl.Vertex(hhh);
            gl.Vertex(hlh);
            gl.Vertex(llh);
            gl.Vertex(lll);
            gl.Vertex(hll);
            gl.Vertex(hhh);
            gl.Vertex(lhh);
            gl.Vertex(llh);
            gl.Vertex(hlh);
            gl.Vertex(hll);
            gl.Vertex(lll);
            gl.Vertex(lhl);
            gl.Vertex(hhl);
            gl.Vertex(lhh);
            gl.Vertex(lhl);
            gl.Vertex(lll);
            gl.Vertex(llh);
            gl.Vertex(hhl);
            gl.Vertex(hhh);
            gl.Vertex(hlh);
            gl.Vertex(hll);
            gl.End();
            gl.PopAttrib();
        }
    }
}