using System.Linq;
using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.PostgreSQL.PostgreSQLQueriesHelper;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL
{
    internal class PostgreSQLCreateForeignKeyQuery : CreateForeignKeyQuery
    {
        public PostgreSQLCreateForeignKeyQuery(ForeignKey fk, string tableName)
            : base(fk, tableName) { }

        protected override string GetSql(ForeignKey fk, string tableName)
        {
            string query =
$@"ALTER TABLE ""{tableName}"" ADD CONSTRAINT ""{fk.Name}"" FOREIGN KEY ({string.Join(", ", fk.ThisColumnNames.Select(x => $@"""{x}"""))})
    REFERENCES ""{fk.ReferencedTableName}"" ({string.Join(", ", fk.ReferencedTableColumnNames.Select(x => $@"""{x}"""))})
    ON UPDATE {MapActionName(fk.OnUpdate)} ON DELETE {MapActionName(fk.OnDelete)};";

            return query;
        }
    }
}
