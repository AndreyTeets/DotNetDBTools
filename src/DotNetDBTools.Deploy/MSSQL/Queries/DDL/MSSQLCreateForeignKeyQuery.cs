using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DDL;

internal class MSSQLCreateForeignKeyQuery : CreateForeignKeyQuery
{
    public MSSQLCreateForeignKeyQuery(ForeignKey fk)
        : base(fk) { }

    protected override string GetSql(ForeignKey fk) => GetSqlBase<MSSQLTableDiff>(fk);
}
