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
        public void AgnosticSampleDB_DbInfoFromDNDBTSysInfo_IsEquivalentTo_DbInfoFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(ConnectionString));
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);

            MSSQLDatabaseInfo dbInfoFromDbAssembly = (MSSQLDatabaseInfo)new MSSQLDbModelConverter().FromAgnostic(
                (AgnosticDatabaseInfo)DbDefinitionParser.CreateDatabaseInfo(s_agnosticSampleDbAssemblyPath));
            MSSQLDatabaseInfo dbInfoFromDNDBTSysInfo = (MSSQLDatabaseInfo)interactor.GetDatabaseModelFromDNDBTSysInfo();

            dbInfoFromDNDBTSysInfo.Should().BeEquivalentTo(dbInfoFromDbAssembly, options => options
                .Excluding(dbInfo => dbInfo.Name)
                .Excluding(dbInfo => dbInfo.Views));
        }

        [TestMethod]
        public void AgnosticSampleDB_DbInfoFromDBMSSysInfo_IsEquivalentTo_DbInfoFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(ConnectionString));
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_agnosticSampleDbAssemblyPath, ConnectionString);
            deployManager.UnregisterAsDNDBT(ConnectionString);

            MSSQLDatabaseInfo dbInfoFromDbAssembly = (MSSQLDatabaseInfo)new MSSQLDbModelConverter().FromAgnostic(
                (AgnosticDatabaseInfo)DbDefinitionParser.CreateDatabaseInfo(s_agnosticSampleDbAssemblyPath));
            MSSQLDatabaseInfo dbInfoFromDBMSSysInfo = (MSSQLDatabaseInfo)interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbInfoFromDBMSSysInfo.Should().BeEquivalentTo(dbInfoFromDbAssembly, options => options
                .Excluding(dbInfo => dbInfo.Name)
                .Excluding(dbInfo => dbInfo.Views)
                .Excluding(dbInfo => dbInfo.Path.EndsWith(".ID", StringComparison.Ordinal)));
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
        public void MSSQLSampleDB_DbInfoFromDNDBTSysInfo_IsEquivalentTo_DbInfoFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(ConnectionString));
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_mssqlSampleDbAssemblyPath, ConnectionString);

            MSSQLDatabaseInfo dbInfoFromDbAssembly = (MSSQLDatabaseInfo)DbDefinitionParser.CreateDatabaseInfo(s_mssqlSampleDbAssemblyPath);
            MSSQLDatabaseInfo dbInfoFromDNDBTSysInfo = (MSSQLDatabaseInfo)interactor.GetDatabaseModelFromDNDBTSysInfo();

            dbInfoFromDNDBTSysInfo.Should().BeEquivalentTo(dbInfoFromDbAssembly, options => options
                .Excluding(dbInfo => dbInfo.Name)
                .Excluding(dbInfo => dbInfo.Functions)
                .Excluding(dbInfo => dbInfo.Views)
                .Using(new MSSQLDefaultValueAsFunctionComparer()));
        }

        [TestMethod]
        public void MSSQLSampleDB_DbInfoFromDBMSSysInfo_IsEquivalentTo_DbInfoFromDbAssembly()
        {
            DropDatabaseIfExists(ConnectionString);
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(ConnectionString));
            MSSQLDeployManager deployManager = new(true, false);
            deployManager.PublishDatabase(s_mssqlSampleDbAssemblyPath, ConnectionString);
            deployManager.UnregisterAsDNDBT(ConnectionString);

            MSSQLDatabaseInfo dbInfoFromDbAssembly = (MSSQLDatabaseInfo)DbDefinitionParser.CreateDatabaseInfo(s_mssqlSampleDbAssemblyPath);
            MSSQLDatabaseInfo dbInfoFromDBMSSysInfo = (MSSQLDatabaseInfo)interactor.GenerateDatabaseModelFromDBMSSysInfo();

            dbInfoFromDBMSSysInfo.Should().BeEquivalentTo(dbInfoFromDbAssembly, options => options
                .Excluding(dbInfo => dbInfo.Name)
                .Excluding(dbInfo => dbInfo.Views)
                .Excluding(dbInfo => dbInfo.Functions)
                .Excluding(dbInfo => dbInfo.Path.EndsWith(".ID", StringComparison.Ordinal))
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
