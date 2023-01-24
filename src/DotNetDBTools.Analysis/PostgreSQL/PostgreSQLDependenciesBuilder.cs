using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using DotNetDBTools.Models.PostgreSQL.UserDefinedTypes;

namespace DotNetDBTools.Analysis.PostgreSQL;

internal class PostgreSQLDependenciesBuilder : DependenciesBuilder
{
    public override void BuildDependencies(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        Build_Parent_Property_ForAllObjects(db);
        Build_DependsOn_Property_ForAllObjects(db);
    }

    private void Build_Parent_Property_ForAllObjects(PostgreSQLDatabase database)
    {
        foreach (PostgreSQLDomainType type in database.DomainTypes)
        {
            foreach (CheckConstraint ck in type.CheckConstraints)
                ck.Parent = type;
        }

        Build_Parent_Property_ForTableChildObjects(database);
    }

    private void Build_DependsOn_Property_ForAllObjects(PostgreSQLDatabase database)
    {
        IEnumerable<DbObject> userDefinedTypes =
           database.CompositeTypes.Select(x => (DbObject)x)
           .Concat(database.DomainTypes.Select(x => (DbObject)x))
           .Concat(database.EnumTypes.Select(x => (DbObject)x))
           .Concat(database.RangeTypes.Select(x => (DbObject)x));

        Dictionary<string, DbObject> udtNameToUdtMap = new();
        foreach (DbObject udt in userDefinedTypes)
            udtNameToUdtMap.Add($@"""{udt.Name}""", udt);

        Dictionary<string, Table> tableNameToTableMap = database.Tables.ToDictionary(x => x.Name, x => x);
        Dictionary<string, View> viewNameToViewMap = database.Views.ToDictionary(x => x.Name, x => x);
        Dictionary<string, PostgreSQLFunction> funcNameToFuncMap = database.Functions.ToDictionary(x => x.Name, x => x);
        Dictionary<string, PostgreSQLSequence> sequenceNameToFuncMap = database.Sequences.ToDictionary(x => x.Name, x => x);

        PostgreSQLCodeParser parser = new();
        AddForTypes();
        AddForTableObjects();
        AddForProgrammableObjects();

        void AddForTypes()
        {
            foreach (PostgreSQLCompositeType type in database.CompositeTypes)
            {
                foreach (PostgreSQLCompositeTypeAttribute attr in type.Attributes)
                    AddDependencyIfTypeIsUdt(attr.DataType.DependsOn, attr.DataType.Name);
            }
            foreach (PostgreSQLDomainType type in database.DomainTypes)
            {
                AddDependencyIfTypeIsUdt(type.UnderlyingType.DependsOn, type.UnderlyingType.Name);

                if (type.Default is not null)
                {
                    List<Dependency> deps = ExecuteParsingFunc(
                        () => parser.GetExpressionDependencies(type.Default.Code),
                        $"Error while parsing domain type '{type.Name}' default expression");

                    AddDependencies(type.Default.DependsOn, deps);
                }

                foreach (CheckConstraint ck in type.CheckConstraints)
                {
                    List<Dependency> deps = ExecuteParsingFunc(
                        () => parser.GetExpressionDependencies(ck.Expression.Code),
                        $"Error while parsing domain type '{type.Name}' check constraint '{ck.Name}' expression");

                    AddDependencies(ck.Expression.DependsOn, deps);
                }
            }
            foreach (PostgreSQLRangeType type in database.RangeTypes)
            {
                AddDependencyIfTypeIsUdt(type.Subtype.DependsOn, type.Subtype.Name);
                // TODO + OperatorFuncsDependsOn
            }
        }

        void AddForTableObjects()
        {
            foreach (PostgreSQLTable table in database.Tables)
            {
                foreach (PostgreSQLColumn column in table.Columns)
                {
                    AddDependencyIfTypeIsUdt(column.DataType.DependsOn, column.DataType.Name);

                    if (column.Default is not null)
                    {
                        List<Dependency> deps = ExecuteParsingFunc(
                        () => parser.GetExpressionDependencies(column.Default.Code),
                        $"Error while parsing table '{table.Name}' column '{table.Name}' default expression");

                        AddDependencies(column.Default.DependsOn, deps);
                    }
                }
                foreach (CheckConstraint ck in table.CheckConstraints)
                {
                    List<Dependency> deps = ExecuteParsingFunc(
                        () => parser.GetExpressionDependencies(ck.Expression.Code),
                        $"Error while parsing table '{table.Name}' check constraint '{ck.Name}' expression");

                    AddDependencies(ck.Expression.DependsOn, deps);
                }
                foreach (PostgreSQLIndex index in table.Indexes)
                {
                    if (index.Expression is not null)
                    {
                        List<Dependency> deps = ExecuteParsingFunc(
                        () => parser.GetExpressionDependencies(index.Expression.Code),
                        $"Error while parsing index '{index.Name}' expression");

                        AddDependencies(index.Expression.DependsOn, deps);
                    }
                }
                foreach (PostgreSQLTrigger trigger in table.Triggers)
                {
                    List<Dependency> deps = ExecuteParsingFunc(
                        () => parser.GetTriggerDependencies(trigger.CreateStatement.Code),
                        $"Error while parsing trigger '{trigger.Name}' expression");

                    AddDependencies(trigger.CreateStatement.DependsOn, deps);
                }
            }
        }

        void AddForProgrammableObjects()
        {
            foreach (PostgreSQLView view in database.Views)
            {
                List<Dependency> deps = ExecuteParsingFunc(
                    () => parser.GetViewDependencies(view.CreateStatement.Code),
                    $"Error while parsing view '{view.Name}' code");

                AddDependencies(view.CreateStatement.DependsOn, deps);
            }
            foreach (PostgreSQLFunction func in database.Functions)
            {
                string language = null;
                List<Dependency> deps = ExecuteParsingFunc(
                    () => parser.GetFunctionDependencies(func.CreateStatement.Code, out language),
                    $"Error while parsing function '{func.Name}' code");

                if (language == "SQL")
                    AddDependencies(func.CreateStatement.DependsOn, deps);
            }
            foreach (PostgreSQLProcedure proc in database.Procedures)
            {
                string language = null;
                List<Dependency> deps = ExecuteParsingFunc(
                    () => parser.GetProcedureDependencies(proc.CreateStatement.Code, out language),
                    $"Error while parsing procedure '{proc.Name}' code");

                if (language == "SQL")
                    AddDependencies(proc.CreateStatement.DependsOn, deps);
            }
        }

        void AddDependencyIfTypeIsUdt(List<DbObject> destDeps, string typeName)
        {
            string typeNameWithoutArray = PostgreSQLHelperMethods.GetNormalizedTypeNameWithoutArray(
                typeName, out string _, out string _);
            if (udtNameToUdtMap.ContainsKey(typeNameWithoutArray))
                destDeps.Add(udtNameToUdtMap[typeNameWithoutArray]);
        }

        void AddDependencies(List<DbObject> destDeps, IEnumerable<Dependency> sourceDeps)
        {
            foreach (Dependency dep in sourceDeps)
            {
                if (dep.Type == DependencyType.Sequence)
                {
                    if (sequenceNameToFuncMap.ContainsKey(dep.Name))
                        destDeps.Add(sequenceNameToFuncMap[dep.Name]);
                    // Otherwise it may be a system sequence
                }
                else if (dep.Type == DependencyType.DataType)
                {
                    string quotedDataTypeName = $@"""{dep.Name}""";
                    if (udtNameToUdtMap.ContainsKey(quotedDataTypeName))
                        destDeps.Add(udtNameToUdtMap[quotedDataTypeName]);
                    // Otherwise it may be a system data type
                }
                else if (dep.Type == DependencyType.TableOrView)
                {
                    if (tableNameToTableMap.ContainsKey(dep.Name))
                        destDeps.Add(tableNameToTableMap[dep.Name]);
                    else if (viewNameToViewMap.ContainsKey(dep.Name))
                        destDeps.Add(viewNameToViewMap[dep.Name]);
                    // Otherwise it may be a system table or view
                }
                else if (dep.Type == DependencyType.Function)
                {
                    if (funcNameToFuncMap.ContainsKey(dep.Name))
                        destDeps.Add(funcNameToFuncMap[dep.Name]);
                    // Otherwise it may be a system function
                }
                else
                {
                    throw new Exception($"Invalid dependency type {dep.Type}");
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
