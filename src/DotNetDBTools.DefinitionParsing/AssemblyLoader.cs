using System.IO;
using System.Reflection;

namespace DotNetDBTools.DefinitionParsing;

public static class AssemblyLoader
{
    public static Assembly LoadDbAssemblyFromDll(string dbAssemblyPath)
    {
        string fullDbAssemblyPath = Path.GetFullPath(dbAssemblyPath);
        byte[] assemblyBytes = File.ReadAllBytes(fullDbAssemblyPath);
        Assembly dbAssembly = Assembly.Load(assemblyBytes);
        return dbAssembly;
    }
}
