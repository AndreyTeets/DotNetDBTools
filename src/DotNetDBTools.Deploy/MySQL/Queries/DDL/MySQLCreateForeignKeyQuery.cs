using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL.Queries.DDL;

internal class MySQLCreateForeignKeyQuery : CreateForeignKeyQuery
{
    public MySQLCreateForeignKeyQuery(ForeignKey fk)
        : base(fk) { }

    protected override string GetSql(ForeignKey fk) => GetSqlBase<MySQLTableDiff>(fk);
}
