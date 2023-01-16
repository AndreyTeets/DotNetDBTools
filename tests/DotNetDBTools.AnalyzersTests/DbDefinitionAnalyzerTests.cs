using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using Verify = DotNetDBTools.AnalyzersTests.DbDefinitionAnalyzerVerifier<DotNetDBTools.DefinitionAnalyzer.DbDefinitionAnalyzer>;

namespace DotNetDBTools.AnalyzersTests;

public class DbDefinitionAnalyzerTests
{
    [Fact]
    public async Task DbDefinitionAnalyzer_DoesntReportForGoodDbCode_And_ReportsAllErrorsForBadDbCode()
    {
        string goodDbCode =
@"using System;
using System.Collections.Generic;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.Core.CSharpDefaultValues;
using DotNetDBTools.Definition.PostgreSQL;
using DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes;

namespace SampleTestCode
{
    public class TestCompositeType1 : ICompositeType
    {
        public Guid DNDBT_OBJECT_ID => new(""AF622311-F791-4DC3-995D-D03C75236A1F"");
        public IDictionary<string, IDataType> Attributes => new Dictionary<string, IDataType>()
        {
            { ""MyAttribute1"", new StringDataType() },
            { ""MyAttribute2"", new IntDataType() },
        };
    }

    public class TestTable2 : ITable
    {
        public Guid DNDBT_OBJECT_ID => new(""31347B68-6BE3-4F2B-BCD1-0CC82ECB97FA"");

        public Column {|#0:TestColumn1|} = new(""032611EB-AA7C-4FEA-8B3C-BB8609E140F6"")
        {
            DataType = new TestCompositeType1(),
            NotNull = true,
        };

        public Column TestColumn2 = new(""2FD848D5-A689-419F-9214-AFCF9F742564"")
        {
            DataType = new IntDataType(),
            NotNull = true,
            Default = new IntDefaultValue(145),
        };

        public Trigger {|#2:TR_TestName1|} = new(""EE64FFC3-5536-4624-BEAF-BC3A61D06A1A"")
        {
            CreateStatement = ""CREATE TRIGGER \""TR_TestName1\"" after insert on \""TestTable2\"" for each row execute function f1()"",
        };
    }

    public class TestTable1 : ITable
    {
        public Guid DNDBT_OBJECT_ID => new(""250542BA-31D6-49C5-9178-F86319343109"");

        public Column TestColumn1 = new(""0C322A6A-7D5F-418C-BC5E-DF120D16CF4C"")
        {
            DataType = new IntDataType(),
            NotNull = true,
        };

        public ForeignKey {|#1:FK_TestName1|} = new(""1955F440-4333-4F33-9755-1C618816C9FB"")
        {
            ThisColumns = new[] { nameof(TestColumn1) },
            ReferencedTable = nameof(TestTable2),
            ReferencedTableColumns = new[] { nameof(TestTable2.TestColumn2) },
            OnUpdate = ForeignKeyActions.NoAction,
            OnDelete = ForeignKeyActions.Cascade,
        };
    }
}";

        await Verify.VerifyAnalyzerAsync(goodDbCode);

        string badDbCode = goodDbCode
            .Replace(
                @"DataType = new TestCompositeType1(),",
                @"DataType = new VerbatimDataType(""SomeNonExistentType""),")
            .Replace(
                @"ReferencedTable = nameof(TestTable2),",
                @"ReferencedTable = ""NonExistentTableName"",")
            .Replace(
                @"CreateStatement = ""CREATE TRIGGER \""TR_TestName1\""",
                @"CreateStatement = ""CREATE TRIGGER \""TR_OtherTriggerName\""");

        string expectedColumnErrorMessage =
"Column 'TestColumn1' in table 'TestTable2' datatype is invalid: Unknown data type '\"SomeNonExistentType\"'.";
        string expectedForeignKeyErrorMessage =
"Foreign key 'FK_TestName1' in table 'TestTable1' references unknown table 'NonExistentTableName'.";
        string expectedTriggerErrorMessage =
"Trigger 'TR_TestName1' in table 'TestTable2' has different name in it's creation code.";

        List<DiagnosticResult> expectedDiagnostics = new()
        {
            DiagnosticResult.CompilerError("DNDBT_DA_01")
                .WithArguments(expectedColumnErrorMessage)
                .WithLocation(0),
            DiagnosticResult.CompilerError("DNDBT_DA_01")
                .WithArguments(expectedForeignKeyErrorMessage)
                .WithLocation(1),
            DiagnosticResult.CompilerError("DNDBT_DA_01")
                .WithArguments(expectedTriggerErrorMessage)
                .WithLocation(2),
        };

        await Verify.VerifyAnalyzerAsync(badDbCode, expectedDiagnostics.ToArray());
    }
}
