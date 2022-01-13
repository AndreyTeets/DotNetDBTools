using System.Collections.Generic;
using System.IO;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Core.Models;
using DotNetDBTools.CodeParsing.SQLite;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.CodeParsing.SQLite
{
    public class SQLiteCodeParserTests
    {
        private const string TestDataDir = "./TestData/SQLite";

        [Fact]
        public void GetModelFromCreateStatement_ParsesTableCorrectly()
        {
            string input = File.ReadAllText(@$"{TestDataDir}/CreateTable.sql");
            SQLiteCodeParser parser = new();
            TableInfo table = (TableInfo)parser.GetModelFromCreateStatement(input);

            TableInfo expectedTable = GetExpectedTableModel();
            table.Should().BeEquivalentTo(expectedTable);
        }

        [Fact]
        public void GetModelFromCreateStatement_ParsesViewCorrectly()
        {
            string input = File.ReadAllText(@$"{TestDataDir}/CreateView.sql");
            SQLiteCodeParser parser = new();
            ViewInfo view = (ViewInfo)parser.GetModelFromCreateStatement(input);

            ViewInfo expectedView = GetExpectedViewModel();
            view.Should().BeEquivalentTo(expectedView);
        }

        [Fact]
        public void GetModelFromCreateStatement_ParsesIndexCorrectly()
        {
            string input = File.ReadAllText(@$"{TestDataDir}/CreateIndex.sql");
            SQLiteCodeParser parser = new();
            IndexInfo index = (IndexInfo)parser.GetModelFromCreateStatement(input);

            IndexInfo expectedIndex = GetExpectedIndexModel();
            index.Should().BeEquivalentTo(expectedIndex);
        }

        [Fact]
        public void GetModelFromCreateStatement_ParsesTriggerCorrectly()
        {
            string input = File.ReadAllText(@$"{TestDataDir}/CreateTrigger.sql");
            SQLiteCodeParser parser = new();
            TriggerInfo trigger = (TriggerInfo)parser.GetModelFromCreateStatement(input);

            TriggerInfo expectedTrigger = GetExpectedTriggerModel();
            trigger.Should().BeEquivalentTo(expectedTrigger);
        }

        [Fact]
        public void GetModelFromCreateStatement_ThrowsOnMalformedInput()
        {
            string input = "some trash input";
            SQLiteCodeParser parser = new();
            FluentActions.Invoking(() => parser.GetModelFromCreateStatement(input))
                .Should().Throw<ParseException>().WithMessage($"ParserError(line=1,pos=0): mismatched input 'some' *");
        }

        private static TableInfo GetExpectedTableModel()
        {
            return new()
            {
                Name = "Table1",
                Columns = new List<ColumnInfo>()
                {
                    new ColumnInfo()
                    {
                        Name = "Col1",
                        DataType = "INTEGER",
                        DefaultType = DefaultType.Number,
                        DefaultValue = "15",
                        NotNull = true,
                        Unique = true,
                    },
                    new ColumnInfo()
                    {
                        Name = "Col2",
                        DataType = "numeric",
                        DefaultType = DefaultType.Number,
                        DefaultValue = "7.36",
                    },
                    new ColumnInfo()
                    {
                        Name = "Col3",
                        DataType = "INTEGER",
                        PrimaryKey = true,
                        Autoincrement = true,
                        NotNull = true,
                    },
                    new ColumnInfo()
                    {
                        Name = "Col4",
                        DataType = "TEXT",
                        DefaultType = DefaultType.String,
                        DefaultValue = "CONSTRAINT CK_String_Check1 CHECK (Col3 >= 0),",
                        NotNull = true,
                    },
                    new ColumnInfo()
                    {
                        Name = "Col5",
                        DataType = "BLOB",
                        DefaultType = DefaultType.Function,
                        DefaultValue = "DATETIME('now')",
                        NotNull = true,
                    },
                },
                Constraints = new List<ConstraintInfo>()
                {
                    new ConstraintInfo()
                    {
                        Name = "PK_Table1",
                        Type = ConstraintType.PrimaryKey,
                        Columns = new List<string>() { "Col1", "Col2" },
                    },
                    new ConstraintInfo()
                    {
                        Name = "UQ_Table1_Col1",
                        Type = ConstraintType.Unique,
                        Columns = new List<string>() { "Col1" },
                    },
                    new ConstraintInfo()
                    {
                        Name = "UQ_Table1_Col2Col4",
                        Type = ConstraintType.Unique,
                        Columns = new List<string>() { "Col2", "Col4" },
                    },
                    new ConstraintInfo()
                    {
                        Name = "FK_Table1_Col1_Table2_Col1",
                        Type = ConstraintType.ForeignKey,
                        Columns = new List<string>() { "Col1" },
                        RefTable = "Table2",
                        RefColumns = new List<string>() { "Col1" },
                        UpdateAction = "NO ACTION",
                        DeleteAction = "CASCADE",
                    },
                    new ConstraintInfo()
                    {
                        Name = "FK_Table1_Col1Col2_Table2_Col2Col4",
                        Type = ConstraintType.ForeignKey,
                        Columns = new List<string>() { "Col1", "Col2" },
                        RefTable = "Table2",
                        RefColumns = new List<string>() { "Col2", "Col4" },
                    },
                    new ConstraintInfo()
                    {
                        Name = "CK_Table1_Check1",
                        Type = ConstraintType.Check,
                        Code = "CHECK ([Col2] != 'Col2 NUMERIC NOT NULL DEFAULT 7.36,')",
                    },
                    new ConstraintInfo()
                    {
                        Name = null,
                        Type = ConstraintType.Check,
                        Code = "CHECK (Col4 = 'CONSTRAINT [CK_String_Check2] CHECK ( [Col3] >= 0 ),' AND f1(f2())=' quo''te g1(g2(g3)))' AND TRUE)",
                    },
                    new ConstraintInfo()
                    {
                        Name = "CK_Table1_Check3",
                        Type = ConstraintType.Check,
                        Code = "CHECK ([Col3] >= 0)",
                    },
                }
            };
        }

        private static ViewInfo GetExpectedViewModel()
        {
            return new()
            {
                Name = "MyView1",
                Code = File.ReadAllText(@$"{TestDataDir}/CreateView.sql"),
            };
        }

        private static IndexInfo GetExpectedIndexModel()
        {
            return new()
            {
                Name = "IDX_SomeTable1",
                Table = "Contacts",
                Unique = true,
                Columns = new List<string>() { "Email", "phone" },
            };
        }

        private static TriggerInfo GetExpectedTriggerModel()
        {
            return new()
            {
                Name = "TR_MyTable2_MyTrigger1",
                Table = "MyTable2",
                Code = File.ReadAllText(@$"{TestDataDir}/CreateTrigger.sql"),
            };
        }
    }
}
