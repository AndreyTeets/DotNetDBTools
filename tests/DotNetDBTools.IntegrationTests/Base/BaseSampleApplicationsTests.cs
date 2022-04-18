using System;
using System.IO;
using DotNetDBTools.IntegrationTests.Utilities;
using FluentAssertions;
using NUnit.Framework;

namespace DotNetDBTools.IntegrationTests.Base;

[TestFixture]
public abstract class BaseSampleApplicationsTests
{
    protected abstract string SyncScopeName { get; }

    protected abstract string DtDbms { get; }
    protected abstract string DtAsm { get; }
    protected abstract string DtAsmV2 { get; }
    protected abstract string DtCs { get; }

    protected abstract string DeployToolAssemblyPath { get; }
    protected abstract string SampleDeployManagerUsageAssemblyPath { get; }
    protected abstract string SampleBusinessLogicOnlyAppAssemblyPath { get; }
    protected abstract string SampleSelfUpdatingAppAssemblyPath { get; }

    [Test]
    public void DeployTool_Runs_WithoutErrors()
    {
        string dtPath = DeployToolAssemblyPath;
        string outDir = Path.GetFullPath($"{Directory.GetCurrentDirectory()}/DeployTool_Output/{DtDbms}");
        using IDisposable _ = ExclusiveExecutionScope.CreateScope(SyncScopeName);

        DropDeployToolDatabaseIfExists();
        CreateDeployToolDatabase();

        Exec(dtPath, $"register --dbms={DtDbms} \"--cs={DtCs}\"");

        Exec(dtPath, $"scriptnew --dbms={DtDbms} --asm={DtAsm} \"--out={outDir}/new.sql\"");
        ExecuteSqlOnDeployToolDatabase(File.ReadAllText($"{outDir}/new.sql"));

        Exec(dtPath, $"publish --loss --dbms={DtDbms} --asm={DtAsmV2} \"--cs={DtCs}\"");

        Exec(dtPath, $"scriptasmdiff --loss --dbms={DtDbms} --newasm={DtAsm} --oldasm={DtAsmV2} \"--out={outDir}/v2tov1.sql\"");
        ExecuteSqlOnDeployToolDatabase(File.ReadAllText($"{outDir}/v2tov1.sql"));

        Exec(dtPath, $"scriptupdate --loss --dbms={DtDbms} --asm={DtAsmV2} \"--cs={DtCs}\" \"--out={outDir}/v1tov2.sql\"");
        ExecuteSqlOnDeployToolDatabase(File.ReadAllText($"{outDir}/v1tov2.sql"));

        Exec(dtPath, $"definition --dbms={DtDbms} \"--cs={DtCs}\" \"--out={outDir}/v2def\"");

        Exec(dtPath, $"unregister --dbms={DtDbms} \"--cs={DtCs}\"");

        Exec(dtPath, $"scriptasmdiff --loss --no-dndbt-info --dbms={DtDbms} --newasm={DtAsm} --oldasm={DtAsmV2} \"--out={outDir}/v2tov1nodndbtinfo.sql\"");
        ExecuteSqlOnDeployToolDatabase(File.ReadAllText($"{outDir}/v2tov1nodndbtinfo.sql"));
    }

    [Test]
    public void SampleDeployManagerUsage_And_SampleBusinessLogicOnlyApp_Run_And_Rerun_WithoutErrors()
    {
        using IDisposable _ = ExclusiveExecutionScope.CreateScope(SyncScopeName);

        Exec(SampleDeployManagerUsageAssemblyPath);
        Exec(SampleBusinessLogicOnlyAppAssemblyPath);
        Exec(SampleBusinessLogicOnlyAppAssemblyPath);
    }

    [Test]
    public void SampleSelfUpdatingApp_Runs_And_Reruns_WithoutErrors()
    {
        using IDisposable _ = ExclusiveExecutionScope.CreateScope(SyncScopeName);

        DropSelfUpdatingAppDatabaseIfExists();

        Exec(SampleSelfUpdatingAppAssemblyPath);
        Exec(SampleSelfUpdatingAppAssemblyPath);
    }

    protected abstract void ExecuteSqlOnDeployToolDatabase(string sql);
    protected abstract void DropDeployToolDatabaseIfExists();
    protected abstract void CreateDeployToolDatabase();
    protected abstract void DropSelfUpdatingAppDatabaseIfExists();

    private void Exec(string filePath, string args = null)
    {
        (int exitCode, string output) = ProcessRunner.RunProcess(filePath, args);
        exitCode.Should().Be(0, $"process output: '{output}'");
    }
}
