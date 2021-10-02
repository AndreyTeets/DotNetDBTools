using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Analysis
{
    public class TableOrderingTests
    {
        [Fact]
        public void TableOrderingExtensions_ProduceCorrectResultOnValidNonEmptyInput()
        {
            IEnumerable<Table> tables = new List<AgnosticTable>()
            {
                CreateTableModel("T01", new string[] { }),
                CreateTableModel("T02", new string[] { }),
                CreateTableModel("T03", new string[] { }),
                CreateTableModel("T04", new string[] { }),
                CreateTableModel("T05", new string[] { }),
                CreateTableModel("T06", new string[] { "T01", "T02", "T07", "T09", "T10" }),
                CreateTableModel("T07", new string[] { "T02", "T03" }),
                CreateTableModel("T08", new string[] { "T05" }),
                CreateTableModel("T09", new string[] { "T07" }),
                CreateTableModel("T10", new string[] { "T09" }),
                CreateTableModel("T11", new string[] { "T09" }),
            };

            string[] expectedReferencedFirstOrder = new string[]
            {
                "T02", "T03",
                "T07",
                "T09",
                "T01", "T05", "T10",
                "T04", "T06", "T08", "T11",
            };
            string[] expectedReferencedLastOrder = new string[]
            {
                "T04", "T06", "T08", "T11",
                "T01", "T05", "T10",
                "T09",
                "T07",
                "T02", "T03",
            };

            IEnumerable<string> referencedFirstTablesNames = tables.PutReferencedFirst().Select(x => x.Name);
            IEnumerable<string> referencedLastTablesNames = tables.PutReferencedLast().Select(x => x.Name);

            referencedFirstTablesNames.Should().BeEquivalentTo(expectedReferencedFirstOrder, options => options.WithStrictOrdering());
            referencedLastTablesNames.Should().BeEquivalentTo(expectedReferencedLastOrder, options => options.WithStrictOrdering());
        }

        [Fact]
        public void TableOrderingExtensions_ProduceCorrectResultOnEmptyInput()
        {
            IEnumerable<Table> tables = new List<AgnosticTable>();

            string[] expectedReferencedFirstOrder = new string[] { };
            string[] expectedReferencedLastOrder = new string[] { };

            IEnumerable<string> referencedFirstTablesNames = tables.PutReferencedFirst().Select(x => x.Name);
            IEnumerable<string> referencedLastTablesNames = tables.PutReferencedLast().Select(x => x.Name);

            referencedFirstTablesNames.Should().BeEquivalentTo(expectedReferencedFirstOrder, options => options.WithStrictOrdering());
            referencedLastTablesNames.Should().BeEquivalentTo(expectedReferencedLastOrder, options => options.WithStrictOrdering());
        }

        [Fact]
        public void TableOrderingExtensions_ThrowOnInvalidInput()
        {
            IEnumerable<Table> tables = new List<AgnosticTable>()
            {
                CreateTableModel("T01", new string[] { "T02" }),
                CreateTableModel("T02", new string[] { "T03" }),
                CreateTableModel("T03", new string[] { "T01" }),
            };

            string expectedExceptionMessageWildcard = "Invalid table references graph*";
            tables.Invoking(x => x.PutReferencedFirst()).Should().Throw<Exception>().WithMessage(expectedExceptionMessageWildcard);
            tables.Invoking(x => x.PutReferencedLast()).Should().Throw<Exception>().WithMessage(expectedExceptionMessageWildcard);
        }

        private static AgnosticTable CreateTableModel(string tableName, string[] referencedTableNames)
        {
            List<ForeignKey> foreignKeys = new();
            foreach (string referencedTableName in referencedTableNames)
            {
                ForeignKey fk = new()
                {
                    ReferencedTableName = referencedTableName,
                };
                foreignKeys.Add(fk);
            }

            AgnosticTable table = new()
            {
                Name = tableName,
                ForeignKeys = foreignKeys,
            };

            return table;
        }
    }
}
