using DotNetDBTools.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MSSQL;

[TestClass]
public class MSSQLSampleApplicationsTests : BaseSampleApplicationsTests
{
    protected override string SyncScopeName => nameof(MSSQLContainerHelper);

    protected override string SampleDeployManagerUsageAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleDeployManagerUsage.MSSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployManagerUsage.MSSQL.dll";

    protected override string SampleBusinessLogicOnlyAppAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.MSSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleBusinessLogicOnlyApp.MSSQL.dll";

    protected override string SampleSelfUpdatingAppAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.MSSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleSelfUpdatingApp.MSSQL.dll";

    protected override void DropSelfUpdatingAppDatabaseIfExists()
    {
        string connectionStringWithoutDb = MSSQLContainerHelper.MSSQLContainerConnectionString;
        MSSQLDatabaseHelper.DropDatabaseIfExists(connectionStringWithoutDb, "AgnosticSampleDB_SelfUpdatingApp");
    }
}
