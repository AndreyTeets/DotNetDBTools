using DotNetDBTools.CodeParsing;
using DotNetDBTools.UnitTests.CodeParsing.Base;

namespace DotNetDBTools.UnitTests.CodeParsing.SQLite;

public class SQLiteCodeParserTests : BaseCodeParserTests<SQLiteCodeParser>
{
    protected override BaseCodeParserTestsData TestData => new SQLiteCodeParserTestsData();
}
