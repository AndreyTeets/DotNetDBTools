using System.Reflection;
using DotNetDBTools.Deploy.SQLite;

namespace DotNetDBTools.SampleDeployUtil.SQLite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine($"{nameof(DeployAgnosticSampleDB)}:");
            DeployAgnosticSampleDB();
            System.Console.WriteLine(System.Environment.NewLine);

            System.Console.WriteLine($"{nameof(DeploySQLiteSampleDB)}:");
            DeploySQLiteSampleDB();
            System.Console.WriteLine(System.Environment.NewLine);
        }

        private static void DeployAgnosticSampleDB()
        {
            string dbAssemblyPath = "../../../../DotNetDBTools.SampleDB.Agnostic/bin/DbAssembly/DotNetDBTools.SampleDB.Agnostic.dll";
            SQLiteDeployManager deployManager = new();
            deployManager.UpdateDatabase(dbAssemblyPath);
        }

        private static void DeploySQLiteSampleDB()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.SQLite.Tables.MyTable1));
            SQLiteDeployManager deployManager = new();
            deployManager.UpdateDatabase(dbAssembly);
        }
    }
}
