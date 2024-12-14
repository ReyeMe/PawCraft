namespace Obj2Nya
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Main program class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Program entry
        /// </summary>
        /// <param name="args">
        /// Program arguments
        /// <para>First is path to the obj file, second is path to nya file</para>
        /// </param>
        private static void Main(string[] args)
        {
            Console.ReadLine();
            if (args.Length >= 2)
            {
                bool hasFlag = args.Last().StartsWith("/");
                string[] inputFiles = args.Take(args.Length - (hasFlag ? 2 : 1)).ToArray();
                string target = args[args.Length - (hasFlag ? 2 : 1)];
                Console.WriteLine("sources: " + string.Join(", ", inputFiles));
                Console.WriteLine("target: " + target);

                if (inputFiles.All(file => File.Exists(file)))
                {
                    object result = null;

                    if (hasFlag && args.Last(file => file.StartsWith("/")) == "/s")
                    {
                        result = Program.MergeSmoothGroup(inputFiles.Select(file => WavefrontSmooth.Import(file)));
                    }
                    else
                    {
                        result = Program.MergeGroup(inputFiles.Select(file => Wavefront.Import(file)));
                    }

                    if (result != null)
                    {
                        File.WriteAllBytes(target, PawCraft.Utils.Serializer.CustomMarshal.MarshalAsBytes(result));
                        Console.WriteLine("Done.");
                    }
                    else
                    {
                        Console.WriteLine("Failed!");
                        Environment.ExitCode = 3;
                    }
                }
                else
                {
                    Console.WriteLine("Source not found!");
                    Environment.ExitCode = 2;
                }
            }
            else
            {
                Console.WriteLine("Source and target path reuqired!");
                Environment.ExitCode = 1;
            }
        }

        /// <summary>
        /// Merge mesh groups together
        /// </summary>
        /// <param name="group">All groups</param>
        /// <returns>Merged groups</returns>
        private static NyaGroup MergeGroup(IEnumerable<NyaGroup> group)
        {
            NyaGroup result = group.First();

            foreach (NyaGroup toMerge in group.Skip(1))
            {
                // Merge textures
                result.Textures = result.Textures.Concat(toMerge.Textures.Where(texture => !result.Textures.Any(has => has.Name == texture.Name))).ToArray();
                result.TextureCount = result.Textures.Length;

                // Merge meshes
                foreach (NyaMesh mesh in toMerge.Meshes)
                {
                    foreach (NyaFaceFlags flag in mesh.FaceFlags.Where(flag => flag.HasTexture))
                    {
                        flag.TextureId = result.Textures.ToList().FindIndex(texture => texture.Name == toMerge.Textures[flag.TextureId].Name);
                    }

                    result.Meshes = result.Meshes.Concat(new[] { mesh }).ToArray();
                }

                result.MeshCount = result.Meshes.Length;
            }

            return result;
        }

        /// <summary>
        /// Merge smooth mesh groups together
        /// </summary>
        /// <param name="group">All groups</param>
        /// <returns>Merged groups</returns>
        private static NyaSmoothGroup MergeSmoothGroup(IEnumerable<NyaSmoothGroup> group)
        {
            NyaSmoothGroup result = group.First();

            foreach (NyaSmoothGroup toMerge in group.Skip(1))
            {
                // Merge textures
                result.Textures = result.Textures.Concat(toMerge.Textures.Where(texture => !result.Textures.Any(has => has.Name == texture.Name))).ToArray();
                result.TextureCount = result.Textures.Length;

                // Merge meshes
                foreach (NyaSmoothMesh mesh in toMerge.Meshes)
                {
                    foreach (NyaFaceFlags flag in mesh.FaceFlags.Where(flag => flag.HasTexture))
                    {
                        flag.TextureId = result.Textures.ToList().FindIndex(texture => texture.Name == toMerge.Textures[flag.TextureId].Name);
                    }

                    result.Meshes = result.Meshes.Concat(new[] { mesh }).ToArray();
                }

                result.MeshCount = result.Meshes.Length;
            }

            return result;
        }
    }
}