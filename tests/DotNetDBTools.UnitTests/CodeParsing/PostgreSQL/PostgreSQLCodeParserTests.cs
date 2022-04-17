using System;
using System.Collections.Generic;
using System.IO;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Core.Models;
using DotNetDBTools.CodeParsing.PostgreSQL;
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
        string input = File.ReadAllText($@"{TestDataDir}/CreateView.sql");
        PostgreSQLCodeParser parser = new();
        List<Dependency> dependencies = parser.GetViewDependencies(input);

        List<Dependency> expectedDependencies = new()
        {
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable2" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView3".ToLower() },
            new Dependency { Type = DependencyType.Function, Name = "MyFunc4" },
        };
        dependencies.Should().BeEquivalentTo(expectedDependencies);
    }

    [Fact]
    public void GetFunctionDependencies_GetsCorrectData_FromSQLFunc()
    {
        string input = File.ReadAllText($@"{TestDataDir}/CreateSQLFunction.sql");
        PostgreSQLCodeParser parser = new();
        List<Dependency> dependencies = parser.GetFunctionDependencies(input);

        List<Dependency> expectedDependencies = new()
        {
            new Dependency { Type = DependencyType.Table, Name = "MyTable1" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView2".ToLower() },
            new Dependency { Type = DependencyType.Function, Name = "MyFunc3" },
        };
        dependencies.Should().BeEquivalentTo(expectedDependencies);
    }

    [Fact]
    public void GetFunctionDependencies_GetsCorrectData_FromPLPGSQLFunc()
    {
        string input = File.ReadAllText($@"{TestDataDir}/CreatePLPGSQLFunction.sql");
        PostgreSQLCodeParser parser = new();
        List<Dependency> dependencies = parser.GetFunctionDependencies(input);

        List<Dependency> expectedDependencies = new()
        {
            new Dependency { Type = DependencyType.Table, Name = "MyTable1" },
            new Dependency { Type = DependencyType.Function, Name = "MyFunc2" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView3" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable4" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView5".ToLower() },
            new Dependency { Type = DependencyType.Table, Name = "MyTable6" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView7".ToLower() },
            new Dependency { Type = DependencyType.Table, Name = "MyTable8" },
            new Dependency { Type = DependencyType.Function, Name = "MyFunc9".ToLower() },
        };
        dependencies.Should().BeEquivalentTo(expectedDependencies);
    }
}
