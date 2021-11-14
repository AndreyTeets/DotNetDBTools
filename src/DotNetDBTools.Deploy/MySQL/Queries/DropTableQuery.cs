using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL.Queries
{
    internal class DropTableQuery : IQuery
    {
        private readonly string _sql;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        public DropTableQuery(MySQLTable table)
        {
            _sql = GetSql(table);
        }

        private static string GetSql(MySQLTable table)
        {
            string query =
$@"DROP TABLE {table.Name};";

            return query;
        }
    }
}
