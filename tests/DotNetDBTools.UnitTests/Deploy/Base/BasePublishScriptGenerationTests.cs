using System.IO;
using System.Reflection;
using DotNetDBTools.Deploy;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Deploy.Base
{
    public abstract class BasePublishScriptGenerationTests<TDeployManager>
        where TDeployManager : IDeployManager, new()
    {
        protected abstract string SampleDbV1AssemblyName { get; }
        protected abstract string SampleDbV2AssemblyName { get; }
        protected abstract string ActualFilesDir { get; }
        protected abstract string ExpectedFilesDir { get; }

        private readonly IDeployManager _deployManager;
        private readonly Assembly _dbAssemblyV1;
        private readonly Assembly _dbAssemblyV2;

        protected BasePublishScriptGenerationTests()
        {
            _deployManager = new TDeployManager();
            _dbAssemblyV1 = TestDbAssembliesHelper.GetSampleDbAssembly(SampleDbV1AssemblyName);
            _dbAssemblyV2 = TestDbAssembliesHelper.GetSampleDbAssembly(SampleDbV2AssemblyName);
        }

        [Fact]
        public void Generate_PublishScript_For_SampleDB_CreatesCorrectScript_WhenCreatingV1()
        {
            string outputPath = @$"{ActualFilesDir}/Actual_PublishScript_For_SampleDB_WhenCreatingV1.sql";
            _deployManager.GeneratePublishScript(_dbAssemblyV1, outputPath);

            string actualScript = File.ReadAllText(outputPath);
            string expectedScript = File.ReadAllText(@$"{ExpectedFilesDir}/Expected_PublishScript_For_SampleDB_WhenCreatingV1.sql");
            actualScript.NormalizeLineEndings().Should().Be(expectedScript.NormalizeLineEndings());
        }

        [Fact]
        public void Generate_PublishScript_For_SampleDB_CreatesCorrectScript_WhenUpdatingFromV1ToV2()
        {
            _deployManager.Options = new DeployOptions { AllowDataLoss = true };

            string outputPath = @$"{ActualFilesDir}/Actual_PublishScript_For_SampleDB_WhenUpdatingFromV1ToV2.sql";
            _deployManager.GeneratePublishScript(_dbAssemblyV2, _dbAssemblyV1, outputPath);

            string actualScript = File.ReadAllText(outputPath);
            string expectedScript = File.ReadAllText(@$"{ExpectedFilesDir}/Expected_PublishScript_For_SampleDB_WhenUpdatingFromV1ToV2.sql");
            actualScript.NormalizeLineEndings().Should().Be(expectedScript.NormalizeLineEndings());
        }

        [Fact]
        public void Generate_PublishScript_For_SampleDB_CreatesCorrectScript_WhenExistingDbIsEqual()
        {
            string outputPath1 = @$"{ActualFilesDir}/ActualPublishScript_For_SampleDB_WhenUpdatingFromV1ToV1.sql";
            _deployManager.GeneratePublishScript(_dbAssemblyV1, _dbAssemblyV1, outputPath1);
            string actualScript1 = File.ReadAllText(outputPath1);
            actualScript1.Should().Be("");

            string outputPath2 = @$"{ActualFilesDir}/Actual_PublishScript_For_SampleDB_WhenUpdatingFromV2ToV2.sql";
            _deployManager.GeneratePublishScript(_dbAssemblyV2, _dbAssemblyV2, outputPath2);
            string actualScript2 = File.ReadAllText(outputPath2);
            actualScript2.Should().Be("");
        }
    }
}
