using System;
using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.UnitTests.DefinitionParsing.Base;
using DotNetDBTools.UnitTests.Utilities;

namespace DotNetDBTools.UnitTests.DefinitionParsing.PostgreSQL;

public class PostgreSQLBuildSqlDefDbModelTests : BaseBuildSqlDefDbModelTests
{
    private const string TestDataDir = "./TestData/PostgreSQL/Parsing";
    protected override string SpecificDbmsSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.PostgreSQL";
    protected override string SpecificDbmsSampleDbV2SqlDefAssemblyName => "DotNetDBTools.SampleDBv2SqlDef.PostgreSQL";

    protected override List<string> ListOfSqlStatementsForDbModelCreation => new()
    {
        MiscHelper.ReadFromFile($"{TestDataDir}/CreateTable.sql"),
        MiscHelper.ReadFromFile($"{TestDataDir}/CreateView.sql"),
    };
    protected override DatabaseKind DatabaseKindForDbModelCreation => DatabaseKind.PostgreSQL;
    protected override Database ExpectedDbModelFromListOfSqlStatements => new PostgreSQLDatabase()
    {
        Version = 3,
        Tables = new()
        {
            new PostgreSQLTable()
            {
                ID = new Guid("4C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                Name = "Table1",
                Columns = new()
                {
                    new PostgreSQLColumn()
                    {
                        ID = new Guid("5C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "Col1".ToLower(),
                        DataType = new DataType() { Name = "BIGINT" },
                        Default = new CodePiece() { Code = "15" },
                        NotNull = true,
                    },
                    new PostgreSQLColumn()
                    {
                        ID = new Guid("6C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "Col2",
                        DataType = new DataType() { Name = "DECIMAL(6,1)" },
                        Default = new CodePiece() { Code = "7.36" },
                    },
                    new PostgreSQLColumn()
                    {
                        ID = new Guid("7C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "Col3".ToLower(),
                        DataType = new DataType() { Name = "BIGINT" },
                        NotNull = true,
                        Identity = true,
                        IdentityGenerationKind = "ALWAYS",
                        IdentitySequenceOptions = new()
                        {
                            StartWith = 0,
                            IncrementBy = -3,
                            MinValue = -2222,
                            MaxValue = int.MaxValue,
                            Cache = 1,
                            Cycle = false,
                        },
                    },
                    new PostgreSQLColumn()
                    {
                        ID = new Guid("8C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "Col4".ToLower(),
                        DataType = new DataType() { Name = "TEXT" },
                        Default = new CodePiece() { Code = "'CONSTRAINT CK_String_Check1 CHECK (Col3 >= 0),'" },
                        NotNull = true,
                    },
                    new PostgreSQLColumn()
                    {
                        ID = new Guid("9C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "Col5",
                        DataType = new DataType() { Name = "TIMESTAMP" },
                        Default = new CodePiece() { Code = "( SomeFunc(123) )" },
                        NotNull = true,
                    },
                },
                PrimaryKey = new()
                {
                    ID = new Guid("A136AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "PK_Table1".ToLower(),
                    Columns = new() { "Col1".ToLower(), "Col2".ToLower() },
                },
                UniqueConstraints = new()
                {
                    new UniqueConstraint()
                    {
                        ID = new Guid("A236AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "UQ_Table1_Col1",
                        Columns = new() { "Col1".ToLower() },
                    },
                    new UniqueConstraint()
                    {
                        ID = new Guid("A336AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "UQ_Table1_Col2Col4".ToLower(),
                        Columns = new() { "Col2".ToLower(), "Col4" },
                    },
                },
                CheckConstraints = new()
                {
                    new CheckConstraint()
                    {
                        ID = new Guid("A636AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "CK_Table1_Check1".ToLower(),
                        Expression = new CodePiece() { Code = "Col2 != 'Col2 DECIMAL(6, 1) NOT NULL DEFAULT 7.36,'" },
                    },
                    new CheckConstraint()
                    {
                        ID = new Guid("A736AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = null,
                        Expression = new CodePiece() { Code = "Col4 = 'CONSTRAINT CK_String_Check2 CHECK ( Col3 >= 0 ),' AND f1(f2())=' quo''te g1(g2(g3)))' AND TRUE" },
                    },
                    new CheckConstraint()
                    {
                        ID = new Guid("A836AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "CK_Table1_Check3",
                        Expression = new CodePiece() { Code = @"""Col3"" >= 0" },
                    },
                },
                ForeignKeys = new()
                {
                    new ForeignKey()
                    {
                        ID = new Guid("A436AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "FK_Table1_Col1_Table2_Col1".ToLower(),
                        ThisTableName = "Table1",
                        ThisColumnNames = new() { "Col1".ToLower() },
                        ReferencedTableName = "Table2",
                        ReferencedTableColumnNames = new() { "Col1".ToLower() },
                        OnUpdate = "NO ACTION",
                        OnDelete = "CASCADE",
                    },
                    new ForeignKey()
                    {
                        ID = new Guid("A536AE77-B7E4-40C3-824F-BD20DC270A14"),
                        Name = "FK_Table1_Col1Col2_Table2_Col2Col4",
                        ThisTableName = "Table1",
                        ThisColumnNames = new() { "Col1".ToLower(), "Col2" },
                        ReferencedTableName = "Table2".ToLower(),
                        ReferencedTableColumnNames = new() { "Col2".ToLower(), "Col4".ToLower() },
                        OnUpdate = "NO ACTION",
                        OnDelete = "NO ACTION",
                    },
                },
            },
        },
        Views = new()
        {
            new PostgreSQLView()
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
