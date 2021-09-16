using DotNetDBTools.CommonTestsUtils;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetDBTools.IntegrationTests.MSSQL
{
    [TestClass]
    public class MSSQLSampleApplicationsTests
    {
#if DEBUG
        private const string Configuration = "Debug";
#else
        private const string Configuration = "Release";
#endif
        private const string DeployAssemblyBinDir = "../../../../../Samples/DotNetDBTools.SampleDeployUtil.MSSQL/bin";
        private static readonly string s_deployAssemblyPath = $"{DeployAssemblyBinDir}/{Configuration}/netcoreapp3.1/DotNetDBTools.SampleDeployUtil.MSSQL.exe";

        [TestMethod]
        public void SampleMSSQLProjects_Run_WithoutErrors()
        {
            (int exitCodeDeploy, string outputDeploy) = ProcessHelper.RunProcess(s_deployAssemblyPath);
            exitCodeDeploy.Should().Be(0, $"process output: '{outputDeploy}'");
        }
    }
}
