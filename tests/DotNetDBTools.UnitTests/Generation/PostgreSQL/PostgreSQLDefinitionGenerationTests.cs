using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.UnitTests.Generation.Base;

namespace DotNetDBTools.UnitTests.Generation.PostgreSQL;

public class PostgreSQLDefinitionGenerationTests : BaseDefinitionGenerationTests<PostgreSQLDatabase>
{
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.PostgreSQL";
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.PostgreSQL";
}
