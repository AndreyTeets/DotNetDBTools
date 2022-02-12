using DotNetDBTools.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MySQL;

[TestClass]
public class MySQLSampleApplicationsTests : BaseSampleApplicationsTests
{
    protected override string SyncScopeName => null;

    protected override string SampleDeployManagerUsageAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleDeployManagerUsage.MySQL/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployManagerUsage.MySQL.dll";

    protected override string SampleBusinessLogicOnlyAppAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.MySQL/{ProjectsOutDirPath}/DotNetDBTools.SampleBusinessLogicOnlyApp.MySQL.dll";

    protected override string SampleSelfUpdatingAppAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.MySQL/{ProjectsOutDirPath}/DotNetDBTools.SampleSelfUpdatingApp.MySQL.dll";

    protected override void DropSelfUpdatingAppDatabaseIfExists()
    {
        string connectionStringWithoutDb = MySQLContainerHelper.MySQLContainerConnectionString;
        MySQLDatabaseHelper.DropDatabaseIfExists(connectionStringWithoutDb, "AgnosticSampleDB_SelfUpdatingApp");
    }
}
