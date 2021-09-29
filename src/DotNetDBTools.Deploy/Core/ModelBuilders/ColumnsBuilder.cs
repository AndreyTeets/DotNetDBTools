using System;
using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.ModelBuilders
{
    internal static class ColumnsBuilder
    {
        public static Dictionary<string, TableInfo> BuildTablesListWithColumns<TTable>(
            IEnumerable<ColumnRecord> columnRecords,
            Func<ColumnRecord, ColumnInfo> mapToColumnInfo)
            where TTable : TableInfo, new()
        {
            Dictionary<string, TableInfo> tables = new();
            foreach (ColumnRecord columnRecord in columnRecords)
            {
                if (!tables.ContainsKey(columnRecord.TableName))
                {
                    TTable table = new()
                    {
                        ID = Guid.NewGuid(),
                        Name = columnRecord.TableName,
                        Columns = new List<ColumnInfo>(),
                        UniqueConstraints = new List<UniqueConstraintInfo>(),
                        CheckConstraints = new List<CheckConstraintInfo>(),
                        Indexes = new List<IndexInfo>(),
                        Triggers = new List<TriggerInfo>(),
                        ForeignKeys = new List<ForeignKeyInfo>(),
                    };
                    tables.Add(columnRecord.TableName, table);
                }
                ColumnInfo columnInfo = mapToColumnInfo(columnRecord);
                ((List<ColumnInfo>)tables[columnRecord.TableName].Columns).Add(columnInfo);
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
