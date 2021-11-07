﻿using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo
{
    internal class GetUniqueConstraintsFromDBMSSysInfoQuery : IQuery
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
WHERE i.is_unique_constraint = 1
    AND t.name != '{DNDBTSysTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class UniqueConstraintRecord : UniqueConstraintsBuilder.UniqueConstraintRecord { }
        internal static class ResultsInterpreter
        {
            public static void BuildTablesUniqueConstraints(
                Dictionary<string, Table> tables,
                IEnumerable<UniqueConstraintRecord> uniqueConstraintRecords)
            {
                UniqueConstraintsBuilder.BuildTablesUniqueConstraints(tables, uniqueConstraintRecords);
            }
        }
    }
}