using System;
using System.IO;
using System.Reflection;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy.Core.Factories;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core
{
    public abstract class DeployManager : IDeployManager
    {
        protected readonly bool AllowDbCreation; // TODO DeployOptions
        protected readonly bool AllowDataLoss;
        protected readonly IDbModelConverter DbModelConverter;
        protected readonly IQueryExecutorFactory QueryExecutorFactory;
        protected readonly IGenSqlScriptQueryExecutorFactory GenSqlScriptQueryExecutorFactory;
        protected readonly IInteractorFactory InteractorFactory;

        protected DeployManager(
            bool allowDbCreation,
            bool allowDataLoss,
            IDbModelConverter dbModelConverter,
            IQueryExecutorFactory queryExecutorFactory,
            IGenSqlScriptQueryExecutorFactory genSqlScriptQueryExecutorFactory,
            IInteractorFactory interactorFactory)
        {
            AllowDbCreation = allowDbCreation;
            AllowDataLoss = allowDataLoss;
            DbModelConverter = dbModelConverter;
            QueryExecutorFactory = queryExecutorFactory;
            GenSqlScriptQueryExecutorFactory = genSqlScriptQueryExecutorFactory;
            InteractorFactory = interactorFactory;
        }

        public void PublishDatabase(string dbAssemblyPath, string connectionString)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            PublishDatabase(dbAssembly, connectionString);
        }

        public void PublishDatabase(Assembly dbAssembly, string connectionString)
        {
            DatabaseInfo newDatabase = CreateDatabaseModelFromDbAssembly(dbAssembly);
            DatabaseInfo oldDatabase = GetDatabaseModelIfDbExistsOrCreateEmptyDbAndModel(connectionString);
            DatabaseDiff databaseDiff = AnalysisHelper.CreateDatabaseDiff(newDatabase, oldDatabase);

            if (!AllowDataLoss && AnalysisHelper.LeadsToDataLoss(databaseDiff))
                throw new Exception("Update would lead to data loss and it's not allowed.");

            Interactor interactor = InteractorFactory.Create(QueryExecutorFactory.Create(connectionString));
            interactor.ApplyDatabaseDiff(databaseDiff);
        }

        public void GeneratePublishScript(string newDbAssemblyPath, string oldDbConnectionString, string outputPath)
        {
            Assembly newDbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(newDbAssemblyPath);
            GeneratePublishScript(newDbAssembly, oldDbConnectionString, outputPath);
        }

        public void GeneratePublishScript(Assembly newDbAssembly, string oldDbConnectionString, string outputPath)
        {
            DatabaseInfo newDatabase = CreateDatabaseModelFromDbAssembly(newDbAssembly);
            DatabaseInfo oldDatabase = GetDatabaseModelIfDbExistsAndRegisteredOrCreateEmptyModel(oldDbConnectionString);
            GeneratePublishScript(newDatabase, oldDatabase, outputPath);
        }

        public void GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath)
        {
            DatabaseInfo newDatabase = CreateDatabaseModelFromDbAssembly(newDbAssembly);
            DatabaseInfo oldDatabase = CreateDatabaseModelFromDbAssembly(oldDbAssembly);
            GeneratePublishScript(newDatabase, oldDatabase, outputPath);
        }

        public void GeneratePublishScript(string dbAssemblyPath, string outputPath)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            GeneratePublishScript(dbAssembly, outputPath);
        }

        public void GeneratePublishScript(Assembly dbAssembly, string outputPath)
        {
            DatabaseInfo newDatabase = CreateDatabaseModelFromDbAssembly(dbAssembly);
            DatabaseInfo oldDatabase = CreateEmptyDatabaseModel();
            GeneratePublishScript(newDatabase, oldDatabase, outputPath);
        }

        public void RegisterAsDNDBT(string connectionString)
        {
            Interactor interactor = InteractorFactory.Create(QueryExecutorFactory.Create(connectionString));
            if (interactor.DNDBTSysTablesExist())
                throw new InvalidOperationException("Database is already registered");
            DatabaseInfo oldDatabase = interactor.GenerateDatabaseModelFromDBMSSysInfo();
            interactor.CreateDNDBTSysTables();
            interactor.PopulateDNDBTSysTables(oldDatabase);
        }

        public void UnregisterAsDNDBT(string connectionString)
        {
            Interactor interactor = InteractorFactory.Create(QueryExecutorFactory.Create(connectionString));
            interactor.DropDNDBTSysTables();
        }

        public void GenerateDefinition(string connectionString, string outputDirectory)
        {
            Interactor interactor = InteractorFactory.Create(QueryExecutorFactory.Create(connectionString));
            DatabaseInfo oldDatabase;
            if (interactor.DNDBTSysTablesExist())
                oldDatabase = interactor.GetDatabaseModelFromDNDBTSysInfo();
            else
                oldDatabase = interactor.GenerateDatabaseModelFromDBMSSysInfo();
            DbDefinitionGenerator.GenerateDefinition(oldDatabase, outputDirectory);
        }

        protected abstract DatabaseInfo GetDatabaseModelIfDbExistsOrCreateEmptyDbAndModel(string connectionString);
        protected abstract DatabaseInfo GetDatabaseModelIfDbExistsAndRegisteredOrCreateEmptyModel(string connectionString);
        protected abstract DatabaseInfo CreateEmptyDatabaseModel();

        private void GeneratePublishScript(DatabaseInfo newDatabase, DatabaseInfo oldDatabase, string outputPath)
        {
            IGenSqlScriptQueryExecutor genSqlScriptQueryExecutor = GenSqlScriptQueryExecutorFactory.Create();
            Interactor interactor = InteractorFactory.Create(genSqlScriptQueryExecutor);
            DatabaseDiff databaseDiff = AnalysisHelper.CreateDatabaseDiff(newDatabase, oldDatabase);
            interactor.ApplyDatabaseDiff(databaseDiff);
            string generatedScript = genSqlScriptQueryExecutor.GetFinalScript();

            string fullPath = Path.GetFullPath(outputPath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllText(fullPath, generatedScript);
        }

        private DatabaseInfo CreateDatabaseModelFromDbAssembly(Assembly dbAssembly)
        {
            DatabaseInfo database = DbDefinitionParser.CreateDatabaseInfo(dbAssembly);
            if (database.Kind == DatabaseKind.Agnostic)
                database = DbModelConverter.FromAgnostic(database);

            if (!AnalysisHelper.DbIsValid(database, out DbError error))
                throw new Exception($"Db is invalid: {error}");
            return database;
        }
    }
}
