using System.Diagnostics;
using System.Reflection;

namespace report
{
    public class AssemblyReporter
    {
        public static AssemblyName[] GetReferences(Assembly assembly)
        {
            return assembly.GetReferencedAssemblies();
        }

        public static string GetVersion(Assembly assembly)
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return versionInfo.ProductVersion;
        }
    }
}
