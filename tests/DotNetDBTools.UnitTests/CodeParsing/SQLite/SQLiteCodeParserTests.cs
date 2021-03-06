using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.CodeParsing.SQLite;

public class SQLiteCodeParserTests
{
    private const string TestDataDir = "./TestData/SQLite";

    [Fact]
    public void GetObjectInfo_ParsesTableCorrectly()
    {
        string input = FilesHelper.GetFromFile($@"{TestDataDir}/CreateTable.sql");
        SQLiteCodeParser parser = new();
        TableInfo table = (TableInfo)parser.GetObjectInfo(input);

        TableInfo expectedTable = GetExpectedTable();
        table.Should().BeEquivalentTo(expectedTable, options => options.WithStrictOrdering());
    }

    [Fact]
    public void GetObjectInfo_ParsesViewCorrectly()
    {
        string input = FilesHelper.GetFromFile($@"{TestDataDir}/CreateView.sql");
        SQLiteCodeParser parser = new();
        ViewInfo view = (ViewInfo)parser.GetObjectInfo(input);

        ViewInfo expectedView = GetExpectedView();
        view.Should().BeEquivalentTo(expectedView, options => options.WithStrictOrdering());
    }

    [Fact]
    public void GetObjectInfo_ParsesIndexCorrectly()
    {
        string input = FilesHelper.GetFromFile($@"{TestDataDir}/CreateIndex.sql");
        SQLiteCodeParser parser = new();
        IndexInfo index = (IndexInfo)parser.GetObjectInfo(input);

        IndexInfo expectedIndex = GetExpectedIndex();
        index.Should().BeEquivalentTo(expectedIndex, options => options.WithStrictOrdering());
    }

    [Fact]
    public void GetObjectInfo_ParsesTriggerCorrectly()
    {
        string input = FilesHelper.GetFromFile($@"{TestDataDir}/CreateTrigger.sql");
        SQLiteCodeParser parser = new();
        TriggerInfo trigger = (TriggerInfo)parser.GetObjectInfo(input);

        TriggerInfo expectedTrigger = GetExpectedTrigger();
        trigger.Should().BeEquivalentTo(expectedTrigger, options => options.WithStrictOrdering());
    }

    [Fact]
    public void GetObjectInfo_ThrowsOnMalformedInput()
    {
        string input = "some trash input";
        SQLiteCodeParser parser = new();
        FluentActions.Invoking(() => parser.GetObjectInfo(input))
            .Should().Throw<ParseException>().WithMessage($"ParserError(line=1,pos=0): mismatched input 'some' *");
    }

    private static TableInfo GetExpectedTable()
    {
        return new()
        {
            ID = new Guid("4C36AE77-B7E4-40C3-824F-BD20DC270A14"),
            Name = "Table1",
            Columns = new List<ColumnInfo>()
            {
                new ColumnInfo()
                {
                    ID = new Guid("5C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "Col1",
                    DataType = "INTEGER",
                    NotNull = true,
                    Default = "15",
                    Unique = true,
                },
                new ColumnInfo()
                {
                    ID = new Guid("6C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "Col2",
                    DataType = "numeric",
                    Default = "7.36",
                },
                new ColumnInfo()
                {
                    ID = new Guid("7C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "Col3",
                    DataType = "INTEGER",
                    NotNull = true,
                    Identity = true,
                    PrimaryKey = true,
                },
                new ColumnInfo()
                {
                    ID = new Guid("8C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "Col4",
                    DataType = "TEXT",
                    NotNull = true,
                    Default = "'CONSTRAINT CK_String_Check1 CHECK (Col3 >= 0),'",
                },
                new ColumnInfo()
                {
                    ID = new Guid("9C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "Col5",
                    DataType = "BLOB",
                    NotNull = true,
                    Default = "(DATETIME('now'))",
                },
            },
            Constraints = new List<ConstraintInfo>()
            {
                new ConstraintInfo()
                {
                    ID = new Guid("A136AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "PK_Table1",
                    Type = ConstraintType.PrimaryKey,
                    Columns = new List<string>() { "Col1", "Col2" },
                },
                new ConstraintInfo()
                {
                    ID = new Guid("A236AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "UQ_Table1_Col1",
                    Type = ConstraintType.Unique,
                    Columns = new List<string>() { "Col1" },
                },
                new ConstraintInfo()
                {
                    ID = new Guid("A336AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "UQ_Table1_Col2Col4",
                    Type = ConstraintType.Unique,
                    Columns = new List<string>() { "Col2", "Col4" },
                },
                new ConstraintInfo()
                {
                    ID = new Guid("A436AE77-B7E4-40C3-824F-BD20DC270A14"),
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
                    ID = new Guid("A536AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "FK_Table1_Col1Col2_Table2_Col2Col4",
                    Type = ConstraintType.ForeignKey,
                    Columns = new List<string>() { "Col1", "Col2" },
                    RefTable = "Table2",
                    RefColumns = new List<string>() { "Col2", "Col4" },
                },
                new ConstraintInfo()
                {
                    ID = new Guid("A636AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "CK_Table1_Check1",
                    Type = ConstraintType.Check,
                    Code = "CHECK ([Col2] != 'Col2 NUMERIC NOT NULL DEFAULT 7.36,')",
                },
                new ConstraintInfo()
                {
                    ID = new Guid("A736AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = null,
                    Type = ConstraintType.Check,
                    Code = "CHECK (Col4 = 'CONSTRAINT [CK_String_Check2] CHECK ( [Col3] >= 0 ),' AND f1(f2())=' quo''te g1(g2(g3)))' AND TRUE)",
                },
                new ConstraintInfo()
                {
                    ID = new Guid("A836AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "CK_Table1_Check3",
                    Type = ConstraintType.Check,
                    Code = "CHECK ([Col3] >= 0)",
                },
            }
        };
    }

    private static ViewInfo GetExpectedView()
    {
        return new()
        {
            ID = new Guid("3C36AE77-B7E4-40C3-824F-BD20DC270A14"),
            Name = "MyView1",
            Code = ReadStatementFromFile($@"{TestDataDir}/CreateView.sql"),
        };
    }

    private static IndexInfo GetExpectedIndex()
    {
        return new()
        {
            ID = new Guid("1C36AE77-B7E4-40C3-824F-BD20DC270A14"),
            Name = "IDX_SomeTable1",
            Table = "Contacts",
            Unique = true,
            Columns = new List<string>() { "Email", "phone" },
        };
    }

    private static TriggerInfo GetExpectedTrigger()
    {
        return new()
        {
            ID = new Guid("2C36AE77-B7E4-40C3-824F-BD20DC270A14"),
            Name = "TR_MyTable2_MyTrigger1",
            Table = "MyTable2",
            Code = ReadStatementFromFile($@"{TestDataDir}/CreateTrigger.sql"),
        };
    }

    private static string ReadStatementFromFile(string filePath)
    {
        string fileContent = FilesHelper.GetFromFile(filePath);
        return RemoveIdDeclarations(RemoveSemicolonIfAny(fileContent));
    }

    private static string RemoveIdDeclarations(string input)
    {
        return Regex.Replace(input, @"--ID:#{[\w|-]{36}}#\r?\n", "");
    }

    private static string RemoveSemicolonIfAny(string input)
    {
        if (input.EndsWith(";", StringComparison.OrdinalIgnoreCase))
            return input.Remove(input.Length - 1, 1);
        else
            return input;
    }
}
