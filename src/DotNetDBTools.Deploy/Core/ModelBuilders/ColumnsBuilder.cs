using System;
using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.ModelBuilders
{
    internal static class ColumnsBuilder
    {
        public static Dictionary<string, Table> BuildTablesListWithColumns<TTable>(
            IEnumerable<ColumnRecord> columnRecords,
            Func<ColumnRecord, Column> mapToColumnModel)
            where TTable : Table, new()
        {
            Dictionary<string, Table> tables = new();
            foreach (ColumnRecord columnRecord in columnRecords)
            {
                if (!tables.ContainsKey(columnRecord.TableName))
                {
                    TTable table = new()
                    {
                        ID = Guid.NewGuid(),
                        Name = columnRecord.TableName,
                        Columns = new List<Column>(),
                        UniqueConstraints = new List<UniqueConstraint>(),
                        CheckConstraints = new List<CheckConstraint>(),
                        Indexes = new List<Index>(),
                        Triggers = new List<Trigger>(),
                        ForeignKeys = new List<ForeignKey>(),
                    };
                    tables.Add(columnRecord.TableName, table);
                }
                Column column = mapToColumnModel(columnRecord);
                ((List<Column>)tables[columnRecord.TableName].Columns).Add(column);
            }
            return tables;
        }

        internal class ColumnRecord
        {
            public string TableName { get; set; }
            public string ColumnName { get; set; }
            public string DataType { get; set; }
            public bool Nullable { get; set; }
            public string Default { get; set; }
        }
    }
}
