using System.Data.Common;
using System.Reflection;

namespace DotNetDBTools.Deploy;

public interface IDeployManager
{
    public DeployOptions Options { get; set; }
    public Events Events { get; }

    /// <summary>
    /// Adds DNDBT system tables and populates them with DNDBTInfo generated from actual database.
    /// IDs for all objects are generated randomly.
    /// </summary>
    public void RegisterAsDNDBT(DbConnection connection);
    /// <summary>
    /// Adds DNDBT system tables and populates them with DNDBTInfo taken from provided dbAssembly.
    /// Provided dbAssembly and actual database are first checked for equivalency.
    /// </summary>
    public void RegisterAsDNDBT(DbConnection connection, string dbWithDNDBTInfoAssemblyPath);
    /// <summary>
    /// Adds DNDBT system tables and populates them with DNDBTInfo taken from provided dbAssembly.
    /// Provided dbAssembly and actual database are first checked for equivalency.
    /// </summary>
    public void RegisterAsDNDBT(DbConnection connection, Assembly dbWithDNDBTInfoAssembly);
    /// <summary>
    /// Deletes all DNDBT system tables.
    /// </summary>
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
    /// Do not include DNDBT system info changes in script.
    /// </summary>
    public void GenerateNoDNDBTInfoPublishScript(string dbAssemblyPath, string outputPath);
    /// <summary>
    /// Do not include DNDBT system info changes in script.
    /// </summary>
    public void GenerateNoDNDBTInfoPublishScript(Assembly dbAssembly, string outputPath);

    /// <summary>
    /// Do not include DNDBT system info changes in script.
    /// </summary>
    public void GenerateNoDNDBTInfoPublishScript(string newDbAssemblyPath, string oldDbAssemblyPath, string outputPath);
    /// <summary>
    /// Do not include DNDBT system info changes in script.
    /// </summary>
    public void GenerateNoDNDBTInfoPublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath);

    public void GenerateDefinition(DbConnection connection, string outputDirectory);
}
