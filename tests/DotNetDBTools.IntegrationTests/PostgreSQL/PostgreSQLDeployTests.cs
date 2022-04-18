using System.Data;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL;
using DotNetDBTools.IntegrationTests.Base;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using FluentAssertions.Equivalency;
using Npgsql;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.PostgreSQL;

public class PostgreSQLDeployTests : BaseDeployTests<
    PostgreSQLDatabase,
    NpgsqlConnection,
    PostgreSQLDeployManager>
{
    protected override string SpecificDbmsSampleDbV1AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.PostgreSQL.dll";
    protected override string SpecificDbmsSampleDbV2AssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDBv2.PostgreSQL.dll";
    private protected override BaseDataTester DataTester { get; set; } = new PostgreSQLDataTester();

    private static string ConnectionStringWithoutDb => PostgreSQLContainerHelper.PostgreSQLContainerConnectionString;

    public PostgreSQLDeployTests() : base(DatabaseKind.PostgreSQL) { }

    protected override EquivalencyAssertionOptions<PostgreSQLDatabase> AddAdditionalDbModelEquivalenceyOptions(
        EquivalencyAssertionOptions<PostgreSQLDatabase> options, CompareMode compareMode)
    {
        if (compareMode.HasFlag(CompareMode.NormalizeCodePieces))
            options = options.Excluding(database => database.Functions);
        return options;
    }

    protected override string GetNormalizedCodeFromCodePiece(CodePiece codePiece)
    {
        return codePiece.Code.ToUpper()
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace(" ", "")
            .Replace(";", "")
            .Replace("'", "")
            .Replace("::INTEGER", "")
            .Replace("::NUMERIC", "")
            .Replace("::TEXT", "")
            .Replace("::CHARACTERVARYING", "")
            .Replace("::BPCHAR", "")
            .Replace("::BYTEA", "")
            .Replace("::UUID", "")
            .Replace("::DATE", "")
            .Replace("::TIMEWITHOUTTIMEZONE", "")
            .Replace("::TIMESTAMPWITHOUTTIMEZONE", "")
            .Replace("::TIMESTAMPWITHTIMEZONE", "")
            .Replace("PUBLIC.", "");
    }

    protected override void CreateDatabase(string testName)
    {
        string databaseName = testName;
        PostgreSQLDatabaseHelper.CreateDatabase(ConnectionStringWithoutDb, databaseName);
    }

    protected override void DropDatabaseIfExists(string testName)
    {
        string databaseName = testName;
        PostgreSQLDatabaseHelper.DropDatabaseIfExists(ConnectionStringWithoutDb, databaseName);
    }

    protected override string CreateConnectionString(string testName)
    {
        string databaseName = testName;
        return PostgreSQLDatabaseHelper.CreateConnectionString(ConnectionStringWithoutDb, databaseName);
    }

    private protected override IDbModelFromDBMSProvider CreateDbModelFromDBMSProvider(IDbConnection connection)
    {
        return new PostgreSQLDbModelFromDBMSProvider(new PostgreSQLQueryExecutor(connection, new Events()));
    }
}
