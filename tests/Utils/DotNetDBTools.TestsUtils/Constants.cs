namespace DotNetDBTools.TestsUtils
{
    public static class Constants
    {
        public const string RepoRoot = "../../../../../..";
#if DEBUG
        public const string Configuration = "Debug";
#else
        public const string Configuration = "Release";
#endif
        public const string TargetFramework = "netcoreapp3.1";

        public static readonly string ProjectsOutDirPath = $"bin/{Configuration}/{TargetFramework}";
    }
}
