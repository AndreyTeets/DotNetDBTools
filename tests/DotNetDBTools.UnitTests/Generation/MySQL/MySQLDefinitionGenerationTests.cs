using DotNetDBTools.UnitTests.Generation.Base;

namespace DotNetDBTools.UnitTests.Generation.MySQL;

public class MySQLDefinitionGenerationTests : BaseDefinitionGenerationTests
{
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.MySQL";
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.MySQL";
}
