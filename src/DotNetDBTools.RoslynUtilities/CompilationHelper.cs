using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Emit;

namespace DotNetDBTools.RoslynUtilities;

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
        if (embeddedFilesPaths.Any())
        {
            if (!analyzerConfigOptions.TryGetValue("build_property.ProjectDir", out string projectDirectory))
                throw new Exception("MSBuild property 'ProjectDir' is missing");

            if (!analyzerConfigOptions.TryGetValue("build_property.RootNamespace", out string rootNamespace))
                throw new Exception("MSBuild property 'RootNamespace' is missing");

            resources = CreateEmbeddedResourcesDescriptions(projectDirectory, rootNamespace, embeddedFilesPaths);
        }
        return resources;
    }

    public static Assembly CompileInMemoryAnLoad(
        Compilation compilation,
        IEnumerable<ResourceDescription> resources = null)
    {
        using MemoryStream assemblyStream = new();
        EmitResult result = compilation.Emit(assemblyStream, manifestResources: resources);
        if (!result.Success)
            throw new Exception(GetCompilationErrorString(result));

        Assembly dbAssembly;
        string currentRuntime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        if (currentRuntime.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase))
            dbAssembly = LoadNetFramework(assemblyStream);
        else // NET Core
            dbAssembly = LoadNetCore(assemblyStream, compilation.AssemblyName);
        return dbAssembly;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Assembly LoadNetFramework(MemoryStream assemblyStream)
    {
        byte[] assemblyBytes = assemblyStream.ToArray();
        return Assembly.Load(assemblyBytes);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static Assembly LoadNetCore(MemoryStream assemblyStream, string assemblyName)
    {
        AssemblyLoadContext currentALC = AssemblyLoadContext.GetLoadContext(Assembly.GetExecutingAssembly());
        if (currentALC == AssemblyLoadContext.Default)
        {
            byte[] assemblyBytes = assemblyStream.ToArray();
            return Assembly.Load(assemblyBytes);
        }
        else // Analyzer in NET SDK 6.0+
        {
            assemblyStream.Position = 0;
            return GetAssemblyIfAlreadyLoadedInCurrentALCOrLoad(assemblyStream, assemblyName, currentALC);
        }
    }

    private static readonly object s_assemblyLoadSectionSyncObject = new();
    private static Assembly GetAssemblyIfAlreadyLoadedInCurrentALCOrLoad(
        MemoryStream assemblyStream,
        string assemblyName,
        AssemblyLoadContext currentALC)
    {
        // Load to current ALC because NET SDK 6.0+ started loading each analyzer+it's dependencies into separate ALC.
        // Check if assembly is loaded into current ALC because build server may have already loaded (attempt to load same assembly again will fail)
        // all assemblies (including those that are loaded manually here) on previous builds (ALC's stay same on each build).
        lock (s_assemblyLoadSectionSyncObject)
        {
            IEnumerable<Assembly> allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            IEnumerable<Assembly> currentALCAssemblies = allAssemblies
                .Where(x => AssemblyLoadContext.GetLoadContext(x) == currentALC);

            Assembly alreadyLoadedAssembly = currentALCAssemblies
                .FirstOrDefault(x => x.GetName().Name == assemblyName);

            if (alreadyLoadedAssembly is not null)
                return alreadyLoadedAssembly;
            else
                return currentALC.LoadFromStream(assemblyStream);
        }
    }

    private static IEnumerable<ResourceDescription> CreateEmbeddedResourcesDescriptions(
        string projectDirectory,
        string rootNamespace,
        IEnumerable<string> embeddedFilesPaths)
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

            ResourceDescription resource = new(
                resourceName,
                () => File.OpenRead(filePath),
                isPublic: true
            );
            resourcesList.Add(resource);
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
