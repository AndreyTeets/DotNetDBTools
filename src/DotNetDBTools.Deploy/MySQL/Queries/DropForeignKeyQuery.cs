using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries
{
    internal class DropForeignKeyQuery : IQuery
    {
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public DropForeignKeyQuery(ForeignKey fk, string tableName)
        {
            _sql = GetSql(fk, tableName);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(ForeignKey fk, string tableName)
        {
            string query =
$@"ALTER TABLE {tableName} DROP CONSTRAINT {fk.Name};";

            return query;
        }
    }
}
