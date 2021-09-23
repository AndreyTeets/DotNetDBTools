using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.MSSQL.MSSQLQueriesHelper;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class CreateForeignKeyQuery : IQuery
    {
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public CreateForeignKeyQuery(ForeignKeyInfo fk)
        {
            _sql = GetSql(fk);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(ForeignKeyInfo fk)
        {
            string query =
$@"ALTER TABLE {fk.ThisTableName} ADD CONSTRAINT {fk.Name} FOREIGN KEY ({string.Join(",", fk.ThisColumnNames)})
    REFERENCES {fk.ForeignTableName} ({string.Join(",", fk.ForeignColumnNames)})
    ON UPDATE {MapActionName(fk.OnUpdate)} ON DELETE {MapActionName(fk.OnDelete)};";

            return query;
        }
    }
}
