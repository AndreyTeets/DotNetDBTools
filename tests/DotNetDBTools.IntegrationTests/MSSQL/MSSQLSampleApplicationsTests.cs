using DotNetDBTools.IntegrationTests.TestHelpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MSSQL
{
    [TestClass]
    public class MSSQLSampleApplicationsTests
    {
        private static readonly string s_sampleDeployToolAssemblyPath = $"{RepoRoot}/samples/DeployTools/DotNetDBTools.SampleDeployTool.MSSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployTool.MSSQL.dll";
        private static readonly string s_sampleBusinessLogicOnlyAppAssemblyPath = $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.MSSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleBusinessLogicOnlyApp.MSSQL.dll";
        private static readonly string s_sampleSelfUpdatingAppAssemblyPath = $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.MSSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleSelfUpdatingApp.MSSQL.dll";

        [TestMethod]
        public void SampleMSSQL_DeployToolAndBusinessLogicOnlyApps_Run_WithoutErrors()
        {
            (int exitCodeDeploy, string outputDeploy) = ProcessHelper.RunProcess(s_sampleDeployToolAssemblyPath);
            exitCodeDeploy.Should().Be(0, $"process output: '{outputDeploy}'");

            (int exitCodeApplication, string outputApplication) = ProcessHelper.RunProcess(s_sampleBusinessLogicOnlyAppAssemblyPath);
            exitCodeApplication.Should().Be(0, $"process output: '{outputApplication}'");
        }

        [TestMethod]
        public void SampleMSSQL_SelfUpdatingApp_Run_WithoutErrors()
        {
            (int exitCodeApplication, string outputApplication) = ProcessHelper.RunProcess(s_sampleSelfUpdatingAppAssemblyPath);
            exitCodeApplication.Should().Be(0, $"process output: '{outputApplication}'");
        }
    }
}
