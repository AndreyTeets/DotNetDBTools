using System;
using System.Collections.Generic;
using System.IO;
using DotNetDBTools.Analysis.PostgreSQL;
using DotNetDBTools.DefinitionParsing.Core;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Analysis.PostgreSQL
{
    public class PostgreSQLStatementsParserTests
    {
        private const string TestDataDir = "./TestData/PostgreSQL";

        [Fact]
        public void ParseToStatementsList_GetsCorrectData()
        {
            string statementsStr = File.ReadAllText(@$"{TestDataDir}/StatementsList.sql").NormalizeLineEndings();
            List<string> statementsList = PostgreSQLStatementsParser.ParseToStatementsList(statementsStr);

            statementsList.Count.Should().Be(3);

            statementsList[0].Should().StartWith("create TABLE \"Table1\"");
            statementsList[0].Should().EndWith("( \"Col3\" >= 0 )\n);");

            statementsList[1].Should().StartWith("--FunctionID:#{DC36AE77-B7E4-40C3-824F-BD20DC270A14}#\nCREATE FUNCTION \"TR_MyTa");
            statementsList[1].Should().EndWith("END;\n$FuncBody$;");

            statementsList[2].Should().StartWith("CREATE TRIGGER \"TR_MyTable2_MyTrigger1\"");
            statementsList[2].Should().EndWith("EXECUTE FUNCTION \"TR_MyTable2_MyTrigger1_Handler\"();");
        }

        [Theory]
        [InlineData("some statement")]
        [InlineData("some statement--comment;")]
        [InlineData("some 'statement;")]
        [InlineData("some $statement;")]
        [InlineData("some $$statement;")]
        [InlineData("some $$statem$ent;")]
        [InlineData("some $$statem$x$ent;")]
        [InlineData("some '$$statement'$$;")]
        public void ParseToStatementsList_ThrowsOnMalformedInput(string statementStr)
        {
            FluentActions.Invoking(() => PostgreSQLStatementsParser.ParseToStatementsList(statementStr))
                .Should().Throw<Exception>().WithMessage($"Failed to find first statement length in [{statementStr}]");
        }
    }
}
