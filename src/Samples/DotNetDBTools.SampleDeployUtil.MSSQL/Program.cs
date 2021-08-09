using System.Reflection;
using DotNetDBTools.Deploy.MSSQL;

namespace DotNetDBTools.SampleDeployUtil.MSSQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine($"{nameof(DeployAgnosticSampleDB)}:");
            DeployAgnosticSampleDB();
            System.Console.WriteLine(System.Environment.NewLine);

            System.Console.WriteLine($"{nameof(DeployMSSQLSampleDB)}:");
            DeployMSSQLSampleDB();
            System.Console.WriteLine(System.Environment.NewLine);
        }

        private static void DeployAgnosticSampleDB()
        {
            string dbAssemblyPath = "../../../../DotNetDBTools.SampleDB.Agnostic/bin/DbAssembly/DotNetDBTools.SampleDB.Agnostic.dll";
            MSSQLDeployManager deployManager = new();
            deployManager.UpdateDatabase(dbAssemblyPath);
        }

        private static void DeployMSSQLSampleDB()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.MSSQL.Tables.MyTable1));
            MSSQLDeployManager deployManager = new();
            deployManager.UpdateDatabase(dbAssembly);
        }
    }
}
