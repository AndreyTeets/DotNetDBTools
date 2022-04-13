using System.Data.Common;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.IntegrationTests.Base;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;
using FluentAssertions.Equivalency;
using Microsoft.Data.Sqlite;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.SQLite;

public class SQLiteDeployTests : BaseDeployTests<
    SQLiteDatabase,
    SqliteConnection,
    SQLiteDbModelConverter,
    SQLiteDeployManager>
{
    protected override string SpecificDbmsSampleDbV1AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.SQLite.dll";
    protected override string SpecificDbmsSampleDbV2AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDBv2.SQLite.dll";
    protected override BaseDataTester DataTester { get; set; } = new SQLiteDataTester();

    private const string DbFilesFolder = @"./tmp";

    protected override EquivalencyAssertionOptions<SQLiteDatabase> AddAdditionalDbModelEquivalenceyOptions(
        EquivalencyAssertionOptions<SQLiteDatabase> options, CompareMode compareMode)
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

    private protected override IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(DbConnection connection)
    {
        return new SQLiteDbModelFromDBMSProvider(new SQLiteQueryExecutor(connection, new Events()));
    }
}
