using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL.Editors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL;

internal class PostgreSQLFactory : Factory<
    PostgreSQLQueryExecutor,
    PostgreSQLGenSqlScriptQueryExecutor,
    PostgreSQLDbEditor,
    PostgreSQLDbModelFromDBMSProvider>
{
    public PostgreSQLFactory() : base(DatabaseKind.PostgreSQL) { }
}
