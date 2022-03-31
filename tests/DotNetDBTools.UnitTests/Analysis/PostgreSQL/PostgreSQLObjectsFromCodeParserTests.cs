using System;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Analysis.PostgreSQL;

public class PostgreSQLObjectsFromCodeParserTests
{
    private const string FuncIDDefinition = "--FunctionID:#{A1159A6A-35F1-4B70-86A1-5427E08942DE}#";

    [Fact]
    public void ParseFunction_GetsCorrectData()
    {
        string functionStatement =
@$"{FuncIDDefinition}
CREATE   function ""TR_SomeTriggerFunction"" ()
bla bla;".NormalizeLineEndings();

        PostgreSQLFunction func = PostgreSQLObjectsFromCodeParser.ParseFunction(functionStatement);

        func.ID.Should().Be(Guid.Parse("A1159A6A-35F1-4B70-86A1-5427E08942DE"));
        func.Name.Should().Be("TR_SomeTriggerFunction");
        string functionCode = functionStatement.Replace(FuncIDDefinition, "").Trim();
        func.CodePiece.Code.Should().Be(functionCode);
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

        FluentActions.Invoking(() => PostgreSQLObjectsFromCodeParser.ParseFunction(functionStatement))
            .Should().Throw<Exception>().WithMessage($"Failed to parse function from statement [{functionStatement}]");
    }
}
