using System;
using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;
using DotNetDBTools.UnitTests.DefinitionParsing.Base;
using DotNetDBTools.UnitTests.Utilities;

namespace DotNetDBTools.UnitTests.DefinitionParsing.SQLite;

public class SQLiteBuildSqlDefDbModelTests : BaseBuildSqlDefDbModelTests<SQLiteDatabase>
{
    private const string TestDataDir = "./TestData/SQLite/Parsing";
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.SQLite";
    protected override string SpecificDbmsSampleDbV2SqlDefAssemblyName => "DotNetDBTools.SampleDBv2SqlDef.SQLite";

    protected override List<string> ListOfSqlStatementsForDbModelCreation => new()
    {
        MiscHelper.ReadFromFile($"{TestDataDir}/CreateTable.sql"),
        MiscHelper.ReadFromFile($"{TestDataDir}/CreateView.sql"),
    };
    protected override DatabaseKind DatabaseKindForDbModelCreation => DatabaseKind.SQLite;
    protected override SQLiteDatabase ExpectedDbModelFromListOfSqlStatements => new()
    {
        Version = 3,
        Tables = new()
        {
            new SQLiteTable()
            {
                ID = new Guid("4C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                Name = "Table1",
                Columns = new()
                {
                    new Column()
                    {
                        ID = new Guid("5C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "Col1",
                        DataType = new DataType() { Name = "INTEGER" },
                        Default = new CodePiece() { Code = "15" },
                        NotNull = true,
                    },
                    new Column()
                    {
                        ID = new Guid("6C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "Col2",
                        DataType = new DataType() { Name = "NUMERIC" },
                        Default = new CodePiece() { Code = "7.36" },
                    },
                    new Column()
                    {
                        ID = new Guid("7C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "Col3",
                        DataType = new DataType() { Name = "INTEGER" },
                        NotNull = true,
                    },
                    new Column()
                    {
                        ID = new Guid("8C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "Col4",
                        DataType = new DataType() { Name = "TEXT" },
                        Default = new CodePiece() { Code = "'CONSTRAINT CK_String_Check1 CHECK (Col3 >= 0),'" },
                        NotNull = true,
                    },
                    new Column()
                    {
                        ID = new Guid("9C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "Col5",
                        DataType = new DataType() { Name = "BLOB" },
                        Default = new CodePiece() { Code = "(DATETIME('now'))" },
                        NotNull = true,
                    },
                },
                PrimaryKey = new()
                {
                    ID = new Guid("A136AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "PK_Table1",
                    Columns = new() { "Col1", "Col2" },
                },
                UniqueConstraints = new()
                {
                    new UniqueConstraint()
                    {
                        ID = new Guid("A236AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "UQ_Table1_Col1",
                        Columns = new() { "Col1" },
                    },
                    new UniqueConstraint()
                    {
                        ID = new Guid("A336AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "UQ_Table1_Col2Col4",
                        Columns = new() { "Col2", "Col4" },
                    },
                },
                CheckConstraints = new()
                {
                    new CheckConstraint()
                    {
                        ID = new Guid("A636AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "CK_Table1_Check1",
                        Expression = new CodePiece() { Code = "[Col2] != 'Col2 NUMERIC NOT NULL DEFAULT 7.36,'" },
                    },
                    new CheckConstraint()
                    {
                        ID = new Guid("A736AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = null,
                        Expression = new CodePiece() { Code = "Col4 = 'CONSTRAINT [CK_String_Check2] CHECK ( [Col3] >= 0 ),' AND f1(f2())=' quo''te g1(g2(g3)))' AND TRUE" },
                    },
                    new CheckConstraint()
                    {
                        ID = new Guid("A836AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "CK_Table1_Check3",
                        Expression = new CodePiece() { Code = "[Col3] >= 0" },
                    },
                },
                ForeignKeys = new()
                {
                    new ForeignKey()
                    {
                        ID = new Guid("A436AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "FK_Table1_Col1_Table2_Col1",
                        ThisTableName = "Table1",
                        ThisColumnNames = new() { "Col1" },
                        ReferencedTableName = "Table2",
                        ReferencedTableColumnNames = new() { "Col1" },
                        OnUpdate = "NO ACTION",
                        OnDelete = "CASCADE",
                    },
                    new ForeignKey()
                    {
                        ID = new Guid("A536AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "FK_Table1_Col1Col2_Table2_Col2Col4",
                        ThisTableName = "Table1",
                        ThisColumnNames = new() { "Col1", "Col2" },
                        ReferencedTableName = "Table2",
                        ReferencedTableColumnNames = new() { "Col2", "Col4" },
                        OnUpdate = "NO ACTION",
                        OnDelete = "NO ACTION",
                    },
                },
            },
        },
        Views = new()
        {
            new SQLiteView()
            {
                ID = new Guid("3C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                Name = "MyView1",
                CreateStatement = new CodePiece()
                {
                    Code = MiscHelper.ReadFromFileWithoutIdDeclarations($"{TestDataDir}/CreateView.sql"),
                },
            },
        },
    };
}
