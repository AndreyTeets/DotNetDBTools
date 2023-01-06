using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class SQLiteAlterTableQuery : AlterTableQuery
{
    public SQLiteAlterTableQuery(TableDiff tableDiff)
        : base(tableDiff) { }

    protected override string GetSql(TableDiff tableDiff)
    {
        return GenerationManager.GenerateSqlAlterStatement(tableDiff);
    }
}
