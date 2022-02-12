using System.Linq;
using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLCreateForeignKeyQuery : CreateForeignKeyQuery
{
    public PostgreSQLCreateForeignKeyQuery(ForeignKey fk, string tableName)
        : base(fk, tableName) { }

    protected override string GetSql(ForeignKey fk, string tableName)
    {
        string query =
$@"ALTER TABLE ""{tableName}"" ADD CONSTRAINT ""{fk.Name}"" FOREIGN KEY ({string.Join(", ", fk.ThisColumnNames.Select(x => $@"""{x}"""))})
    REFERENCES ""{fk.ReferencedTableName}"" ({string.Join(", ", fk.ReferencedTableColumnNames.Select(x => $@"""{x}"""))})
    ON UPDATE {fk.OnUpdate} ON DELETE {fk.OnDelete};";

        return query;
    }
}
