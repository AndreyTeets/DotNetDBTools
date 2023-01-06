using System;
using System.Collections.Generic;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.CodeParsing.PostgreSQL;

public class PostgreSQLStatementsSplitterTests
{
    private const string TestDataDir = "./TestData/PostgreSQL";

    [Fact]
    public void ParseToStatementsList_GetsCorrectData()
    {
        string input = MiscHelper.ReadFromFile($@"{TestDataDir}/StatementsList.sql");
        List<string> statements = PostgreSQLStatementsSplitter.Split(input);

        statements.Count.Should().Be(3);

        statements[0].Should().StartWith("create TABLE \"Table1\"");
        statements[0].Should().EndWith("( \"Col3\" >= 0 )\n);");

        statements[1].Should().StartWith("--ID:#{DC36AE77-B7E4-40C3-824F-BD20DC270A14}#\nCREATE FUNCTION \"TR_MyTa");
        statements[1].Should().EndWith("END;\n$FuncBody$;");

        statements[2].Should().StartWith("CREATE TRIGGER \"TR_MyTable2_MyTrigger1\"");
        statements[2].Should().EndWith("EXECUTE FUNCTION \"TR_MyTable2_MyTrigger1_Handler\"()");
    }

    [Theory]
    [InlineData("some statement")]
    [InlineData("some statement--comment")]
    [InlineData("some statement--co'mment")]
    [InlineData("some statement--co$mment")]
    [InlineData("some statement--co$$mment")]
    public void ParseToStatementsList_ParsesValidInput(string statementStr)
    {
        List<string> statementsList = PostgreSQLStatementsSplitter.Split(statementStr);
        statementsList.Count.Should().Be(1);
        statementsList[0].Should().Be(statementStr);
    }

    [Theory]
    [InlineData("some 'statement;")]
    [InlineData("some $statement;")]
    [InlineData("some $$statement;")]
    [InlineData("some $$statem$ent;")]
    [InlineData("some $$statem$x$ent;")]
    [InlineData("some '$$statement'$$;")]
    public void ParseToStatementsList_ThrowsOnMalformedInput(string statementStr)
    {
        FluentActions.Invoking(() => PostgreSQLStatementsSplitter.Split(statementStr))
            .Should().Throw<Exception>().WithMessage($"Failed to find first statement length in [{statementStr}]");
    }
}
