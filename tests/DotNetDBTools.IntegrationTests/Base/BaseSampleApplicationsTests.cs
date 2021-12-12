﻿using System;
using DotNetDBTools.IntegrationTests.TestHelpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetDBTools.IntegrationTests.Base
{
    public abstract class BaseSampleApplicationsTests
    {
        protected abstract string SyncScopeName { get; }
        protected abstract string SampleDeployToolAssemblyPath { get; }
        protected abstract string SampleBusinessLogicOnlyAppAssemblyPath { get; }
        protected abstract string SampleSelfUpdatingAppAssemblyPath { get; }

        [TestMethod]
        public void Sample_DeployToolAndBusinessLogicOnlyApps_Run_WithoutErrors()
        {
            using IDisposable _ = ExclusiveExecutionScope.CreateScope(SyncScopeName);

            (int exitCodeDeploy, string outputDeploy) = ProcessHelper.RunProcess(SampleDeployToolAssemblyPath);
            exitCodeDeploy.Should().Be(0, $"process output: '{outputDeploy}'");

            (int exitCodeApplication, string outputApplication) = ProcessHelper.RunProcess(SampleBusinessLogicOnlyAppAssemblyPath);
            exitCodeApplication.Should().Be(0, $"process output: '{outputApplication}'");
        }

        [TestMethod]
        public void Sample_SelfUpdatingApp_Runs_WithoutErrors()
        {
            using IDisposable _ = ExclusiveExecutionScope.CreateScope(SyncScopeName);

            (int exitCodeApplication, string outputApplication) = ProcessHelper.RunProcess(SampleSelfUpdatingAppAssemblyPath);
            exitCodeApplication.Should().Be(0, $"process output: '{outputApplication}'");
        }
    }
}
