using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.Deploy.MSSQL;
using DotNetDBTools.Deploy.MSSQL.Editors;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.UnitTests.Deploy.Base;

namespace DotNetDBTools.UnitTests.Deploy.MSSQL;

public class MSSQLDeployManagerTests : BaseDeployManagerTests<MSSQLDatabase>
{
    public MSSQLDeployManagerTests()
        : base(new MSSQLMockCreator())
    {
    }

    internal class MSSQLMockCreator : MockCreator<
        MSSQLFactory,
        MSSQLQueryExecutor,
        MSSQLGenSqlScriptQueryExecutor,
        MSSQLDbModelConverter,
        MSSQLDbEditor,
        MSSQLDbModelFromDBMSProvider>
    {
    }
}
