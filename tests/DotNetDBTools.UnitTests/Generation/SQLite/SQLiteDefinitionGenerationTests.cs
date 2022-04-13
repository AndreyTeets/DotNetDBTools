using DotNetDBTools.Models.SQLite;
using DotNetDBTools.UnitTests.Generation.Base;

namespace DotNetDBTools.UnitTests.Generation.SQLite;

public class SQLiteDefinitionGenerationTests : BaseDefinitionGenerationTests<SQLiteDatabase>
{
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.SQLite";
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.SQLite";
}
