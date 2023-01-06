using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Analysis.Core;

public class DNDBTModelsEqualityComparerTests
{
    private readonly IAnalysisManager _analysisManager = new AnalysisManager();
    private readonly IDefinitionParsingManager _definitionParsingManager = new DefinitionParsingManager();
    private readonly DNDBTModelsEqualityComparer _comparer = new();
    private readonly AgnosticTable _tableModel1 = CreateTemplateAgnosticTableModel();
    private AgnosticTable _tableModel2;

    [Fact]
    public void DNDBTModelsEqualityComparer_Equals_ReturnsTrue_ForDatabase_WhenModelsAreEqual()
    {
        Assembly dbAssemblyV2 = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDBv2.Agnostic");
        Database database1 = _definitionParsingManager.CreateDbModel(dbAssemblyV2);
        Database database2 = _definitionParsingManager.CreateDbModel(dbAssemblyV2);
        _comparer.Equals(database1, database2).Should().BeTrue();
    }

    [Fact]
    public void DNDBTModelsEqualityComparer_Equals_ReturnsFalse_ForDatabase_WhenModelsAreDifferent()
    {
        Assembly dbAssembly = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDB.Agnostic");
        Assembly dbAssemblyV2 = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDBv2.Agnostic");
        Database database1 = _definitionParsingManager.CreateDbModel(dbAssembly);
        Database database2 = _definitionParsingManager.CreateDbModel(dbAssemblyV2);
        _comparer.Equals(database1, database2).Should().BeFalse();

        Database database3 = _definitionParsingManager.CreateDbModel(dbAssembly);
        database3.Version++;
        _comparer.Equals(database1, database3).Should().BeFalse();

        database3 = _definitionParsingManager.CreateDbModel(dbAssembly);
        ((AgnosticCodePiece)database3.Views.Single(x => x.Name == "MyView1").CodePiece)
                .DbKindToCodeMap[DatabaseKind.PostgreSQL] = "some other view code";
        _comparer.Equals(database1, database3).Should().BeFalse();
    }

    [Fact]
    public void DNDBTModelsEqualityComparer_Equals_ReturnsTrue_ForDbDiff_WhenModelsAreEqual()
    {
        Assembly dbAssemblyV1 = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDB.PostgreSQL");
        Assembly dbAssemblyV2 = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDBv2.PostgreSQL");
        Database databaseV1 = _definitionParsingManager.CreateDbModel(dbAssemblyV1);
        Database databaseV2 = _definitionParsingManager.CreateDbModel(dbAssemblyV2);
        DatabaseDiff dbDiff1 = _analysisManager.CreateDatabaseDiff(databaseV1, databaseV2);
        DatabaseDiff dbDiff2 = _analysisManager.CreateDatabaseDiff(databaseV1, databaseV2);
        _comparer.Equals(dbDiff1, dbDiff2).Should().BeTrue();
    }

    [Fact]
    public void DNDBTModelsEqualityComparer_Equals_ReturnsFalse_ForDbDiff_WhenModelsAreDifferent()
    {
        Assembly dbAssemblyV1 = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDB.SQLite");
        Assembly dbAssemblyV2 = TestDbAssembliesHelper.GetSampleDbAssembly("DotNetDBTools.SampleDBv2.SQLite");
        Database databaseV1 = _definitionParsingManager.CreateDbModel(dbAssemblyV1);
        Database databaseV2 = _definitionParsingManager.CreateDbModel(dbAssemblyV2);
        DatabaseDiff dbDiff1 = _analysisManager.CreateDatabaseDiff(databaseV1, databaseV2);
        DatabaseDiff dbDiff2 = _analysisManager.CreateDatabaseDiff(databaseV2, databaseV1);
        _comparer.Equals(dbDiff1, dbDiff2).Should().BeFalse();

        DatabaseDiff dbDiff3 = _analysisManager.CreateDatabaseDiff(databaseV1, databaseV2);
        dbDiff3.NewDatabase = databaseV2;
        _comparer.Equals(dbDiff1, dbDiff3).Should().BeFalse();

        dbDiff3 = _analysisManager.CreateDatabaseDiff(databaseV1, databaseV2);
        dbDiff3.ChangedTables.Single(x => x.NewTable.Name == "MyTable2").ChangedColumns = new List<ColumnDiff>();
        _comparer.Equals(dbDiff1, dbDiff3).Should().BeFalse();
    }

    [Fact]
    public void DNDBTModelsEqualityComparer_Equals_ReturnsTrue_ForTable_WhenModelsAreEqual()
    {
        _tableModel2 = CreateTemplateAgnosticTableModel();
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeTrue();

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.PrimaryKey = new PrimaryKey()
        {
            ID = new Guid("C51E94AF-3E2D-4D6A-840E-78B8C23C6BE8"),
            Name = "PK_T1",
            Columns = new List<string> { "C1", "C2" },
        };
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeTrue();
    }

    [Fact]
    public void DNDBTModelsEqualityComparer_Equals_ReturnsFalse_ForTable_WhenModelsAreDifferent()
    {
        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.ID = new Guid("BC255E91-E83B-4D8F-AA83-DDF558D901C7");
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.Name = "T1NewName";
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.Columns.Skip(1).First().ID = new Guid("BC255E91-E83B-4D8F-AA83-DDF558D901C7");
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.Columns.Skip(1).First().Name = "C2NewName";
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.Columns.Skip(1).First().DataType = new CSharpDataType()
        {
            Name = CSharpDataTypeNames.String,
        };
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.Columns.Skip(1).First().Default.Code = "125";
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.PrimaryKey = null;
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.PrimaryKey = new PrimaryKey()
        {
            ID = new Guid("C51E94AF-3E2D-4D6A-840E-78B8C23C6BE8"),
            Name = "PK_T1NewName",
            Columns = new List<string>() { "C1", "C2" },
        };
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.Indexes.First().IncludeColumns = new List<string>() { "C2" };
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.ForeignKeys = new List<ForeignKey>();
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();
    }

    [Fact]
    public void DNDBTModelsEqualityComparer_Equals_IgnoresPropertiesCorrectly()
    {
        _comparer.IgnoredProperties.Add(new PropInfo { Name = "ID", DeclaringTypeName = nameof(DbObject) });
        _comparer.IgnoredProperties.Add(new PropInfo { Name = "Name", DeclaringTypeName = nameof(Database) });
        _comparer.IgnoredProperties.Add(new PropInfo { Name = "Code" });

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.ID = new Guid("00255E91-E83B-4D8F-AA83-DDF558D901C7");
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeTrue();

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.Name = "T1NewName";
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.Columns.Skip(1).First().ID = new Guid("00255E91-E83B-4D8F-AA83-DDF558D901C7");
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeTrue();

        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.CheckConstraints.First().CodePiece = new CodePiece { Code = "other code value" };
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeTrue();
    }

    [Fact]
    public void DNDBTModelsEqualityComparer_Equals_CreatesCorrectDiffLog()
    {
        _tableModel2 = CreateTemplateAgnosticTableModel();
        _tableModel2.PrimaryKey = new PrimaryKey()
        {
            ID = new Guid("C51E94AF-3E2D-4D6A-840E-78B8C23C6BE8"),
            Name = "PK_T1NewName",
            Columns = new List<string>() { "C1", "C2" },
        };
        _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

        string expectedDiffLog =
@"Values [PK_T1] and [PK_T1NewName] differ at place [ValueOfSimpleProperty:Name].
Values [DotNetDBTools.Models.Core.PrimaryKey] and [DotNetDBTools.Models.Core.PrimaryKey] differ at place [ComplexValueOfComplexProperty:PrimaryKey].";
        _comparer.DiffLog.Should().Be(expectedDiffLog.NormalizeLineEndings());
    }

    private static AgnosticTable CreateTemplateAgnosticTableModel()
    {
        return new()
        {
            ID = new Guid("7E0B0953-283C-4CA0-8A99-A00AF7B2D9F8"),
            Name = "T1",
            Columns = new List<Column>()
            {
                new Column()
                {
                    ID = new Guid("33457F8A-AAA0-467A-A098-CE349157A493"),
                    Name = "C1",
                    DataType = new CSharpDataType()
                    {
                        Name = CSharpDataTypeNames.String,
                        IsFixedLength = true,
                        Length = 1000,
                    },
                    Default = new CodePiece { Code = "testval1" },
                },
                new Column()
                {
                    ID = new Guid("33457F8A-AAA0-467A-A098-CE349157A493"),
                    Name = "C2",
                    DataType = new CSharpDataType()
                    {
                        Name = CSharpDataTypeNames.Int,
                    },
                    NotNull = true,
                    Default = new CodePiece { Code = "325" },
                }
            },
            PrimaryKey = new PrimaryKey()
            {
                ID = new Guid("C51E94AF-3E2D-4D6A-840E-78B8C23C6BE8"),
                Name = "PK_T1",
                Columns = new List<string>() { "C1", "C2" },
            },
            UniqueConstraints = new List<UniqueConstraint>(),
            CheckConstraints = new List<CheckConstraint>()
            {
                new CheckConstraint()
                {
                    ID = new Guid("75D55104-DDF0-4F9D-B0B2-CF8F85A3A0A7"),
                    Name = "CK_T1_1",
                    CodePiece = new CodePiece { Code = "CHECK (C2 >= 0)" },
                }
            },
            Indexes = new List<DotNetDBTools.Models.Core.Index>()
            {
                new AgnosticIndex()
                {
                    ID = new Guid("65C65B34-E769-4826-8F06-B8E83BF7D06A"),
                    Name = "IDX_T1_1",
                    TableName = "T1",
                    Columns = new List<string>() { "C1" },
                    IncludeColumns = new List<string>(),
                    Unique = true,
                }
            },
            Triggers = new List<Trigger>(),
            ForeignKeys = new List<ForeignKey>()
            {
                new ForeignKey()
                {
                    ID = new Guid("D23109D4-CB94-40B2-BDB2-BC6292F4A5FA"),
                    Name = "FK_T1_1",
                    ThisTableName = "T1",
                    ThisColumnNames = new List<string>() { "C1", "C2" },
                    ReferencedTableName = "T2",
                    ReferencedTableColumnNames = new List<string>() { "C1", "C2" },
                    OnUpdate = "NO ACTION",
                    OnDelete = "SET NULL",
                }
            },
        };
    }
}
