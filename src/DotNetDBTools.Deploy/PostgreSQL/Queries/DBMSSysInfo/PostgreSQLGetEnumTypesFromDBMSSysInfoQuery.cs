﻿using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo;

internal class PostgreSQLGetEnumTypesFromDBMSSysInfoQuery : IQuery
{
    public string Sql =>
$@"SELECT
    t.typname AS ""{nameof(EnumTypeRecord.TypeName)}"",
    e.enumlabel AS ""{nameof(EnumTypeRecord.LabelName)}""
FROM pg_catalog.pg_type t
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = t.typnamespace
INNER JOIN pg_catalog.pg_enum e
    ON e.enumtypid = t.oid
WHERE t.typtype = 'e'
    AND n.nspname NOT IN ('information_schema', 'pg_catalog');";

    public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

    public class EnumTypeRecord
    {
        public string TypeName { get; set; }
        public string LabelName { get; set; }
    }
}
