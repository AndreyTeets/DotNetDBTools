using System.IO;
using System.Reflection;
using DotNetDBTools.Deploy.SQLite;

namespace DotNetDBTools.SampleDeployUtil.SQLite
{
    public class Program
    {
        private const string AgnosticDbFilePath = @".\tmp\AgnosticSampleDB.db";
        private static readonly string s_agnosticConnectionString = $"DataSource={AgnosticDbFilePath};Mode=ReadWriteCreate;";
        private const string SQLiteDbFilePath = @".\tmp\SQLiteSampleDB.db";
        private static readonly string s_sqliteConnectionString = $"DataSource={SQLiteDbFilePath};Mode=ReadWriteCreate;";

        public static void Main()
        {
            System.Console.WriteLine("Deploying AgnosticSampleDB...");
            Directory.CreateDirectory(Path.GetDirectoryName(AgnosticDbFilePath));
            DeployAgnosticSampleDB();

            System.Console.WriteLine("Deploying SQLiteSampleDB...");
            Directory.CreateDirectory(Path.GetDirectoryName(SQLiteDbFilePath));
            DeploySQLiteSampleDB();
        }

        private static void DeployAgnosticSampleDB()
        {
            string dbAssemblyPath = "../../../../DotNetDBTools.SampleDB.Agnostic/bin/DbAssembly/DotNetDBTools.SampleDB.Agnostic.dll";
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(dbAssemblyPath, s_agnosticConnectionString);
        }

        private static void DeploySQLiteSampleDB()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.SQLite.Tables.MyTable1));
            SQLiteDeployManager deployManager = new(true, false);
            deployManager.UpdateDatabase(dbAssembly, s_sqliteConnectionString);
        }
    }
}
