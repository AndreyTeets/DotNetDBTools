using DotNetDBTools.Models.SQLite;
using DotNetDBTools.UnitTests.Generation.Base;

namespace DotNetDBTools.UnitTests.Generation.SQLite;

public class SQLiteDefinitionGenerationTests : BaseDefinitionGenerationTests<SQLiteDatabase>
{
    protected override string SampleDbV1CSharpAssemblyName => "DotNetDBTools.SampleDB.SQLite";
    protected override string GeneratedFilesDir => "./generated/SQLite";
}
