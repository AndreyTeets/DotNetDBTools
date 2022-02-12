using System;
using System.Linq;
using DotNetDBTools.Analysis.Agnostic;
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

    public static bool LeadsToDataLoss(DatabaseDiff databaseDiff)
    {
        if (databaseDiff.RemovedTables.Any())
            return true;
        foreach (TableDiff tableDiff in databaseDiff.ChangedTables)
        {
            if (tableDiff.RemovedColumns.Any())
                return true;
        }
        return false;
    }
}
