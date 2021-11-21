using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetDBTools.IntegrationTests.MSSQL;
using DotNetDBTools.IntegrationTests.MySQL;
using DotNetDBTools.IntegrationTests.PostgreSQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetDBTools.IntegrationTests
{
    [TestClass]
    public class AssemblyInitFixture
    {
        [AssemblyInitialize]
        public static async Task AssemblyInitialize(TestContext _)
        {
            List<Task> tasks = new();
            tasks.Add(MSSQLContainerHelper.InitContainer());
            tasks.Add(MySQLContainerHelper.InitContainer());
            tasks.Add(PostgreSQLContainerHelper.InitContainer());
            await Task.WhenAll(tasks);
        }
    }
}
