﻿using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Queries
{
    internal class DropTableQuery : IQuery
    {
        private readonly string _sql;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        public DropTableQuery(SQLiteTable table)
        {
            _sql = GetSql(table);
        }

        private static string GetSql(SQLiteTable table)
        {
            string query =
$@"DROP TABLE {table.Name};";

            return query;
        }
    }
}
