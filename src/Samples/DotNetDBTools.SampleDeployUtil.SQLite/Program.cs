using System;
using System.IO;
using DotNetDBTools.Deploy.SQLite;

namespace DotNetDBTools.SampleDeployUtil.SQLite
{
    public class Program
    {
        private const string AgnosticDbProjectBinDir = "../../../../../Samples/DotNetDBTools.SampleDB.Agnostic/bin";
        private static readonly string s_agnosticDbAssemblyPath = $"{AgnosticDbProjectBinDir}/DbAssembly/DotNetDBTools.SampleDB.Agnostic.dll";
        private static readonly string s_agnosticDbFilePath = $"{AgnosticDbProjectBinDir}/DbFile/AgnosticSampleDB.db";
        private static readonly string s_agnosticConnectionString = $"DataSource={s_agnosticDbFilePath};Mode=ReadWriteCreate;";

        private const string SQLiteDbProjectBinDir = "../../../../../Samples/DotNetDBTools.SampleDB.SQLite/bin";
        private static readonly string s_sqliteDbAssemblyPath = $"{SQLiteDbProjectBinDir}/DbAssembly/DotNetDBTools.SampleDB.SQLite.dll";
        private static readonly string s_sqliteDbFilePath = $"{SQLiteDbProjectBinDir}/DbFile/SQLiteSampleDB.db";
        private static readonly string s_sqliteConnectionString = $"DataSource={s_sqliteDbFilePath};Mode=ReadWriteCreate;";

        public static void Main()
        {
            RunAgnosticSampleDBExample();
            RunSQLiteSampleDBExample();
        }

        private static void RunAgnosticSampleDBExample()
        {
            DropDatabaseIfExists(s_agnosticDbFilePath);
            Directory.CreateDirectory(Path.GetDirectoryName(s_agnosticDbFilePath));
            Console.WriteLine("Creating new AgnosticSampleDB...");
            DeployAgnosticSampleDB();
            Console.WriteLine("Updating existing AgnosticSampleDB...");
            DeployAgnosticSampleDB();
        }

        private static void RunSQLiteSampleDBExample()
        {
            DropDatabaseIfExists(s_sqliteDbFilePath);
            Directory.CreateDirectory(Path.GetDirectoryName(s_sqliteDbFilePath));
            Console.WriteLine("Creating new SQLiteSampleDB...");
            DeploySQLiteSampleDB();
            Console.WriteLine("Updating existing SQLiteSampleDB...");
            DeploySQLiteSampleDB();
        }

        private static void DeployAgnosticSampleDB()
        {
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(s_agnosticDbAssemblyPath, s_agnosticConnectionString);
        }

        private static void DeploySQLiteSampleDB()
        {
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(s_sqliteDbAssemblyPath, s_sqliteConnectionString);
        }

        private static void DropDatabaseIfExists(string dbFilePath)
        {
            if (File.Exists(dbFilePath))
                File.Delete(dbFilePath);
        }
    }
}
