using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.ModelBuilders
{
    internal static class UniqueConstraintsBuilder
    {
        public static void BuildTablesUniqueConstraints(
            Dictionary<string, Table> tables,
            IEnumerable<UniqueConstraintRecord> uniqueConstraintRecords)
        {
            Dictionary<string, SortedDictionary<int, string>> columnNames = new();
            foreach (UniqueConstraintRecord ucr in uniqueConstraintRecords)
            {
                if (!columnNames.ContainsKey(ucr.ConstraintName))
                    columnNames.Add(ucr.ConstraintName, new SortedDictionary<int, string>());

                columnNames[ucr.ConstraintName].Add(
                    ucr.ColumnPosition, ucr.ColumnName);

                UniqueConstraint uc = MapExceptColumnsToUniqueConstraintModel(ucr);
                ((List<UniqueConstraint>)tables[ucr.TableName].UniqueConstraints).Add(uc);
            }

            foreach (Table table in tables.Values)
            {
                foreach (UniqueConstraint uc in table.UniqueConstraints)
                {
                    uc.Columns = columnNames[uc.Name].Select(x => x.Value).ToList();
                }
            }
        }

        private static UniqueConstraint MapExceptColumnsToUniqueConstraintModel(UniqueConstraintRecord ucr)
        {
            return new UniqueConstraint()
            {
                ID = Guid.NewGuid(),
                Name = ucr.ConstraintName,
            };
        }

        internal class UniqueConstraintRecord
        {
            public string TableName { get; set; }
            public string ConstraintName { get; set; }
            public string ColumnName { get; set; }
            public int ColumnPosition { get; set; }
        }
    }
}
