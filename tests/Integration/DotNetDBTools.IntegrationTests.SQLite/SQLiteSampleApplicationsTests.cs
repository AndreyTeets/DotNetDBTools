using DotNetDBTools.TestsUtils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.TestsUtils.Constants;

namespace DotNetDBTools.IntegrationTests.SQLite
{
    [TestClass]
    public class SQLiteSampleApplicationsTests
    {
        private static readonly string s_deployAssemblyPath = $"{RepoRoot}/samples/DeployUtils/DotNetDBTools.SampleDeployUtil.SQLite/{ProjectsOutDirPath}/DotNetDBTools.SampleDeployUtil.SQLite.exe";
        private static readonly string s_applicationAssemblyPath = $"{RepoRoot}/samples/Applications/DotNetDBTools.SampleApplication.Agnostic/{ProjectsOutDirPath}/DotNetDBTools.SampleApplication.Agnostic.exe";

        [TestMethod]
        public void SampleSQLiteProjects_Run_WithoutErrors()
        {
            (int exitCodeDeploy, string outputDeploy) = ProcessHelper.RunProcess(s_deployAssemblyPath);
            exitCodeDeploy.Should().Be(0, $"process output: '{outputDeploy}'");

            (int exitCodeAgnosticApplication, string outputAgnosticApplication) = ProcessHelper.RunProcess(s_applicationAssemblyPath);
            exitCodeAgnosticApplication.Should().Be(0, $"process output: '{outputAgnosticApplication}'");
        }
    }
}
