using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DotNetDBTools.RoslynUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DotNetDBTools.UnitTests.Utilities
{
    public static class TestDatabasesCompiler
    {
        public static Assembly CompileSampleDbProject(string projectDir)
        {
            string assemblyName = Path.GetFileName(projectDir);
            List<SyntaxTree> syntaxTrees = CreateSampleDbSyntaxTrees(projectDir);
            List<PortableExecutableReference> references = CreateSampleDbReferences();

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees,
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            IEnumerable<ResourceDescription> resources = CreateSampleDbResources(projectDir, assemblyName);
            return CompilationHelper.CompileInMemoryAnLoad(compilation, resources);
        }

        private static List<SyntaxTree> CreateSampleDbSyntaxTrees(string projectDir)
        {
            List<SyntaxTree> syntaxTrees = new();

            syntaxTrees.Add(CreateSyntaxTree("global using Index = DotNetDBTools.Definition.Agnostic.Index;"));
            foreach (string filePath in Directory.EnumerateFiles(projectDir, "*.cs", SearchOption.TopDirectoryOnly))
                syntaxTrees.Add(CreateSyntaxTree(File.ReadAllText(filePath)));

            foreach (string subdir in Directory.EnumerateDirectories(projectDir, "*", SearchOption.TopDirectoryOnly)
                .Where(dir => !dir.Replace(@"\", "/").EndsWith("obj", StringComparison.OrdinalIgnoreCase)))
            {
                foreach (string filePath in Directory.EnumerateFiles(subdir, "*.cs", SearchOption.AllDirectories))
                    syntaxTrees.Add(CreateSyntaxTree(File.ReadAllText(filePath)));
            }

            return syntaxTrees;

            static SyntaxTree CreateSyntaxTree(string code)
            {
                return CSharpSyntaxTree.ParseText(code, new CSharpParseOptions(LanguageVersion.Preview));
            }
        }

        private static List<PortableExecutableReference> CreateSampleDbReferences()
        {
            return new string[]
            {
                "System.Runtime",
                "System.Private.CoreLib",
                "netstandard",
                "DotNetDBTools.Definition",
            }.Select(asmName => MetadataReference.CreateFromFile(Assembly.Load(asmName).Location))
            .ToList();
        }

        private static IEnumerable<ResourceDescription> CreateSampleDbResources(
            string projectDir, string assemblyName)
        {
            List<ResourceDescription> resourcesList = new();
            foreach (string filePath in Directory.EnumerateFiles(projectDir, "*.sql", SearchOption.AllDirectories))
            {
                string relativeFilePath;
                if (filePath.StartsWith(projectDir, StringComparison.OrdinalIgnoreCase))
                    relativeFilePath = filePath.Substring(projectDir.Length + 1);
                else
                    throw new Exception($"Invalid embedded file path '{filePath}' for project directory '{projectDir}'");
                string normalizedFilePath = relativeFilePath.Replace(@"/", ".").Replace(@"\", ".");
                string resourceName = $"{assemblyName}.{normalizedFilePath}";

                ResourceDescription resourse = new(
                    resourceName,
                    () => File.OpenRead(filePath),
                    isPublic: true
                );
                resourcesList.Add(resourse);
            }
            return resourcesList;
        }
    }
}
