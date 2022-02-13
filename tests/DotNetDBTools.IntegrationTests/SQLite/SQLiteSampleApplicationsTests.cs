using Dapper;
using DotNetDBTools.IntegrationTests.Base;
using Microsoft.Data.Sqlite;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.SQLite;

public class SQLiteSampleApplicationsTests : BaseSampleApplicationsTests
{
    protected override string SyncScopeName => null;

    protected override string DtDbms => "SQLite";
    protected override string DtAsm => $"{SamplesOutputDirRelToSampleApps}/DotNetDBTools.SampleDB.SQLite.dll";
    protected override string DtAsmV2 => $"{SamplesOutputDirRelToSampleApps}/DotNetDBTools.SampleDBv2.SQLite.dll";
    protected override string DtCs => SQLiteDatabaseHelper.CreateConnectionString(
        $"{SamplesOutputDirRelToSampleApps}/sqlite_databases", "SQLiteSampleDB_DeployTool");

    protected override string DeployToolAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.DeployTool/{ProjectsOutDirPathNet60}/DotNetDBTools.DeployTool.dll";

    protected override string SampleDeployManagerUsageAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.SampleDeployManagerUsage.SQLite/{ProjectsOutDirPathNetCore31}/DotNetDBTools.SampleDeployManagerUsage.SQLite.dll";

    protected override string SampleBusinessLogicOnlyAppAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.SQLite/{ProjectsOutDirPathNetCore31}/DotNetDBTools.SampleBusinessLogicOnlyApp.SQLite.dll";

    protected override string SampleSelfUpdatingAppAssemblyPath =>
$"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.SQLite/{ProjectsOutDirPathNetCore31}/DotNetDBTools.SampleSelfUpdatingApp.SQLite.dll";

    private static string DbFilesFolder => $"{SamplesOutputDir}/sqlite_databases";

    protected override void ExecuteSqlOnDeployToolDatabase(string sql)
    {
        string connectionString = SQLiteDatabaseHelper.CreateConnectionString(
            $"{SamplesOutputDir}/sqlite_databases", "SQLiteSampleDB_DeployTool");
        using SqliteConnection connection = new(connectionString);
        connection.Execute(sql);
    }

    protected override void CreateDeployToolDatabase()
    {
        SQLiteDatabaseHelper.CreateDatabase(DbFilesFolder, "SQLiteSampleDB_DeployTool");
    }

    protected override void DropDeployToolDatabaseIfExists()
    {
        SQLiteDatabaseHelper.DropDatabaseIfExists(DbFilesFolder, "SQLiteSampleDB_DeployTool");
    }

    protected override void DropSelfUpdatingAppDatabaseIfExists()
    {
        SQLiteDatabaseHelper.DropDatabaseIfExists(DbFilesFolder, "AgnosticSampleDB_SelfUpdatingApp");
    }
}
