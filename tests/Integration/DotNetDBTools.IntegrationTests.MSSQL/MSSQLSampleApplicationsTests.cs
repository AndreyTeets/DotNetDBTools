using DotNetDBTools.TestsUtils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.TestsUtils.Constants;

namespace DotNetDBTools.IntegrationTests.MSSQL
{
    [TestClass]
    public class MSSQLSampleApplicationsTests
    {
        private static readonly string s_deployAssemblyPath = $"{RepoRoot}/samples/DeployUtils/DotNetDBTools.SampleDeployUtil.MSSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployUtil.MSSQL.exe";

        [TestMethod]
        public void SampleMSSQLProjects_Run_WithoutErrors()
        {
            (int exitCodeDeploy, string outputDeploy) = ProcessHelper.RunProcess(s_deployAssemblyPath);
            exitCodeDeploy.Should().Be(0, $"process output: '{outputDeploy}'");
        }
    }
}
