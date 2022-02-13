using Dapper;
using DotNetDBTools.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySqlConnector;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MySQL;

[TestClass]
public class MySQLSampleApplicationsTests : BaseSampleApplicationsTests
{
    protected override string SyncScopeName => null;

    protected override string DtDbms => "MySQL";
    protected override string DtAsm => $"{SamplesOutputDirRelToSampleApps}/DotNetDBTools.SampleDB.MySQL.dll";
    protected override string DtAsmV2 => $"{SamplesOutputDirRelToSampleApps}/DotNetDBTools.SampleDBv2.MySQL.dll";
    protected override string DtCs => MySQLDatabaseHelper.CreateConnectionString(
        ConnectionStringWithoutDb, "MySQLSampleDB_DeployTool");

    protected override string DeployToolAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.DeployTool/{ProjectsOutDirPathNet60}/DotNetDBTools.DeployTool.dll";

    protected override string SampleDeployManagerUsageAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.SampleDeployManagerUsage.MySQL/{ProjectsOutDirPathNetCore31}/DotNetDBTools.SampleDeployManagerUsage.MySQL.dll";

    protected override string SampleBusinessLogicOnlyAppAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.MySQL/{ProjectsOutDirPathNetCore31}/DotNetDBTools.SampleBusinessLogicOnlyApp.MySQL.dll";

    protected override string SampleSelfUpdatingAppAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.MySQL/{ProjectsOutDirPathNetCore31}/DotNetDBTools.SampleSelfUpdatingApp.MySQL.dll";

    private static string ConnectionStringWithoutDb => MySQLContainerHelper.MySQLContainerConnectionString;

    protected override void ExecuteSqlOnDeployToolDatabase(string sql)
    {
        using MySqlConnection connection = new(DtCs);
        connection.Execute(sql);
    }

    protected override void CreateDeployToolDatabase()
    {
        MySQLDatabaseHelper.CreateDatabase(ConnectionStringWithoutDb, "MySQLSampleDB_DeployTool");
    }

    protected override void DropDeployToolDatabaseIfExists()
    {
        MySQLDatabaseHelper.DropDatabaseIfExists(ConnectionStringWithoutDb, "MySQLSampleDB_DeployTool");
    }

    protected override void DropSelfUpdatingAppDatabaseIfExists()
    {
        MySQLDatabaseHelper.DropDatabaseIfExists(ConnectionStringWithoutDb, "AgnosticSampleDB_SelfUpdatingApp");
    }
}
