﻿using System;
using System.Collections.Generic;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.UnitTests.CodeParsing.Base;

namespace DotNetDBTools.UnitTests.CodeParsing.SQLite;

public class SQLiteCodeParserTestsData : BaseCodeParserTestsData
{
    public override string TestDataDir => "./TestData/SQLite";
    public override TableInfo ExpectedTable => GetExpectedTable();
    public override ViewInfo ExpectedView => GetExpectedView();
    public override IndexInfo ExpectedIndex => GetExpectedIndex();
    public override TriggerInfo ExpectedTrigger => GetExpectedTrigger();

    private TableInfo GetExpectedTable()
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

    private ViewInfo GetExpectedView()
    {
        return new()
        {
            ID = new Guid("3C36AE77-B7E4-40C3-824F-BD20DC270A14"),
            Name = "MyView1",
            Code = ReadStatementFromFileWithoutIdDeclarations($@"{TestDataDir}/CreateView.sql"),
        };
    }

    private IndexInfo GetExpectedIndex()
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

    private TriggerInfo GetExpectedTrigger()
    {
        return new()
        {
            ID = new Guid("2C36AE77-B7E4-40C3-824F-BD20DC270A14"),
            Name = "TR_MyTable2_MyTrigger1",
            Table = "MyTable2",
            Code = ReadStatementFromFileWithoutIdDeclarations($@"{TestDataDir}/CreateTrigger.sql"),
        };
    }
}