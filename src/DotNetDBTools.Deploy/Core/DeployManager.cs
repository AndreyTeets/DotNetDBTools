using System;
using System.Data.Common;
using System.IO;
using System.Reflection;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Core;
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

    private readonly IFactory _factory;
    private readonly IDbModelConverter _dbModelConverter;

    private protected DeployManager(
        DeployOptions options,
        IFactory factory)
    {
        Options = options;
        _factory = factory;
        _dbModelConverter = _factory.CreateDbModelConverter();
    }

    public void RegisterAsDNDBT(DbConnection connection)
    {
        IQueryExecutor queryExecutor = _factory.CreateQueryExecutor(connection);
        IDbModelFromDBMSProvider dbModelFromDBMSProvider = _factory.CreateDbModelFromDBMSProvider(queryExecutor);

        Database dbWithDNDBTInfo = dbModelFromDBMSProvider.CreateDbModelUsingDBMSSysInfo();
        RegisterAsDNDBTImpl(connection, dbWithDNDBTInfo);
    }
    public void RegisterAsDNDBT(DbConnection connection, string dbWithDNDBTInfoAssemblyPath)
    {
        Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbWithDNDBTInfoAssemblyPath);
        RegisterAsDNDBT(connection, dbAssembly);
    }
    public void RegisterAsDNDBT(DbConnection connection, Assembly dbWithDNDBTInfoAssembly)
    {
        IQueryExecutor queryExecutor = _factory.CreateQueryExecutor(connection);
        IDbModelFromDBMSProvider dbModelFromDBMSProvider = _factory.CreateDbModelFromDBMSProvider(queryExecutor);

        Database actualDb = dbModelFromDBMSProvider.CreateDbModelUsingDBMSSysInfo();
        Database dbWithDNDBTInfo = CreateDatabaseModelFromDbAssembly(dbWithDNDBTInfoAssembly);

        if (!AnalysisHelper.DatabasesAreEquivalentExcludingDNDBTInfo(actualDb, dbWithDNDBTInfo, out string diffLog))
            throw new Exception($"Actual database differs from the one provided for DNDBTInfo. DiffLog:\n{diffLog}");
        RegisterAsDNDBTImpl(connection, dbWithDNDBTInfo);
    }
    public void UnregisterAsDNDBT(DbConnection connection)
    {
        IDbEditor dbEditor = _factory.CreateDbEditor(_factory.CreateQueryExecutor(connection));
        dbEditor.DropDNDBTSysTables();
    }

    public void PublishDatabase(string dbAssemblyPath, DbConnection connection)
    {
        Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
        PublishDatabase(dbAssembly, connection);
    }
    public void PublishDatabase(Assembly dbAssembly, DbConnection connection)
    {
        Database newDatabase = CreateDatabaseModelFromDbAssembly(dbAssembly);
        Database oldDatabase = GetDatabaseModelFromRegisteredDb(connection);
        DatabaseDiff dbDiff = AnalysisHelper.CreateDatabaseDiff(newDatabase, oldDatabase);
        ValidateDatabaseDiff(dbDiff);

        IDbEditor dbEditor = _factory.CreateDbEditor(_factory.CreateQueryExecutor(connection));
        dbEditor.ApplyDatabaseDiff(dbDiff, Options);
    }

    public void GeneratePublishScript(string dbAssemblyPath, DbConnection connection, string outputPath)
    {
        Assembly newDbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
        GeneratePublishScript(newDbAssembly, connection, outputPath);
    }
    public void GeneratePublishScript(Assembly dbAssembly, DbConnection connection, string outputPath)
    {
        Database newDatabase = CreateDatabaseModelFromDbAssembly(dbAssembly);
        Database oldDatabase = GetDatabaseModelFromRegisteredDb(connection);
        GeneratePublishScriptImpl(newDatabase, oldDatabase, outputPath, noDNDBTInfo: false);
    }

    public void GeneratePublishScript(string dbAssemblyPath, string outputPath)
    {
        Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
        GeneratePublishScript(dbAssembly, outputPath);
    }
    public void GeneratePublishScript(Assembly dbAssembly, string outputPath)
    {
        Database newDatabase = CreateDatabaseModelFromDbAssembly(dbAssembly);
        Database oldDatabase = new TDatabase();
        GeneratePublishScriptImpl(newDatabase, oldDatabase, outputPath, noDNDBTInfo: false);
    }

    public void GeneratePublishScript(string newDbAssemblyPath, string oldDbAssemblyPath, string outputPath)
    {
        Assembly newDbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(newDbAssemblyPath);
        Assembly oldDbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(oldDbAssemblyPath);
        GeneratePublishScript(newDbAssembly, oldDbAssembly, outputPath);
    }
    public void GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath)
    {
        Database newDatabase = CreateDatabaseModelFromDbAssembly(newDbAssembly);
        Database oldDatabase = CreateDatabaseModelFromDbAssembly(oldDbAssembly);
        GeneratePublishScriptImpl(newDatabase, oldDatabase, outputPath, noDNDBTInfo: false);
    }

    public void GenerateNoDNDBTInfoPublishScript(string dbAssemblyPath, string outputPath)
    {
        Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
        GenerateNoDNDBTInfoPublishScript(dbAssembly, outputPath);
    }
    public void GenerateNoDNDBTInfoPublishScript(Assembly dbAssembly, string outputPath)
    {
        Database newDatabase = CreateDatabaseModelFromDbAssembly(dbAssembly);
        Database oldDatabase = new TDatabase();
        GeneratePublishScriptImpl(newDatabase, oldDatabase, outputPath, noDNDBTInfo: true);
    }

    public void GenerateNoDNDBTInfoPublishScript(string newDbAssemblyPath, string oldDbAssemblyPath, string outputPath)
    {
        Assembly newDbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(newDbAssemblyPath);
        Assembly oldDbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(oldDbAssemblyPath);
        GenerateNoDNDBTInfoPublishScript(newDbAssembly, oldDbAssembly, outputPath);
    }
    public void GenerateNoDNDBTInfoPublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath)
    {
        Database newDatabase = CreateDatabaseModelFromDbAssembly(newDbAssembly);
        Database oldDatabase = CreateDatabaseModelFromDbAssembly(oldDbAssembly);
        GeneratePublishScriptImpl(newDatabase, oldDatabase, outputPath, noDNDBTInfo: true);
    }

    public void GenerateDefinition(DbConnection connection, string outputDirectory)
    {
        IQueryExecutor queryExecutor = _factory.CreateQueryExecutor(connection);
        IDbEditor dbEditor = _factory.CreateDbEditor(queryExecutor);
        IDbModelFromDBMSProvider dbModelFromDBMSProvider = _factory.CreateDbModelFromDBMSProvider(queryExecutor);

        Database database;
        if (dbEditor.DNDBTSysTablesExist())
            database = dbModelFromDBMSProvider.CreateDbModelUsingDNDBTSysInfo();
        else
            database = dbModelFromDBMSProvider.CreateDbModelUsingDBMSSysInfo();
        DbDefinitionGenerator.GenerateDefinition(database, outputDirectory);
    }

    private void RegisterAsDNDBTImpl(DbConnection connection, Database dbWithDNDBTInfo)
    {
        IQueryExecutor queryExecutor = _factory.CreateQueryExecutor(connection);
        IDbEditor dbEditor = _factory.CreateDbEditor(queryExecutor);

        if (dbEditor.DNDBTSysTablesExist())
            throw new InvalidOperationException("Database is already registered");
        dbEditor.CreateDNDBTSysTables();
        dbEditor.PopulateDNDBTSysTables(dbWithDNDBTInfo);
    }

    private void GeneratePublishScriptImpl(Database newDatabase, Database oldDatabase, string outputPath, bool noDNDBTInfo)
    {
        DatabaseDiff dbDiff = AnalysisHelper.CreateDatabaseDiff(newDatabase, oldDatabase);
        ValidateDatabaseDiff(dbDiff);

        IGenSqlScriptQueryExecutor genSqlScriptQueryExecutor = _factory.CreateGenSqlScriptQueryExecutor();
        genSqlScriptQueryExecutor.NoDNDBTInfo = noDNDBTInfo;

        IDbEditor dbEditor = _factory.CreateDbEditor(genSqlScriptQueryExecutor);
        dbEditor.ApplyDatabaseDiff(dbDiff, Options);
        string generatedScript = genSqlScriptQueryExecutor.GetFinalScript();

        string fullPath = Path.GetFullPath(outputPath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        File.WriteAllText(fullPath, generatedScript);
    }

    private Database CreateDatabaseModelFromDbAssembly(Assembly dbAssembly)
    {
        Database database = new GenericDbModelFromDefinitionProvider().CreateDbModel(dbAssembly);
        if (database.Kind == DatabaseKind.Agnostic)
            database = _dbModelConverter.FromAgnostic(database);

        if (!AnalysisHelper.DbIsValid(database, out DbError error))
            throw new Exception($"Db is invalid: {error}");
        return database;
    }

    private Database GetDatabaseModelFromRegisteredDb(DbConnection connection)
    {
        IQueryExecutor queryExecutor = _factory.CreateQueryExecutor(connection);
        IDbEditor dbEditor = _factory.CreateDbEditor(queryExecutor);
        IDbModelFromDBMSProvider dbModelFromDBMSProvider = _factory.CreateDbModelFromDBMSProvider(queryExecutor);

        if (dbEditor.DNDBTSysTablesExist())
            return dbModelFromDBMSProvider.CreateDbModelUsingDNDBTSysInfo();
        else
            throw new InvalidOperationException("Database is not registered");
    }

    private void ValidateDatabaseDiff(DatabaseDiff dbDiff)
    {
        if (!Options.AllowDataLoss && AnalysisHelper.LeadsToDataLoss(dbDiff))
            throw new Exception("Update would lead to data loss and it's not allowed.");
        if (!AnalysisHelper.DiffIsEmpty(dbDiff) && dbDiff.NewDatabase.Version == dbDiff.OldDatabase.Version)
            throw new Exception("New and old databases are different but their versions are the same.");
    }
}
