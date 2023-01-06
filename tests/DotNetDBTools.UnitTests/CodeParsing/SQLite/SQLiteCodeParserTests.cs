using DotNetDBTools.CodeParsing;
using DotNetDBTools.UnitTests.CodeParsing.Base;
using Xunit;

namespace DotNetDBTools.UnitTests.CodeParsing.SQLite;

public class SQLiteCodeParserTests : BaseCodeParserTests<SQLiteCodeParser>
{
    protected override BaseCodeParserTestsData TestData => new SQLiteCodeParserTestsData();
    protected SQLiteCodeParserTestsData TD => (SQLiteCodeParserTestsData)TestData;

    [Fact]
    public void GetObjectInfo_ParsesPkColumnCorrectly()
    {
        Assert_GetObjectInfo_ParsesObjectCorrectly("CreateTableWithPkColumn.sql", TD.ExpectedTableWithPkColumn);
    }
}
