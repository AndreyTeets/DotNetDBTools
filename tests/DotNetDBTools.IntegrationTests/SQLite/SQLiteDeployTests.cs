using System.Data.Common;
using System.IO;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Deploy;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.IntegrationTests.Base;
using DotNetDBTools.Models.SQLite;
using FluentAssertions.Equivalency;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DotNetDBTools.IntegrationTests.Constants;

namespace DotNetDBTools.IntegrationTests.SQLite
{
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

        protected override string NormalizeDefaultValueAsFunctionText(string value)
        {
            return value.ToUpper();
        }

        protected override void CreateDatabase(string connectionString)
        {
        }

        protected override void DropDatabaseIfExists(string connectionString)
        {
            SqliteConnectionStringBuilder connectionStringBuilder = new(connectionString);
            string dbFilePath = connectionStringBuilder.DataSource;

            if (File.Exists(dbFilePath))
                File.Delete(dbFilePath);
            Directory.CreateDirectory(Path.GetDirectoryName(dbFilePath));
        }

        protected override string CreateConnectionString(string testName)
        {
            return $@"DataSource={DbFilesFolder}/{testName}.db;Mode=ReadWriteCreate;";
        }

        private protected override IDbModelFromDbSysInfoBuilder CreateDbModelFromDbSysInfoBuilder(DbConnection connection)
        {
            return new SQLiteDbModelFromDbSysInfoBuilder(new SQLiteQueryExecutor(connection));
        }
    }
}
