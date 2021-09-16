using DotNetDBTools.CommonTestsUtils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetDBTools.IntegrationTests.SQLite
{
    [TestClass]
    public class SQLiteSampleApplicationsTests
    {
#if DEBUG
        private const string Configuration = "Debug";
#else
        private const string Configuration = "Release";
#endif
        private const string DeployAssemblyBinDir = "../../../../../Samples/DotNetDBTools.SampleDeployUtil.SQLite/bin";
        private const string AgnosticApplicationAssemblBinDir = "../../../../../Samples/DotNetDBTools.SampleApplication.Agnostic/bin";
        private static readonly string s_deployAssemblyPath = $"{DeployAssemblyBinDir}/{Configuration}/netcoreapp3.1/DotNetDBTools.SampleDeployUtil.SQLite.exe";
        private static readonly string s_applicationAssemblyPath = $"{AgnosticApplicationAssemblBinDir}/{Configuration}/netcoreapp3.1/DotNetDBTools.SampleApplication.Agnostic.exe";

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
