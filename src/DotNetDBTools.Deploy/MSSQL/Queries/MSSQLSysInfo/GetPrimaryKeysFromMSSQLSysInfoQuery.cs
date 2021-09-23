using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.MSSQLSysInfo
{
    internal class GetPrimaryKeysFromMSSQLSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    t.name AS {nameof(PrimaryKeyRecord.TableName)},
    i.name AS {nameof(PrimaryKeyRecord.ConstraintName)},
    c.name AS {nameof(PrimaryKeyRecord.ColumnName)},
    ic.key_ordinal AS {nameof(PrimaryKeyRecord.ColumnPosition)}
FROM sys.tables t
INNER JOIN sys.columns c
      ON c.object_id = t.object_id
INNER JOIN sys.indexes i
    ON i.object_id = t.object_id
INNER JOIN sys.index_columns ic
    ON ic.object_id = t.object_id
        AND ic.index_id = i.index_id
        AND ic.column_id = c.column_id
WHERE i.is_primary_key = 1;";

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
                Dictionary<string, MSSQLTableInfo> tables,
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

                foreach (MSSQLTableInfo table in tables.Values)
                {
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
