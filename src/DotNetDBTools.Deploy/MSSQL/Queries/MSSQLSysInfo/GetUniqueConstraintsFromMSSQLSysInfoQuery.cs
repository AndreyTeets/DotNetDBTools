using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.MSSQLSysInfo
{
    internal class GetUniqueConstraintsFromMSSQLSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    t.name AS {nameof(UniqueConstraintRecord.TableName)},
    i.name AS {nameof(UniqueConstraintRecord.ConstraintName)},
    c.name AS {nameof(UniqueConstraintRecord.ColumnName)},
    ic.key_ordinal AS {nameof(UniqueConstraintRecord.ColumnPosition)}
FROM sys.tables t
INNER JOIN sys.columns c
      ON c.object_id = t.object_id
INNER JOIN sys.indexes i
    ON i.object_id = t.object_id
INNER JOIN sys.index_columns ic
    ON ic.object_id = t.object_id
        AND ic.index_id = i.index_id
        AND ic.column_id = c.column_id
WHERE i.is_unique_constraint = 1;";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class UniqueConstraintRecord
        {
            public string TableName { get; set; }
            public string ConstraintName { get; set; }
            public string ColumnName { get; set; }
            public int ColumnPosition { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static void BuildTablesUniqueConstraints(
                Dictionary<string, MSSQLTableInfo> tables,
                IEnumerable<UniqueConstraintRecord> uniqueConstraintRecords)
            {
                Dictionary<string, SortedDictionary<int, string>> columnNames = new();
                foreach (UniqueConstraintRecord ucr in uniqueConstraintRecords)
                {
                    if (!columnNames.ContainsKey(ucr.ConstraintName))
                        columnNames.Add(ucr.ConstraintName, new SortedDictionary<int, string>());

                    columnNames[ucr.ConstraintName].Add(
                        ucr.ColumnPosition, ucr.ColumnName);

                    UniqueConstraintInfo uc = MapExceptColumnsToUniqueConstraintInfo(ucr);
                    ((List<UniqueConstraintInfo>)tables[ucr.TableName].UniqueConstraints).Add(uc);
                }

                foreach (MSSQLTableInfo table in tables.Values)
                {
                    foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                    {
                        uc.Columns = columnNames[uc.Name].Select(x => x.Value).ToList();
                    }
                }
            }

            private static UniqueConstraintInfo MapExceptColumnsToUniqueConstraintInfo(UniqueConstraintRecord ucr)
            {
                return new UniqueConstraintInfo()
                {
                    ID = Guid.NewGuid(),
                    Name = ucr.ConstraintName,
                };
            }
        }
    }
}
