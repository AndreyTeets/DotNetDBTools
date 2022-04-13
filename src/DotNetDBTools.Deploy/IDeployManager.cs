using System.Data.Common;
using System.Reflection;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy;

public interface IDeployManager
{
    public DeployOptions Options { get; set; }
    public Events Events { get; }

    /// <summary>
    /// Checks if DNDBT system tables exist.
    /// </summary>
    public bool IsRegisteredAsDNDBT(DbConnection connection);
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

    /// <summary>
    /// Updates database to the state specified in first argument.
    /// Database must be registered (even if it's empty).
    /// </summary>
    public void PublishDatabase(string dbAssemblyPath, DbConnection connection);
    /// <summary>
    /// Updates database to the state specified in first argument.
    /// Database must be registered (even if it's empty).
    /// </summary>
    public void PublishDatabase(Assembly dbAssembly, DbConnection connection);
    /// <summary>
    /// Updates database to the state specified in first argument.
    /// Database must be registered (even if it's empty).
    /// </summary>
    public void PublishDatabase(Database database, DbConnection connection);

    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state found at connection.
    /// Database must be registered (even if it's empty) during both script-generation and script-execution.
    /// </summary>
    public void GeneratePublishScript(string dbAssemblyPath, DbConnection connection, string outputPath);
    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state found at connection.
    /// Database must be registered (even if it's empty) during both script-generation and script-execution.
    /// </summary>
    public void GeneratePublishScript(Assembly dbAssembly, DbConnection connection, string outputPath);
    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state found at connection.
    /// Database must be registered (even if it's empty) during both script-generation and script-execution.
    /// </summary>
    public void GeneratePublishScript(Database database, DbConnection connection, string outputPath);

    /// <summary>
    /// Generates sql script to create all defined DNDBT user objects.
    /// Database must be empty and registered during script-execution.
    /// </summary>
    public void GeneratePublishScript(string dbAssemblyPath, string outputPath);
    /// <summary>
    /// Generates sql script to create all defined DNDBT user objects.
    /// Database must be empty and registered during script-execution.
    /// </summary>
    public void GeneratePublishScript(Assembly dbAssembly, string outputPath);
    /// <summary>
    /// Generates sql script to create all defined DNDBT user objects.
    /// Database must be empty and registered during script-execution.
    /// </summary>
    public void GeneratePublishScript(Database database, string outputPath);

    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state specified in second argument.
    /// Database must be registered (even if it's empty) during script-execution.
    /// </summary>
    public void GeneratePublishScript(string newDbAssemblyPath, string oldDbAssemblyPath, string outputPath);
    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state specified in second argument.
    /// Database must be registered (even if it's empty) during script-execution.
    /// </summary>
    public void GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath);
    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state specified in second argument.
    /// Database must be registered (even if it's empty) during script-execution.
    /// </summary>
    public void GeneratePublishScript(Database newDatabase, Database oldDatabase, string outputPath);

    /// <summary>
    /// Generates sql script to create all defined DNDBT user objects.
    /// DNDBT system info changes will not be included.
    /// Database must be empty but doesn't have to be registered during script-execution.
    /// </summary>
    public void GenerateNoDNDBTInfoPublishScript(string dbAssemblyPath, string outputPath);
    /// <summary>
    /// Generates sql script to create all defined DNDBT user objects.
    /// DNDBT system info changes will not be included.
    /// Database must be empty but doesn't have to be registered during script-execution.
    /// </summary>
    public void GenerateNoDNDBTInfoPublishScript(Assembly dbAssembly, string outputPath);
    /// <summary>
    /// Generates sql script to create all defined DNDBT user objects.
    /// DNDBT system info changes will not be included.
    /// Database must be empty but doesn't have to be registered during script-execution.
    /// </summary>
    public void GenerateNoDNDBTInfoPublishScript(Database database, string outputPath);

    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state specified in second argument.
    /// DNDBT system info changes will not be included.
    /// Database doesn't have to be registered during script-execution.
    /// </summary>
    public void GenerateNoDNDBTInfoPublishScript(string newDbAssemblyPath, string oldDbAssemblyPath, string outputPath);
    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state specified in second argument.
    /// DNDBT system info changes will not be included.
    /// Database doesn't have to be registered during script-execution.
    /// </summary>
    public void GenerateNoDNDBTInfoPublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath);
    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state specified in second argument.
    /// DNDBT system info changes will not be included.
    /// Database doesn't have to be registered during script-execution.
    /// </summary>
    public void GenerateNoDNDBTInfoPublishScript(Database newDatabase, Database oldDatabase, string outputPath);

    /// <summary>
    /// Generates dotnet project for a registerd or unregistered database.
    /// If database is unregistered IDs for all objects are generated randomly.
    /// </summary>
    public void GenerateDefinition(DbConnection connection, string outputDirectory);

    /// <summary>
    /// Creates database model using DBMS and DNDBT system tables.
    /// </summary>
    public Database CreateDatabaseModelUsingDNDBTSysInfo(DbConnection connection);
    /// <summary>
    /// Creates database model using DBMS system tables only.
    /// IDs for all objects are generated randomly.
    /// </summary>
    public Database CreateDatabaseModelUsingDBMSSysInfo(DbConnection connection);
}
