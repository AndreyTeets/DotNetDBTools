using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.UnitTests.DefinitionParsing.Base;

namespace DotNetDBTools.UnitTests.DefinitionParsing.MSSQL;

public class MSSQLBuildDbModelTests : BaseBuildDbModelTests<MSSQLDatabase>
{
    protected override string ExpectedFilesDir => "./TestData/MSSQL/SampleDbRelated";
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.MSSQL";
}
