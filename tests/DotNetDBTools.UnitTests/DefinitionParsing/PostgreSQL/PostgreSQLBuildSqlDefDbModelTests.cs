using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.UnitTests.DefinitionParsing.Base;

namespace DotNetDBTools.UnitTests.DefinitionParsing.PostgreSQL;

public class PostgreSQLBuildSqlDefDbModelTests : BaseBuildSqlDefDbModelTests<PostgreSQLDatabase>
{
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.PostgreSQL";
    protected override string SpecificDbmsSampleDbV2SqlDefAssemblyName => "DotNetDBTools.SampleDBv2SqlDef.PostgreSQL";
}
