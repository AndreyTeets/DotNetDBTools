using System;
using System.Data;
using System.Threading.Tasks;

namespace DotNetDBTools.IntegrationTests.Utilities;

internal static class DbAvailabilityChecker
{
    public static async Task WaitUntilDatabaseAvailableAsync(IDbConnection connection, int timeoutSeconds)
    {
        DateTime start = DateTime.UtcNow;
        bool connectionEstablised = false;
        while (!connectionEstablised && start.AddSeconds(timeoutSeconds) > DateTime.UtcNow)
        {
            try
            {
                connection.Open();
                connectionEstablised = true;
            }
            catch
            {
                await Task.Delay(500);
            }
        }

        if (!connectionEstablised)
            throw new Exception($"Connection to database could not be established within {timeoutSeconds} seconds.");
    }
}
