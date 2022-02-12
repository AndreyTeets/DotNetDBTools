using DotNetDBTools.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.PostgreSQL;

[TestClass]
public class PostgreSQLSampleApplicationsTests : BaseSampleApplicationsTests
{
    protected override string SyncScopeName => null;

    protected override string SampleDeployManagerUsageAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleDeployManagerUsage.PostgreSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployManagerUsage.PostgreSQL.dll";

    protected override string SampleBusinessLogicOnlyAppAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.PostgreSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleBusinessLogicOnlyApp.PostgreSQL.dll";

    protected override string SampleSelfUpdatingAppAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.PostgreSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleSelfUpdatingApp.PostgreSQL.dll";

    protected override void DropSelfUpdatingAppDatabaseIfExists()
    {
        string connectionStringWithoutDb = PostgreSQLContainerHelper.PostgreSQLContainerConnectionString;
        PostgreSQLDatabaseHelper.DropDatabaseIfExists(connectionStringWithoutDb, "AgnosticSampleDB_SelfUpdatingApp");
    }
}
