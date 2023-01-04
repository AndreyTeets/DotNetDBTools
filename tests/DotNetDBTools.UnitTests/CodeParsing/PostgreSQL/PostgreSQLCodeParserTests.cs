using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.CodeParsing.PostgreSQL;

public class PostgreSQLCodeParserTests
{
    private const string TestDataDir = "./TestData/PostgreSQL";
    private const string FuncIDDefinition = "--FunctionID:#{A1159A6A-35F1-4B70-86A1-5427E08942DE}#";

    [Fact]
    public void GetObjectInfo_ParsesFunctionCorrectly()
    {
        string functionStatement =
$@"{FuncIDDefinition}
CREATE   function ""TR_SomeTriggerFunction"" ()
bla bla;".NormalizeLineEndings();

        PostgreSQLCodeParser parser = new();
        FunctionInfo func = (FunctionInfo)parser.GetObjectInfo(functionStatement);

        func.ID.Should().Be(Guid.Parse("A1159A6A-35F1-4B70-86A1-5427E08942DE"));
        func.Name.Should().Be("TR_SomeTriggerFunction");
        string functionCode = functionStatement.Replace(FuncIDDefinition, "").Trim();
        func.Code.Should().Be(functionCode);
    }

    [Theory]
    [InlineData("create function f1() no funcID definition bla bla;", false)]
    [InlineData("create function f1() no end-statement semicolon bla bla")]
    [InlineData("create function invalid-func-name() bla bla;")]
    [InlineData("create function f2 no parantheses bla bla;")]
    public void ParseFunction_ThrowsOnMalformedInput(string statement, bool prependValidFuncIDDefinition = true)
    {
        string functionStatement = statement;
        if (prependValidFuncIDDefinition)
            functionStatement = $"{FuncIDDefinition}\n{statement}";

        PostgreSQLCodeParser parser = new();
        FluentActions.Invoking(() => parser.GetObjectInfo(functionStatement))
            .Should().Throw<Exception>().WithMessage($"Failed to parse function from statement [{functionStatement}]");
    }

    [Fact]
    public void GetViewDependencies_GetsCorrectData()
    {
        string input = FilesHelper.GetFromFile($@"{TestDataDir}/CreateView.sql");
        PostgreSQLCodeParser parser = new();
        List<Dependency> dependencies = parser.GetViewDependencies(input);

        List<Dependency> expectedDependencies = new()
        {
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable2" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView3".ToLower() },
            new Dependency { Type = DependencyType.Function, Name = "MyFunc4" },
        };
        dependencies.Should().BeEquivalentTo(expectedDependencies, options => options.WithStrictOrdering());
    }

    [Fact]
    public void GetFunctionDependencies_GetsCorrectData_FromSQLFunc()
    {
        string input = FilesHelper.GetFromFile($@"{TestDataDir}/CreateSQLFunction.sql");
        PostgreSQLCodeParser parser = new();
        List<Dependency> dependencies = parser.GetFunctionDependencies(input);

        List<Dependency> expectedDependencies = new()
        {
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable1" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView2".ToLower() },
            new Dependency { Type = DependencyType.Function, Name = "MyFunc3" },
        };
        dependencies.Should().BeEquivalentTo(expectedDependencies, options => options.WithStrictOrdering());
    }

    [Fact]
    public void GetFunctionDependencies_GetsCorrectData_FromPLPGSQLFunc()
    {
        string input = FilesHelper.GetFromFile($@"{TestDataDir}/CreatePLPGSQLFunction.sql");
        PostgreSQLCodeParser parser = new();
        List<Dependency> dependencies = parser.GetFunctionDependencies(input);

        List<Dependency> expectedDependencies = new()
        {
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
            new Dependency { Type = DependencyType.Function, Name = "MyFunc1" },
            new Dependency { Type = DependencyType.Function, Name = "MyFunc2".ToLower() },
            new Dependency { Type = DependencyType.Function, Name = "MyFunc3" },
            new Dependency { Type = DependencyType.Function, Name = "_some_func" },
            new Dependency { Type = DependencyType.Function, Name = "min" },
            new Dependency { Type = DependencyType.Function, Name = "max" },
            new Dependency { Type = DependencyType.Function, Name = "count" },
            new Dependency { Type = DependencyType.Function, Name = "now" },
            new Dependency { Type = DependencyType.Function, Name = "round" },
            new Dependency { Type = DependencyType.Function, Name = "length" },
            new Dependency { Type = DependencyType.Function, Name = "strpos" },
            new Dependency { Type = DependencyType.Function, Name = "unnest" },
            new Dependency { Type = DependencyType.Function, Name = "array_append" },
            new Dependency { Type = DependencyType.Function, Name = "array_upper" },
            new Dependency { Type = DependencyType.Function, Name = "array_cat" },
            new Dependency { Type = DependencyType.Function, Name = "concat" },
            new Dependency { Type = DependencyType.Function, Name = "string_agg" },
            new Dependency { Type = DependencyType.Function, Name = "generate_series" },
            new Dependency { Type = DependencyType.Function, Name = "abs" },
            new Dependency { Type = DependencyType.Function, Name = "pg_sleep" },
            new Dependency { Type = DependencyType.Function, Name = "pg_function_is_visible" },
        };

        IEnumerable<Dependency> excessDeps = dependencies.Where(dep => !expectedDependencies.Contains(dep));
        IEnumerable<Dependency> missingDeps = expectedDependencies.Where(dep => !dependencies.Contains(dep));

        excessDeps.Should().BeEmpty();
        missingDeps.Should().BeEmpty();
    }
}
