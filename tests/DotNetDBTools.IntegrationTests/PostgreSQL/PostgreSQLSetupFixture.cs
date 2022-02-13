using System.Threading.Tasks;
using NUnit.Framework;

namespace DotNetDBTools.IntegrationTests.PostgreSQL;

[SetUpFixture]
[SingleThreaded]
[Parallelizable(ParallelScope.Self)]
internal class PostgreSQLSetupFixture
{
    [OneTimeSetUp]
    public static async Task OneTimeSetUp()
    {
        await PostgreSQLContainerHelper.InitContainer();
    }
}
