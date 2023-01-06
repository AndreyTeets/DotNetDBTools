using System;
using System.Collections.Generic;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.UnitTests.CodeParsing.Base;
using DotNetDBTools.UnitTests.Utilities;

namespace DotNetDBTools.UnitTests.CodeParsing.PostgreSQL;

public class PostgreSQLCodeParserTestsData : BaseCodeParserTestsData
{
    public override string TestDataDir => "./TestData/PostgreSQL";
    public override TableInfo ExpectedTable => GetExpectedTable();
    public override TableInfo ExpectedTableWithPkColumn => GetExpectedTableWithPkColumn();
    public override ViewInfo ExpectedView => GetExpectedView();
    public override IndexInfo ExpectedIndex => GetExpectedIndex();
    public override TriggerInfo ExpectedTrigger => GetExpectedTrigger();
    public TypeInfo ExpectedCompositeType => GetExpectedCompositeType();
    public TypeInfo ExpectedDomainType => GetExpectedDomainType();
    public TypeInfo ExpectedEnumType => GetExpectedEnumType();
    public TypeInfo ExpectedRangeType => GetExpectedRangeType();
    public FunctionInfo ExpectedSQLFunction => GetExpectedSQLFunction();
    public FunctionInfo ExpectedPLPGSQLFunction => GetExpectedPLPGSQLFunction();

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
                    Name = "Col1".ToLower(),
                    DataType = "bigINT",
                    NotNull = true,
                    Default = "15",
                    Unique = true,
                },
                new ColumnInfo()
                {
                    ID = new Guid("6C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "Col2",
                    DataType = "decIMAL ( 6, 1 )",
                    Default = "7.36",
                },
                new ColumnInfo()
                {
                    ID = new Guid("7C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "Col3".ToLower(),
                    DataType = "BIGINT",
                    NotNull = true,
                    Identity = true,
                },
                new ColumnInfo()
                {
                    ID = new Guid("8C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "Col4".ToLower(),
                    DataType = "text",
                    NotNull = true,
                    Default = "'CONSTRAINT CK_String_Check1 CHECK (Col3 >= 0),'",
                },
                new ColumnInfo()
                {
                    ID = new Guid("9C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "Col5",
                    DataType = "TIMESTAMP",
                    NotNull = true,
                    Default = "( SomeFunc(123) )",
                },
            },
            Constraints = new List<ConstraintInfo>()
            {
                new ConstraintInfo()
                {
                    ID = new Guid("A136AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "PK_Table1".ToLower(),
                    Type = ConstraintType.PrimaryKey,
                    Columns = new List<string>() { "Col1".ToLower(), "Col2".ToLower() },
                },
                new ConstraintInfo()
                {
                    ID = new Guid("A236AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "UQ_Table1_Col1",
                    Type = ConstraintType.Unique,
                    Columns = new List<string>() { "Col1".ToLower() },
                },
                new ConstraintInfo()
                {
                    ID = new Guid("A336AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "UQ_Table1_Col2Col4".ToLower(),
                    Type = ConstraintType.Unique,
                    Columns = new List<string>() { "Col2".ToLower(), "Col4" },
                },
                new ConstraintInfo()
                {
                    ID = new Guid("A436AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "FK_Table1_Col1_Table2_Col1".ToLower(),
                    Type = ConstraintType.ForeignKey,
                    Columns = new List<string>() { "Col1".ToLower() },
                    RefTable = "Table2",
                    RefColumns = new List<string>() { "Col1".ToLower() },
                    UpdateAction = "NO actION",
                    DeleteAction = "casCADE",
                },
                new ConstraintInfo()
                {
                    ID = new Guid("A536AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "FK_Table1_Col1Col2_Table2_Col2Col4",
                    Type = ConstraintType.ForeignKey,
                    Columns = new List<string>() { "Col1".ToLower(), "Col2" },
                    RefTable = "Table2".ToLower(),
                    RefColumns = new List<string>() { "Col2".ToLower(), "Col4".ToLower() },
                },
                new ConstraintInfo()
                {
                    ID = new Guid("A636AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "CK_Table1_Check1".ToLower(),
                    Type = ConstraintType.Check,
                    Code = "CHECK (Col2 != 'Col2 DECIMAL(6, 1) NOT NULL DEFAULT 7.36,')",
                },
                new ConstraintInfo()
                {
                    ID = new Guid("A736AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = null,
                    Type = ConstraintType.Check,
                    Code = "CHECK (Col4 = 'CONSTRAINT CK_String_Check2 CHECK ( Col3 >= 0 ),' AND f1(f2())=' quo''te g1(g2(g3)))' AND TRUE)",
                },
                new ConstraintInfo()
                {
                    ID = new Guid("A836AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "CK_Table1_Check3",
                    Type = ConstraintType.Check,
                    Code = @"CHECK (""Col3"" >= 0)",
                },
            }
        };
    }

    private TableInfo GetExpectedTableWithPkColumn()
    {
        return new()
        {
            ID = new Guid("4C36AE77-B7E4-40C3-824F-BD20DC270A14"),
            Name = "MyTableWithPkColumn".ToLower(),
            Columns = new List<ColumnInfo>()
            {
                new ColumnInfo()
                {
                    ID = new Guid("5C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "Col1".ToLower(),
                    DataType = "BIGINT",
                    NotNull = true,
                    Default = "15",
                    Unique = true,
                },
                new ColumnInfo()
                {
                    ID = new Guid("7C36AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = "Col3".ToLower(),
                    DataType = "BIGINT",
                    NotNull = true,
                    Identity = true,
                    PrimaryKey = true,
                },
            },
            Constraints = new List<ConstraintInfo>()
            {
                new ConstraintInfo()
                {
                    ID = new Guid("A836AE77-B7E4-40C3-824F-BD20DC270A14"),
                    Name = null,
                    Type = ConstraintType.Check,
                    Code = @"CHECK (""Col3"" >= 0)",
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
            Code = MiscHelper.ReadFromFileWithoutIdDeclarations($@"{TestDataDir}/CreateView.sql"),
        };
    }

    private IndexInfo GetExpectedIndex()
    {
        return new()
        {
            ID = new Guid("1C36AE77-B7E4-40C3-824F-BD20DC270A14"),
            Name = "IDX_SomeTable1",
            Table = "Contacts".ToLower(),
            Unique = true,
            Columns = new List<string>() { "Email".ToLower(), "phone" },
        };
    }

    private TriggerInfo GetExpectedTrigger()
    {
        return new()
        {
            ID = new Guid("2C36AE77-B7E4-40C3-824F-BD20DC270A14"),
            Name = "TR_MyTable2_MyTrigger1".ToLower(),
            Table = "MyTable2",
            Code = MiscHelper.ReadFromFileWithoutIdDeclarations($@"{TestDataDir}/CreateTrigger.sql"),
        };
    }

    private TypeInfo GetExpectedCompositeType()
    {
        return new()
        {
            ID = new Guid("145E059B-F65C-4CDC-8846-953302544703"),
            Name = "MyCompositeType1",
            TypeType = TypeType.Composite,
            Attributes = new Dictionary<string, string>()
            {
                { "MyAttribute1".ToLower(), "VARCHAR (110)" },
                { "MyAttribute2", "INT".ToLower() },
            },
        };
    }

    private TypeInfo GetExpectedDomainType()
    {
        return new()
        {
            ID = new Guid("4B92637E-F6C8-42B8-99CB-4B988D98CAEE"),
            Name = "MyDomain1",
            TypeType = TypeType.Domain,
            UnderlyingType = "decimal(6, 1)",
            NotNull = true,
            Default = "abs(-33)",
            CheckConstraints = new List<ConstraintInfo>()
            {
                new ConstraintInfo()
                {
                    ID = new Guid("3C7DF430-DDC3-4EE7-93CC-70E7427E7937"),
                    Name = null, // TODO set {type.Name}_CKsomething
                    Type = ConstraintType.Check,
                    Code = @"CHECK (value = lower(value) || 'CHECK (TRUE)')",
                },
                new ConstraintInfo()
                {
                    ID = new Guid("960EFE55-2985-4057-8A83-EF7F5FF6C3CA"),
                    Name = "MyDomain1_CK2",
                    Type = ConstraintType.Check,
                    Code = @"check (char_length(value) > 3)",
                },
            },
        };
    }

    private TypeInfo GetExpectedEnumType()
    {
        return new()
        {
            ID = new Guid("2678DE76-0BA4-4794-BB29-C072FA4A60E9"),
            Name = "MyEnumType1",
            TypeType = TypeType.Enum,
            AllowedValues = new List<string>() { "Label1", "Label2" },
        };
    }

    private TypeInfo GetExpectedRangeType()
    {
        return new()
        {
            ID = new Guid("3B48FE5B-E812-4359-96A6-0FEA4613CBB2"),
            Name = "MyRangeType1",
            TypeType = TypeType.Range,
            Subtype = "floAT8",
            SubtypeOperatorClass = "TIMESTAMP_OPS",
            Collation = "C",
            CanonicalFunction = "some_func",
            SubtypeDiff = "float8mi",
            MultirangeTypeName = "MyRangeType1_multirange",
        };
    }

    private FunctionInfo GetExpectedSQLFunction()
    {
        return new()
        {
            ID = new Guid("FE72177E-52D8-48E3-975E-408AF5A1A44B"),
            Name = "TR_MyTable2_MyTrigger1_Handler",
            Code = MiscHelper.ReadFromFileWithoutIdDeclarations($@"{TestDataDir}/CreateSQLFunction.sql"),
        };
    }

    private FunctionInfo GetExpectedPLPGSQLFunction()
    {
        return new()
        {
            ID = new Guid("316C7688-D510-4A61-9D09-E15D465D0EFF"),
            Name = "public._Some_Complex_PLPGSQL_Function",
            Code = MiscHelper.ReadFromFileWithoutIdDeclarations($@"{TestDataDir}/CreatePLPGSQLFunction.sql"),
        };
    }
}
