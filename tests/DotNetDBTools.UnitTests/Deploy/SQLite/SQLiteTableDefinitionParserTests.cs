using System;
using System.Collections.Generic;
using System.IO;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Deploy.SQLite
{
    public class SQLiteTableDefinitionParserTests
    {
        private const string TestDataDir = "./TestData/SQLite";
        private static readonly List<string> s_definitionStatements = new()
        {
            "[Col1] INTEGER NOT NULL DEFAULT 15 unique",
            "Col2 numeric DEFAULT 7.36",
            "`Col3` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL",
            "Col4 TEXT NOT NULL default 'CONSTRAINT CK_String_Check1 CHECK (Col3 >= 0),'",
            "Col5 BLOB NOT NULL default (DATETIME('now'))",
            "constraint PK_Table1 PRIMARY KEY ([Col1], `Col2`)",
            "constraint UQ_Table1_Col1 UNIQUE ( [Col1] )",
            "CONSTRAINT [UQ_Table1_Col2Col4] unique (Col2, \"Col4\")",
            "constraint FK_Table1_Col1_Table2_Col1 FOREIGN KEY (Col1)\n        REFERENCES Table2(`Col1`)\n        ON UPDATE NO ACTION ON DELETE CASCADE",
            "CONSTRAINT [FK_Table1_Col1Col2_Table2_Col2Col4] foreign KEY ( Col1, \"Col2\" ) REFERENCES Table2([Col2],Col4)",
            "constraint CK_Table1_Check1 CHECK ([Col2] != 'Col2 NUMERIC NOT NULL DEFAULT 7.36,')",
            "CHECK (Col4 = 'CONSTRAINT [CK_String_Check2] CHECK ( [Col3] >= 0 ),' AND f1(f2())=' quo''te g1(g2(g3)))' AND TRUE)",
            "CONSTRAINT `CK_Table1_Check3` check ( [Col3] >= 0 )",
        };

        [Fact]
        public void ParseToDefinitionStatements_GetsCorrectData()
        {
            string tableDefinition = File.ReadAllText(@$"{TestDataDir}/CreateTable.sql").NormalizeLineEndings();
            List<string> definitionStatements = SQLiteTableDefinitionParser.ParseToDefinitionStatements(tableDefinition);
            definitionStatements.Should().BeEquivalentTo(s_definitionStatements, options => options.WithStrictOrdering());
        }

        [Fact]
        public void ParseToDefinitionStatements_ThrowsOnMalformedTableDefinition()
        {
            FluentActions.Invoking(() => SQLiteTableDefinitionParser.ParseToDefinitionStatements("create table t1 c1 int"))
                .Should().Throw<Exception>().WithMessage("Failed to parse definition statements substring in [*]");

            FluentActions.Invoking(() => SQLiteTableDefinitionParser.ParseToDefinitionStatements("create table t1 (c1 int,)"))
                .Should().Throw<Exception>().WithMessage("Failed to find first statement length in [,]");

            FluentActions.Invoking(() => SQLiteTableDefinitionParser.ParseToDefinitionStatements("create table t1 (c1 int, c2 text ')"))
                .Should().Throw<Exception>().WithMessage("Failed to find first statement length in [c2 text ',]");
        }

        [Fact]
        public void GetCheckConstraints_GetsCorrectData()
        {
            List<(string ckName, string ckCode)> checkConstraints = SQLiteTableDefinitionParser.GetCheckConstraints(s_definitionStatements);

            checkConstraints.Count.Should().Be(3);

            checkConstraints[0].ckName.Should().Be("CK_Table1_Check1");
            checkConstraints[0].ckCode.Should().Be("CHECK ([Col2] != 'Col2 NUMERIC NOT NULL DEFAULT 7.36,')");

            checkConstraints[1].ckName.Should().Be("");
            checkConstraints[1].ckCode.Should().Be(
                "CHECK (Col4 = 'CONSTRAINT [CK_String_Check2] CHECK ( [Col3] >= 0 ),' AND f1(f2())=' quo''te g1(g2(g3)))' AND TRUE)");

            checkConstraints[2].ckName.Should().Be("CK_Table1_Check3");
            checkConstraints[2].ckCode.Should().Be("CHECK ( [Col3] >= 0 )");
        }

        [Fact]
        public void GetUniqueConstraintName_GetsCorrectData()
        {
            SQLiteTableDefinitionParser.GetUniqueConstraintName(s_definitionStatements, new string[] { "Col1" })
                .Should().Be("UQ_Table1_Col1");

            SQLiteTableDefinitionParser.GetUniqueConstraintName(s_definitionStatements, new string[] { "Col2", "Col4" })
                .Should().Be("UQ_Table1_Col2Col4");
        }

        [Fact]
        public void GetForeignKeyConstraintName_GetsCorrectData()
        {
            SQLiteTableDefinitionParser.GetForeignKeyConstraintName(s_definitionStatements, new string[] { "Col1" })
                .Should().Be("FK_Table1_Col1_Table2_Col1");

            SQLiteTableDefinitionParser.GetForeignKeyConstraintName(s_definitionStatements, new string[] { "Col1", "Col2" })
                .Should().Be("FK_Table1_Col1Col2_Table2_Col2Col4");
        }
    }
}
