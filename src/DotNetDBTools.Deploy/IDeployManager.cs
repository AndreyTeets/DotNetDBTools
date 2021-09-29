using System.Reflection;

namespace DotNetDBTools.Deploy
{
    public interface IDeployManager
    {
        public void PublishDatabase(string dbAssemblyPath, string connectionString);
        public void PublishDatabase(Assembly dbAssembly, string connectionString);
        public void GeneratePublishScript(string newDbAssemblyPath, string oldDbConnectionString, string outputPath);
        public void GeneratePublishScript(Assembly newDbAssembly, string oldDbConnectionString, string outputPath);
        public void GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath);
        public void GeneratePublishScript(string dbAssemblyPath, string outputPath);
        public void GeneratePublishScript(Assembly dbAssembly, string outputPath);
        public void RegisterAsDNDBT(string connectionString);
        public void UnregisterAsDNDBT(string connectionString);
        public void GenerateDefinition(string connectionString, string outputDirectory);
    }
}
