using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Queries.SQLiteSysInfo
{
    internal class GetPrimaryKeysFromSQLiteSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    sm.name AS {nameof(PrimaryKeyRecord.TableName)},
    'PK_' || sm.name AS {nameof(PrimaryKeyRecord.ConstraintName)},
    ii.name AS {nameof(PrimaryKeyRecord.ColumnName)},
    ii.seqno AS {nameof(PrimaryKeyRecord.ColumnPosition)}
FROM sqlite_master sm
INNER JOIN pragma_index_list(sm.name) il
INNER JOIN pragma_index_info(il.name) ii
WHERE sm.type = 'table'
    AND il.origin='pk'
UNION
SELECT
    sm.name AS {nameof(PrimaryKeyRecord.TableName)},
    'PK_' || sm.name AS {nameof(PrimaryKeyRecord.ConstraintName)},
    ti.name AS {nameof(PrimaryKeyRecord.ColumnName)},
    0 AS {nameof(PrimaryKeyRecord.ColumnPosition)}
FROM sqlite_master sm
INNER JOIN pragma_table_info(sm.name) ti
WHERE sm.type = 'table'
    AND ti.pk=1
    AND lower(ti.type)='integer';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class PrimaryKeyRecord
        {
            public string TableName { get; set; }
            public string ConstraintName { get; set; }
            public string ColumnName { get; set; }
            public int ColumnPosition { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static void BuildTablesPrimaryKeys(
                Dictionary<string, SQLiteTableInfo> tables,
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

                foreach (SQLiteTableInfo table in tables.Values)
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
        }
    }
}
