using System.Data.Common;
using Dapper;
using DotNetDBTools.Analysis.PostgreSQL;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL;
using DotNetDBTools.IntegrationTests.Base;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using FluentAssertions.Equivalency;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.PostgreSQL;

[TestClass]
public class PostgreSQLDeployTests : BaseDeployTests<
    PostgreSQLDatabase,
    NpgsqlConnection,
    PostgreSQLDbModelConverter,
    PostgreSQLDeployManager>
{
    protected override string AgnosticSampleDbAssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
    protected override string SpecificDBMSSampleDbAssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.PostgreSQL.dll";

    private static string ConnectionStringWithoutDb => PostgreSQLContainerHelper.PostgreSQLContainerConnectionString;

    protected override EquivalencyAssertionOptions<PostgreSQLDatabase> AddAdditionalDbModelEquivalenceyOptions(
        EquivalencyAssertionOptions<PostgreSQLDatabase> options)
    {
        return options.Excluding(database => database.Functions);
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
            .Replace("PUBLIC.", "");
    }

    protected override void CreateDatabase(string connectionString)
    {
        NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
        string databaseName = connectionStringBuilder.Database;

        using NpgsqlConnection connection = new(ConnectionStringWithoutDb);
        connection.Execute(
$@"CREATE DATABASE ""{databaseName}"";");
    }

    protected override void DropDatabaseIfExists(string connectionString)
    {
        NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
        string databaseName = connectionStringBuilder.Database;

        using NpgsqlConnection connection = new(ConnectionStringWithoutDb);
        connection.Execute(
$@"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{databaseName}';
DROP DATABASE IF EXISTS ""{databaseName}"";");
    }

    protected override string CreateConnectionString(string testName)
    {
        string databaseName = testName;
        NpgsqlConnectionStringBuilder connectionStringBuilder = new(ConnectionStringWithoutDb);
        connectionStringBuilder.Database = databaseName;
        string connectionString = connectionStringBuilder.ConnectionString;
        return connectionString;
    }

    private protected override IDbModelFromDbSysInfoBuilder CreateDbModelFromDbSysInfoBuilder(DbConnection connection)
    {
        return new PostgreSQLDbModelFromDbSysInfoBuilder(new PostgreSQLQueryExecutor(connection));
    }
}
