using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.UnitTests.DefinitionParsing.Base;

namespace DotNetDBTools.UnitTests.DefinitionParsing.PostgreSQL;

public class PostgreSQLBuildDbModelTests : BaseBuildDbModelTests<PostgreSQLDatabase>
{
    protected override string ExpectedFilesDir => "./TestData/PostgreSQL/SampleDbRelated";
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.PostgreSQL";
}
