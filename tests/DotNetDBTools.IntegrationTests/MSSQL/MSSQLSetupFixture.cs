using System.Threading.Tasks;
using NUnit.Framework;

namespace DotNetDBTools.IntegrationTests.MSSQL;

[SetUpFixture]
[SingleThreaded]
[Parallelizable(ParallelScope.Self)]
internal class MSSQLSetupFixture
{
    [OneTimeSetUp]
    public static async Task OneTimeSetUp()
    {
        await MSSQLContainerHelper.InitContainer();
    }
}
