using System;
using System.Collections.Generic;
using System.Reflection;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;
using DotNetDBTools.RoslynUtilities;
using Microsoft.CodeAnalysis;

namespace DotNetDBTools.DescriptionSourceGenerator
{
    [Generator]
    internal class DbDescriptionSourceGenerator : ISourceGenerator
    {
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

                Assembly dbAssembly = CompilationHelper.CompileInMemoryAnLoad(context.Compilation, resources);
                Database database = DbDefinitionParser.CreateDatabaseModel(dbAssembly);
                string dbDescriptionCode = DbDescriptionGenerator.GenerateDescription(database);
                context.AddSource($"{database.Name}Description", dbDescriptionCode);
            }
            catch (Exception ex)
            {
                DiagnosticDescriptor diagnosticDescriptor = new(
                    id: "DNDBT_DSG_01",
                    title: "Failed to create database description",
                    messageFormat: $"Failed to create database description: {ex}",
                    category: "SourceGeneration",
                    defaultSeverity: DiagnosticSeverity.Error,
                    isEnabledByDefault: true);
                context.ReportDiagnostic(Diagnostic.Create(diagnosticDescriptor, Location.None));
            }
        }
    }
}
