using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using Verify = DotNetDBTools.AnalyzersTests.DbDescriptionSourceGeneratorVerifier<DotNetDBTools.DescriptionSourceGenerator.DbDescriptionSourceGenerator>;

namespace DotNetDBTools.AnalyzersTests;

public class DbDescriptionSourceGeneratorTests
{
    [Fact]
    public async Task DbDefinitionSourceGenerator_GeneratesSourceForGoodDbCode_And_ReportsErrorForBadDbCode()
    {
        string goodDbCode =
@"using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.Core.CSharpDataTypes;
using DotNetDBTools.Definition.MSSQL;

namespace SampleTestCode
{
    public class TestTable1 : ITable
    {
        public Guid ID => new(""250542BA-31D6-49C5-9178-F86319343109"");

        public Column TestColumn1 = new(""0C322A6A-7D5F-418C-BC5E-DF120D16CF4C"")
        {
            DataType = new IntDataType(),
            NotNull = true,
        };
    }
}
";

        string expectedGeneratedCode =
@"namespace TestProjectDescription
{
    public static class TestProjectTables
    {
        public static readonly TestTable1Description TestTable1 = new();

        public class TestTable1Description
        {
            public readonly string TestColumn1 = nameof(TestColumn1);

            public override string ToString() => nameof(TestTable1);
            public static implicit operator string(TestTable1Description description) => description.ToString();
        }
    }
}".Replace("\r\n", "\n");

        await Verify.VerifyGeneratorAsync(goodDbCode, expectedGeneratedCode);

        string badDbCode = goodDbCode.Replace(
            @"DataType = new IntDataType(),",
            @"DataType = new Func<IDataType>(() => throw new Exception(""Some db model creation error""))(),");

        DiagnosticResult expected = DiagnosticResult.CompilerError("DNDBT_DSG_01");
        await Verify.VerifyGeneratorAsync(badDbCode, null, expected);
    }
}
