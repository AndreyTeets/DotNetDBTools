using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.Deploy.SQLite.Editors;
using DotNetDBTools.Models.SQLite;
using DotNetDBTools.UnitTests.Deploy.Base;

namespace DotNetDBTools.UnitTests.Deploy.SQLite;

public class SQLiteDeployManagerTests : BaseDeployManagerTests<SQLiteDatabase>
{
    public SQLiteDeployManagerTests()
        : base(new SQLiteMockCreator())
    {
    }

    internal class SQLiteMockCreator : MockCreator<
        SQLiteFactory,
        SQLiteQueryExecutor,
        SQLiteGenSqlScriptQueryExecutor,
        SQLiteDbModelConverter,
        SQLiteDbEditor,
        SQLiteDbModelFromDBMSProvider>
    {
    }
}
