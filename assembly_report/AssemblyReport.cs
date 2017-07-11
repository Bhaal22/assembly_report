using System;
using System.Reflection;

namespace report
{
    class AssemblyReport
    {
        public static void Usage()
        {
            Environment.Exit(1);
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Usage();
            }

            var filename = args[0];

            try
            {
                var loadedAssembly = Assembly.LoadFile($@"{filename}");
                var references = loadedAssembly.GetReferencedAssemblies();

                Console.WriteLine($"Success {filename}");

                var assemblyInfo = loadedAssembly.GetName();
                Console.WriteLine($"platform: {assemblyInfo.ProcessorArchitecture}");

                foreach (var reference in references)
                {
                    Console.WriteLine($"{reference.Name} {reference.Version}");
                }
                Console.WriteLine($"Done {filename}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Fail {filename}: {ex.Message}");
            }
        }
    }
}
