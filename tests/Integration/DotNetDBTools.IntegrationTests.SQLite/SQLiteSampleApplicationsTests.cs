using DotNetDBTools.TestsUtils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.TestsUtils.Constants;

namespace DotNetDBTools.IntegrationTests.SQLite
{
    [TestClass]
    public class SQLiteSampleApplicationsTests
    {
        private static readonly string s_sampleDeployToolAssemblyPath = $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleDeployTool.SQLite/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployTool.SQLite.exe";
        private static readonly string s_sampleBusinessLogicOnlyAppAssemblyPath = $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleBusinessLogicOnlyApp.SQLite/{ProjectsOutDirPath}/DotNetDBTools.SampleBusinessLogicOnlyApp.SQLite.exe";
        private static readonly string s_sampleSelfUpdatingAppAssemblyPath = $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleSelfUpdatingApp.SQLite/{ProjectsOutDirPath}/DotNetDBTools.SampleSelfUpdatingApp.SQLite.exe";

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
