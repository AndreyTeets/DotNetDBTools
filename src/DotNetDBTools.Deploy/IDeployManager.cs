using System.Data;
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
    public bool IsRegisteredAsDNDBT(IDbConnection connection);
    /// <summary>
    /// Adds DNDBT system tables and populates them with DNDBTInfo generated from actual database.
    /// IDs for all objects are generated randomly.
    /// </summary>
    public void RegisterAsDNDBT(IDbConnection connection);
    /// <summary>
    /// Adds DNDBT system tables and populates them with DNDBTInfo taken from provided dbAssembly.
    /// Provided dbAssembly and actual database are first checked for equivalency.
    /// </summary>
    public void RegisterAsDNDBT(IDbConnection connection, string dbWithDNDBTInfoAssemblyPath);
    /// <summary>
    /// Adds DNDBT system tables and populates them with DNDBTInfo taken from provided dbAssembly.
    /// Provided dbAssembly and actual database are first checked for equivalency.
    /// </summary>
    public void RegisterAsDNDBT(IDbConnection connection, Assembly dbWithDNDBTInfoAssembly);
    /// <summary>
    /// Adds DNDBT system tables and populates them with DNDBTInfo taken from provided database model.
    /// Provided dbAssembly and actual database are first checked for equivalency.
    /// </summary>
    public void RegisterAsDNDBT(IDbConnection connection, Database dbWithDNDBTInfo);
    /// <summary>
    /// Deletes all DNDBT system tables.
    /// </summary>
    public void UnregisterAsDNDBT(IDbConnection connection);

    /// <summary>
    /// Updates database to the state specified in first argument.
    /// Database must be registered (even if it's empty).
    /// </summary>
    public void PublishDatabase(string dbAssemblyPath, IDbConnection connection);
    /// <summary>
    /// Updates database to the state specified in first argument.
    /// Database must be registered (even if it's empty).
    /// </summary>
    public void PublishDatabase(Assembly dbAssembly, IDbConnection connection);
    /// <summary>
    /// Updates database to the state specified in first argument.
    /// Database must be registered (even if it's empty).
    /// </summary>
    public void PublishDatabase(Database database, IDbConnection connection);

    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state found at connection.
    /// Database must be registered (even if it's empty) during both script-generation and script-execution.
    /// </summary>
    public string GeneratePublishScript(string dbAssemblyPath, IDbConnection connection);
    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state found at connection.
    /// Database must be registered (even if it's empty) during both script-generation and script-execution.
    /// </summary>
    public string GeneratePublishScript(Assembly dbAssembly, IDbConnection connection);
    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state found at connection.
    /// Database must be registered (even if it's empty) during both script-generation and script-execution.
    /// </summary>
    public string GeneratePublishScript(Database database, IDbConnection connection);

    /// <summary>
    /// Generates sql script to create all defined DNDBT user objects.
    /// Database must be empty and registered during script-execution.
    /// </summary>
    public string GeneratePublishScript(string dbAssemblyPath);
    /// <summary>
    /// Generates sql script to create all defined DNDBT user objects.
    /// Database must be empty and registered during script-execution.
    /// </summary>
    public string GeneratePublishScript(Assembly dbAssembly);
    /// <summary>
    /// Generates sql script to create all defined DNDBT user objects.
    /// Database must be empty and registered during script-execution.
    /// </summary>
    public string GeneratePublishScript(Database database);

    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state specified in second argument.
    /// Database must be registered (even if it's empty) during script-execution.
    /// </summary>
    public string GeneratePublishScript(string newDbAssemblyPath, string oldDbAssemblyPath);
    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state specified in second argument.
    /// Database must be registered (even if it's empty) during script-execution.
    /// </summary>
    public string GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly);
    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state specified in second argument.
    /// Database must be registered (even if it's empty) during script-execution.
    /// </summary>
    public string GeneratePublishScript(Database newDatabase, Database oldDatabase);

    /// <summary>
    /// Generates sql script to create all defined DNDBT user objects.
    /// DNDBT system info changes will not be included.
    /// Database must be empty but doesn't have to be registered during script-execution.
    /// </summary>
    public string GenerateNoDNDBTInfoPublishScript(string dbAssemblyPath);
    /// <summary>
    /// Generates sql script to create all defined DNDBT user objects.
    /// DNDBT system info changes will not be included.
    /// Database must be empty but doesn't have to be registered during script-execution.
    /// </summary>
    public string GenerateNoDNDBTInfoPublishScript(Assembly dbAssembly);
    /// <summary>
    /// Generates sql script to create all defined DNDBT user objects.
    /// DNDBT system info changes will not be included.
    /// Database must be empty but doesn't have to be registered during script-execution.
    /// </summary>
    public string GenerateNoDNDBTInfoPublishScript(Database database);

    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state specified in second argument.
    /// DNDBT system info changes will not be included.
    /// Database doesn't have to be registered during script-execution.
    /// </summary>
    public string GenerateNoDNDBTInfoPublishScript(string newDbAssemblyPath, string oldDbAssemblyPath);
    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state specified in second argument.
    /// DNDBT system info changes will not be included.
    /// Database doesn't have to be registered during script-execution.
    /// </summary>
    public string GenerateNoDNDBTInfoPublishScript(Assembly newDbAssembly, Assembly oldDbAssembly);
    /// <summary>
    /// Generates sql script to update database to the state specified in first argument from the state specified in second argument.
    /// DNDBT system info changes will not be included.
    /// Database doesn't have to be registered during script-execution.
    /// </summary>
    public string GenerateNoDNDBTInfoPublishScript(Database newDatabase, Database oldDatabase);

    /// <summary>
    /// Generates dotnet project for a registerd or unregistered database.
    /// If database is unregistered IDs for all objects are generated randomly.
    /// </summary>
    public void GenerateDefinition(IDbConnection connection, string outputDirectory);

    /// <summary>
    /// Creates database model using DBMS and DNDBT system tables.
    /// </summary>
    public Database CreateDatabaseModelUsingDNDBTSysInfo(IDbConnection connection);
    /// <summary>
    /// Creates database model using DBMS system tables only.
    /// IDs for all objects are generated randomly.
    /// </summary>
    public Database CreateDatabaseModelUsingDBMSSysInfo(IDbConnection connection);
}
