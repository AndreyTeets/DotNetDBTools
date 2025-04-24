using Dapper;
using DotNetDBTools.IntegrationTests.Base;
using Npgsql;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.PostgreSQL;

public class PostgreSQLSampleApplicationsTests : BaseSampleApplicationsTests
{
    protected override string SyncScopeName => null;

    protected override string DtDbms => "PostgreSQL";
    protected override string DtAsm => $"{SamplesOutputDirRelToSampleApps}/DotNetDBTools.SampleDB.PostgreSQL.dll";
    protected override string DtAsmV2 => $"{SamplesOutputDirRelToSampleApps}/DotNetDBTools.SampleDBv2.PostgreSQL.dll";
    protected override string DtCs => PostgreSQLDatabaseHelper.CreateConnectionString(
        ConnectionStringWithoutDb, "PostgreSQLSampleDB_DeployTool");

    protected override string DeployToolAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.DeployTool/{ProjectsOutDirPathNet90}/DotNetDBTools.DeployTool.dll";

    protected override string SampleDeployManagerUsageAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.SampleDeployManagerUsage.PostgreSQL/{ProjectsOutDirPathNetCore31}/DotNetDBTools.SampleDeployManagerUsage.PostgreSQL.dll";

    protected override string SampleBusinessLogicOnlyAppAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.PostgreSQL/{ProjectsOutDirPathNetCore31}/DotNetDBTools.SampleBusinessLogicOnlyApp.PostgreSQL.dll";

    protected override string SampleSelfUpdatingAppAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.PostgreSQL/{ProjectsOutDirPathNetCore31}/DotNetDBTools.SampleSelfUpdatingApp.PostgreSQL.dll";

    private static string ConnectionStringWithoutDb => PostgreSQLContainerHelper.PostgreSQLContainerConnectionString;

    protected override void ExecuteSqlOnDeployToolDatabase(string sql)
    {
        using NpgsqlConnection connection = new(DtCs);
        connection.Execute(sql);
    }

    protected override void CreateDeployToolDatabase()
    {
        PostgreSQLDatabaseHelper.CreateDatabase(ConnectionStringWithoutDb, "PostgreSQLSampleDB_DeployTool");
    }

    protected override void DropDeployToolDatabaseIfExists()
    {
        PostgreSQLDatabaseHelper.DropDatabaseIfExists(ConnectionStringWithoutDb, "PostgreSQLSampleDB_DeployTool");
    }

    protected override void DropSelfUpdatingAppDatabaseIfExists()
    {
        PostgreSQLDatabaseHelper.DropDatabaseIfExists(ConnectionStringWithoutDb, "AgnosticSampleDB_SelfUpdatingApp");
    }
}
