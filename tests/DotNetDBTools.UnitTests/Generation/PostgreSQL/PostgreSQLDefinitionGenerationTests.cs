using DotNetDBTools.Generation;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.UnitTests.Generation.Base;
using Xunit;

namespace DotNetDBTools.UnitTests.Generation.PostgreSQL;

public class PostgreSQLDefinitionGenerationTests : BaseDefinitionGenerationTests<PostgreSQLDatabase>
{
    protected override string SpecificDbmsSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.PostgreSQL";
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.PostgreSQL";

    [Fact]
    public void DbModelFromGeneratedSqlDefinition_IsEquivalentTo_DbModelFromOriginalDefinition()
    {
        DbModelFromGeneratedDefinition_IsEquivalentTo_DbModelFromOriginalDefinition_TestCase(
            SpecificDbmsSampleDbV1AssemblyName, OutputDefinitionKind.Sql);

        DbModelFromGeneratedDefinition_IsEquivalentTo_DbModelFromOriginalDefinition_TestCase(
            SpecificDbmsSampleDbV2AssemblyName, OutputDefinitionKind.Sql);
    }
}
