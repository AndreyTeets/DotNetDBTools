using System;
using System.Data.Common;
using System.IO;
using System.Reflection;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core
{
    public abstract class DeployManager : IDeployManager
    {
        private readonly DeployOptions _options;
        private readonly IDbModelConverter _dbModelConverter;
        private readonly IFactory _factory;

        private protected DeployManager(
            DeployOptions options,
            IDbModelConverter dbModelConverter,
            IFactory factory)
        {
            _options = options;
            _dbModelConverter = dbModelConverter;
            _factory = factory;
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
            DatabaseDiff databaseDiff = AnalysisHelper.CreateDatabaseDiff(newDatabase, oldDatabase);
            ValidateDatabaseDiff(databaseDiff);

            Interactor interactor = _factory.CreateInteractor(_factory.CreateQueryExecutor(connection));
            interactor.ApplyDatabaseDiff(databaseDiff);
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
            GeneratePublishScript(newDatabase, oldDatabase, outputPath);
        }

        public void GeneratePublishScript(string dbAssemblyPath, string outputPath)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            GeneratePublishScript(dbAssembly, outputPath);
        }

        public void GeneratePublishScript(Assembly dbAssembly, string outputPath)
        {
            Database newDatabase = CreateDatabaseModelFromDbAssembly(dbAssembly);
            Database oldDatabase = CreateEmptyDatabaseModel();
            GeneratePublishScript(newDatabase, oldDatabase, outputPath);
        }
        protected abstract Database CreateEmptyDatabaseModel();

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
            GeneratePublishScript(newDatabase, oldDatabase, outputPath);
        }

        public void RegisterAsDNDBT(DbConnection connection)
        {
            Interactor interactor = _factory.CreateInteractor(_factory.CreateQueryExecutor(connection));
            if (interactor.DNDBTSysTablesExist())
                throw new InvalidOperationException("Database is already registered");
            Database oldDatabase = interactor.GenerateDatabaseModelFromDBMSSysInfo();
            interactor.CreateDNDBTSysTables();
            interactor.PopulateDNDBTSysTables(oldDatabase);
        }

        public void UnregisterAsDNDBT(DbConnection connection)
        {
            Interactor interactor = _factory.CreateInteractor(_factory.CreateQueryExecutor(connection));
            interactor.DropDNDBTSysTables();
        }

        public void GenerateDefinition(DbConnection connection, string outputDirectory)
        {
            Interactor interactor = _factory.CreateInteractor(_factory.CreateQueryExecutor(connection));
            Database database;
            if (interactor.DNDBTSysTablesExist())
                database = interactor.GetDatabaseModelFromDNDBTSysInfo();
            else
                database = interactor.GenerateDatabaseModelFromDBMSSysInfo();
            DbDefinitionGenerator.GenerateDefinition(database, outputDirectory);
        }

        private void GeneratePublishScript(Database newDatabase, Database oldDatabase, string outputPath)
        {
            DatabaseDiff databaseDiff = AnalysisHelper.CreateDatabaseDiff(newDatabase, oldDatabase);
            ValidateDatabaseDiff(databaseDiff);

            IGenSqlScriptQueryExecutor genSqlScriptQueryExecutor = _factory.CreateGenSqlScriptQueryExecutor();
            Interactor interactor = _factory.CreateInteractor(genSqlScriptQueryExecutor);
            interactor.ApplyDatabaseDiff(databaseDiff);
            string generatedScript = genSqlScriptQueryExecutor.GetFinalScript();

            string fullPath = Path.GetFullPath(outputPath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllText(fullPath, generatedScript);
        }

        private Database CreateDatabaseModelFromDbAssembly(Assembly dbAssembly)
        {
            Database database = DbDefinitionParser.CreateDatabaseModel(dbAssembly);
            if (database.Kind == DatabaseKind.Agnostic)
                database = _dbModelConverter.FromAgnostic(database);

            if (!AnalysisHelper.DbIsValid(database, out DbError error))
                throw new Exception($"Db is invalid: {error}");
            return database;
        }

        private Database GetDatabaseModelFromRegisteredDb(DbConnection connection)
        {
            Interactor interactor = _factory.CreateInteractor(_factory.CreateQueryExecutor(connection));
            if (interactor.DNDBTSysTablesExist())
                return interactor.GetDatabaseModelFromDNDBTSysInfo();
            else
                throw new InvalidOperationException("Database is not registered");
        }

        private void ValidateDatabaseDiff(DatabaseDiff databaseDiff)
        {
            if (!_options.AllowDataLoss && AnalysisHelper.LeadsToDataLoss(databaseDiff))
                throw new Exception("Update would lead to data loss and it's not allowed.");
        }
    }
}
