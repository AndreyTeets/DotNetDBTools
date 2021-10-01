using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace DotNetDBTools.AnalyzersTests
{
    public static class DbDefinitionAnalyzerVerifier<TAnalyzer>
       where TAnalyzer : DiagnosticAnalyzer, new()
    {
        public static DiagnosticResult Diagnostic(string diagnosticId)
        {
            return CSharpAnalyzerVerifier<TAnalyzer, XUnitVerifier>.Diagnostic(diagnosticId);
        }

        public static async Task VerifyAnalyzerAsync(
           string source,
           params DiagnosticResult[] expected)
        {
            DbDefinitionAnalyzerTest test = new(source, expected);
            await test.RunAsync(CancellationToken.None);
        }

        private class DbDefinitionAnalyzerTest : CSharpAnalyzerTest<TAnalyzer, XUnitVerifier>
        {
            public DbDefinitionAnalyzerTest(
               string source,
               params DiagnosticResult[] expected)
            {
                TestCode = source;
                ExpectedDiagnostics.AddRange(expected);
                TestState.AdditionalReferences.Add(typeof(DotNetDBTools.Definition.MSSQL.ITable).Assembly);
            }
        }
    }
}
