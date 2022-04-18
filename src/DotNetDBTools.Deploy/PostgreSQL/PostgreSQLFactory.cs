using DotNetDBTools.Analysis.PostgreSQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL.Editors;

namespace DotNetDBTools.Deploy.PostgreSQL;

internal class PostgreSQLFactory : Factory<
    PostgreSQLQueryExecutor,
    PostgreSQLGenSqlScriptQueryExecutor,
    PostgreSQLDbModelConverter,
    PostgreSQLDbEditor,
    PostgreSQLDbModelFromDBMSProvider>
{
}
