using System;
using Antlr4.Runtime.Tree;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.UnitTests.CodeParsing.Base;

namespace DotNetDBTools.UnitTests.CodeParsing.SQLite;

public class SQLiteGrammarTests : BaseGrammarTests<SQLiteParser, SQLiteLexer>
{
    protected override string TestDataDir => "../../../TestData/SQLite/Grammar/examples";
    protected override Func<SQLiteParser, IParseTree> ListOfStatementsStartRule => x => x.sql();
    protected override Action<TestCodeParser, IParseTree> DoAdditionalParsing => (x, y) => { };
}
