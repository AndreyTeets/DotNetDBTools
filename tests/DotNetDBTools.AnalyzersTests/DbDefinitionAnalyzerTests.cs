using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using Verify = DotNetDBTools.AnalyzersTests.DbDefinitionAnalyzerVerifier<DotNetDBTools.DefinitionAnalyzer.DbDefinitionAnalyzer>;

namespace DotNetDBTools.AnalyzersTests
{
    public class DbDefinitionAnalyzerTests
    {
        [Fact]
        public async Task DbDefinitionAnalyzer_DoesntReportOnGoodDbCode_And_ReportsErrorOnBadDbCode()
        {
            string goodDbCode =
@"using System;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.Definition.Agnostic.DbTypes;

namespace SampleTestCode
{
    public class TestTable2 : ITable
    {
        public Guid ID => new(""BFB9030C-A8C3-4882-9C42-1C6AD025CF8F"");

        public Column TestColumn1 = new(""C480F22F-7C01-4F41-B282-35E9F5CD1FE3"")
        {
            Type = new IntDbType(),
            Nullable = false,
        };
    }

    public class TestTable1 : ITable
    {
        public Guid ID => new(""BFB9030C-A8C3-4882-9C42-1C6AD025CF8F"");

        public Column TestColumn1 = new(""C480F22F-7C01-4F41-B282-35E9F5CD1FE3"")
        {
            Type = new IntDbType(),
            Nullable = false,
        };

        public ForeignKey {|#0:FK_TestName1|} = new()
        {
            ThisColumns = new string[] { nameof(TestColumn1) },
            ForeignTable = nameof(TestTable2),
            ForeignColumns = new string[] { nameof(TestTable2.TestColumn1) },
            OnUpdate = ForeignKeyActions.NoAction,
            OnDelete = ForeignKeyActions.Cascade,
        };
    }
}
";

            await Verify.VerifyAnalyzerAsync(goodDbCode);

            string badDbCode = goodDbCode.Replace(
                "ForeignTable = nameof(TestTable2),",
                "ForeignTable = \"NonExistentTableName\",");

            string expectedErrorMessage =
"Couldn't find table 'NonExistentTableName' referenced by foreign key 'FK_TestName1' in table 'TestTable1'";

            DiagnosticResult expected = DiagnosticResult
                .CompilerError("DNDBT_DA_01")
                .WithArguments(expectedErrorMessage)
                .WithLocation(0);
            await Verify.VerifyAnalyzerAsync(badDbCode, expected);
        }
    }
}
