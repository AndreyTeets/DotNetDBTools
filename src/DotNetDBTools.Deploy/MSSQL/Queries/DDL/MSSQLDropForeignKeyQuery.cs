using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DDL;

internal class MSSQLDropForeignKeyQuery : DropForeignKeyQuery
{
    public MSSQLDropForeignKeyQuery(ForeignKey fk, string tableName)
        : base(fk, tableName) { }

    protected override string GetSql(ForeignKey fk, string tableName)
    {
        string query =
$@"ALTER TABLE [{tableName}] DROP CONSTRAINT [{fk.Name}];";

        return query;
    }
}
