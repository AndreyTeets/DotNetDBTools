using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL.Queries.DDL;

internal class MySQLDropForeignKeyQuery : DropForeignKeyQuery
{
    public MySQLDropForeignKeyQuery(ForeignKey fk)
        : base(fk) { }

    protected override string GetSql(ForeignKey fk) => GetSqlBase<MySQLTable, MySQLTableDiff>(fk);
}
