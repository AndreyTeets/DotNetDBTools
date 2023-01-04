using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.CodeParsing.Base;

public abstract class BaseCodeParserTests<TCodeParser>
    where TCodeParser : ICodeParser, new()
{
    protected abstract BaseCodeParserTestsData TestData { get; }

    [Fact]
    public void GetObjectInfo_ParsesTableCorrectly()
    {
        Assert_GetObjectInfo_ParsesObjectCorrectly("CreateTable.sql", TestData.ExpectedTable);
    }

    [Fact]
    public void GetObjectInfo_ParsesViewCorrectly()
    {
        Assert_GetObjectInfo_ParsesObjectCorrectly("CreateView.sql", TestData.ExpectedView);
    }

    [Fact]
    public void GetObjectInfo_ParsesIndexCorrectly()
    {
        Assert_GetObjectInfo_ParsesObjectCorrectly("CreateIndex.sql", TestData.ExpectedIndex);
    }

    [Fact]
    public void GetObjectInfo_ParsesTriggerCorrectly()
    {
        Assert_GetObjectInfo_ParsesObjectCorrectly("CreateTrigger.sql", TestData.ExpectedTrigger);
    }

    [Fact]
    public void GetObjectInfo_ThrowsOnMalformedInput()
    {
        string input = "some trash input";
        TCodeParser parser = new();
        FluentActions.Invoking(() => parser.GetObjectInfo(input))
            .Should().Throw<ParseException>().WithMessage($"Failed to parse object info: ParserError(line=1,pos=0): mismatched input 'some' *");
    }

    [Theory]
    [InlineData("create table t1(c1 int)")]
    [InlineData("--ID:#{5E9695C6-D8C6-4C86-A699-574F4D2794F1}#\ncreate table t1(c1 int)")]
    [InlineData("create table t1(--ID:#{5E9695C6-D8C6-4C86-A699-574F4D2794F1}#\nc1 int)")]
    public void GetObjectInfo_ThrowsOnMissingId(string input)
    {
        TCodeParser parser = new();
        FluentActions.Invoking(() => parser.GetObjectInfo(input))
            .Should().Throw<ParseException>().WithMessage($"Failed to parse object info: Id declaration comment is missing for *");
    }

    protected void Assert_GetObjectInfo_ParsesObjectCorrectly<TObjectInfo>(
        string sqlFileName, TObjectInfo expectedObjectInfo)
        where TObjectInfo : ObjectInfo
    {
        string input = FilesHelper.GetFromFile($@"{TestData.TestDataDir}/{sqlFileName}");
        TCodeParser parser = new();
        TObjectInfo actualObjectInfo = (TObjectInfo)parser.GetObjectInfo(input);

        actualObjectInfo.Should().BeEquivalentTo(expectedObjectInfo, options => options.WithStrictOrdering());
    }
}
