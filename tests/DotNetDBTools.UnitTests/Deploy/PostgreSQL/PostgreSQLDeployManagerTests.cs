using DotNetDBTools.Deploy.PostgreSQL;
using DotNetDBTools.Deploy.PostgreSQL.Editors;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.UnitTests.Deploy.Base;

namespace DotNetDBTools.UnitTests.Deploy.PostgreSQL;

public class PostgreSQLDeployManagerTests : BaseDeployManagerTests<PostgreSQLDatabase>
{
    public PostgreSQLDeployManagerTests()
        : base(new PostgreSQLMockCreator())
    {
    }

    internal class PostgreSQLMockCreator : MockCreator<
        PostgreSQLFactory,
        PostgreSQLQueryExecutor,
        PostgreSQLGenSqlScriptQueryExecutor,
        PostgreSQLDbEditor,
        PostgreSQLDbModelFromDBMSProvider>
    {
    }
}
