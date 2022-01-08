using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.PostgreSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Analysis.PostgreSQL
{
    public class PostgreSQLDependenciesBuilder
    {
        public static void BuildDependencies(PostgreSQLDatabase database)
        {
            Dictionary<string, Table> tableNameToTableMap = database.Tables.ToDictionary(x => x.Name, x => x);
            Dictionary<string, View> viewNameToViewMap = database.Views.ToDictionary(x => x.Name, x => x);
            Dictionary<string, PostgreSQLFunction> funcNameToFuncMap = database.Functions.ToDictionary(x => x.Name, x => x);

            PostgreSQLCodeParser parser = new();
            foreach (PostgreSQLView view in database.Views)
            {
                view.Dependencies = new List<DBObject>();
                List<Dependency> dependencies = ExecuteParsingFunc(
                    () => parser.GetViewDependencies(view.CodePiece.Code),
                    $"Error while parsing view '{view.Name}' code");

                foreach (Dependency dep in dependencies)
                    AddDependency(view.Dependencies, dep, $"view '{view.Name}'");
            }
            foreach (PostgreSQLFunction func in database.Functions)
            {
                func.Dependencies = new List<DBObject>();
                List<Dependency> dependencies = ExecuteParsingFunc(
                    () => parser.GetFunctionDependencies(func.CodePiece.Code),
                    $"Error while parsing function '{func.Name}' code");

                foreach (Dependency dep in dependencies)
                    AddDependency(func.Dependencies, dep, $"function '{func.Name}'");
            }

            void AddDependency(List<DBObject> dependencies, Dependency dep, string referencingObjectName)
            {
                if (dep.Type == ObjectType.Table)
                {
                    if (tableNameToTableMap.ContainsKey(dep.Name))
                        dependencies.Add(tableNameToTableMap[dep.Name]);
                    else
                        throw new Exception($"Failed to find table '{dep.Name}' referenced by {referencingObjectName}");
                }
                else if (dep.Type == ObjectType.TableOrView)
                {
                    if (tableNameToTableMap.ContainsKey(dep.Name))
                        dependencies.Add(tableNameToTableMap[dep.Name]);
                    else if (viewNameToViewMap.ContainsKey(dep.Name))
                        dependencies.Add(viewNameToViewMap[dep.Name]);
                    else
                        throw new Exception($"Failed to find table or view '{dep.Name}' referenced by {referencingObjectName}");
                }
                else if (dep.Type == ObjectType.Function)
                {
                    if (funcNameToFuncMap.ContainsKey(dep.Name))
                        dependencies.Add(funcNameToFuncMap[dep.Name]);
                    else
                        throw new Exception($"Failed to find function '{dep.Name}' referenced by {referencingObjectName}");
                }
            }
        }

        private static TResult ExecuteParsingFunc<TResult>(Func<TResult> func, string onParseErrorMessageHeader)
        {
            try
            {
                return func();
            }
            catch (ParseException ex)
            {
                throw new Exception($"{onParseErrorMessageHeader}: {ex.Message}");
            }
        }
    }
}
