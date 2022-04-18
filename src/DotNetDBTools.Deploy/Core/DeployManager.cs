using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core;

public abstract class DeployManager<TDatabase> : IDeployManager
    where TDatabase : Database, new()
{
    public DeployOptions Options { get; set; }
    public Events Events { get; } = new();

    private readonly IFactory _factory;
    private readonly IAnalysisManager _analysisManager;
    private readonly IDefinitionParsingManager _definitionParsingManager;

    private protected DeployManager(
        DeployOptions options,
        IFactory factory)
    {
        Options = options;
        _factory = factory;
        _analysisManager = new AnalysisManager();
        _definitionParsingManager = new DefinitionParsingManager();
    }

    public bool IsRegisteredAsDNDBT(IDbConnection connection)
    {
        Events.InvokeEventFired(EventType.IsRegisteredBegan);
        IQueryExecutor queryExecutor = _factory.CreateQueryExecutor(connection, Events);
        IDbEditor dbEditor = _factory.CreateDbEditor(queryExecutor);
        bool isRegistered = dbEditor.DNDBTSysTablesExist();
        Events.InvokeEventFired(EventType.IsRegisteredFinished);
        return isRegistered;
    }
    public void RegisterAsDNDBT(IDbConnection connection)
    {
        Database dbWithDNDBTInfo = CreateDbModelFromDBMS(connection, ExpectedRegistrationState.Unregistered, useDNDBTSysInfoIfAvailable: null);
        RegisterAsDNDBTImpl(connection, dbWithDNDBTInfo);
    }
    public void RegisterAsDNDBT(IDbConnection connection, string dbWithDNDBTInfoAssemblyPath)
    {
        Assembly dbAssembly = _definitionParsingManager.LoadDbAssembly(dbWithDNDBTInfoAssemblyPath);
        RegisterAsDNDBT(connection, dbAssembly);
    }
    public void RegisterAsDNDBT(IDbConnection connection, Assembly dbWithDNDBTInfoAssembly)
    {
        Database dbWithDNDBTInfo = CreateDbModelFromDefinition(dbWithDNDBTInfoAssembly);
        RegisterAsDNDBT(connection, dbWithDNDBTInfo);
    }
    public void RegisterAsDNDBT(IDbConnection connection, Database dbWithDNDBTInfo)
    {
        Database actualDb = CreateDbModelFromDBMS(connection, ExpectedRegistrationState.Unregistered, useDNDBTSysInfoIfAvailable: null);
        if (!_analysisManager.DatabasesAreEquivalentExcludingDNDBTInfo(actualDb, dbWithDNDBTInfo, out string diffLog))
            throw new Exception($"Actual database differs from the one provided for DNDBTInfo. DiffLog:\n{diffLog}");
        RegisterAsDNDBTImpl(connection, dbWithDNDBTInfo);
    }
    public void UnregisterAsDNDBT(IDbConnection connection)
    {
        Events.InvokeEventFired(EventType.UnregisterBegan);
        IDbEditor dbEditor = _factory.CreateDbEditor(_factory.CreateQueryExecutor(connection, Events));
        dbEditor.DropDNDBTSysTables();
        Events.InvokeEventFired(EventType.UnregisterFinished);
    }

    public void PublishDatabase(string dbAssemblyPath, IDbConnection connection)
    {
        Assembly dbAssembly = _definitionParsingManager.LoadDbAssembly(dbAssemblyPath);
        PublishDatabase(dbAssembly, connection);
    }
    public void PublishDatabase(Assembly dbAssembly, IDbConnection connection)
    {
        Database newDatabase = CreateDbModelFromDefinition(dbAssembly);
        PublishDatabase(newDatabase, connection);
    }
    public void PublishDatabase(Database database, IDbConnection connection)
    {
        Database oldDatabase = CreateDbModelFromDBMS(connection, ExpectedRegistrationState.Registered, useDNDBTSysInfoIfAvailable: true);
        Events.InvokeEventFired(EventType.PublishBegan);
        DatabaseDiff dbDiff = CreateDbDiff(database, oldDatabase);
        ApplyDbDiff(dbDiff, _factory.CreateQueryExecutor(connection, Events));
        Events.InvokeEventFired(EventType.PublishFinished);
    }

    public string GeneratePublishScript(string dbAssemblyPath, IDbConnection connection)
    {
        Assembly newDbAssembly = _definitionParsingManager.LoadDbAssembly(dbAssemblyPath);
        return GeneratePublishScript(newDbAssembly, connection);
    }
    public string GeneratePublishScript(Assembly dbAssembly, IDbConnection connection)
    {
        Database newDatabase = CreateDbModelFromDefinition(dbAssembly);
        return GeneratePublishScript(newDatabase, connection);
    }
    public string GeneratePublishScript(Database database, IDbConnection connection)
    {
        Database oldDatabase = CreateDbModelFromDBMS(connection, ExpectedRegistrationState.Registered, useDNDBTSysInfoIfAvailable: true);
        return GeneratePublishScriptImpl(database, oldDatabase, noDNDBTInfo: false);
    }

    public string GeneratePublishScript(string dbAssemblyPath)
    {
        Assembly dbAssembly = _definitionParsingManager.LoadDbAssembly(dbAssemblyPath);
        return GeneratePublishScript(dbAssembly);
    }
    public string GeneratePublishScript(Assembly dbAssembly)
    {
        Database newDatabase = CreateDbModelFromDefinition(dbAssembly);
        return GeneratePublishScript(newDatabase);
    }
    public string GeneratePublishScript(Database database)
    {
        Database oldDatabase = CreateEmptyDbModel();
        return GeneratePublishScriptImpl(database, oldDatabase, noDNDBTInfo: false);
    }

    public string GeneratePublishScript(string newDbAssemblyPath, string oldDbAssemblyPath)
    {
        Assembly newDbAssembly = _definitionParsingManager.LoadDbAssembly(newDbAssemblyPath);
        Assembly oldDbAssembly = _definitionParsingManager.LoadDbAssembly(oldDbAssemblyPath);
        return GeneratePublishScript(newDbAssembly, oldDbAssembly);
    }
    public string GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly)
    {
        Database newDatabase = CreateDbModelFromDefinition(newDbAssembly);
        Database oldDatabase = CreateDbModelFromDefinition(oldDbAssembly);
        return GeneratePublishScript(newDatabase, oldDatabase);
    }
    public string GeneratePublishScript(Database newDatabase, Database oldDatabase)
    {
        return GeneratePublishScriptImpl(newDatabase, oldDatabase, noDNDBTInfo: false);
    }

    public string GenerateNoDNDBTInfoPublishScript(string dbAssemblyPath)
    {
        Assembly dbAssembly = _definitionParsingManager.LoadDbAssembly(dbAssemblyPath);
        return GenerateNoDNDBTInfoPublishScript(dbAssembly);
    }
    public string GenerateNoDNDBTInfoPublishScript(Assembly dbAssembly)
    {
        Database newDatabase = CreateDbModelFromDefinition(dbAssembly);
        return GenerateNoDNDBTInfoPublishScript(newDatabase);
    }
    public string GenerateNoDNDBTInfoPublishScript(Database database)
    {
        Database oldDatabase = CreateEmptyDbModel();
        return GeneratePublishScriptImpl(database, oldDatabase, noDNDBTInfo: true);
    }

    public string GenerateNoDNDBTInfoPublishScript(string newDbAssemblyPath, string oldDbAssemblyPath)
    {
        Assembly newDbAssembly = _definitionParsingManager.LoadDbAssembly(newDbAssemblyPath);
        Assembly oldDbAssembly = _definitionParsingManager.LoadDbAssembly(oldDbAssemblyPath);
        return GenerateNoDNDBTInfoPublishScript(newDbAssembly, oldDbAssembly);
    }
    public string GenerateNoDNDBTInfoPublishScript(Assembly newDbAssembly, Assembly oldDbAssembly)
    {
        Database newDatabase = CreateDbModelFromDefinition(newDbAssembly);
        Database oldDatabase = CreateDbModelFromDefinition(oldDbAssembly);
        return GenerateNoDNDBTInfoPublishScript(newDatabase, oldDatabase);
    }
    public string GenerateNoDNDBTInfoPublishScript(Database newDatabase, Database oldDatabase)
    {
        return GeneratePublishScriptImpl(newDatabase, oldDatabase, noDNDBTInfo: true);
    }

    public void GenerateDefinition(IDbConnection connection, string outputDirectory)
    {
        Database database = CreateDbModelFromDBMS(connection, ExpectedRegistrationState.Any, useDNDBTSysInfoIfAvailable: true);
        Events.InvokeEventFired(EventType.GenerateDefinitionBegan);
        new GenerationManager().GenerateDefinition(database, outputDirectory);
        Events.InvokeEventFired(EventType.GenerateDefinitionFinished);
    }

    public Database CreateDatabaseModelUsingDNDBTSysInfo(IDbConnection connection)
    {
        return CreateDbModelFromDBMS(connection, ExpectedRegistrationState.Registered, useDNDBTSysInfoIfAvailable: true);
    }
    public Database CreateDatabaseModelUsingDBMSSysInfo(IDbConnection connection)
    {
        return CreateDbModelFromDBMS(connection, ExpectedRegistrationState.Any, useDNDBTSysInfoIfAvailable: false);
    }

    private void RegisterAsDNDBTImpl(IDbConnection connection, Database dbWithDNDBTInfo)
    {
        Events.InvokeEventFired(EventType.RegisterBegan);
        IQueryExecutor queryExecutor = _factory.CreateQueryExecutor(connection, Events);
        IDbEditor dbEditor = _factory.CreateDbEditor(queryExecutor);
        dbEditor.CreateDNDBTSysTables();
        dbEditor.PopulateDNDBTSysTables(dbWithDNDBTInfo);
        Events.InvokeEventFired(EventType.RegisterFinished);
    }

    private string GeneratePublishScriptImpl(Database newDatabase, Database oldDatabase, bool noDNDBTInfo)
    {
        Events.InvokeEventFired(EventType.GeneratePublishScriptBegan);
        DatabaseDiff dbDiff = CreateDbDiff(newDatabase, oldDatabase);

        IGenSqlScriptQueryExecutor genSqlScriptQueryExecutor = _factory.CreateGenSqlScriptQueryExecutor();
        genSqlScriptQueryExecutor.NoDNDBTInfo = noDNDBTInfo;

        ApplyDbDiff(dbDiff, genSqlScriptQueryExecutor);
        string script = genSqlScriptQueryExecutor.GetFinalScript();
        Events.InvokeEventFired(EventType.GeneratePublishScriptFinished);
        return script;
    }

    private Database CreateEmptyDbModel()
    {
        TDatabase database = new();
        database.InitializeProperties();
        return database;
    }

    private Database CreateDbModelFromDefinition(Assembly dbAssembly)
    {
        Events.InvokeEventFired(EventType.CreateDbModelFromDefinitionBegan);
        Database database = _definitionParsingManager.CreateDbModel(dbAssembly);
        if (database.Kind == DatabaseKind.Agnostic)
            database = _analysisManager.ConvertFromAgnostic(database, _factory.GetDatabaseKind());

        if (!_analysisManager.DbIsValid(database, out List<DbError> dbErrors))
            throw new Exception($"Db is invalid:\n{string.Join("\n", dbErrors.Select(x => x.ErrorMessage))}");

        Events.InvokeEventFired(EventType.CreateDbModelFromDefinitionFinished);
        return database;
    }

    private Database CreateDbModelFromDBMS(
        IDbConnection connection,
        ExpectedRegistrationState expectedRegistrationState,
        bool? useDNDBTSysInfoIfAvailable)
    {
        bool isRegistered = IsRegisteredAsDNDBT(connection);
        if (expectedRegistrationState == ExpectedRegistrationState.Registered && !isRegistered)
            throw new InvalidOperationException("Database is expected to be registered but it's unregistered");
        if (expectedRegistrationState == ExpectedRegistrationState.Unregistered && isRegistered)
            throw new InvalidOperationException("Database is expected to be unregistered but it's registered");

        Events.InvokeEventFired(EventType.CreateDbModelFromDBMSBegan);
        IQueryExecutor queryExecutor = _factory.CreateQueryExecutor(connection, Events);
        IDbModelFromDBMSProvider dbModelFromDBMSProvider = _factory.CreateDbModelFromDBMSProvider(queryExecutor);
        Database database = isRegistered && useDNDBTSysInfoIfAvailable.Value
            ? dbModelFromDBMSProvider.CreateDbModelUsingDNDBTSysInfo()
            : dbModelFromDBMSProvider.CreateDbModelUsingDBMSSysInfo();
        Events.InvokeEventFired(EventType.CreateDbModelFromDBMSFinished);
        return database;
    }

    private DatabaseDiff CreateDbDiff(Database newDatabase, Database oldDatabase)
    {
        Events.InvokeEventFired(EventType.CreateDbDiffBegan);
        DatabaseDiff dbDiff = _analysisManager.CreateDatabaseDiff(newDatabase, oldDatabase);
        ValidateDbDiff(dbDiff);
        Events.InvokeEventFired(EventType.CreateDbDiffFinished);
        return dbDiff;
    }

    private void ValidateDbDiff(DatabaseDiff dbDiff)
    {
        if (!Options.AllowDataLoss && _analysisManager.DiffLeadsToDataLoss(dbDiff))
        {
            throw new Exception(
"Update would lead to data loss and it's not allowed. This check can be disabled in DeployOptions.");
        }

        if (!Options.AllowUnchangedDbVersionForNonEmptyDbDiff &&
            !_analysisManager.DiffIsEmpty(dbDiff) && dbDiff.NewDatabase.Version == dbDiff.OldDatabase.Version)
        {
            throw new Exception(
"New and old databases are different but their versions are the same. This check can be disabled in DeployOptions.");
        }
    }

    private void ApplyDbDiff(DatabaseDiff dbDiff, IQueryExecutor queryExecutor)
    {
        Events.InvokeEventFired(EventType.ApplyDbDiffBegan);
        IDbEditor dbEditor = _factory.CreateDbEditor(queryExecutor);
        dbEditor.ApplyDatabaseDiff(dbDiff, Options);
        Events.InvokeEventFired(EventType.ApplyDbDiffFinished);
    }

    private enum ExpectedRegistrationState
    {
        Any,
        Registered,
        Unregistered,
    }
}
