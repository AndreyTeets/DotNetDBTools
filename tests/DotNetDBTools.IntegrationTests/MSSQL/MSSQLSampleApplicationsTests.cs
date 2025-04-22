using Dapper;
using DotNetDBTools.IntegrationTests.Base;
using Microsoft.Data.SqlClient;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MSSQL;

public class MSSQLSampleApplicationsTests : BaseSampleApplicationsTests
{
    protected override string SyncScopeName => nameof(MSSQLContainerHelper);

    protected override string DtDbms => "MSSQL";
    protected override string DtAsm => $"{SamplesOutputDirRelToSampleApps}/DotNetDBTools.SampleDB.MSSQL.dll";
    protected override string DtAsmV2 => $"{SamplesOutputDirRelToSampleApps}/DotNetDBTools.SampleDBv2.MSSQL.dll";
    protected override string DtCs => MSSQLDatabaseHelper.CreateConnectionString(
        ConnectionStringWithoutDb, "MSSQLSampleDB_DeployTool");

    protected override string DeployToolAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.DeployTool/{ProjectsOutDirPathNet60}/DotNetDBTools.DeployTool.dll";

    protected override string SampleDeployManagerUsageAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.SampleDeployManagerUsage.MSSQL/{ProjectsOutDirPathNetCore31}/DotNetDBTools.SampleDeployManagerUsage.MSSQL.dll";

    protected override string SampleBusinessLogicOnlyAppAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.MSSQL/{ProjectsOutDirPathNetCore31}/DotNetDBTools.SampleBusinessLogicOnlyApp.MSSQL.dll";

    protected override string SampleSelfUpdatingAppAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.MSSQL/{ProjectsOutDirPathNetCore31}/DotNetDBTools.SampleSelfUpdatingApp.MSSQL.dll";

    private static string ConnectionStringWithoutDb => MSSQLContainerHelper.MSSQLContainerConnectionString;

    protected override void ExecuteSqlOnDeployToolDatabase(string sql)
    {
        using SqlConnection connection = new(DtCs);
        connection.Execute(sql);
    }

    protected override void CreateDeployToolDatabase()
    {
        MSSQLDatabaseHelper.CreateDatabase(ConnectionStringWithoutDb, "MSSQLSampleDB_DeployTool");
    }

    protected override void DropDeployToolDatabaseIfExists()
    {
        MSSQLDatabaseHelper.DropDatabaseIfExists(ConnectionStringWithoutDb, "MSSQLSampleDB_DeployTool");
    }

    protected override void DropSelfUpdatingAppDatabaseIfExists()
    {
        MSSQLDatabaseHelper.DropDatabaseIfExists(ConnectionStringWithoutDb, "AgnosticSampleDB_SelfUpdatingApp");
    }
}
