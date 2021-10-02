﻿using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class DropTableQuery : IQuery
    {
        private readonly string _sql;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        public DropTableQuery(MSSQLTable table)
        {
            _sql = GetSql(table);
        }

        private static string GetSql(MSSQLTable table)
        {
            string query =
$@"DROP TABLE {table.Name};";

            return query;
        }
    }
}
