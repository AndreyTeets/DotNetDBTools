using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.ModelBuilders
{
    internal static class PrimaryKeysBuilder
    {
        public static void BuildTablesPrimaryKeys(
            Dictionary<string, Table> tables,
            IEnumerable<PrimaryKeyRecord> primaryKeyRecords)
        {
            Dictionary<string, SortedDictionary<int, string>> columnNames = new();
            foreach (PrimaryKeyRecord pkr in primaryKeyRecords)
            {
                if (!columnNames.ContainsKey(pkr.ConstraintName))
                    columnNames.Add(pkr.ConstraintName, new SortedDictionary<int, string>());

                columnNames[pkr.ConstraintName].Add(
                    pkr.ColumnPosition, pkr.ColumnName);

                tables[pkr.TableName].PrimaryKey = MapExceptColumnsToPrimaryKeyModel(pkr);
            }

            foreach (Table table in tables.Values)
            {
                if (table.PrimaryKey is null)
                    continue;
                table.PrimaryKey.Columns = columnNames[table.PrimaryKey.Name].Select(x => x.Value).ToList();
            }
        }

        private static PrimaryKey MapExceptColumnsToPrimaryKeyModel(PrimaryKeyRecord pkr)
        {
            return new PrimaryKey()
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
