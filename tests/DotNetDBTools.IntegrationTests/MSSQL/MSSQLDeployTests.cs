using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.MSSQL;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.MSSQL;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MSSQL
{
    [TestClass]
    public class MSSQLDeployTests
    {
        private static readonly string s_connectionStringWithoutDb = MSSQLContainerAssemblyFixture.MsSqlContainerConnectionString;

        private static readonly string s_agnosticSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_mssqlSampleDbAssemblyPath = $"{SamplesOutputDir}/DotNetDBTools.SampleDB.MSSQL.dll";

        private string ConnectionString => CreateConnectionString(s_connectionStringWithoutDb, TestContext.TestName);
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Publish_AgnosticSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
        }

        [TestMethod]
        public void AgnosticSampleDB_DbModelFromDNDBTSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(ConnectionString));
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);

            MSSQLDatabase dbModelFromDbAssembly = (MSSQLDatabase)new MSSQLDbModelConverter().FromAgnostic(
                (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(s_agnosticSampleDbAssemblyPath));
            MSSQLDatabase dbModelFromDNDBTSysInfo = (MSSQLDatabase)interactor.GetDatabaseModelFromDNDBTSysInfo();

            dbModelFromDNDBTSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views));
        }

        [TestMethod]
        public void AgnosticSampleDB_DbModelFromDBMSSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(ConnectionString));
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
            deployManager.UnregisterAsDNDBT(ConnectionString);

            MSSQLDatabase dbModelFromDbAssembly = (MSSQLDatabase)new MSSQLDbModelConverter().FromAgnostic(
                (AgnosticDatabase)DbDefinitionParser.CreateDatabaseModel(s_agnosticSampleDbAssemblyPath));
            MSSQLDatabase dbModelFromDBMSSysInfo = (MSSQLDatabase)interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbModelFromDBMSSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views)
                .Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal)));
        }

        [TestMethod]
        public void Publish_MSSQLSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_mssqlSampleDbAssemblyPath, ConnectionString);
            deployManager.PublishDatabase(s_mssqlSampleDbAssemblyPath, ConnectionString);
        }

        [TestMethod]
        public void MSSQLSampleDB_DbModelFromDNDBTSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(ConnectionString));
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_mssqlSampleDbAssemblyPath, ConnectionString);

            MSSQLDatabase dbModelFromDbAssembly = (MSSQLDatabase)DbDefinitionParser.CreateDatabaseModel(s_mssqlSampleDbAssemblyPath);
            MSSQLDatabase dbModelFromDNDBTSysInfo = (MSSQLDatabase)interactor.GetDatabaseModelFromDNDBTSysInfo();

            dbModelFromDNDBTSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Functions)
                .Excluding(database => database.Views)
                .Using(new MSSQLDefaultValueAsFunctionComparer()));
        }

        [TestMethod]
        public void MSSQLSampleDB_DbModelFromDBMSSysInfo_IsEquivalentTo_DbModelFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(ConnectionString));
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_mssqlSampleDbAssemblyPath, ConnectionString);
            deployManager.UnregisterAsDNDBT(ConnectionString);

            MSSQLDatabase dbModelFromDbAssembly = (MSSQLDatabase)DbDefinitionParser.CreateDatabaseModel(s_mssqlSampleDbAssemblyPath);
            MSSQLDatabase dbModelFromDBMSSysInfo = (MSSQLDatabase)interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbModelFromDBMSSysInfo.Should().BeEquivalentTo(dbModelFromDbAssembly, options => options
                .Excluding(database => database.Name)
                .Excluding(database => database.Views)
                .Excluding(database => database.Functions)
                .Excluding(database => database.Path.EndsWith(".ID", StringComparison.Ordinal))
                .Using(new MSSQLDefaultValueAsFunctionComparer()));
        }

        private static void DropDatabaseIfExists(string connectionString)
        {
            SqlConnectionStringBuilder sqlConnectionBuilder = new(connectionString);
            string databaseName = sqlConnectionBuilder.InitialCatalog;
            sqlConnectionBuilder.InitialCatalog = string.Empty;
            string connectionStringWithoutDb = sqlConnectionBuilder.ConnectionString;

            using SqlConnection connection = new(connectionStringWithoutDb);
            connection.Execute(
$@"IF EXISTS (SELECT * FROM [sys].[databases] WHERE [name] = '{databaseName}')
BEGIN
    ALTER DATABASE {databaseName}
    SET OFFLINE WITH ROLLBACK IMMEDIATE;

    ALTER DATABASE {databaseName}
    SET ONLINE;

    DROP DATABASE {databaseName};
END;");
        }

        private static string CreateConnectionString(string connectionStringWithoutDb, string databaseName)
        {
            SqlConnectionStringBuilder sqlConnectionBuilder = new(connectionStringWithoutDb);
            sqlConnectionBuilder.InitialCatalog = databaseName;
            string connectionString = sqlConnectionBuilder.ConnectionString;
            return connectionString;
        }

        private class MSSQLDefaultValueAsFunctionComparer : IEqualityComparer<MSSQLDefaultValueAsFunction>
        {
            public bool Equals(MSSQLDefaultValueAsFunction x, MSSQLDefaultValueAsFunction y)
            {
                if (!char.IsLetter(x.FunctionText.FirstOrDefault()) || !char.IsLetter(y.FunctionText.FirstOrDefault()))
                    return string.Equals(x.FunctionText, y.FunctionText, StringComparison.Ordinal);
                string xNormalizedFunctionText = x.FunctionText.Replace("(", "").Replace(")", "");
                string yNormalizedFunctionText = y.FunctionText.Replace("(", "").Replace(")", "");
                return string.Equals(xNormalizedFunctionText, yNormalizedFunctionText, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(MSSQLDefaultValueAsFunction obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
