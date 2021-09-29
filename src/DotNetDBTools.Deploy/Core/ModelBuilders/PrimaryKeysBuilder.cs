using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.ModelBuilders
{
    internal static class PrimaryKeysBuilder
    {
        public static void BuildTablesPrimaryKeys(
            Dictionary<string, TableInfo> tables,
            IEnumerable<PrimaryKeyRecord> primaryKeyRecords)
        {
            Dictionary<string, SortedDictionary<int, string>> columnNames = new();
            foreach (PrimaryKeyRecord pkr in primaryKeyRecords)
            {
                if (!columnNames.ContainsKey(pkr.ConstraintName))
                    columnNames.Add(pkr.ConstraintName, new SortedDictionary<int, string>());

                columnNames[pkr.ConstraintName].Add(
                    pkr.ColumnPosition, pkr.ColumnName);

                PrimaryKeyInfo pk = MapExceptColumnsToPrimaryKeyInfo(pkr);
                tables[pkr.TableName].PrimaryKey = MapExceptColumnsToPrimaryKeyInfo(pkr);
            }

            foreach (TableInfo table in tables.Values)
            {
                if (table.PrimaryKey is null)
                    continue;
                table.PrimaryKey.Columns = columnNames[table.PrimaryKey.Name].Select(x => x.Value).ToList();
            }
        }

        private static PrimaryKeyInfo MapExceptColumnsToPrimaryKeyInfo(PrimaryKeyRecord pkr)
        {
            return new PrimaryKeyInfo()
            {
                ID = Guid.NewGuid(),
                Name = pkr.ConstraintName,
            };
        }

        internal class PrimaryKeyRecord
        {
            public string TableName { get; set; }
            public string ConstraintName { get; set; }
            public string ColumnName { get; set; }
            public int ColumnPosition { get; set; }
        }
    }
}
