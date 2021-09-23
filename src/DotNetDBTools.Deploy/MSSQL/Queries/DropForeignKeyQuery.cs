using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class DropForeignKeyQuery : IQuery
    {
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public DropForeignKeyQuery(ForeignKeyInfo fk)
        {
            _sql = GetSql(fk);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(ForeignKeyInfo fk)
        {
            string query =
$@"ALTER TABLE {fk.ThisTableName} DROP CONSTRAINT {fk.Name};";

            return query;
        }
    }
}
