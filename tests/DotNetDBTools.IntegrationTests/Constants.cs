namespace DotNetDBTools.IntegrationTests
{
    public static class Constants
    {
        public const string RepoRoot = "../../../../..";
        public static readonly string SamplesOutputDir = $"{RepoRoot}/SamplesOutput";
#if DEBUG
        public const string Configuration = "Debug";
#else
        public const string Configuration = "Release";
#endif
        public const string TargetFramework = "netcoreapp3.1";

        public static readonly string ProjectsOutDirPath = $"bin/{Configuration}/{TargetFramework}";
    }
}
