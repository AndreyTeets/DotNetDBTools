using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.PostgreSQL.PostgreSQLQueriesHelper;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries
{
    internal class CreateForeignKeyQuery : IQuery
    {
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public CreateForeignKeyQuery(ForeignKey fk, string tableName)
        {
            _sql = GetSql(fk, tableName);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(ForeignKey fk, string tableName)
        {
            string query =
$@"ALTER TABLE ""{tableName}"" ADD CONSTRAINT ""{fk.Name}"" FOREIGN KEY ({string.Join(", ", fk.ThisColumnNames.Select(x => $@"""{x}"""))})
    REFERENCES ""{fk.ReferencedTableName}"" ({string.Join(", ", fk.ReferencedTableColumnNames.Select(x => $@"""{x}"""))})
    ON UPDATE {MapActionName(fk.OnUpdate)} ON DELETE {MapActionName(fk.OnDelete)};";

            return query;
        }
    }
}
