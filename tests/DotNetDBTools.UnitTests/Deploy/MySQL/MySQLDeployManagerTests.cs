using DotNetDBTools.Deploy.MySQL;
using DotNetDBTools.Deploy.MySQL.Editors;
using DotNetDBTools.Models.MySQL;
using DotNetDBTools.UnitTests.Deploy.Base;

namespace DotNetDBTools.UnitTests.Deploy.MySQL;

public class MySQLDeployManagerTests : BaseDeployManagerTests<MySQLDatabase>
{
    public MySQLDeployManagerTests()
        : base(new MySQLMockCreator())
    {
    }

    internal class MySQLMockCreator : MockCreator<
        MySQLFactory,
        MySQLQueryExecutor,
        MySQLGenSqlScriptQueryExecutor,
        MySQLDbEditor,
        MySQLDbModelFromDBMSProvider>
    {
    }
}
