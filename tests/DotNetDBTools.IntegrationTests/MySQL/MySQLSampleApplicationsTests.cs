using DotNetDBTools.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MySQL
{
    [TestClass]
    public class MySQLSampleApplicationsTests : BaseSampleApplicationsTests
    {
        protected override string SyncScopeName => null;

        protected override string SampleDeployToolAssemblyPath =>
            $"{RepoRoot}/samples/DeployTools/DotNetDBTools.SampleDeployTool.MySQL/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployTool.MySQL.dll";

        protected override string SampleBusinessLogicOnlyAppAssemblyPath =>
            $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.MySQL/{ProjectsOutDirPath}/DotNetDBTools.SampleBusinessLogicOnlyApp.MySQL.dll";

        protected override string SampleSelfUpdatingAppAssemblyPath =>
            $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.MySQL/{ProjectsOutDirPath}/DotNetDBTools.SampleSelfUpdatingApp.MySQL.dll";
    }
}
