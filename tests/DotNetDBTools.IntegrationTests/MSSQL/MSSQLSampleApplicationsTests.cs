using DotNetDBTools.IntegrationTests.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MSSQL
{
    [TestClass]
    public class MSSQLSampleApplicationsTests : BaseSampleApplicationsTests
    {
        protected override string SyncScopeName => nameof(MSSQLContainerHelper);

        protected override string SampleDeployToolAssemblyPath =>
            $"{RepoRoot}/samples/DeployTools/DotNetDBTools.SampleDeployTool.MSSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployTool.MSSQL.dll";

        protected override string SampleBusinessLogicOnlyAppAssemblyPath =>
            $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.MSSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleBusinessLogicOnlyApp.MSSQL.dll";

        protected override string SampleSelfUpdatingAppAssemblyPath =>
            $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.MSSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleSelfUpdatingApp.MSSQL.dll";
    }
}
