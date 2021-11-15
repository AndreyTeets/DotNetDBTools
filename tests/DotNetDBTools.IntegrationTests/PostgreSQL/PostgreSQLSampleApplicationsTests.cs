using DotNetDBTools.IntegrationTests.TestHelpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.PostgreSQL
{
    [TestClass]
    public class PostgreSQLSampleApplicationsTests
    {
        private static readonly string s_sampleDeployToolAssemblyPath = $"{RepoRoot}/samples/DeployTools/DotNetDBTools.SampleDeployTool.PostgreSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployTool.PostgreSQL.dll";
        private static readonly string s_sampleBusinessLogicOnlyAppAssemblyPath = $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.PostgreSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleBusinessLogicOnlyApp.PostgreSQL.dll";
        private static readonly string s_sampleSelfUpdatingAppAssemblyPath = $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.PostgreSQL/{ProjectsOutDirPath}/DotNetDBTools.SampleSelfUpdatingApp.PostgreSQL.dll";

        [TestMethod]
        public void SamplePostgreSQL_DeployToolAndBusinessLogicOnlyApps_Run_WithoutErrors()
        {
            (int exitCodeDeploy, string outputDeploy) = ProcessHelper.RunProcess(s_sampleDeployToolAssemblyPath);
            exitCodeDeploy.Should().Be(0, $"process output: '{outputDeploy}'");

            (int exitCodeApplication, string outputApplication) = ProcessHelper.RunProcess(s_sampleBusinessLogicOnlyAppAssemblyPath);
            exitCodeApplication.Should().Be(0, $"process output: '{outputApplication}'");
        }

        [TestMethod]
        public void SamplePostgreSQL_SelfUpdatingApp_Run_WithoutErrors()
        {
            (int exitCodeApplication, string outputApplication) = ProcessHelper.RunProcess(s_sampleSelfUpdatingAppAssemblyPath);
            exitCodeApplication.Should().Be(0, $"process output: '{outputApplication}'");
        }
    }
}
