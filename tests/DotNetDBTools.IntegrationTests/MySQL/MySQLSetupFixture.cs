using System.Threading.Tasks;
using NUnit.Framework;

namespace DotNetDBTools.IntegrationTests.MySQL;

[SetUpFixture]
[SingleThreaded]
[Parallelizable(ParallelScope.Self)]
internal class MySQLSetupFixture
{
    [OneTimeSetUp]
    public static async Task OneTimeSetUp()
    {
        await MySQLContainerHelper.InitContainer();
    }
}
