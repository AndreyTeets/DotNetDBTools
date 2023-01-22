using DotNetDBTools.UnitTests.Generation.Base;

namespace DotNetDBTools.UnitTests.Generation.MSSQL;

public class MSSQLDefinitionGenerationTests : BaseDefinitionGenerationTests
{
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.MSSQL";
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.MSSQL";
}
