using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.UnitTests.Generation.Base;

namespace DotNetDBTools.UnitTests.Generation.MSSQL;

public class MSSQLDefinitionGenerationTests : BaseDefinitionGenerationTests<MSSQLDatabase>
{
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.MSSQL";
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.MSSQL";
}
