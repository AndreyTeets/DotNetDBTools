using DotNetDBTools.IntegrationTests.TestHelpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MySQL
{
    [TestClass]
    public class MySQLSampleApplicationsTests
    {
        private static readonly string s_sampleDeployToolAssemblyPath = $"{RepoRoot}/samples/DeployTools/DotNetDBTools.SampleDeployTool.MySQL/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployTool.MySQL.dll";
        private static readonly string s_sampleBusinessLogicOnlyAppAssemblyPath = $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.MySQL/{ProjectsOutDirPath}/DotNetDBTools.SampleBusinessLogicOnlyApp.MySQL.dll";
        private static readonly string s_sampleSelfUpdatingAppAssemblyPath = $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.MySQL/{ProjectsOutDirPath}/DotNetDBTools.SampleSelfUpdatingApp.MySQL.dll";

        [TestMethod]
        public void SampleMySQL_DeployToolAndBusinessLogicOnlyApps_Run_WithoutErrors()
        {
            (int exitCodeDeploy, string outputDeploy) = ProcessHelper.RunProcess(s_sampleDeployToolAssemblyPath);
            exitCodeDeploy.Should().Be(0, $"process output: '{outputDeploy}'");

            (int exitCodeApplication, string outputApplication) = ProcessHelper.RunProcess(s_sampleBusinessLogicOnlyAppAssemblyPath);
            exitCodeApplication.Should().Be(0, $"process output: '{outputApplication}'");
        }

        [TestMethod]
        public void SampleMySQL_SelfUpdatingApp_Run_WithoutErrors()
        {
            (int exitCodeApplication, string outputApplication) = ProcessHelper.RunProcess(s_sampleSelfUpdatingAppAssemblyPath);
            exitCodeApplication.Should().Be(0, $"process output: '{outputApplication}'");
        }
    }
}
