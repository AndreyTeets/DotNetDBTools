namespace DotNetDBTools.IntegrationTests;

public static class Constants
{
    public const string RepoRoot = "../../../../..";
    public static readonly string SamplesOutputDir = $"{RepoRoot}/SamplesOutput";
    public static readonly string SamplesOutputDirRelToSampleApps = $"../{RepoRoot}/SamplesOutput";
#if DEBUG
    public const string Configuration = "Debug";
#else
    public const string Configuration = "Release";
#endif
    public const string TargetFrameworkNetCore31 = "netcoreapp3.1";
    public const string TargetFrameworkNet60 = "net6.0";

    public static readonly string ProjectsOutDirPathNetCore31 = $"bin/{Configuration}/{TargetFrameworkNetCore31}";
    public static readonly string ProjectsOutDirPathNet60 = $"bin/{Configuration}/{TargetFrameworkNet60}";
}
