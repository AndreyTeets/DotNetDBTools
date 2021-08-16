using System.Reflection;
using DotNetDBTools.Deploy.SQLite;
using Xunit;

namespace DotNetDBTools.IntegrationTests.SQLite
{
    public class UnitTest1
    {
        [Fact]
        public void Update_SQLiteSampleDB_Works()
        {
            Assembly dbAssembly = Assembly.GetAssembly(typeof(SampleDB.SQLite.Tables.MyTable1));
            SQLiteDeployManager deployManager = new();
            deployManager.UpdateDatabase(dbAssembly);
        }
    }
}
