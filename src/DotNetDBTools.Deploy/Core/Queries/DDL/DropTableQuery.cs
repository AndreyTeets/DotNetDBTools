using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DDL;

internal class DropTableQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public DropTableQuery(Table table)
    {
        _sql = GenerationManager.GenerateSqlDropStatement(table);
    }
}
