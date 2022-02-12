using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Models.Core;
using DotNetDBTools.RoslynUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DotNetDBTools.DefinitionAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class DbDefinitionAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor s_diagnosticDescriptor = new(
        id: "DNDBT_DA_01",
        title: "Database is invalid",
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
            IEnumerable<ResourceDescription> resources = CompilationHelper.GetEmbeddedResourcesIfAny(
                context.Options.AnalyzerConfigOptionsProvider.GlobalOptions,
                context.Options.AnalyzerConfigOptionsProvider,
                context.Options.AdditionalFiles);

            Assembly dbAssembly = CompilationHelper.CompileInMemoryAnLoad(context.Compilation, resources);
            Database database = DbDefinitionParser.CreateDatabaseModel(dbAssembly);
            if (!AnalysisHelper.DbIsValid(database, out DbError dbError))
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
}
