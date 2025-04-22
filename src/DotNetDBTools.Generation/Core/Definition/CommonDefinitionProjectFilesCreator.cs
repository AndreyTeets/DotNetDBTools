using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetDBTools.Generation.Core.Definition;

internal static class CommonDefinitionProjectFilesCreator
{
    public static IEnumerable<DefinitionSourceFile> Create(
        string projectNamespace, OutputDefinitionKind outputDefinitionKind, string declaredDefitionKind)
    {
        List<DefinitionSourceFile> res = new();
        if (outputDefinitionKind == OutputDefinitionKind.CSharp)
            res.Add(CreateStringExtensionsFile(projectNamespace));
        res.Add(CreateDatabaseSettingsFile(declaredDefitionKind));
        res.Add(CreateProjectFile(projectNamespace, outputDefinitionKind));
        return res;
    }

    private static DefinitionSourceFile CreateStringExtensionsFile(string projectNamespace)
    {
        string code =
$@"using System;
using System.IO;
using System.Reflection;

namespace {projectNamespace}
{{
    public static class StringExtensions
    {{
        public static string AsSqlResource(this string sqlResource)
        {{
            return GetEmbeddedResourceAsString($""Sql.{{sqlResource}}"");
        }}

        private static string GetEmbeddedResourceAsString(string resourceName)
        {{
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream($""{{assembly.GetName().Name}}.{{resourceName}}"");
            if (stream is null)
                throw new Exception($""Failed to get embedded resource '{{resourceName}}'"");
            using StreamReader sr = new(stream);
            return sr.ReadToEnd();
        }}
    }}
}}";

        return new DefinitionSourceFile()
        {
            RelativePath = $"StringExtensions.cs",
            SourceText = code.NormalizeLineEndings(),
        };
    }

    private static DefinitionSourceFile CreateDatabaseSettingsFile(string declaredDefitionKind)
    {
        string code =
$@"using DotNetDBTools.Definition;

[assembly: DatabaseSettings(
    DefinitionKind = DefinitionKind.{declaredDefitionKind},
    DatabaseVersion = 1
)]";

        return new DefinitionSourceFile()
        {
            RelativePath = $"DatabaseSettings.cs",
            SourceText = code.NormalizeLineEndings(),
        };
    }

    private static DefinitionSourceFile CreateProjectFile(string projectNamespace, OutputDefinitionKind outputDefinitionKind)
    {
        string dndbtVersion = Assembly.GetCallingAssembly()
            .GetCustomAttributes<AssemblyInformationalVersionAttribute>()
            .SingleOrDefault()
            .InformationalVersion;

        string embeddedResourcesPaths = outputDefinitionKind == OutputDefinitionKind.CSharp
            ? @"Sql\**\*.sql"
            : @"*\**\*.sql";

        string code =
$@"<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <LangVersion>9.0</LangVersion>
    <TargetFramework>netstandard2.0</TargetFramework>
    <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
    <CompilerGeneratedFilesOutputPath>obj/Generated</CompilerGeneratedFilesOutputPath>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""DotNetDBTools.Definition"" Version=""{dndbtVersion}"" PrivateAssets=""all"" />
    <PackageReference Include=""DotNetDBTools.DefinitionAnalyzer"" Version=""{dndbtVersion}"" PrivateAssets=""all"" />
    <PackageReference Include=""DotNetDBTools.DescriptionSourceGenerator"" Version=""{dndbtVersion}"" PrivateAssets=""all"" />
  </ItemGroup>

  <ItemGroup>
    <EmbeddedResource Include=""{embeddedResourcesPaths}"">
      <CopyToOutputDirectory>Never</CopyToOutputDirectory>
    </EmbeddedResource>
    <CompilerVisibleItemMetadata Include=""AdditionalFiles"" MetadataName=""Category"" />
    <AdditionalFiles Include=""@(EmbeddedResource)"" Category=""EmbeddedFile"" />
  </ItemGroup>

</Project>";

        return new DefinitionSourceFile()
        {
            RelativePath = $"{projectNamespace}.csproj",
            SourceText = code.NormalizeLineEndings(),
        };
    }
}
