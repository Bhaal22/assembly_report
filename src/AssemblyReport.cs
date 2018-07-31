using asssembly_report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var checkReferences = false;
            if (args.Length < 2)
            {
                Usage();
            }

            var exclude = string.Empty;
            if (args.Length == 3)
                exclude = args[2];

            var filename__ = args[0];

            var filenames = new List<string>();
            var excludeFileNames = new List<string>();

            if (File.Exists(filename__))
            {
                filenames.Add(filename__);
            }
            else
            {
                filenames.AddRange(Directory.GetFiles(".", filename__));
                excludeFileNames.AddRange(Directory.GetFiles(".", exclude));

                filenames = filenames.Except(excludeFileNames).ToList();
            }

            var assemblyDetails = new List<AssemblyDetail>();

            foreach (var filename in filenames)
            {
                var file = new FileInfo(filename);
                Console.WriteLine($"Processing ... {filename}");
                try
                {
                    var loadedAssembly = Assembly.LoadFile($@"{file.FullName}");
                    var references = AssemblyReporter.GetReferences(loadedAssembly);

                    var assemblyName = loadedAssembly.GetName();

                    var assemblyDetail = new AssemblyDetail()
                    {
                        Name = file.Name,
                        Version = assemblyName.Version.ToString(),
                        Platform = assemblyName.ProcessorArchitecture.ToString()
                    };

                    assemblyDetails.Add(assemblyDetail);

                    if (checkReferences)
                    {
                        foreach (var reference in references)
                        {
                            Console.Write("  |---");
                            Console.WriteLine($"{reference.Name} {reference.Version}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fail {filename}: {ex.Message}");
                }
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(assemblyDetails);

            File.WriteAllText(args[1], json);

        }
    }
}
