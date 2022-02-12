using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using Verify = DotNetDBTools.AnalyzersTests.DbDefinitionAnalyzerVerifier<DotNetDBTools.DefinitionAnalyzer.DbDefinitionAnalyzer>;

namespace DotNetDBTools.AnalyzersTests;

public class DbDefinitionAnalyzerTests
{
    [Fact]
    public async Task DbDefinitionAnalyzer_DoesntReportOnGoodDbCode_And_ReportsErrorOnBadDbCode()
    {
        string goodDbCode =
@"using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;

namespace SampleTestCode
{
    public class TestUserDefinedType1 : IUserDefinedType
    {
        public Guid ID => new(""AF622311-F791-4DC3-995D-D03C75236A1F"");
        public IDataType UnderlyingType => new StringDataType() { Length = 100 };
        public bool Nullable => true;
    }

    public class TestTable2 : ITable
    {
        public Guid ID => new(""31347B68-6BE3-4F2B-BCD1-0CC82ECB97FA"");

        public Column TestColumn1 = new(""032611EB-AA7C-4FEA-8B3C-BB8609E140F6"")
        {
            DataType = new IntDataType(),
            Nullable = false,
            Default = 145,
            DefaultConstraintName = ""DF_TestTable2_TestColumn1"",
        };

        public Column TestColumn2 = new(""2FD848D5-A689-419F-9214-AFCF9F742564"")
        {
            DataType = new TestUserDefinedType1(),
            Nullable = false,
        };
    }

    public class TestTable1 : ITable
    {
        public Guid ID => new(""250542BA-31D6-49C5-9178-F86319343109"");

        public Column TestColumn1 = new(""0C322A6A-7D5F-418C-BC5E-DF120D16CF4C"")
        {
            DataType = new IntDataType(),
            Nullable = false,
        };

        public ForeignKey {|#0:FK_TestName1|} = new(""1955F440-4333-4F33-9755-1C618816C9FB"")
        {
            ThisColumns = new string[] { nameof(TestColumn1) },
            ReferencedTable = nameof(TestTable2),
            ReferencedTableColumns = new string[] { nameof(TestTable2.TestColumn1) },
            OnUpdate = ForeignKeyActions.NoAction,
            OnDelete = ForeignKeyActions.Cascade,
        };
    }
}
";

        await Verify.VerifyAnalyzerAsync(goodDbCode);

        string badDbCode = goodDbCode.Replace(
            "ReferencedTable = nameof(TestTable2),",
            "ReferencedTable = \"NonExistentTableName\",");

        string expectedErrorMessage =
"Couldn't find table 'NonExistentTableName' referenced by foreign key 'FK_TestName1' in table 'TestTable1'";

        DiagnosticResult expected = DiagnosticResult
            .CompilerError("DNDBT_DA_01")
            .WithArguments(expectedErrorMessage)
            .WithLocation(0);
        await Verify.VerifyAnalyzerAsync(badDbCode, expected);
    }
}
