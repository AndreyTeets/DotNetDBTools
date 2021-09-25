using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Queries.SQLiteSysInfo
{
    internal class GetUniqueConstraintsFromSQLiteSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    sm.name AS {nameof(UniqueConstraintRecord.TableName)},
    'UQ_' || sm.name || '_' || il.seq AS {nameof(UniqueConstraintRecord.ConstraintName)},
    ii.name AS {nameof(UniqueConstraintRecord.ColumnName)},
    ii.seqno AS {nameof(UniqueConstraintRecord.ColumnPosition)}
FROM sqlite_master sm
INNER JOIN pragma_index_list(sm.name) il
INNER JOIN pragma_index_info(il.name) ii
WHERE sm.type = 'table'
    AND il.origin='u';";

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
                Dictionary<string, SQLiteTableInfo> tables,
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

                foreach (SQLiteTableInfo table in tables.Values)
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
