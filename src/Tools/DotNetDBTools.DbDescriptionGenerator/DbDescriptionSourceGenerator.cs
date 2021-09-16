using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.DefinitionParser.MSSQL;
using DotNetDBTools.DefinitionParser.SQLite;
using DotNetDBTools.Description.Agnostic;
using DotNetDBTools.Description.MSSQL;
using DotNetDBTools.Description.SQLite;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.SQLite;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;

namespace DotNetDBTools.DbDescriptionGenerator
{
    [Generator]
    public class DbDescriptionSourceGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            //if (!System.Diagnostics.Debugger.IsAttached)
            //{
            //    System.Diagnostics.Debugger.Launch();
            //}
#endif
        }

        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                Assembly dbAssembly = CompileInMemoryAnLoad(context.Compilation);
                string dbDescriptionCode;
                if (IsAgnosticDb(dbAssembly))
                {
                    AgnosticDatabaseInfo databaseInfo = AgnosticDefinitionParser.CreateDatabaseInfo(dbAssembly);
                    dbDescriptionCode = AgnosticDbDescriptionGenerator.GenerateDescription(databaseInfo);
                }
                else if (IsMSSQLDb(dbAssembly))
                {
                    MSSQLDatabaseInfo databaseInfo = MSSQLDefinitionParser.CreateDatabaseInfo(dbAssembly);
                    dbDescriptionCode = MSSQLDbDescriptionGenerator.GenerateDescription(databaseInfo);
                }
                else if (IsSQLiteDb(dbAssembly))
                {
                    SQLiteDatabaseInfo databaseInfo = SQLiteDefinitionParser.CreateDatabaseInfo(dbAssembly);
                    dbDescriptionCode = SQLiteDbDescriptionGenerator.GenerateDescription(databaseInfo);
                }
                else
                {
                    throw new InvalidOperationException("Invalid dbAssembly for description generation");
                }
                context.AddSource("DbDescription", dbDescriptionCode);
            }
            catch (Exception ex)
            {
                DiagnosticDescriptor generationError = new(
                    "GN0001",
                    "Failed to create database descriptions",
                    $"{ex}",
                    nameof(DbDescriptionSourceGenerator),
                    DiagnosticSeverity.Error,
                    true);
                context.ReportDiagnostic(Diagnostic.Create(generationError, Location.None));
            }
        }

        public static bool IsAgnosticDb(Assembly dbAssembly)
        {
            return dbAssembly
               .GetTypes()
                .Any(x => x.GetInterfaces()
                    .Any(y => y == typeof(Definition.Agnostic.ITable)));
        }

        public static bool IsMSSQLDb(Assembly dbAssembly)
        {
            return dbAssembly
               .GetTypes()
                .Any(x => x.GetInterfaces()
                    .Any(y => y == typeof(Definition.MSSQL.ITable)));
        }

        public static bool IsSQLiteDb(Assembly dbAssembly)
        {
            return dbAssembly
                .GetTypes()
                .Any(x => x.GetInterfaces()
                    .Any(y => y == typeof(Definition.SQLite.ITable)));
        }

        private static Assembly CompileInMemoryAnLoad(Compilation compilation, IEnumerable<ResourceDescription> resources = null)
        {
            using MemoryStream pdbStream = new();
            using MemoryStream assemblyStream = new();
            EmitResult result = compilation.Emit(assemblyStream, pdbStream: pdbStream, manifestResources: resources);
            if (!result.Success)
                throw new Exception(GetCompilationErrorString(result));
            byte[] assemblyBytes = assemblyStream.ToArray();
            byte[] pdbBytes = pdbStream.ToArray();
            Assembly assembly = Assembly.Load(assemblyBytes, pdbBytes);
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
