using DotNetDBTools.Models.MySQL;
using DotNetDBTools.UnitTests.DefinitionParsing.Base;

namespace DotNetDBTools.UnitTests.DefinitionParsing.MySQL;

public class MySQLBuildDbModelTests : BaseBuildDbModelTests<MySQLDatabase>
{
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.MySQL";
    protected override string ExpectedFilesDir => "./TestData/MySQL";
}
