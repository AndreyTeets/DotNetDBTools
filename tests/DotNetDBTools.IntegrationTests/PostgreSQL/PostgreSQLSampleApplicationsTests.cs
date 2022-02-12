using DotNetDBTools.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.PostgreSQL;

[TestClass]
public class PostgreSQLSampleApplicationsTests : BaseSampleApplicationsTests
{
    protected override string SyncScopeName => null;

    protected override string SampleDeployToolAssemblyPath =>
        $"{RepoRoot}/samples/DeployTools/DotNetDBTools.SampleDeployTool.PostgreSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployTool.PostgreSQL.dll";

    protected override string SampleBusinessLogicOnlyAppAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.PostgreSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleBusinessLogicOnlyApp.PostgreSQL.dll";

    protected override string SampleSelfUpdatingAppAssemblyPath =>
        $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.PostgreSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleSelfUpdatingApp.PostgreSQL.dll";
}
