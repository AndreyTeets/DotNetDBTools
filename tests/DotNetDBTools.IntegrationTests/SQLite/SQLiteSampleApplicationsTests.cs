using DotNetDBTools.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.SQLite
{
    [TestClass]
    public class SQLiteSampleApplicationsTests : BaseSampleApplicationsTests
    {
        protected override string SampleDeployToolAssemblyPath =>
            $"{RepoRoot}/samples/DeployTools/DotNetDBTools.SampleDeployTool.SQLite/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployTool.SQLite.dll";

        protected override string SampleBusinessLogicOnlyAppAssemblyPath =>
            $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.SQLite/{ProjectsOutDirPath}/DotNetDBTools.SampleBusinessLogicOnlyApp.SQLite.dll";

        protected override string SampleSelfUpdatingAppAssemblyPath =>
            $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.SQLite/{ProjectsOutDirPath}/DotNetDBTools.SampleSelfUpdatingApp.SQLite.dll";
    }
}
