namespace Tex2Pak
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using Obj2Nya;
    using PawCraft.Utils;

    /// <summary>
    /// Main program class
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main program entry
        /// </summary>
        /// <param name="args">
        /// Program agrs
        /// <para>First is path to the folder with png/bmp/tga textures, second is path to the pak file</para>
        /// </param>
        private static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                if (Directory.Exists(args[0]))
                {
                    List<NyaTexture> textures = new List<NyaTexture>();

                    foreach (string file in Directory.GetFiles(args[0], "*.tga").Concat(Directory.GetFiles(args[0], "*.bmp")).Concat(Directory.GetFiles(args[0], "*.png")).OrderBy(file => file))
                    {
                        using (Bitmap bitmap = Helpers.GetBitmap(file))
                        {
                            textures.Add(new NyaTexture(Path.GetFileNameWithoutExtension(file), bitmap));
                        }
                    }

                    using (FileStream stream = File.Create(args[1]))
                    {
                        List<byte> bytes = new List<byte>();

                        foreach (NyaTexture texture in textures)
                        {
                            bytes.AddRange(PawCraft.Utils.Serializer.CustomMarshal.MarshalAsBytes(texture));
                        }

                        stream.Write(bytes.ToArray(), 0, bytes.Count);
                    }
                }
            }
        }
    }
}