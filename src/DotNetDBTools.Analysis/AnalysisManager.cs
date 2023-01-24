using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Agnostic;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Errors;
using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.Analysis.MySQL;
using DotNetDBTools.Analysis.PostgreSQL;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis;

public class AnalysisManager : IAnalysisManager
{
    /// <inheritdoc />
    public bool DbIsValid(Database database, out List<DbError> dbErrors)
    {
        return database.Kind switch
        {
            DatabaseKind.Agnostic => new AgnosticDbValidator().DbIsValid(database, out dbErrors),
            DatabaseKind.MSSQL => new MSSQLDbValidator().DbIsValid(database, out dbErrors),
            DatabaseKind.MySQL => new MySQLDbValidator().DbIsValid(database, out dbErrors),
            DatabaseKind.PostgreSQL => new PostgreSQLDbValidator().DbIsValid(database, out dbErrors),
            DatabaseKind.SQLite => new SQLiteDbValidator().DbIsValid(database, out dbErrors),
            _ => throw new InvalidOperationException($"Invalid {nameof(DbIsValid)} dbKind: {database.Kind}"),
        };
    }
    /// <inheritdoc />
    public bool DatabasesAreEquivalentExcludingDNDBTInfo(Database database1, Database database2, out string diffLog)
    {
        DNDBTModelsEqualityComparer comparer = new();
        comparer.IgnoredProperties.Add(new PropInfo { Name = nameof(Database.Version), InType = nameof(Database) });
        comparer.IgnoredProperties.Add(new PropInfo { Name = nameof(Database.Scripts), InType = nameof(Database) });
        comparer.IgnoredProperties.Add(new PropInfo { Name = nameof(DbObject.ID), InType = nameof(DbObject) });
        comparer.IgnoredProperties.Add(new PropInfo { Name = nameof(CodePiece.Code), InType = nameof(CodePiece) });

        bool res = comparer.Equals(database1, database2);
        diffLog = comparer.DiffLog;
        return res;
    }
    /// <inheritdoc />
    public bool DbObjectsAreEquivalentExcludingDNDBTInfo(DbObject dbObject1, DbObject dbObject2, out string diffLog)
    {
        DNDBTModelsEqualityComparer comparer = new();
        comparer.IgnoredProperties.Add(new PropInfo { Name = nameof(DbObject.ID), InType = nameof(DbObject) });
        comparer.IgnoredProperties.Add(new PropInfo { Name = nameof(CodePiece.Code), InType = nameof(CodePiece) });

        bool res = comparer.Equals(dbObject1, dbObject2);
        diffLog = comparer.DiffLog;
        return res;
    }

    /// <inheritdoc />
    public DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase)
    {
        return newDatabase.Kind switch
        {
            DatabaseKind.MSSQL => new MSSQLDiffCreator().CreateDatabaseDiff(newDatabase, oldDatabase),
            DatabaseKind.MySQL => new MySQLDiffCreator().CreateDatabaseDiff(newDatabase, oldDatabase),
            DatabaseKind.PostgreSQL => new PostgreSQLDiffCreator().CreateDatabaseDiff(newDatabase, oldDatabase),
            DatabaseKind.SQLite => new SQLiteDiffCreator().CreateDatabaseDiff(newDatabase, oldDatabase),
            _ => throw new InvalidOperationException($"Invalid {nameof(CreateDatabaseDiff)} dbKind: {newDatabase.Kind}"),
        };
    }
    /// <inheritdoc />
    public bool DiffLeadsToDataLoss(DatabaseDiff dbDiff)
    {
        if (dbDiff.RemovedTables.Any())
            return true;
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
        {
            if (tableDiff.ColumnsToDrop.Any())
                return true;
        }
        return false;
    }
    /// <inheritdoc />
    public bool DiffIsEmpty(DatabaseDiff dbDiff)
    {
        return DiffAnalyzer.IsEmpty(dbDiff);
    }
    /// <inheritdoc />
    public bool DiffIsEmpty(DbObjectDiff dbObjectDiff)
    {
        return DiffAnalyzer.IsEmpty(dbObjectDiff);
    }

    /// <inheritdoc />
    public Database ConvertFromAgnostic(Database database, DatabaseKind targetKind)
    {
        return targetKind switch
        {
            DatabaseKind.MSSQL => new MSSQLDbModelConverter().FromAgnostic(database),
            DatabaseKind.MySQL => new MySQLDbModelConverter().FromAgnostic(database),
            DatabaseKind.PostgreSQL => new PostgreSQLDbModelConverter().FromAgnostic(database),
            DatabaseKind.SQLite => new SQLiteDbModelConverter().FromAgnostic(database),
            _ => throw new InvalidOperationException($"Invalid {nameof(ConvertFromAgnostic)} dbKind: {targetKind}"),
        };
    }
    /// <inheritdoc />
    public DataType ConvertDataType(CSharpDataType dataType, DatabaseKind targetKind)
    {
        return targetKind switch
        {
            DatabaseKind.MSSQL => new MSSQLDataTypeConverter().Convert(dataType),
            DatabaseKind.MySQL => new MySQLDataTypeConverter().Convert(dataType),
            DatabaseKind.PostgreSQL => new PostgreSQLDataTypeConverter().Convert(dataType),
            DatabaseKind.SQLite => new SQLiteDataTypeConverter().Convert(dataType),
            _ => throw new InvalidOperationException($"Invalid {nameof(ConvertDataType)} dbKind: {targetKind}"),
        };
    }
    /// <inheritdoc />
    public CodePiece ConvertDefaultValue(CSharpDefaultValue defaultValue, DatabaseKind targetKind)
    {
        return targetKind switch
        {
            DatabaseKind.MSSQL => new MSSQLDefaultValueConverter().Convert(defaultValue),
            DatabaseKind.MySQL => new MySQLDefaultValueConverter().Convert(defaultValue),
            DatabaseKind.PostgreSQL => new PostgreSQLDefaultValueConverter().Convert(defaultValue),
            DatabaseKind.SQLite => new SQLiteDefaultValueConverter().Convert(defaultValue),
            _ => throw new InvalidOperationException($"Invalid {nameof(ConvertDefaultValue)} dbKind: {targetKind}"),
        };
    }

    /// <inheritdoc />
    public void DoCreateSpecificDbmsDbModelFromDefinitionPostProcessing(Database database)
    {
        IDbModelPostProcessor dbModelPostProcessor = database.Kind switch
        {
            DatabaseKind.MSSQL => new MSSQLDbModelPostProcessor(),
            DatabaseKind.MySQL => new MySQLDbModelPostProcessor(),
            DatabaseKind.PostgreSQL => new PostgreSQLDbModelPostProcessor(),
            DatabaseKind.SQLite => new SQLiteDbModelPostProcessor(),
            _ => throw new InvalidOperationException($"Invalid {nameof(DoCreateSpecificDbmsDbModelFromDefinitionPostProcessing)} dbKind: {database.Kind}"),
        };
        dbModelPostProcessor.DoSpecificDbmsDbModelCreationFromDefinitionPostProcessing(database);
    }
    /// <inheritdoc />
    public void DoPostProcessing(Database database)
    {
        IDbModelPostProcessor dbModelPostProcessor = database.Kind switch
        {
            DatabaseKind.Agnostic => new AgnosticDbModelPostProcessor(),
            DatabaseKind.MSSQL => new MSSQLDbModelPostProcessor(),
            DatabaseKind.MySQL => new MySQLDbModelPostProcessor(),
            DatabaseKind.PostgreSQL => new PostgreSQLDbModelPostProcessor(),
            DatabaseKind.SQLite => new SQLiteDbModelPostProcessor(),
            _ => throw new InvalidOperationException($"Invalid {nameof(DoPostProcessing)} dbKind: {database.Kind}"),
        };
        dbModelPostProcessor.DoPostProcessing(database);
    }
    /// <inheritdoc />
    public void BuildDependencies(Database database)
    {
        IDependenciesBuilder dependenciesBuilder = database.Kind switch
        {
            DatabaseKind.MSSQL => new MSSQLDependenciesBuilder(),
            DatabaseKind.MySQL => new MySQLDependenciesBuilder(),
            DatabaseKind.PostgreSQL => new PostgreSQLDependenciesBuilder(),
            DatabaseKind.SQLite => new SQLiteDependenciesBuilder(),
            _ => throw new InvalidOperationException($"Invalid {nameof(BuildDependencies)} dbKind: {database.Kind}"),
        };
        dependenciesBuilder.BuildDependencies(database);
    }

    public static string GetStandardSqlTypeNameBase(string typeName, DatabaseKind dbmsKind)
    {
        return dbmsKind switch
        {
            DatabaseKind.MSSQL => throw new NotImplementedException(),
            DatabaseKind.MySQL => throw new NotImplementedException(),
            DatabaseKind.PostgreSQL => GetForPostgreSQL(typeName),
            DatabaseKind.SQLite => throw new NotImplementedException(),
            _ => throw new InvalidOperationException($"Invalid {nameof(GetStandardSqlTypeNameBase)} dbmsKind: {dbmsKind}"),
        };

        static string GetForPostgreSQL(string typeName)
        {
            PostgreSQLHelperMethods.GetNormalizedTypeNameWithoutArray(
                typeName, out string standardSqlTypeNameBase, out string _);
            return standardSqlTypeNameBase;
        }
    }
}
