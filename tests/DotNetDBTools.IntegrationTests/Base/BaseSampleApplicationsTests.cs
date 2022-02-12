using System;
using DotNetDBTools.IntegrationTests.Utilities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetDBTools.IntegrationTests.Base;

public abstract class BaseSampleApplicationsTests
{
    protected abstract string SyncScopeName { get; }
    protected abstract string SampleDeployManagerUsageAssemblyPath { get; }
    protected abstract string SampleBusinessLogicOnlyAppAssemblyPath { get; }
    protected abstract string SampleSelfUpdatingAppAssemblyPath { get; }

    [TestMethod]
    public void Sample_DeployToolAndBusinessLogicOnlyApps_Run_WithoutErrors()
    {
        using IDisposable _ = ExclusiveExecutionScope.CreateScope(SyncScopeName);

        (int exitCodeDeploy, string outputDeploy) = ProcessRunner.RunProcess(SampleDeployManagerUsageAssemblyPath);
        exitCodeDeploy.Should().Be(0, $"process output: '{outputDeploy}'");

        (int exitCodeApplication, string outputApplication) = ProcessRunner.RunProcess(SampleBusinessLogicOnlyAppAssemblyPath);
        exitCodeApplication.Should().Be(0, $"process output: '{outputApplication}'");
    }

    [TestMethod]
    public void Sample_SelfUpdatingApp_Runs_WithoutErrors()
    {
        using IDisposable _ = ExclusiveExecutionScope.CreateScope(SyncScopeName);

        DropSelfUpdatingAppDatabaseIfExists();

        (int exitCodeApplication, string outputApplication) = ProcessRunner.RunProcess(SampleSelfUpdatingAppAssemblyPath);
        exitCodeApplication.Should().Be(0, $"process output: '{outputApplication}'");
    }

    protected abstract void DropSelfUpdatingAppDatabaseIfExists();
}
