using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries
{
    internal class DropTableQuery : IQuery
    {
        private readonly string _sql;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        public DropTableQuery(PostgreSQLTable table)
        {
            _sql = GetSql(table);
        }

        private static string GetSql(PostgreSQLTable table)
        {
            string query =
$@"DROP TABLE ""{table.Name}"";";

            return query;
        }
    }
}
