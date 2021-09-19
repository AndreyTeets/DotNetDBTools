using System.Reflection;

namespace DotNetDBTools.Deploy
{
    public interface IDeployManager
    {
        public void UpdateDatabase(string dbAssemblyPath, string connectionString);
        public void UpdateDatabase(Assembly dbAssembly, string connectionString);
        public string GenerateUpdateScript(string dbAssemblyPath, string connectionString);
        public string GenerateUpdateScript(Assembly dbAssembly, string connectionString);
    }
}
