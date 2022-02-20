using System;
using System.Linq;
using DotNetDBTools.Analysis.Agnostic;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.Analysis.MySQL;
using DotNetDBTools.Analysis.PostgreSQL;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis;

public static class AnalysisHelper
{
    public static bool DbIsValid(Database database, out DbError dbError)
    {
        return database.Kind switch
        {
            DatabaseKind.Agnostic => new AgnosticDbValidator().DbIsValid(database, out dbError),
            DatabaseKind.MSSQL => new MSSQLDbValidator().DbIsValid(database, out dbError),
            DatabaseKind.MySQL => new MySQLDbValidator().DbIsValid(database, out dbError),
            DatabaseKind.PostgreSQL => new PostgreSQLDbValidator().DbIsValid(database, out dbError),
            DatabaseKind.SQLite => new SQLiteDbValidator().DbIsValid(database, out dbError),
            _ => throw new InvalidOperationException($"Invalid dbKind: {database.Kind}"),
        };
    }

    public static DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase)
    {
        return newDatabase.Kind switch
        {
            DatabaseKind.MSSQL => new MSSQLDiffCreator().CreateDatabaseDiff(newDatabase, oldDatabase),
            DatabaseKind.MySQL => new MySQLDiffCreator().CreateDatabaseDiff(newDatabase, oldDatabase),
            DatabaseKind.PostgreSQL => new PostgreSQLDiffCreator().CreateDatabaseDiff(newDatabase, oldDatabase),
            DatabaseKind.SQLite => new SQLiteDiffCreator().CreateDatabaseDiff(newDatabase, oldDatabase),
            _ => throw new InvalidOperationException($"Invalid dbKind: {newDatabase.Kind}"),
        };
    }

    public static bool LeadsToDataLoss(DatabaseDiff dbDiff)
    {
        if (dbDiff.RemovedTables.Any())
            return true;
        foreach (TableDiff tableDiff in dbDiff.ChangedTables)
        {
            if (tableDiff.RemovedColumns.Any())
                return true;
        }
        return false;
    }

    public static bool DiffIsEmpty(DatabaseDiff dbDiff)
    {
        return DiffAnalyzer.IsEmpty(dbDiff);
    }

    public static bool DatabasesAreEquivalentExcludingDNDBTInfo(Database database1, Database database2, out string diffLog)
    {
        DNDBTModelsEqualityComparer comparer = new();
        comparer.IgnoredProperties.Add(new PropInfo { Name = "Name", DeclaringTypeName = nameof(Database) });
        comparer.IgnoredProperties.Add(new PropInfo { Name = "Version", DeclaringTypeName = nameof(Database) });
        comparer.IgnoredProperties.Add(new PropInfo { Name = "Scripts", DeclaringTypeName = nameof(Database) });
        comparer.IgnoredProperties.Add(new PropInfo { Name = "ID", DeclaringTypeName = nameof(DbObject) });
        comparer.IgnoredProperties.Add(new PropInfo { Name = "Code", DeclaringTypeName = nameof(CodePiece) });

        bool res = comparer.Equals(database1, database2);
        diffLog = comparer.DiffLog;
        return res;
    }
}
