using DotNetDBTools.Generation;
using DotNetDBTools.Models;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DDL;

internal class CreateTableQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public CreateTableQuery(Table table)
    {
        _sql = GetSql(table);
    }

    protected virtual string GetSql(Table table)
    {
        Table tableWithoutForeignKeys = table.CopyModel();
        tableWithoutForeignKeys.ForeignKeys.Clear();
        return GenerationManager.GenerateSqlCreateStatement(tableWithoutForeignKeys, false);
    }
}
