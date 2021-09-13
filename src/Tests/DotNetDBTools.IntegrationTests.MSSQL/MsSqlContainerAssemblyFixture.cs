using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetDBTools.IntegrationTests.MSSQL
{
    [TestClass]
    public class MsSqlContainerAssemblyFixture
    {
        private const string MsSqlImage = "mcr.microsoft.com/mssql/server";
        private const string MsSqlImageTag = "2019-latest";
        private const string MsSqlContainerName = "DotNetDBTools_IntegrationTests_MSSQL";
        private const string MsSqlServerPassword = "Strong(!)Passw0rd";
        private const string MsSqlServerHostPort = "5005";

        public static string MsSqlContainerConnectionString => $"Data Source=localhost,{MsSqlServerHostPort};Integrated Security=False;User ID=SA;Password={MsSqlServerPassword}";

        [AssemblyInitialize]
        public static async Task AssemblyInitialize(TestContext _)
        {
            await StopAndRemoveMsSqlContainerIfExistsAndOld(oldMinutes: 60);
            await CreateAndStartMsSqlContainerAndWaitUntilReadyIfDoesntExist();
        }

        private static async Task CreateAndStartMsSqlContainerAndWaitUntilReadyIfDoesntExist()
        {
            DockerClient dockerClient = CreateDockerClient();

            ContainerListResponse msSqlContainer = await TryGetExistingContainer(dockerClient);
            if (msSqlContainer is not null)
                return;

            await dockerClient.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = $"{MsSqlImage}:{MsSqlImageTag}"
                },
                null,
                new Progress<JSONMessage>());

            CreateContainerResponse msSqlServerContainer = await dockerClient.Containers.CreateContainerAsync(
                new CreateContainerParameters
                {
                    Name = MsSqlContainerName,
                    Image = $"{MsSqlImage}:{MsSqlImageTag}",
                    Env = new List<string>
                    {
                        "ACCEPT_EULA=Y",
                        $"SA_PASSWORD={MsSqlServerPassword}",
                    },
                    HostConfig = new HostConfig
                    {
                        PortBindings = new Dictionary<string, IList<PortBinding>>
                        {
                            {
                                "1433/tcp",
                                new PortBinding[]
                                {
                                    new PortBinding
                                    {
                                        HostPort = MsSqlServerHostPort
                                    }
                                }
                            }
                        }
                    },
                });

            await dockerClient.Containers.StartContainerAsync(
                msSqlServerContainer.ID,
                new ContainerStartParameters());

            await WaitUntilDatabaseAvailableAsync();
        }

        private static DockerClient CreateDockerClient()
        {
            string dockerUri = IsRunningOnWindows()
                ? "npipe://./pipe/docker_engine"
                : "unix:///var/run/docker.sock";
            DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerUri)).CreateClient();
            return dockerClient;
        }

        private static bool IsRunningOnWindows() => Environment.OSVersion.Platform == PlatformID.Win32NT;

        private static async Task StopAndRemoveMsSqlContainerIfExistsAndOld(int oldMinutes = 0)
        {
            DockerClient dockerClient = CreateDockerClient();

            ContainerListResponse msSqlContainer = await TryGetExistingContainer(dockerClient);
            if (msSqlContainer is null)
                return;

            if (msSqlContainer.Created.AddMinutes(oldMinutes) < DateTime.UtcNow)
            {
                await dockerClient.Containers.StopContainerAsync(msSqlContainer.ID, new ContainerStopParameters());
                await dockerClient.Containers.RemoveContainerAsync(msSqlContainer.ID, new ContainerRemoveParameters());
            }
        }

        private static async Task<ContainerListResponse> TryGetExistingContainer(DockerClient dockerClient)
        {
            IList<ContainerListResponse> runningContainers = await dockerClient.Containers
                            .ListContainersAsync(new ContainersListParameters());
            ContainerListResponse msSqlContainer = runningContainers
                .SingleOrDefault(cont => cont.Names.Any(n => n.Contains(MsSqlContainerName)));
            return msSqlContainer;
        }

        private static async Task WaitUntilDatabaseAvailableAsync()
        {
            DateTime start = DateTime.UtcNow;
            const int MaxWaitTimeSeconds = 60;
            bool connectionEstablised = false;
            while (!connectionEstablised && start.AddSeconds(MaxWaitTimeSeconds) > DateTime.UtcNow)
            {
                try
                {
                    string sqlConnectionString = MsSqlContainerConnectionString;
                    using SqlConnection sqlConnection = new(sqlConnectionString);
                    await sqlConnection.OpenAsync();
                    connectionEstablised = true;
                }
                catch
                {
                    await Task.Delay(500);
                }
            }

            if (!connectionEstablised)
                throw new Exception("Connection to the SQL docker database could not be established within 60 seconds.");
        }
    }
}
