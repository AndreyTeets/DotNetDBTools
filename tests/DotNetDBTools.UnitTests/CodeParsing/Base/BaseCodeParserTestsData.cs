using DotNetDBTools.CodeParsing.Models;

namespace DotNetDBTools.UnitTests.CodeParsing.Base;

public abstract class BaseCodeParserTestsData
{
    public abstract string TestDataDir { get; }
    public abstract TableInfo ExpectedTable { get; }
    public abstract TableInfo ExpectedTableWithPkColumn { get; }
    public abstract ViewInfo ExpectedView { get; }
    public abstract IndexInfo ExpectedIndex { get; }
    public abstract TriggerInfo ExpectedTrigger { get; }
}
