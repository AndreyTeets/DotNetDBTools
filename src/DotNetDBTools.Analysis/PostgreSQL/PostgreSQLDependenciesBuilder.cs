using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.PostgreSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Analysis.PostgreSQL;

public class PostgreSQLDependenciesBuilder
{
    public static void BuildDependencies(PostgreSQLDatabase database)
    {
        Build_DependsOn_Property_ForAllObjects(database);
        Build_IsDependencyOf_Property_ForAllObjects(database);
        SetFunctionsSimplicity(database);
    }

    private static void Build_DependsOn_Property_ForAllObjects(PostgreSQLDatabase database)
    {
        Dictionary<string, Table> tableNameToTableMap = database.Tables.ToDictionary(x => x.Name, x => x);
        Dictionary<string, View> viewNameToViewMap = database.Views.ToDictionary(x => x.Name, x => x);
        Dictionary<string, PostgreSQLFunction> funcNameToFuncMap = database.Functions.ToDictionary(x => x.Name, x => x);

        PostgreSQLCodeParser parser = new();
        foreach (PostgreSQLView view in database.Views)
        {
            view.DependsOn = new List<DbObject>();
            List<Dependency> dependencies = ExecuteParsingFunc(
                () => parser.GetViewDependencies(view.CodePiece.Code),
                $"Error while parsing view '{view.Name}' code");

            foreach (Dependency dep in dependencies)
                AddDependency(view.DependsOn, dep, $"view '{view.Name}'");
        }
        foreach (PostgreSQLFunction func in database.Functions)
        {
            func.DependsOn = new List<DbObject>();
            List<Dependency> dependencies = ExecuteParsingFunc(
                () => parser.GetFunctionDependencies(func.CodePiece.Code),
                $"Error while parsing function '{func.Name}' code");

            foreach (Dependency dep in dependencies)
                AddDependency(func.DependsOn, dep, $"function '{func.Name}'");
        }
        foreach (PostgreSQLProcedure proc in database.Procedures)
        {
            proc.DependsOn = new List<DbObject>();
        }

        foreach (PostgreSQLCompositeType type in database.CompositeTypes)
        {
            type.DependsOn = new List<DbObject>();
        }
        foreach (PostgreSQLDomainType type in database.DomainTypes)
        {
            type.DependsOn = new List<DbObject>();
        }
        foreach (PostgreSQLEnumType type in database.EnumTypes)
        {
            type.DependsOn = new List<DbObject>();
        }
        foreach (PostgreSQLRangeType type in database.RangeTypes)
        {
            type.DependsOn = new List<DbObject>();
        }

        void AddDependency(List<DbObject> dependencies, Dependency dep, string referencingObjectName)
        {
            if (dep.Type == DependencyType.Table)
            {
                if (tableNameToTableMap.ContainsKey(dep.Name))
                    dependencies.Add(tableNameToTableMap[dep.Name]);
                else
                    throw new Exception($"Failed to find table '{dep.Name}' referenced by {referencingObjectName}");
            }
            else if (dep.Type == DependencyType.TableOrView)
            {
                if (tableNameToTableMap.ContainsKey(dep.Name))
                    dependencies.Add(tableNameToTableMap[dep.Name]);
                else if (viewNameToViewMap.ContainsKey(dep.Name))
                    dependencies.Add(viewNameToViewMap[dep.Name]);
                else
                    throw new Exception($"Failed to find table or view '{dep.Name}' referenced by {referencingObjectName}");
            }
            else if (dep.Type == DependencyType.Function)
            {
                if (funcNameToFuncMap.ContainsKey(dep.Name))
                    dependencies.Add(funcNameToFuncMap[dep.Name]);
                else
                    throw new Exception($"Failed to find function '{dep.Name}' referenced by {referencingObjectName}");
            }
            else
            {
                throw new Exception($"Invalid dependency type {dep.Type}");
            }
        }
    }

    private static void Build_IsDependencyOf_Property_ForAllObjects(PostgreSQLDatabase database)
    {
        IEnumerable<DbObject> dbObjectsWithDependencies =
            database.Views.Select(x => (DbObject)x)
            .Concat(database.Functions.Select(x => (DbObject)x))
            .Concat(database.Procedures.Select(x => (DbObject)x))
            .Concat(database.CompositeTypes.Select(x => (DbObject)x))
            .Concat(database.DomainTypes.Select(x => (DbObject)x))
            .Concat(database.EnumTypes.Select(x => (DbObject)x))
            .Concat(database.RangeTypes.Select(x => (DbObject)x));

        Dictionary<Guid, List<DbObject>> isDependencyOfMap = new();
        foreach (DbObject dbObject in dbObjectsWithDependencies)
        {
            foreach (DbObject dep in dbObject.DependsOn)
            {
                if (!isDependencyOfMap.ContainsKey(dep.ID))
                    isDependencyOfMap[dep.ID] = new List<DbObject>();
                isDependencyOfMap[dep.ID].Add(dbObject);
            }
        }

        foreach (DbObject dbObject in dbObjectsWithDependencies)
        {
            if (isDependencyOfMap.ContainsKey(dbObject.ID))
                dbObject.IsDependencyOf = isDependencyOfMap[dbObject.ID];
            else
                dbObject.IsDependencyOf = new List<DbObject>();
        }
    }

    private static void SetFunctionsSimplicity(PostgreSQLDatabase database)
    {
        foreach (PostgreSQLFunction func in database.Functions)
            func.IsSimple = !ObjectDependsOnTablesTransitively(func);
    }

    private static bool ObjectDependsOnTablesTransitively(DbObject dbObject)
    {
        foreach (DbObject dep in dbObject.DependsOn)
        {
            if (ObjectIsTableOrDependsOnTables(dep))
                return true;
            else
                return ObjectDependsOnTablesTransitively(dep);
        }
        return false;
    }

    private static bool ObjectIsTableOrDependsOnTables(DbObject dbObject)
    {
        if (dbObject is Table || dbObject is View || dbObject is PostgreSQLProcedure)
            return true;
        else
            return false;
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
