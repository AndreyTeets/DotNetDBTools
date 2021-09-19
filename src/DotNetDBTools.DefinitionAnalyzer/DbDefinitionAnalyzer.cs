using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Analysis.Shared;
using DotNetDBTools.Analysis.Shared.Errors;
using DotNetDBTools.DefinitionParser;
using DotNetDBTools.Models.Shared;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Emit;

namespace DotNetDBTools.DefinitionAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DbDefinitionAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor s_diagnosticDescriptor = new(
                id: "DNDBT_DA_01",
                title: "Database should be valid",
                messageFormat: "Database is invalid: {0}",
                category: "DbAnalysis",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(s_diagnosticDescriptor);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationAction(AnalyzeCompilation);
        }

        private static void AnalyzeCompilation(CompilationAnalysisContext context)
        {
            try
            {
                Assembly dbAssembly = CompileInMemoryAnLoad(context.Compilation);
                IDatabaseInfo<ITableInfo<IColumnInfo>> databaseInfo = DbDefinitionParser.CreateDatabaseInfo(dbAssembly);
                if (!DbValidator.HasNoBadTables(databaseInfo, out IDbError dbError))
                {
                    Location location = InvalidDbObjectsFinder.GetInvalidDbObjectLocation(context.Compilation, dbError);
                    context.ReportDiagnostic(Diagnostic.Create(s_diagnosticDescriptor, location, dbError.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                context.ReportDiagnostic(Diagnostic.Create(s_diagnosticDescriptor, Location.None, ex.ToString()));
            }
        }

        private static Assembly CompileInMemoryAnLoad(Compilation compilation, IEnumerable<ResourceDescription> resources = null)
        {
            using MemoryStream assemblyStream = new();
            EmitResult result = compilation.Emit(assemblyStream, manifestResources: resources);
            if (!result.Success)
                throw new Exception(GetCompilationErrorString(result));
            byte[] assemblyBytes = assemblyStream.ToArray();
            Assembly assembly = Assembly.Load(assemblyBytes);
            return assembly;
        }

        private static string GetCompilationErrorString(EmitResult result)
        {
            DiagnosticFormatter formatter = new();
            List<string> errors = new(result.Diagnostics.Select(d => formatter.Format(d)));
            return string.Join("\n", errors);
        }
    }
}
