using System;
using System.Linq;
using System.Reflection;

namespace DotNetDBTools.UnitTests.Utilities
{
    public static class TestDbAssembliesHelper
    {
        public static Assembly GetLoadedAssemblyByName(string assemblyName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Single(x => x.GetName().Name == assemblyName);
        }
    }
}
