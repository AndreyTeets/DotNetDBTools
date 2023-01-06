using System.Reflection;
using DotNetDBTools.Deploy;
using DotNetDBTools.UnitTests.Utilities;
using FluentAssertions;
using Xunit;

namespace DotNetDBTools.UnitTests.Deploy.Base;

public abstract class BasePublishScriptGenerationTests<TDeployManager>
    where TDeployManager : IDeployManager, new()
{
    protected abstract string SpecificDbmsSampleDbV1AssemblyName { get; }
    protected abstract string SpecificDbmsSampleDbV2AssemblyName { get; }
    protected abstract string ExpectedFilesDir { get; }

    private static string AgnosticSampleDbV1AssemblyName => "DotNetDBTools.SampleDB.Agnostic";
    private static string AgnosticSampleDbV2AssemblyName => "DotNetDBTools.SampleDBv2.Agnostic";

    private readonly IDeployManager _deployManager;
    private readonly Assembly _agnosticDbAssemblyV1;
    private readonly Assembly _agnosticDbAssemblyV2;
    private readonly Assembly _specificDbmsDbAssemblyV1;
    private readonly Assembly _specificDbmsDbAssemblyV2;

    protected BasePublishScriptGenerationTests()
    {
        _deployManager = new TDeployManager();
        _agnosticDbAssemblyV1 = MiscHelper.GetSampleDbAssembly(AgnosticSampleDbV1AssemblyName);
        _agnosticDbAssemblyV2 = MiscHelper.GetSampleDbAssembly(AgnosticSampleDbV2AssemblyName);
        _specificDbmsDbAssemblyV1 = MiscHelper.GetSampleDbAssembly(SpecificDbmsSampleDbV1AssemblyName);
        _specificDbmsDbAssemblyV2 = MiscHelper.GetSampleDbAssembly(SpecificDbmsSampleDbV2AssemblyName);
    }

    [Fact]
    public void Generate_PublishScript_CreatesCorrectScript_WhenCreatingV1()
    {
        string actualScript = _deployManager.GeneratePublishScript(_specificDbmsDbAssemblyV1);
        string expectedScript = MiscHelper.ReadFromFile($@"{ExpectedFilesDir}/Expected_PublishScript_CreateV1.sql");
        actualScript.Should().Be(expectedScript);
    }

    [Fact]
    public void Generate_PublishScript_CreatesCorrectScript_WhenUpdatingFromV1ToV2()
    {
        _deployManager.Options = new DeployOptions { AllowDataLoss = true };
        string actualScript = _deployManager.GeneratePublishScript(_specificDbmsDbAssemblyV2, _specificDbmsDbAssemblyV1);
        string expectedScript = MiscHelper.ReadFromFile($@"{ExpectedFilesDir}/Expected_PublishScript_V1ToV2.sql");
        actualScript.Should().Be(expectedScript);
    }

    [Fact]
    public void Generate_NoDNDBTInfoPublishScript_CreatesCorrectScript_WhenUpdatingFromV2ToV1()
    {
        _deployManager.Options = new DeployOptions { AllowDataLoss = true };
        string actualScript = _deployManager.GenerateNoDNDBTInfoPublishScript(_specificDbmsDbAssemblyV1, _specificDbmsDbAssemblyV2);
        string expectedScript = MiscHelper.ReadFromFile($@"{ExpectedFilesDir}/Expected_NoDNDBTInfoPublishScript_V2ToV1.sql");
        actualScript.Should().Be(expectedScript);
    }

    [Fact]
    public void Generate_PublishScript_CreatesEmptyScript_WhenExistingDbIsEqual()
    {
        TestCase(_agnosticDbAssemblyV1);
        TestCase(_agnosticDbAssemblyV2);
        TestCase(_specificDbmsDbAssemblyV1);
        TestCase(_specificDbmsDbAssemblyV2);

        void TestCase(Assembly dbAssembly)
        {
            string actualScript = _deployManager.GeneratePublishScript(dbAssembly, dbAssembly);
            actualScript.Should().Be("");
        }
    }
}
