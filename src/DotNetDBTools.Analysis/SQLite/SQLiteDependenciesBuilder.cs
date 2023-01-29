using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite;

internal class SQLiteDependenciesBuilder : DependenciesBuilder
{
    public override void BuildDependencies(Database database)
    {
        Build_Parent_Property_ForAllObjects(database);
        Build_DependsOn_Property_ForAllObjects(database);
    }

    private void Build_Parent_Property_ForAllObjects(Database database)
    {
        Build_Parent_Property_ForTableChildObjects(database);
    }

    private void Build_DependsOn_Property_ForAllObjects(Database database)
    {
        Dictionary<string, Table> tableNameToTableMap = database.Tables.ToDictionary(x => x.Name, x => x);

        SQLiteCodeParser parser = new();
        foreach (SQLiteView view in database.Views)
        {
            List<Dependency> deps = ExecuteParsingFunc(
                () => parser.GetViewDependencies(view.CreateStatement.Code),
                $"Error while parsing view '{view.Name}' code");

            AddDependencies(view.CreateStatement.DependsOn, deps);
        }

        void AddDependencies(List<DbObject> destDeps, IEnumerable<Dependency> sourceDeps)
        {
            foreach (Dependency dep in sourceDeps)
            {
                if (dep.Type == DependencyType.TableOrView)
                {
                    if (tableNameToTableMap.ContainsKey(dep.Name))
                        destDeps.Add(tableNameToTableMap[dep.Name]);
                    // Otherwise it may be a view or system table
                }
                else
                {
                    throw new Exception($"Invalid dependency type '{dep.Type}'");
                }
            }
        }
    }

    private TResult ExecuteParsingFunc<TResult>(Func<TResult> func, string onParseErrorMessageHeader)
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
