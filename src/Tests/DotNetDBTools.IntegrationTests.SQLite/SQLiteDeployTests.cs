using System;
using System.IO;
using System.Reflection;
using DotNetDBTools.Deploy.SQLite;
using Xunit;

namespace DotNetDBTools.IntegrationTests.SQLite
{
    public class SQLiteDeployTests : IDisposable
    {
        private const string DbFilePath = @".\tmp\SampleDB.db";
        private static readonly string s_connectionString = $"DataSource={DbFilePath};Mode=ReadWriteCreate;";

        public SQLiteDeployTests()
        {
            DropDatabase(DbFilePath);
            Directory.CreateDirectory(Path.GetDirectoryName(DbFilePath));
        }

        public void Dispose()
        {
            DropDatabase(DbFilePath);
        }

        [Fact]
        public void Update_AgnosticSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.Agnostic.Tables.MyTable1));
            SQLiteDeployManager deployManager = new(true);
            deployManager.UpdateDatabase(dbAssembly, s_connectionString);
            deployManager.UpdateDatabase(dbAssembly, s_connectionString);
        }

        [Fact]
        public void Update_SQLiteSampleDB_CreatesDbFromZero_And_UpdatesItAgain_WithoutErrors()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.SQLite.Tables.MyTable1));
            SQLiteDeployManager deployManager = new(true);
            deployManager.UpdateDatabase(dbAssembly, s_connectionString);
            deployManager.UpdateDatabase(dbAssembly, s_connectionString);
        }

        private void DropDatabase(string dbFilePath)
        {
            if (File.Exists(dbFilePath))
                File.Delete(dbFilePath);
        }
    }
}
