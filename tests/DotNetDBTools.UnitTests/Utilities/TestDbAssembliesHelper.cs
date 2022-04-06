using System.Reflection;

namespace DotNetDBTools.UnitTests.Utilities;

public static class TestDbAssembliesHelper
{
    public static Assembly GetSampleDbAssembly(string projectName)
    {
        string projectDir = $@"../../../../../samples/Databases/{projectName}";
        return TestDatabasesCompiler.CompileSampleDbProject(projectDir);
    }
}
