using DotNetDBTools.Models.MySQL;
using DotNetDBTools.UnitTests.DefinitionParsing.Base;

namespace DotNetDBTools.UnitTests.DefinitionParsing.MySQL;

public class MySQLBuildDbModelTests : BaseBuildDbModelTests<MySQLDatabase>
{
    protected override string ExpectedFilesDir => "./TestData/MySQL/SampleDbRelated";
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.MySQL";
}
