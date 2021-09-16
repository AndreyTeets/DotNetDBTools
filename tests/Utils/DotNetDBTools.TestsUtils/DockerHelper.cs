using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace DotNetDBTools.TestsUtils
{
    public static class DockerHelper
    {
        public static async Task StopAndRemoveContainerIfExistsAndNotRunningOrOld(string containerName, int oldMinutes)
        {
            ContainerListResponse container = await TryGetExistingContainer(containerName);
            if (container is null)
                return;

            if (container.State != "running" ||
                container.Created.AddMinutes(oldMinutes) < DateTime.UtcNow)
            {
                DockerClient dockerClient = CreateDockerClient();
                await dockerClient.Containers.StopContainerAsync(container.ID, new ContainerStopParameters());
                await dockerClient.Containers.RemoveContainerAsync(container.ID, new ContainerRemoveParameters());
            }
        }

        public static async Task CreateAndStartContainerIfNotExists(
            string containerName,
            string imageNameWithTag,
            List<string> envVariables,
            Dictionary<string, string> portRedirects)
        {
            DockerClient dockerClient = CreateDockerClient();

            ContainerListResponse existingContainer = await TryGetExistingContainer(containerName);
            if (existingContainer is not null)
                return;

            await dockerClient.Images.CreateImageAsync(
                new ImagesCreateParameters
                {
                    FromImage = imageNameWithTag
                },
                null,
                new Progress<JSONMessage>());

            CreateContainerResponse newContainer = await dockerClient.Containers.CreateContainerAsync(
                new CreateContainerParameters
                {
                    Name = containerName,
                    Image = imageNameWithTag,
                    Env = envVariables,
                    HostConfig = CreateHostConfig(portRedirects),
                });

            await dockerClient.Containers.StartContainerAsync(
                newContainer.ID,
                new ContainerStartParameters());
        }

        private static HostConfig CreateHostConfig(Dictionary<string, string> portRedirects)
        {
            Dictionary<string, IList<PortBinding>> portBindings = new();
            foreach (KeyValuePair<string, string> portRedirect in portRedirects)
            {
                portBindings.Add(
                    portRedirect.Key,
                    new List<PortBinding>()
                    {
                        new PortBinding
                        {
                            HostPort = portRedirect.Value
                        }
                    });
            }

            return new HostConfig()
            {
                PortBindings = portBindings
            };
        }

        private static async Task<ContainerListResponse> TryGetExistingContainer(string containerName)
        {
            DockerClient dockerClient = CreateDockerClient();

            IList<ContainerListResponse> allContainers = await dockerClient.Containers
                .ListContainersAsync(new ContainersListParameters() { All = true });
            ContainerListResponse container = allContainers
                .SingleOrDefault(cont => cont.Names.Any(n => n.Contains(containerName)));

            return container;
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
    }
}
