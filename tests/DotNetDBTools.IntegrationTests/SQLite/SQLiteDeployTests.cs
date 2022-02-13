﻿using System.Data.Common;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.IntegrationTests.Base;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;
using FluentAssertions.Equivalency;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.SQLite;

[TestClass]
public class SQLiteDeployTests : BaseDeployTests<
    SQLiteDatabase,
    SqliteConnection,
    SQLiteDbModelConverter,
    SQLiteDeployManager>
{
    protected override string AgnosticSampleDbAssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
    protected override string SpecificDBMSSampleDbAssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.SQLite.dll";

    private const string DbFilesFolder = @"./tmp";

    protected override EquivalencyAssertionOptions<SQLiteDatabase> AddAdditionalDbModelEquivalenceyOptions(
        EquivalencyAssertionOptions<SQLiteDatabase> options)
    {
        return options;
    }

    protected override string GetNormalizedCodeFromCodePiece(CodePiece codePiece)
    {
        return codePiece.Code.ToUpper()
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace(" ", "")
            .Replace(";", "");
    }

    protected override void CreateDatabase(string testName)
    {
        string databaseName = testName;
        SQLiteDatabaseHelper.CreateDatabase(DbFilesFolder, databaseName);
    }

    protected override void DropDatabaseIfExists(string testName)
    {
        string databaseName = testName;
        SQLiteDatabaseHelper.DropDatabaseIfExists(DbFilesFolder, databaseName);
    }

    protected override string CreateConnectionString(string testName)
    {
        string databaseName = testName;
        return SQLiteDatabaseHelper.CreateConnectionString(DbFilesFolder, databaseName);
    }

    private protected override IDbModelFromDbSysInfoBuilder CreateDbModelFromDbSysInfoBuilder(DbConnection connection)
    {
        return new SQLiteDbModelFromDbSysInfoBuilder(new SQLiteQueryExecutor(connection));
    }
}
