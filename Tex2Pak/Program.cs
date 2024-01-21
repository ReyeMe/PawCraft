namespace Tex2Pak
{
    using Obj2Nya;
    using PawCraft.Utils;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class Program
    {
        static void Main(string[] args)
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
                            textures.Add(new NyaTexture(bitmap));
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
