using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace DotNetDBTools.IntegrationTests.TestHelpers
{
    public static class DbHelper
    {
        public static async Task WaitUntilDatabaseAvailableAsync(DbConnection connection, int timeoutSeconds)
        {
            DateTime start = DateTime.UtcNow;
            bool connectionEstablised = false;
            while (!connectionEstablised && start.AddSeconds(timeoutSeconds) > DateTime.UtcNow)
            {
                try
                {
                    await connection.OpenAsync();
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
}
