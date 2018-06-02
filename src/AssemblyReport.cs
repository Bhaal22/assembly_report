using System;
using System.IO;
using System.Reflection;

namespace report
{
    class AssemblyReport
    { 
        public static void Usage()
        {
            Console.WriteLine("Usage: specify flename to dissect dependencies");
            Environment.Exit(1);
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Usage();
            }

            var filename = args[0];
            var file = new FileInfo(filename);

            try
            {
                var loadedAssembly = Assembly.LoadFile($@"{file.FullName}");
                var references = AssemblyReporter.GetReferences(loadedAssembly);

                var plop = AssemblyReporter.GetVersion(loadedAssembly);

                var assemblyInfo = loadedAssembly.GetName();

                Console.WriteLine("Name\tProc. Archi.\tVersion");
                Console.WriteLine($"{file.Name}\t{assemblyInfo.ProcessorArchitecture}\t{assemblyInfo.Version}");

                foreach (var reference in references)
                {
                    Console.Write("  |---");
                    Console.WriteLine($"{reference.Name} {reference.Version}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Fail {filename}: {ex.Message}");
            }
        }
    }
}
