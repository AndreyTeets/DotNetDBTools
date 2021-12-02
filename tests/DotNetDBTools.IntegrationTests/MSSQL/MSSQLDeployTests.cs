using System.Data.Common;
using System.Data.SqlClient;
using Dapper;
using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL;
using DotNetDBTools.IntegrationTests.Base;
using DotNetDBTools.Models.MSSQL;
using FluentAssertions.Equivalency;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.MSSQL
{
    [TestClass]
    public class MSSQLDeployTests : BaseDeployTests<
        MSSQLDatabase,
        SqlConnection,
        MSSQLDbModelConverter,
        MSSQLDeployManager>
    {
        private static string ConnectionStringWithoutDb => MSSQLContainerHelper.MsSqlContainerConnectionString;
        protected override string AgnosticSampleDbAssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.Agnostic.dll";
        protected override string SpecificDBMSSampleDbAssemblyPath => $"{SamplesOutputDir}/DotNetDBTools.SampleDB.MSSQL.dll";

        protected override EquivalencyAssertionOptions<MSSQLDatabase> AddAdditionalDbModelEquivalenceyOptions(
            EquivalencyAssertionOptions<MSSQLDatabase> options)
        {
            return options.Excluding(database => database.Functions);
        }

        protected override string NormalizeDefaultValueAsFunctionText(string value)
        {
            return value.ToUpper()
                .Replace("(", "")
                .Replace(")", "");
        }

        protected override void CreateDatabase(string connectionString)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.InitialCatalog;

            using SqlConnection connection = new(ConnectionStringWithoutDb);
            connection.Execute(
$@"CREATE DATABASE {databaseName};");
        }

        protected override void DropDatabaseIfExists(string connectionString)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string databaseName = connectionStringBuilder.InitialCatalog;

            using SqlConnection connection = new(ConnectionStringWithoutDb);
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

        protected override string CreateConnectionString(string testName)
        {
            string databaseName = testName;
            SqlConnectionStringBuilder connectionStringBuilder = new(ConnectionStringWithoutDb);
            connectionStringBuilder.InitialCatalog = databaseName;
            string connectionString = connectionStringBuilder.ConnectionString;
            return connectionString;
        }

        private protected override Interactor CreateInteractor(DbConnection connection)
        {
            return new MSSQLInteractor(new MSSQLQueryExecutor(connection));
        }
    }
}
