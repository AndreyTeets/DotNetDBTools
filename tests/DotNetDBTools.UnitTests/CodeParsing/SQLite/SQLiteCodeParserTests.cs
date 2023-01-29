using System.Collections.Generic;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.UnitTests.CodeParsing.Base;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.CodeParsing.SQLite;

public class SQLiteCodeParserTests : BaseCodeParserTests<SQLiteCodeParser>
{
    protected override BaseCodeParserTestsData TestData => new SQLiteCodeParserTestsData();

    [Fact]
    public void GetViewDependencies_GetsCorrectData()
    {
        string input = MiscHelper.ReadFromFile($@"{TestData.TestDataDir}/CreateView.sql");
        SQLiteCodeParser parser = new();
        List<Dependency> dependencies = parser.GetViewDependencies(input);

        List<Dependency> expectedDependencies = new()
        {
            new Dependency { Type = DependencyType.TableOrView, Name = "MyTable2" },
            new Dependency { Type = DependencyType.TableOrView, Name = "MyView3" },
        };
        dependencies.Should().BeEquivalentTo(expectedDependencies, options => options.WithStrictOrdering());
    }
}
