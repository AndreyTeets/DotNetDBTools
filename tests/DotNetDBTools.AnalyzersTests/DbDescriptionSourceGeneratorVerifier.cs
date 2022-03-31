using System;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.Text;

namespace DotNetDBTools.AnalyzersTests;

public static class DbDescriptionSourceGeneratorVerifier<TSourceGenerator>
    where TSourceGenerator : ISourceGenerator, new()
{
    public static async Task VerifyGeneratorAsync(
        string source,
        string expectedGeneratedCode,
        params DiagnosticResult[] expected)
    {
        DbDescriptionSourceGeneratorTest test = new(source, expectedGeneratedCode, expected);
        await test.RunAsync(CancellationToken.None);
    }

    public class DbDescriptionSourceGeneratorTest : CSharpSourceGeneratorTest<TSourceGenerator, XUnitVerifier>
    {
        public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.Default;

        private const string GeneratedFileName = "TestProjectDescription.cs";

        public DbDescriptionSourceGeneratorTest(
            string source,
            string expectedGeneratedCode,
            params DiagnosticResult[] expected)
        {
            TestCode = source;
            ExpectedDiagnostics.AddRange(expected);
            TestState.AdditionalReferences.Add(typeof(DotNetDBTools.Definition.Core.IDbObject).Assembly);
            if (expectedGeneratedCode is not null)
            {
                TestState.GeneratedSources.Add(
                    (typeof(TSourceGenerator),
                    GeneratedFileName,
                    SourceText.From(expectedGeneratedCode, Encoding.UTF8, SourceHashAlgorithm.Sha1)));
            }
        }

        protected override ParseOptions CreateParseOptions()
        {
            return ((CSharpParseOptions)base.CreateParseOptions()).WithLanguageVersion(LanguageVersion);
        }

        protected override CompilationOptions CreateCompilationOptions()
        {
            CompilationOptions compilationOptions = base.CreateCompilationOptions();
            return compilationOptions.WithSpecificDiagnosticOptions(
                compilationOptions.SpecificDiagnosticOptions.SetItems(GetNullableWarningsFromCompiler()));
        }

        private static ImmutableDictionary<string, ReportDiagnostic> GetNullableWarningsFromCompiler()
        {
            string[] args = { "/warnaserror:nullable" };
            CSharpCommandLineArguments commandLineArguments = CSharpCommandLineParser.Default.Parse(
                args, baseDirectory: Environment.CurrentDirectory, sdkDirectory: Environment.CurrentDirectory);
            ImmutableDictionary<string, ReportDiagnostic> nullableWarnings = commandLineArguments.CompilationOptions.SpecificDiagnosticOptions;
            return nullableWarnings;
        }
    }
}
