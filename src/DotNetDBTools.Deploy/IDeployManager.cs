using System.Data.Common;
using System.Reflection;

namespace DotNetDBTools.Deploy;

public interface IDeployManager
{
    public DeployOptions Options { get; set; }

    public void PublishDatabase(string dbAssemblyPath, DbConnection connection);
    public void PublishDatabase(Assembly dbAssembly, DbConnection connection);
    public void GeneratePublishScript(string dbAssemblyPath, DbConnection connection, string outputPath);
    public void GeneratePublishScript(Assembly dbAssembly, DbConnection connection, string outputPath);
    public void GeneratePublishScript(string dbAssemblyPath, string outputPath);
    public void GeneratePublishScript(Assembly dbAssembly, string outputPath);
    public void GeneratePublishScript(string newDbAssemblyPath, string oldDbAssemblyPath, string outputPath);
    public void GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath);
    public void RegisterAsDNDBT(DbConnection connection);
    public void UnregisterAsDNDBT(DbConnection connection);
    public void GenerateDefinition(DbConnection connection, string outputDirectory);
}
