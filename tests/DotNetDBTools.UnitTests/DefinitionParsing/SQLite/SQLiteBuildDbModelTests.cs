using DotNetDBTools.Models.SQLite;
using DotNetDBTools.UnitTests.DefinitionParsing.Base;

namespace DotNetDBTools.UnitTests.DefinitionParsing.SQLite;

public class SQLiteBuildDbModelTests : BaseBuildDbModelTests<SQLiteDatabase>
{
    protected override string ExpectedFilesDir => "./TestData/SQLite";
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.SQLite";
}
