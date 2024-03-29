﻿using System;
using System.Collections.Generic;
using System.Reflection;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;
using DotNetDBTools.RoslynUtilities;
using Microsoft.CodeAnalysis;

namespace DotNetDBTools.DescriptionSourceGenerator;

[Generator]
internal class DbDescriptionSourceGenerator : ISourceGenerator
{
    private static readonly DiagnosticDescriptor s_diagnosticDescriptor = new(
        id: "DNDBT_DSG_01",
        title: "Failed to create database description",
        messageFormat: "Failed to create database description: {0}",
        category: "SourceGeneration",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        try
        {
            IEnumerable<ResourceDescription> resources = CompilationHelper.GetEmbeddedResourcesIfAny(
                context.AnalyzerConfigOptions.GlobalOptions,
                context.AnalyzerConfigOptions,
                context.AdditionalFiles);

            string databaseName = context.Compilation.AssemblyName.Replace(".", "");
            Assembly dbAssembly = CompilationHelper.CompileInMemoryAnLoad(context.Compilation, resources);
            Database database = new DefinitionParsingManager().CreateDbModel(dbAssembly);
            GenerationOptions options = new() { DatabaseName = databaseName };
            string dbDescriptionCode = new GenerationManager(options).GenerateDescription(database);
            context.AddSource($"{databaseName}Description", dbDescriptionCode);
        }
        catch (Exception ex)
        {
            context.ReportDiagnostic(Diagnostic.Create(s_diagnosticDescriptor, Location.None, ex.ToString()));
        }
    }
}
