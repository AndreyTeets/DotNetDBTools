using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DotNetDBTools.DefinitionParser;
using DotNetDBTools.Description;
using DotNetDBTools.Models.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;

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
                Assembly dbAssembly = CompileInMemoryAnLoad(context.Compilation);
                DatabaseInfo databaseInfo = DbDefinitionParser.CreateDatabaseInfo(dbAssembly);
                string dbDescriptionCode = DbDescriptionGenerator.GenerateDescription(databaseInfo);
                context.AddSource($"{databaseInfo.Name}Description", dbDescriptionCode);
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
