using DotNetDBTools.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.SQLite;

[TestClass]
public class SQLiteSampleApplicationsTests : BaseSampleApplicationsTests
{
    protected override string SyncScopeName => null;

    protected override string SampleDeployManagerUsageAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleDeployManagerUsage.SQLite/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployManagerUsage.SQLite.dll";

    protected override string SampleBusinessLogicOnlyAppAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.SQLite/{ProjectsOutDirPath}/DotNetDBTools.SampleBusinessLogicOnlyApp.SQLite.dll";

    protected override string SampleSelfUpdatingAppAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.SQLite/{ProjectsOutDirPath}/DotNetDBTools.SampleSelfUpdatingApp.SQLite.dll";

    protected override void DropSelfUpdatingAppDatabaseIfExists()
    {
        string dbFilePath = $"{RepoRoot}/SamplesOutput/sqlite_databases/AgnosticSampleDB_SelfUpdatingApp.db";
        SQLiteDatabaseHelper.DropDatabaseIfExists(dbFilePath);
    }
}
