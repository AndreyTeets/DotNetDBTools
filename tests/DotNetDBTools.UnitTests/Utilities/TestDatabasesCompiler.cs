using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DotNetDBTools.RoslynUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DotNetDBTools.UnitTests.Utilities;

internal static class TestDatabasesCompiler
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

        foreach (string filePath in Directory.EnumerateFiles(projectDir, "*.cs", SearchOption.TopDirectoryOnly))
            syntaxTrees.Add(CreateSyntaxTree(MiscHelper.ReadFromFile(filePath)));

        foreach (string subdir in Directory.EnumerateDirectories(projectDir, "*", SearchOption.TopDirectoryOnly)
            .Where(dir => !dir.Replace(@"\", "/").EndsWith("obj", StringComparison.OrdinalIgnoreCase)))
        {
            foreach (string filePath in Directory.EnumerateFiles(subdir, "*.cs", SearchOption.AllDirectories))
                syntaxTrees.Add(CreateSyntaxTree(MiscHelper.ReadFromFile(filePath)));
        }

        return syntaxTrees;

        static SyntaxTree CreateSyntaxTree(string code)
        {
            // DbAssembly is always netstandard2.0 and doesn't have conflict with System.Index,
            // but netcoreapp3.0+ does and here it is being compiled with roslyn against 3.0+ system references.
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            List<TypeSyntax> typeSyntaxesOfFieldsWithNameEqualIndex = syntaxTree.GetRoot().DescendantNodes()
                .OfType<FieldDeclarationSyntax>()
                .Where(x => TypeNameEqualIndex(x.Declaration.Type))
                .Select(x => x.Declaration.Type)
                .ToList();

            if (typeSyntaxesOfFieldsWithNameEqualIndex.Any())
            {
                string dbms = GetDBMSNameFromUsings(syntaxTree);
                TypeSyntax fullyQualifiedIndexTypeSyntax = CreateFullyQualifiedIndexTypeSyntax(dbms);
                return SyntaxFactory.SyntaxTree(syntaxTree.GetRoot()
                    .ReplaceNodes(typeSyntaxesOfFieldsWithNameEqualIndex, (type, _) => fullyQualifiedIndexTypeSyntax));
            }
            return syntaxTree;

            static bool TypeNameEqualIndex(TypeSyntax type)
            {
                return ((IdentifierNameSyntax)type).Identifier.Text == "Index";
            }

            static string GetDBMSNameFromUsings(SyntaxTree syntaxTree)
            {
                List<string> usingStatements = syntaxTree.GetRoot().DescendantNodes()
                    .OfType<UsingDirectiveSyntax>()
                    .Select(x => x.ToString())
                    .ToList();
                foreach (string usingStatement in usingStatements)
                {
                    string pattern = "using DotNetDBTools.Definition.(?<dbms>Agnostic|MSSQL|MySQL|PostgreSQL|SQLite);";
                    Match match = Regex.Match(usingStatement, pattern);
                    if (match.Groups[0].Success)
                        return match.Groups["dbms"].Value;
                }
                return null;
            }

            static TypeSyntax CreateFullyQualifiedIndexTypeSyntax(string dbms)
            {
                return SyntaxFactory.ParseTypeName($"DotNetDBTools.Definition.{dbms}.Index");
            }
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

            ResourceDescription resource = new(
                resourceName,
                () => File.OpenRead(filePath),
                isPublic: true
            );
            resourcesList.Add(resource);
        }
        return resourcesList;
    }
}
