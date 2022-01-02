using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Emit;

namespace DotNetDBTools.RoslynUtilities
{
    internal static class CompilationHelper
    {
        public static IEnumerable<ResourceDescription> GetEmbeddedResourcesIfAny(
            AnalyzerConfigOptions analyzerConfigOptions,
            AnalyzerConfigOptionsProvider analyzerConfigOptionsProvider,
            ImmutableArray<AdditionalText> additionalFiles)
        {
            IEnumerable<string> embeddedFilesPaths = additionalFiles
                .Where(f => analyzerConfigOptionsProvider.GetOptions(f)
                    .TryGetValue("build_metadata.AdditionalFiles.Category", out string category) && category == "EmbeddedFile")
                .Select(f => f.Path);

            IEnumerable<ResourceDescription> resources = null;
            if (embeddedFilesPaths.Count() > 0)
            {
                if (!analyzerConfigOptions.TryGetValue("build_property.ProjectDir", out string projectDirectory))
                    throw new Exception("MSBuild property 'ProjectDir' is missing");

                if (!analyzerConfigOptions.TryGetValue("build_property.RootNamespace", out string rootNamespace))
                    throw new Exception("MSBuild property 'RootNamespace' is missing");

                resources = CreateEmbeddedResourcesDescriptions(projectDirectory, rootNamespace, embeddedFilesPaths);
            }
            return resources;
        }

        public static Assembly CompileInMemoryAnLoad(Compilation compilation, IEnumerable<ResourceDescription> resources = null)
        {
            using MemoryStream assemblyStream = new();
            EmitResult result = compilation.Emit(assemblyStream, manifestResources: resources);
            if (!result.Success)
                throw new Exception(GetCompilationErrorString(result));
            byte[] assemblyBytes = assemblyStream.ToArray();
            Assembly assembly = Assembly.Load(assemblyBytes);
            return assembly;
        }

        private static IEnumerable<ResourceDescription> CreateEmbeddedResourcesDescriptions(
            string projectDirectory, string rootNamespace, IEnumerable<string> embeddedFilesPaths)
        {
            List<ResourceDescription> resourcesList = new();
            foreach (string filePath in embeddedFilesPaths)
            {
                string relativeFilePath;
                if (filePath.StartsWith(projectDirectory, StringComparison.OrdinalIgnoreCase))
                    relativeFilePath = filePath.Substring(projectDirectory.Length);
                else
                    throw new Exception($"Invalid embedded file path '{filePath}' for project directory '{projectDirectory}'");
                string normalizedFilePath = relativeFilePath.Replace(@"/", ".").Replace(@"\", ".");
                string resourceName = $"{rootNamespace}.{normalizedFilePath}";

                ResourceDescription resourse = new(
                    resourceName,
                    () => File.OpenRead(filePath),
                    isPublic: true
                );
                resourcesList.Add(resourse);
            }
            return resourcesList;
        }

        private static string GetCompilationErrorString(EmitResult result)
        {
            DiagnosticFormatter formatter = new();
            List<string> errors = new(result.Diagnostics.Select(d => formatter.Format(d)));
            return string.Join("\n", errors);
        }
    }
}
