using System;
using System.IO;
using System.Linq;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.Description.Agnostic;
using DotNetDBTools.Models.Agnostic;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;

namespace DotNetDBTools.SampleDefinitionGenerator.Agnostic
{
    [Generator]
    public class MyDbDescriptionGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            //#if DEBUG
            //    if (!System.Diagnostics.Debugger.IsAttached)
            //    {
            //        System.Diagnostics.Debugger.Launch();
            //    }
            //#endif
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.AnalyzerConfigOptions.GlobalOptions.TryGetValue(
                "build_property.MSBuildProjectDirectory", out string projectDirectory) == false)
            {
                throw new ArgumentException("MSBuildProjectDirectory");
            }

            string configText = context.AdditionalFiles
                .First(e => e.Path.EndsWith("generatorsettings.json", StringComparison.Ordinal))
                .GetText(context.CancellationToken)
                .ToString();
            JObject config = JObject.Parse(configText);

            string fullDbAssemblyPath = GetDbAssemblyPathFromConfig(projectDirectory, config);
            AgnosticDatabaseInfo database = AgnosticDefinitionParser.CreateDatabaseInfo(fullDbAssemblyPath);
            string dbDescriptionCode = AgnosticDbDescriptionGenerator.GenerateDescription(database);
            context.AddSource("DbDescription", dbDescriptionCode);
        }

        private static string GetDbAssemblyPathFromConfig(string projectDirectory, JObject config)
        {
            string dbAssemblyPathRelativeToProjectDir = GetPathFromConfig(config, "DbAssemblyPathRelativeToProjectDir");
            string dbAssemblyPath = Path.Combine(projectDirectory, dbAssemblyPathRelativeToProjectDir);
            string fullDbAssemblyPath = Path.GetFullPath(dbAssemblyPath);
            return fullDbAssemblyPath;
        }

        private static string GetPathFromConfig(JObject config, string configKey)
        {
            string path = config[configKey].ToString().Replace("/", "\\");
            return path;
        }
    }
}
