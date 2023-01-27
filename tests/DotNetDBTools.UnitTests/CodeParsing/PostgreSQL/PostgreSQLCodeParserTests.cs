using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.UnitTests.CodeParsing.Base;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.CodeParsing.PostgreSQL;

public class PostgreSQLCodeParserTests : BaseCodeParserTests<PostgreSQLCodeParser>
{
    protected override BaseCodeParserTestsData TestData => new PostgreSQLCodeParserTestsData();
    protected PostgreSQLCodeParserTestsData TD => (PostgreSQLCodeParserTestsData)TestData;

    [Fact]
    public void GetObjectInfo_ParsesCompositeTypeCorrectly()
    {
        Assert_GetObjectInfo_ParsesObjectCorrectly("CreateCompositeType.sql", TD.ExpectedCompositeType);
    }

    [Fact]
    public void GetObjectInfo_ParsesDomainTypeCorrectly()
    {
        Assert_GetObjectInfo_ParsesObjectCorrectly("CreateDomainType.sql", TD.ExpectedDomainType);
    }

    [Fact]
    public void GetObjectInfo_ParsesEnumTypeCorrectly()
    {
        Assert_GetObjectInfo_ParsesObjectCorrectly("CreateEnumType.sql", TD.ExpectedEnumType);
    }

    [Fact]
    public void GetObjectInfo_ParsesRangeTypeCorrectly()
    {
        Assert_GetObjectInfo_ParsesObjectCorrectly("CreateRangeType.sql", TD.ExpectedRangeType);
    }

    [Fact]
    public void GetObjectInfo_ParsesSQLFunctionCorrectly()
    {
        Assert_GetObjectInfo_ParsesObjectCorrectly("CreateSQLFunction.sql", TD.ExpectedSQLFunction);
    }

    [Fact]
    public void GetObjectInfo_ParsesPLPGSQLFunctionCorrectly()
    {
        Assert_GetObjectInfo_ParsesObjectCorrectly("CreatePLPGSQLFunction.sql", TD.ExpectedPLPGSQLFunction);
    }

    [Fact]
    public void GetObjectInfo_ParsesSQLProcedureCorrectly()
    {
        Assert_GetObjectInfo_ParsesObjectCorrectly("CreateSQLProcedure.sql", TD.ExpectedSQLProcedure);
    }

    [Fact]
    public void GetObjectInfo_ParsesPLPGSQLProcedureCorrectly()
    {
        Assert_GetObjectInfo_ParsesObjectCorrectly("CreatePLPGSQLProcedure.sql", TD.ExpectedPLPGSQLProcedure);
    }

    [Fact]
    public void GetViewDependencies_GetsCorrectData()
    {
        string input = MiscHelper.ReadFromFile($@"{TestData.TestDataDir}/CreateView.sql");
        PostgreSQLCodeParser parser = new();
        List<Dependency> dependencies = parser.GetViewDependencies(input);

        List<Dependency> expectedDependencies = new()
        {
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable2" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView3".ToLower() },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "MyFunc4" },
        };
        dependencies.Should().BeEquivalentTo(expectedDependencies, options => options.WithStrictOrdering());
    }

    [Fact]
    public void GetFunctionDependencies_GetsCorrectData_FromSQLFunction()
    {
        string input = MiscHelper.ReadFromFile($@"{TestData.TestDataDir}/CreateSQLFunction.sql");
        PostgreSQLCodeParser parser = new();
        List<Dependency> dependencies = parser.GetFunctionDependencies(input, out string language);

        List<Dependency> expectedDependencies = new()
        {
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable1" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView2".ToLower() },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "MyFunc3" },
        };
        dependencies.Should().BeEquivalentTo(expectedDependencies, options => options.WithStrictOrdering());
    }

    [Fact]
    public void GetFunctionDependencies_GetsCorrectData_FromPLPGSQLFunction()
    {
        string input = MiscHelper.ReadFromFile($@"{TestData.TestDataDir}/CreatePLPGSQLFunction.sql");
        PostgreSQLCodeParser parser = new();
        List<Dependency> dependencies = parser.GetFunctionDependencies(input, out string language);

        List<Dependency> expectedDependencies = new()
        {
            new Dependency { Type = DependencyType.DataType, Name = "text" },
            new Dependency { Type = DependencyType.DataType, Name = "xml" },
            new Dependency { Type = DependencyType.DataType, Name = "timestamptz" },
            new Dependency { Type = DependencyType.DataType, Name = "uuid" },
            new Dependency { Type = DependencyType.DataType, Name = "regprocedure" },
            new Dependency { Type = DependencyType.DataType, Name = "SQLSTATE" },
            new Dependency { Type = DependencyType.DataType, Name = "int8" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable01" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable02" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable03".ToLower() },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable04" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable1" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable2" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable3" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable4" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable5" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable6" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable7" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable8" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable9".ToLower() },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable10".ToLower() },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable11" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable12".ToLower() },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable13" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable14".ToLower() },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable15" },
            new Dependency { Type = DependencyType.TableOrView, Name = "_some_table" },
            new Dependency { Type = DependencyType.TableOrView, Name = "WithRes" },
            new Dependency { Type = DependencyType.TableOrView, Name = "WithRes2" },
            new Dependency { Type = DependencyType.TableOrView, Name = "_with_res" },
            new Dependency { Type = DependencyType.TableOrView, Name = "pg_proc" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView1" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView2".ToLower() },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView3".ToLower() },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView4" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView5" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView6" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "MyFunc1" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "MyFunc2".ToLower() },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "MyFunc3" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "_some_func" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "min" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "max" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "count" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "now" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "round" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "length" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "strpos" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "unnest" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "array_append" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "array_upper" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "array_cat" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "concat" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "string_agg" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "generate_series" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "abs" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "pg_sleep" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "pg_function_is_visible" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "MyProc1" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "MyProc2".ToLower() },
        };

        IEnumerable<Dependency> excessDeps = dependencies.Where(dep => !expectedDependencies.Contains(dep));
        IEnumerable<Dependency> missingDeps = expectedDependencies.Where(dep => !dependencies.Contains(dep));

        excessDeps.Should().BeEmpty();
        missingDeps.Should().BeEmpty();
    }

    [Fact]
    public void GetProcedureDependencies_GetsCorrectData_FromSQLProcedure()
    {
        string input = MiscHelper.ReadFromFile($@"{TestData.TestDataDir}/CreateSQLProcedure.sql");
        PostgreSQLCodeParser parser = new();
        List<Dependency> dependencies = parser.GetProcedureDependencies(input, out string language);

        List<Dependency> expectedDependencies = new()
        {
            new Dependency { Type = DependencyType.TableOrView, Name = "SomeTable" },
        };
        dependencies.Should().BeEquivalentTo(expectedDependencies, options => options.WithStrictOrdering());
    }

    [Fact]
    public void GetProcedureDependencies_GetsCorrectData_FromPLPGSQLProcedure()
    {
        string input = MiscHelper.ReadFromFile($@"{TestData.TestDataDir}/CreatePLPGSQLProcedure.sql");
        PostgreSQLCodeParser parser = new();
        List<Dependency> dependencies = parser.GetProcedureDependencies(input, out string language);

        List<Dependency> expectedDependencies = new()
        {
            new Dependency { Type = DependencyType.DataType, Name = "text" },
            new Dependency { Type = DependencyType.FunctionOrProcedure, Name = "Proc2".ToLower() },
        };

        IEnumerable<Dependency> excessDeps = dependencies.Where(dep => !expectedDependencies.Contains(dep));
        IEnumerable<Dependency> missingDeps = expectedDependencies.Where(dep => !dependencies.Contains(dep));

        excessDeps.Should().BeEmpty();
        missingDeps.Should().BeEmpty();
    }
}
