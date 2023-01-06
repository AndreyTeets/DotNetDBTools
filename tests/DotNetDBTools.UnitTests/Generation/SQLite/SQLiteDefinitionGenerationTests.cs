using DotNetDBTools.Generation;
using DotNetDBTools.Models.SQLite;
using DotNetDBTools.UnitTests.Generation.Base;
using Xunit;

namespace DotNetDBTools.UnitTests.Generation.SQLite;

public class SQLiteDefinitionGenerationTests : BaseDefinitionGenerationTests<SQLiteDatabase>
{
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.SQLite";
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.SQLite";

    [Fact]
    public void DbModelFromGeneratedSqlDefinition_IsEquivalentTo_DbModelFromOriginalDefinition()
    {
        DbModelFromGeneratedDefinition_IsEquivalentTo_DbModelFromOriginalDefinition_TestCase(
            SpecificDbmsSampleDbV1AssemblyName, OutputDefinitionKind.Sql);

        DbModelFromGeneratedDefinition_IsEquivalentTo_DbModelFromOriginalDefinition_TestCase(
            SpecificDbmsSampleDbV2AssemblyName, OutputDefinitionKind.Sql);
    }
}
