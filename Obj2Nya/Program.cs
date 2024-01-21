namespace Obj2Nya
{
    using System;
    using System.IO;

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
            if (args.Length == 2)
            {
                Console.WriteLine("source: " + args[0]);
                Console.WriteLine("target: " + args[1]);

                if (File.Exists(args[0]))
                {
                    NyaGroup group = Wavefront.Import(args[0]);
                    File.WriteAllBytes(args[1], PawCraft.Utils.Serializer.CustomMarshal.MarshalAsBytes(group));
                    Console.WriteLine("Done.");
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
    }
}