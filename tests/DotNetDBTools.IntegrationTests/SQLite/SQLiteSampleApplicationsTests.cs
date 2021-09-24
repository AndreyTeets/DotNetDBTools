using DotNetDBTools.IntegrationTests.TestHelpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.SQLite
{
    [TestClass]
    public class SQLiteSampleApplicationsTests
    {
        private static readonly string s_sampleDeployToolAssemblyPath = $"{RepoRoot}/samples/DeployTools/DotNetDBTools.SampleDeployTool.SQLite/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployTool.SQLite.dll";
        private static readonly string s_sampleBusinessLogicOnlyAppAssemblyPath = $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.SQLite/{ProjectsOutDirPath}/DotNetDBTools.SampleBusinessLogicOnlyApp.SQLite.dll";
        private static readonly string s_sampleSelfUpdatingAppAssemblyPath = $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.SQLite/{ProjectsOutDirPath}/DotNetDBTools.SampleSelfUpdatingApp.SQLite.dll";

        [TestMethod]
        public void SampleSQLite_DeployToolAndBusinessLogicOnlyApps_Run_WithoutErrors()
        {
            (int exitCodeDeploy, string outputDeploy) = ProcessHelper.RunProcess(s_sampleDeployToolAssemblyPath);
            exitCodeDeploy.Should().Be(0, $"process output: '{outputDeploy}'");

            (int exitCodeApplication, string outputApplication) = ProcessHelper.RunProcess(s_sampleBusinessLogicOnlyAppAssemblyPath);
            exitCodeApplication.Should().Be(0, $"process output: '{outputApplication}'");
        }

        [TestMethod]
        public void SampleSQLite_SelfUpdatingApp_Run_WithoutErrors()
        {
            (int exitCodeApplication, string outputApplication) = ProcessHelper.RunProcess(s_sampleSelfUpdatingAppAssemblyPath);
            exitCodeApplication.Should().Be(0, $"process output: '{outputApplication}'");
        }
    }
}
