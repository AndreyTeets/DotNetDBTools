using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.PostgreSQL;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.CodeParsing.PostgreSQL
{
    public class PostgreSQLCodeParserTests
    {
        private const string TestDataDir = "./TestData/PostgreSQL";

        [Fact]
        public void SplitToStatements_GetsCorrectData()
        {
            string input = File.ReadAllText(@$"{TestDataDir}/StatementsList.sql");
            PostgreSQLCodeParser parser = new();
            List<string> statements = parser.SplitToStatements(input);

            statements = statements.Select(x => x.NormalizeLineEndings()).ToList();
            statements.Count.Should().Be(3);

            statements[0].Should().StartWith("create TABLE \"Table1\"");
            statements[0].Should().EndWith("( \"Col3\" >= 0 )\n)");

            statements[1].Should().StartWith("CREATE FUNCTION \"TR_MyTa");
            statements[1].Should().EndWith("END;\n$FuncBody$");

            statements[2].Should().StartWith("CREATE TRIGGER \"TR_MyTable2_MyTrigger1\"");
            statements[2].Should().EndWith("EXECUTE FUNCTION \"TR_MyTable2_MyTrigger1_Handler\"()");
        }

        [Fact]
        public void SplitToStatements_ThrowsOnMalformedInput()
        {
            string input = "some trash input";
            PostgreSQLCodeParser parser = new();
            FluentActions.Invoking(() => parser.SplitToStatements(input))
                .Should().Throw<ParseException>().WithMessage($"ParserError(line=1,pos=0): mismatched input 'some' *");
        }

        [Fact]
        public void GetViewDependencies_GetsCorrectData()
        {
            string input = File.ReadAllText(@$"{TestDataDir}/CreateView.sql");
            PostgreSQLCodeParser parser = new();
            List<Dependency> dependencies = parser.GetViewDependencies(input);

            List<Dependency> expectedDependencies = new()
            {
                new Dependency { Type = ObjectType.TableOrView, Name = "MyTable2" },
                new Dependency { Type = ObjectType.TableOrView, Name = "MyView3".ToLower() },
                new Dependency { Type = ObjectType.Function, Name = "MyFunc4" },
            };
            dependencies.Should().BeEquivalentTo(expectedDependencies);
        }

        [Fact]
        public void GetFunctionDependencies_GetsCorrectData_FromSQLFunc()
        {
            string input = File.ReadAllText(@$"{TestDataDir}/CreateSQLFunction.sql");
            PostgreSQLCodeParser parser = new();
            List<Dependency> dependencies = parser.GetFunctionDependencies(input);

            List<Dependency> expectedDependencies = new()
            {
                new Dependency { Type = ObjectType.Table, Name = "MyTable1" },
                new Dependency { Type = ObjectType.TableOrView, Name = "MyView2".ToLower() },
                new Dependency { Type = ObjectType.Function, Name = "MyFunc3" },
            };
            dependencies.Should().BeEquivalentTo(expectedDependencies);
        }

        [Fact]
        public void GetFunctionDependencies_GetsCorrectData_FromPLPGSQLFunc()
        {
            string input = File.ReadAllText(@$"{TestDataDir}/CreatePLPGSQLFunction.sql");
            PostgreSQLCodeParser parser = new();
            List<Dependency> dependencies = parser.GetFunctionDependencies(input);

            List<Dependency> expectedDependencies = new()
            {
                new Dependency { Type = ObjectType.Table, Name = "MyTable1" },
                new Dependency { Type = ObjectType.Function, Name = "MyFunc2" },
                new Dependency { Type = ObjectType.TableOrView, Name = "MyView3" },
                new Dependency { Type = ObjectType.TableOrView, Name = "MyTable4" },
                new Dependency { Type = ObjectType.TableOrView, Name = "MyView5".ToLower() },
                new Dependency { Type = ObjectType.Table, Name = "MyTable6" },
                new Dependency { Type = ObjectType.TableOrView, Name = "MyView7".ToLower() },
                new Dependency { Type = ObjectType.Table, Name = "MyTable8" },
                new Dependency { Type = ObjectType.Function, Name = "MyFunc9".ToLower() },
            };
            dependencies.Should().BeEquivalentTo(expectedDependencies);
        }
    }
}
