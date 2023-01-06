using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLCreateForeignKeyQuery : CreateForeignKeyQuery
{
    public PostgreSQLCreateForeignKeyQuery(ForeignKey fk)
        : base(fk) { }

    protected override string GetSql(ForeignKey fk) => GetSqlBase<PostgreSQLTable, PostgreSQLTableDiff>(fk);
}
