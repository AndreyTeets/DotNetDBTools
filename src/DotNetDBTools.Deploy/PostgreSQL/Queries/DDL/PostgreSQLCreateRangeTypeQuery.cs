using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Generation;
using DotNetDBTools.Generation.PostgreSQL;
using DotNetDBTools.Models;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;
using QH = DotNetDBTools.Deploy.PostgreSQL.PostgreSQLQueriesHelper;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;

internal class PostgreSQLCreateRangeTypeQuery : NoParametersQuery
{
    public override string Sql => _sql;
    private readonly string _sql;

    public PostgreSQLCreateRangeTypeQuery(PostgreSQLRangeType type)
    {
        _sql = GetSql(type);
    }

    private static string GetSql(PostgreSQLRangeType type)
    {
        PostgreSQLRangeType typeWithUnsetMultirangeTypeName = type.CopyModel();
        typeWithUnsetMultirangeTypeName.MultirangeTypeName = null;

        string res =
$@"IF ({QH.SelectDbmsVersionStatement}) >= {QH.MultirangeTypeNameAvailableDbmsVersion} THEN
{GenerationManager.GenerateSqlCreateStatement(type, false)}
ELSE
{GenerationManager.GenerateSqlCreateStatement(typeWithUnsetMultirangeTypeName, false)}
END IF;".InsideDoPlPgSqlBlock();

        return res;
    }
}
