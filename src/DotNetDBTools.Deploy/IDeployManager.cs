using System.Data.Common;
using System.Reflection;

namespace DotNetDBTools.Deploy;

public interface IDeployManager
{
    public DeployOptions Options { get; set; }

    public void RegisterAsDNDBT(DbConnection connection);
    public void UnregisterAsDNDBT(DbConnection connection);

    public void PublishDatabase(string dbAssemblyPath, DbConnection connection);
    public void PublishDatabase(Assembly dbAssembly, DbConnection connection);

    public void GeneratePublishScript(string dbAssemblyPath, DbConnection connection, string outputPath);
    public void GeneratePublishScript(Assembly dbAssembly, DbConnection connection, string outputPath);

    public void GeneratePublishScript(string dbAssemblyPath, string outputPath);
    public void GeneratePublishScript(Assembly dbAssembly, string outputPath);

    public void GeneratePublishScript(string newDbAssemblyPath, string oldDbAssemblyPath, string outputPath);
    public void GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath);

    /// <summary>
    /// Include only DDL changes into script (and skip DNDBT system info changes).
    /// </summary>
    public void GenerateDDLOnlyPublishScript(string dbAssemblyPath, string outputPath);
    /// <summary>
    /// Include only DDL changes into script (and skip DNDBT system info changes).
    /// </summary>
    public void GenerateDDLOnlyPublishScript(Assembly dbAssembly, string outputPath);

    /// <summary>
    /// Include only DDL changes into script (and skip DNDBT system info changes).
    /// </summary>
    public void GenerateDDLOnlyPublishScript(string newDbAssemblyPath, string oldDbAssemblyPath, string outputPath);
    /// <summary>
    /// Include only DDL changes into script (and skip DNDBT system info changes).
    /// </summary>
    public void GenerateDDLOnlyPublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath);

    public void GenerateDefinition(DbConnection connection, string outputDirectory);
}
