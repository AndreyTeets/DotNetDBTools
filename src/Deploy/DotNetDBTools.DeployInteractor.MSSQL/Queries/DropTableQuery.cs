﻿using System.Collections.Generic;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.DeployInteractor.MSSQL.Queries
{
    internal class DropTableQuery : IQuery
    {
        private readonly string _sql;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        public DropTableQuery(MSSQLTableInfo table)
        {
            _sql = GetSql(table);
        }

        private static string GetSql(MSSQLTableInfo table)
        {
            string query =
$@"DROP TABLE {table.Name};

DELETE FROM {DNDBTSysTables.DNDBTDbObjects}
WHERE {DNDBTSysTables.DNDBTDbObjects.ID} = '{table.ID}';";

            return query;
        }
    }
}