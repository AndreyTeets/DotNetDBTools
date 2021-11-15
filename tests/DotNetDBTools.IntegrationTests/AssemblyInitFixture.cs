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
            await MSSQLContainerHelper.InitContainer();
            await MySQLContainerHelper.InitContainer();
            await PostgreSQLContainerHelper.InitContainer();
        }
    }
}
