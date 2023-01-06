using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DDL;

internal class MSSQLDropTypeQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public MSSQLDropTypeQuery(MSSQLUserDefinedType type)
    {
        _sql = GenerationManager.GenerateSqlDropStatement(type);
    }
}
