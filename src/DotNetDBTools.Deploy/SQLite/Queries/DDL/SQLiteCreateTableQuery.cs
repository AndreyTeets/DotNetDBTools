using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class SQLiteCreateTableQuery : CreateTableQuery
{
    public SQLiteCreateTableQuery(Table table)
        : base(table) { }

    protected override string GetSql(Table table)
    {
        return GenerationManager.GenerateSqlCreateStatement(table, false);
    }
}
