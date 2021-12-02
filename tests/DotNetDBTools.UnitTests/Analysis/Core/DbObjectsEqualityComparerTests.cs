using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Analysis.Core
{
    public class DbObjectsEqualityComparerTests
    {
        private readonly DbObjectsEqualityComparer _comparer = new();
        private readonly AgnosticTable _tableModel1 = CreateTemplateAgnosticTableModel();
        private AgnosticTable _tableModel2;

        [Fact]
        public void DbObjectsEqualityComparer_Equals_ReturnsTrue_ForTable_WhenModelsAreEqual()
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
        public void DbObjectsEqualityComparer_Equals_ReturnsFalse_ForTable_WhenModelsAreDifferent()
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
            _tableModel2.Columns.Skip(1).First().DataType = new AgnosticDataType()
            {
                Name = AgnosticDataTypeNames.String,
            };
            _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

            _tableModel2 = CreateTemplateAgnosticTableModel();
            _tableModel2.Columns.Skip(1).First().Default = 125;
            _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

            _tableModel2 = CreateTemplateAgnosticTableModel();
            _tableModel2.PrimaryKey = null;
            _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

            _tableModel2 = CreateTemplateAgnosticTableModel();
            _tableModel2.PrimaryKey = new PrimaryKey()
            {
                ID = new Guid("C51E94AF-3E2D-4D6A-840E-78B8C23C6BE8"),
                Name = "PK_T1NewName",
                Columns = new string[] { "C1", "C2" },
            };
            _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

            _tableModel2 = CreateTemplateAgnosticTableModel();
            _tableModel2.Indexes.First().IncludeColumns = new string[] { "C2" };
            _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();

            _tableModel2 = CreateTemplateAgnosticTableModel();
            _tableModel2.ForeignKeys = new List<ForeignKey>();
            _comparer.Equals(_tableModel1, _tableModel2).Should().BeFalse();
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
                        DataType = new AgnosticDataType()
                        {
                            Name = AgnosticDataTypeNames.String,
                            IsFixedLength = true,
                            Length = 1000,
                        },
                        Default = "testval1",
                        Nullable = true,
                    },
                    new Column()
                    {
                        ID = new Guid("33457F8A-AAA0-467A-A098-CE349157A493"),
                        Name = "C2",
                        DataType = new AgnosticDataType()
                        {
                            Name = AgnosticDataTypeNames.Int,
                        },
                        Default = 325,
                        Nullable = true,
                    }
                },
                PrimaryKey = new PrimaryKey()
                {
                    ID = new Guid("C51E94AF-3E2D-4D6A-840E-78B8C23C6BE8"),
                    Name = "PK_T1",
                    Columns = new string[] { "C1", "C2" },
                },
                UniqueConstraints = new List<UniqueConstraint>(),
                CheckConstraints = new List<CheckConstraint>()
                {
                    new CheckConstraint()
                    {
                        ID = new Guid("75D55104-DDF0-4F9D-B0B2-CF8F85A3A0A7"),
                        Name = "CK_T1_1",
                        Code = "C2 >= 0",
                    }
                },
                Indexes = new List<Models.Core.Index>()
                {
                    new Models.Core.Index()
                    {
                        ID = new Guid("65C65B34-E769-4826-8F06-B8E83BF7D06A"),
                        Name = "IDX_T1_1",
                        Columns = new string[] { "C1" },
                        IncludeColumns = null,
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
                        ThisColumnNames = new string[] { "C1", "C2" },
                        ReferencedTableName = "T2",
                        ReferencedTableColumnNames = new string[] { "C1", "C2" },
                        OnUpdate = "NoAction",
                        OnDelete = "SetNull",
                    }
                },
            };
        }
    }
}
