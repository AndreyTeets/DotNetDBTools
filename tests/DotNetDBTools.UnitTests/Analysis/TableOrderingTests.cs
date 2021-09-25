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
            IEnumerable<TableInfo> tables = new List<AgnosticTableInfo>()
            {
                CreateTableInfo("T01", new string[] { }),
                CreateTableInfo("T02", new string[] { }),
                CreateTableInfo("T03", new string[] { }),
                CreateTableInfo("T04", new string[] { }),
                CreateTableInfo("T05", new string[] { }),
                CreateTableInfo("T06", new string[] { "T01", "T02", "T07", "T09", "T10" }),
                CreateTableInfo("T07", new string[] { "T02", "T03" }),
                CreateTableInfo("T08", new string[] { "T05" }),
                CreateTableInfo("T09", new string[] { "T07" }),
                CreateTableInfo("T10", new string[] { "T09" }),
                CreateTableInfo("T11", new string[] { "T09" }),
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
            IEnumerable<TableInfo> tables = new List<AgnosticTableInfo>();

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
            IEnumerable<TableInfo> tables = new List<AgnosticTableInfo>()
            {
                CreateTableInfo("T01", new string[] { "T02" }),
                CreateTableInfo("T02", new string[] { "T03" }),
                CreateTableInfo("T03", new string[] { "T01" }),
            };

            string expectedExceptionMessageWildcard = "Invalid table references graph*";
            tables.Invoking(x => x.PutReferencedFirst()).Should().Throw<Exception>().WithMessage(expectedExceptionMessageWildcard);
            tables.Invoking(x => x.PutReferencedLast()).Should().Throw<Exception>().WithMessage(expectedExceptionMessageWildcard);
        }

        private static AgnosticTableInfo CreateTableInfo(string tableName, string[] referencedTableNames)
        {
            List<ForeignKeyInfo> foreignKeys = new();
            foreach (string referencedTableName in referencedTableNames)
            {
                ForeignKeyInfo foreignKeyInfo = new()
                {
                    ReferencedTableName = referencedTableName,
                };
                foreignKeys.Add(foreignKeyInfo);
            }

            AgnosticTableInfo tableInfo = new()
            {
                Name = tableName,
                ForeignKeys = foreignKeys,
            };

            return tableInfo;
        }
    }
}
