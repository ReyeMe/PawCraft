namespace PawCraft
{
    using SharpGL.SceneGraph;
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows;
    using System.Windows.Media.Media3D;

    /// <summary>
    /// Utility stuff
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Angle to degrees
        /// </summary>
        /// <param name="angle">Angle in radians</param>
        /// <returns>Angle in degrees</returns>
        public static float ConvertToDegrees(this float angle)
        {
            return (180.0f / (float)Math.PI) * angle;
        }

        /// <summary>
        /// Angle to radians
        /// </summary>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>Angle in radians</returns>
        public static float ConvertToRadians(this float angle)
        {
            return ((float)Math.PI / 180.0f) * angle;
        }

        /// <summary>
        /// Check whether point is in frustum
        /// </summary>
        /// <param name="gl">OpenGL instance</param>
        /// <param name="vertex">Vertex to check</param>
        /// <returns>True if in frustum</returns>
        public static bool IsInFrustum(this SharpGL.OpenGL gl, Vertex vertex)
        {
            Matrix modelView = gl.GetModelViewMatrix();
            modelView.Transpose();
            Matrix frustum = gl.GetProjectionMatrix() * modelView;
            int furstumIndex = 0;

            for (int planeIndex = 0; planeIndex < 6; planeIndex++)
            {
                double[] plane = new double[4];

                if (planeIndex % 2 == 0)
                {
                    furstumIndex++;
                }

                for (int component = 0; component < 4; component++)
                {
                    if (planeIndex % 2 == 0)
                    {
                        plane[component] = frustum[3, component] + frustum[furstumIndex, component];
                    }
                    else
                    {
                        plane[component] = frustum[3, component] - frustum[furstumIndex, component];
                    }
                }

                double distance = vertex.ScalarProduct(new Vertex((float)plane[0], (float)plane[1], (float)plane[2])) + plane[3];

                if (distance < 0.0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Get angle between vectors
        /// </summary>
        /// <param name="vector">First vector</param>
        /// <returns>The angle</returns>
        public static float GetAngleTo(this Vertex vector, Vertex other)
        {
            Vertex cross = vector.VectorProduct(other);
            float dot = vector.ScalarProduct(other);
            return (float)Math.PI - (float)Math.Atan2(cross.Magnitude(), dot);
        }

        /// <summary>
        /// Rotate around vector
        /// </summary>
        /// <param name="vector">Vector to rotate</param>
        /// <param name="angle">Rotate by this angle</param>
        /// <param name="direction">Rotation axis</param>
        /// <returns>Rotated vector</returns>
        public static Vertex RotateAroundAxis(this Vertex vector, float angle, Vertex direction)
        {
            // Get radius of the rotation circle
            float radius = (float)vector.Magnitude();

            if (radius != 0)
            {
                // Normal vector must be unit vector
                Vertex axisNormal = new Vertex();
                axisNormal.X = vector.X / radius;
                axisNormal.Y = vector.Y / radius;
                axisNormal.Z = vector.Z / radius;

                // Get cross vector between normal and direction of axis
                Vertex cross = axisNormal.VectorProduct(direction);
                cross.Normalize();

                // Get cos and sin
                float cos = (float)Math.Cos(angle);
                float sin = (float)Math.Sin(angle);

                // Get rotated normal
                Vertex rotatedNormal = new Vertex();
                rotatedNormal.X = (axisNormal.X * cos) + (cross.X * sin);
                rotatedNormal.Y = (axisNormal.Y * cos) + (cross.Y * sin);
                rotatedNormal.Z = (axisNormal.Z * cos) + (cross.Z * sin);

                // Rotated normal must be normalized before it can be used, to prevent drifting
                rotatedNormal.Normalize();

                // Resize rotated vector to the same size as it was before rotated
                Vertex result = new Vertex();
                result.X = rotatedNormal.X * radius;
                result.Y = rotatedNormal.Y * radius;
                result.Z = rotatedNormal.Z * radius;
                return result;
            }
            else
            {
                // If radius (vector length) is 0, point cannot be rotated
                return vector;
            }
        }

        /// <summary>
        /// Snap vertex to plane
        /// </summary>
        /// <param name="vertex">Vertex to snap</param>
        /// <param name="planeOrigin">Plane origin</param>
        /// <param name="planeNormal">Plane normal</param>
        /// <returns>Spapped vertex</returns>
        public static Vertex SnapVertexToPlane(this Vertex vertex, Vertex planeOrigin, Vertex planeNormal)
        {
            Vertex dotVector = vertex - planeOrigin;
            float dot = planeNormal.ScalarProduct(dotVector);
            return vertex - (planeNormal * dot);
        }

        /// <summary>
        /// Conver vertex to point
        /// </summary>
        /// <param name="vertex">OpenGL vertex</param>
        /// <returns>Media point</returns>
        public static Point3D ToPoint3D(this Vertex vertex)
        {
            return new Point3D(vertex.X, vertex.Y, vertex.Z);
        }

        /// <summary>
        /// Conver vertex to vector
        /// </summary>
        /// <param name="vertex">OpenGL vertex</param>
        /// <returns>Media vector</returns>
        public static Vector3D ToVector(this Vertex vertex)
        {
            return new Vector3D(vertex.X, vertex.Y, vertex.Z);
        }

        /// <summary>
        /// Conver point to vertex
        /// </summary>
        /// <param name="point">Media point</param>
        /// <returns>OpenGL vertex</returns>
        public static Vertex ToVertex(this Point3D point)
        {
            return new Vertex((float)point.X, (float)point.Y, (float)point.Z);
        }

        /// <summary>
        /// Conver vector to vertex
        /// </summary>
        /// <param name="point">Media vector</param>
        /// <returns>OpenGL vertex</returns>
        public static Vertex ToVertex(this Vector3D point)
        {
            return new Vertex((float)point.X, (float)point.Y, (float)point.Z);
        }
    }
}