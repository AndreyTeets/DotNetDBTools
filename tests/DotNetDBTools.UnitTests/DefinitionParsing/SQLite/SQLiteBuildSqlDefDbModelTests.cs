using DotNetDBTools.Models.SQLite;
using DotNetDBTools.UnitTests.DefinitionParsing.Base;

namespace DotNetDBTools.UnitTests.DefinitionParsing.SQLite;

public class SQLiteBuildSqlDefDbModelTests : BaseBuildSqlDefDbModelTests<SQLiteDatabase>
{
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.SQLite";
    protected override string SpecificDbmsSampleDbV2SqlDefAssemblyName => "DotNetDBTools.SampleDBv2SqlDef.SQLite";
}
