using System.Reflection;

namespace DotNetDBTools.Deploy
{
    public interface IDeployManager
    {
        public void PublishDatabase(string dbAssemblyPath, string connectionString);
        public void PublishDatabase(Assembly dbAssembly, string connectionString);
        public void GeneratePublishScript(string dbAssemblyPath, string connectionString, string outputPath);
        public void GeneratePublishScript(Assembly dbAssembly, string connectionString, string outputPath);
        public void GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath);
        public void GenerateDefinition(string connectionString, string outputDirectory);
        public void RegisterAsDNDBT(string connectionString);
        public void UnregisterAsDNDBT(string connectionString);
    }
}
