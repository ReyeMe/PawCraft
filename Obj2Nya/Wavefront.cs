namespace Obj2Nya
{
    using Obj2Nya;
    using PawCraft.Utils.Types;
    using SharpGL;
    using SharpGL.Version;
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using SharpGL.SceneGraph.Assets;
    using SharpGL.SceneGraph.Lighting;
    using PawCraft.Utils;
    using SharpGL.SceneGraph;

    public static class Wavefront
    {
        /// <summary>
        /// Import from file
        /// </summary>
        /// <param name="inputFile">File path</param>
        /// <returns>Imported <see cref="Group"/></returns>
        public static NyaGroup Import(string inputFile)
        {
            List<FxVector> vertices = new List<FxVector>();
            List<FxVector> normals = new List<FxVector>();
            List<NyaMesh> models = new List<NyaMesh>();
            List<float[]> texCoords = new List<float[]>();
            List<Tuple<string, NyaTexture>> textures = new List<Tuple<string, NyaTexture>>();
            string lastMaterial = string.Empty;

            Dictionary<string, object> materials = Wavefront.ReadMtl(inputFile);

            foreach (string line in File.ReadLines(inputFile).Where(line => !line.StartsWith("#") && !line.StartsWith("vp") && !line.StartsWith("l") && line.Contains(' ')))
            {
                string lineCode = line.Substring(0, line.IndexOf(' ')).Trim();

                switch (lineCode)
                {
                    case "o":
                        models.Add(new NyaMesh());
                        break;

                    case "usemtl":
                        lastMaterial = line.Substring(6).Trim();
                        break;

                    case "v":
                        vertices.Add(Wavefront.ParseVertex(line));
                        break;

                    case "vn":
                        normals.Add(Wavefront.ParseVertex(line));
                        break;

                    case "vt":
                        texCoords.Add(Wavefront.ParseTex(line));
                        break;

                    case "f":

                        if (!models.Any())
                        {
                            models.Add(new NyaMesh());
                        }

                        Tuple<List<int>, List<int>, List<int>> face = Wavefront.ParseFace(line);
                        object material = materials.ContainsKey(lastMaterial) ? materials[lastMaterial] : null;
                        bool doNotSetFlag = false;
                        NyaFaceFlags faceFlags = new NyaFaceFlags();

                        if (material is string && textures.Any(existing => existing.Item1 == (string)material))
                        {
                            int index = textures.FindIndex(existing => existing.Item1 == (string)material);
                            material = null;
                            doNotSetFlag = true;
                            faceFlags.TextureId = index;
                            faceFlags.HasTexture = true;
                        }

                        NyaTexture texture = Wavefront.AddFaceWithVertices(face, vertices, normals, texCoords, material, models.Last());
                        faceFlags.HasMeshEffect = lastMaterial.EndsWith("_M");

                        if (!doNotSetFlag)
                        {
                            if (texture != null)
                            {
                                textures.Add(new Tuple<string, NyaTexture>((string)material, texture));
                                faceFlags.TextureId = textures.Count - 1;
                                faceFlags.HasTexture = true;
                            }
                            else
                            {
                                faceFlags.BaseColor = (PawCraft.Utils.Types.Color)materials[lastMaterial];
                            }
                        }

                        models.Last().FaceFlags = models.Last().FaceFlags.Concat(new[] { faceFlags }).ToArray();
                        models.Last().PolygonCount = models.Last().Polygons.Length;
                        models.Last().PointCount = models.Last().Points.Length;

                        break;

                    default:
                        break;
                }
            }

            // Return group
            return new NyaGroup 
            {
                MeshCount = models.Count,
                TextureCount = textures.Count,
                Meshes = models.ToArray(),
                Textures = textures.Select(texture => texture.Item2).ToArray()
            };
        }

        /// <summary>
        /// Add face to the mesh
        /// </summary>
        /// <param name="face">Face to add</param>
        /// <param name="vertices">Global vertices</param>
        /// <param name="normals">Global normals</param>
        /// <param name="mesh">Mesh instance</param>
        private static NyaTexture AddFaceWithVertices(Tuple<List<int>, List<int>, List<int>> face, List<FxVector> vertices, List<FxVector> normals, List<float[]> texCoords, object material, NyaMesh mesh)
        {
            List<int> points = face.Item2;
            List<int> norms = face.Item1;
            List<int> tex = face.Item3;
            NyaTexture texture = null;
            Vertex normal = new Vertex();

            foreach (int normalPoint in norms)
            {
                normal += FxVector.ToVertex(normals[normalPoint]);
            }

            normal /= (float)norms.Count;
            normal.Normalize();

            NyaPolygon polygon = new NyaPolygon();
            polygon.Normal = FxVector.FromVertex(normal);

            List<short> facePoints = new List<short>();

            foreach (int facePoint in points)
            {
                FxVector point = vertices[facePoint];
                int index = mesh.Points.ToList().FindIndex(meshPoint => meshPoint.X == point.X && meshPoint.Y == point.Y && meshPoint.Z == point.Z);
                
                if (index < 0)
                {
                    mesh.Points = mesh.Points.Concat(new[] { point }).ToArray();
                    index = mesh.Points.Length - 1;
                }

                facePoints.Add((short)index);
            }

            if (facePoints.Count < 3 || facePoints.Count > 4)
            {
                throw new NotSupportedException("Only quads or triangless are supported");
            }

            if (facePoints.Count != 4)
            {
                facePoints.Add(facePoints.Last());
            }

            polygon.Vertices = facePoints.ToArray();
            mesh.Polygons = mesh.Polygons.Concat(new[] { polygon }).ToArray();

            // Generate texture if any
            if (material != null && material is string)
            {
                // TODO: Add UV unwrap
                using (Bitmap result = GlTexture.GetBitmap((string)material))
                {
                    texture = new NyaTexture(result);
                }
            }

            return texture;
        }

        /// <summary>
        /// Get absolute path to the texture file
        /// </summary>
        /// <param name="mtlPath">MTL texture path</param>
        /// <param name="modelFolder">Model folder path</param>
        /// <returns>Absolute texture file path</returns>
        private static string GetAbsoluteTexturePath(string mtlPath, string modelFolder)
        {
            try
            {
                if (File.Exists(mtlPath))
                {
                    return mtlPath.ToLower();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            if (!string.IsNullOrWhiteSpace(mtlPath))
            {
                return Path.Combine(modelFolder, mtlPath).ToLower();
            }

            return null;
        }

        /// <summary>
        /// parse color
        /// </summary>
        /// <param name="line">Diffuse color</param>
        /// <returns>Solid color</returns>
        private static PawCraft.Utils.Types.Color ParseColor(string line)
        {
            float[] color = Wavefront.ParseVertexFloat(line);
            return PawCraft.Utils.Types.Color.FromRgb((byte)(byte.MaxValue * color[0]), (byte)(byte.MaxValue * color[1]), (byte)(byte.MaxValue * color[2]));
        }

        /// <summary>
        /// Parse face line
        /// </summary>
        /// <param name="line">Face line</param>
        /// <returns>Parsed face</returns>
        private static Tuple<List<int>, List<int>, List<int>> ParseFace(string line)
        {
            List<int> vertices = new List<int>();
            List<int> normals = new List<int>();
            List<int> tex = new List<int>();

            foreach (string vertex in line.Substring(1).Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] components = vertex.Split(new[] { '/' }, StringSplitOptions.None);

                if (components.Any())
                {
                    int temp;

                    if (int.TryParse(components.First(), out temp))
                    {
                        vertices.Add(temp - 1);
                    }

                    if (components.Length > 1 && int.TryParse(components[1], out temp))
                    {
                        tex.Add(temp - 1);
                    }

                    if (components.Length == 3 && int.TryParse(components.Last(), out temp))
                    {
                        normals.Add(temp - 1);
                    }
                }
            }

            return new Tuple<List<int>, List<int>, List<int>>(normals, vertices, tex);
        }

        /// <summary>
        /// Parse vertex
        /// </summary>
        /// <param name="line">Vertex line</param>
        /// <returns>Parsed vertex</returns>
        private static float[] ParseVertexFloat(string line)
        {
            List<float> coordinates = line
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Take(3)
                .Select(coordinate =>
                {
                    float value = 0.0f;
                    float.TryParse(coordinate, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out value);
                    return value;
                })
                .ToList();

            return coordinates.ToArray();
        }

        /// <summary>
        /// Parse vertex
        /// </summary>
        /// <param name="line">Vertex line</param>
        /// <returns>Parsed vertex</returns>
        private static FxVector ParseVertex(string line)
        {
            return FxVector.FromArray(Wavefront.ParseVertexFloat(line));
        }

        /// <summary>
        /// Parse vertex
        /// </summary>
        /// <param name="line">Vertex line</param>
        /// <returns>Parsed vertex</returns>
        private static float[] ParseTex(string line)
        {
            float[] vt = line
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Take(2)
                .Select(coordinate =>
                {
                    float value = 0.0f;
                    float.TryParse(coordinate, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out value);
                    return value;
                })
                .ToArray();

            if (vt.Length == 1)
            {
                return new[] { vt[0], 0.0f }; 
            }

            return vt;
        }

        /// <summary>
        /// Read texture definition file
        /// </summary>
        /// <param name="models">Loaded models</param>
        /// <param name="waveFrontFile">Path to the WaveFront file</param>
        private static Dictionary<string, object> ReadMtl(string waveFrontFile)
        {
            Dictionary<string, object> materials = new Dictionary<string, object>();
            string modelDirectory = Path.GetDirectoryName(waveFrontFile);

            if (string.IsNullOrEmpty(modelDirectory))
            {
                return null;
            }

            string mtlFile = Path.Combine(modelDirectory, Path.GetFileNameWithoutExtension(waveFrontFile) + ".mtl");
            string lastMaterial = string.Empty;

            if (File.Exists(mtlFile))
            {
                foreach (string line in File.ReadLines(mtlFile).Where(line => !string.IsNullOrEmpty(line) && line.Contains(" ")))
                {
                    string lineCode = line.Substring(0, line.IndexOf(' ')).Trim();

                    switch (lineCode.ToLower())
                    {
                        case "newmtl":
                            lastMaterial = line.Replace(lineCode, string.Empty).Trim();
                            materials.Add(lastMaterial, null);
                            break;

                        case "kd":

                            if (materials[lastMaterial] == null)
                            {
                                materials[lastMaterial] = Wavefront.ParseColor(line);
                            }

                            break;

                        case "map_kd":
                            string file = Wavefront.GetAbsoluteTexturePath(line.Replace(lineCode, string.Empty).Trim(), modelDirectory);

                            if (!string.IsNullOrWhiteSpace(file) && File.Exists(file))
                            {
                                materials[lastMaterial] = file;
                            }

                            break;

                        default:
                            break;
                    }
                }
            }

            return materials;
        }
    }
}