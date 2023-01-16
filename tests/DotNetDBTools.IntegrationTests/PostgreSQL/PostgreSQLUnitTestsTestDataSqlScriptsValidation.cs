using System;
using System.Data;
using System.IO;
using Dapper;
using Npgsql;
using NUnit.Framework;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.PostgreSQL;

public class PostgreSQLUnitTestsTestDataSqlScriptsValidation
{
    private static string UnitTestsTestDataDir => $"{RepoRoot}/tests/DotNetDBTools.UnitTests/TestData";
    private static string DeployOrderTestDataDir => $"{UnitTestsTestDataDir}/PostgreSQL/DeployOrder";
    private static string ConnectionStringWithoutDb => PostgreSQLContainerHelper.PostgreSQLContainerConnectionString;
    private static string CurrentTestName => TestContext.CurrentContext.Test.Name;

    [Test]
    public void DeployOrderTests_DatabaseDefinitionScript_IsValid()
    {
        using IDbConnection connection = RecreateDbAndCreateConnection(CurrentTestName);
        connection.Execute(File.ReadAllText($"{DeployOrderTestDataDir}/DatabaseDefinition.sql"));
    }

    [Test]
    public void DeployOrderTests_ExpectedCreateAndUpdateScripts_AreValid()
    {
        string[] updateScriptPaths = Directory.GetFiles(DeployOrderTestDataDir, "ExpectedUpdateScript-*.sql");
        foreach (string updateScriptPath in updateScriptPaths)
        {
            string updateScriptFileName = Path.GetFileName(updateScriptPath);
            TestCase(updateScriptFileName);
        }

        void TestCase(string updateScriptFileName)
        {
            try
            {
                using IDbConnection connection = RecreateDbAndCreateConnection(CurrentTestName);
                connection.Execute(File.ReadAllText($"{DeployOrderTestDataDir}/ExpectedCreateScript.sql"));
                connection.Execute(File.ReadAllText($"{DeployOrderTestDataDir}/{updateScriptFileName}"));
            }
            catch (Exception ex)
            {
                throw new Exception($"Script '{updateScriptFileName}' is invalid.", ex);
            }
        }
    }

    private IDbConnection RecreateDbAndCreateConnection(string databaseName)
    {
        PostgreSQLDatabaseHelper.DropDatabaseIfExists(ConnectionStringWithoutDb, databaseName);
        PostgreSQLDatabaseHelper.CreateDatabase(ConnectionStringWithoutDb, databaseName);

        NpgsqlConnection.ClearAllPools();
        NpgsqlConnection connection = new();
        connection.ConnectionString = PostgreSQLDatabaseHelper.CreateConnectionString(ConnectionStringWithoutDb, databaseName);
        return connection;
    }
}
