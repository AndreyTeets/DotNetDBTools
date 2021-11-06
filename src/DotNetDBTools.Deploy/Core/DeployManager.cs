using System;
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
        protected readonly DeployOptions DeployOptions;
        protected readonly IDbModelConverter DbModelConverter;
        private protected readonly IFactory Factory;

        private protected DeployManager(
            DeployOptions deployOptions,
            IDbModelConverter dbModelConverter,
            IFactory factory)
        {
            DeployOptions = deployOptions;
            DbModelConverter = dbModelConverter;
            Factory = factory;
        }

        public void PublishDatabase(string dbAssemblyPath, string connectionString)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            PublishDatabase(dbAssembly, connectionString);
        }

        public void PublishDatabase(Assembly dbAssembly, string connectionString)
        {
            Database newDatabase = CreateDatabaseModelFromDbAssembly(dbAssembly);
            Database oldDatabase = GetDatabaseModelIfDbExistsOrCreateEmptyDbAndModel(connectionString);
            DatabaseDiff databaseDiff = AnalysisHelper.CreateDatabaseDiff(newDatabase, oldDatabase);

            if (!DeployOptions.AllowDataLoss && AnalysisHelper.LeadsToDataLoss(databaseDiff))
                throw new Exception("Update would lead to data loss and it's not allowed.");

            Interactor interactor = Factory.CreateInteractor(Factory.CreateQueryExecutor(connectionString));
            interactor.ApplyDatabaseDiff(databaseDiff);
        }

        public void GeneratePublishScript(string newDbAssemblyPath, string oldDbConnectionString, string outputPath)
        {
            Assembly newDbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(newDbAssemblyPath);
            GeneratePublishScript(newDbAssembly, oldDbConnectionString, outputPath);
        }

        public void GeneratePublishScript(Assembly newDbAssembly, string oldDbConnectionString, string outputPath)
        {
            Database newDatabase = CreateDatabaseModelFromDbAssembly(newDbAssembly);
            Database oldDatabase = GetDatabaseModelIfDbExistsAndRegisteredOrCreateEmptyModel(oldDbConnectionString);
            GeneratePublishScript(newDatabase, oldDatabase, outputPath);
        }

        public void GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath)
        {
            Database newDatabase = CreateDatabaseModelFromDbAssembly(newDbAssembly);
            Database oldDatabase = CreateDatabaseModelFromDbAssembly(oldDbAssembly);
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

        public void RegisterAsDNDBT(string connectionString)
        {
            Interactor interactor = Factory.CreateInteractor(Factory.CreateQueryExecutor(connectionString));
            if (interactor.DNDBTSysTablesExist())
                throw new InvalidOperationException("Database is already registered");
            Database oldDatabase = interactor.GenerateDatabaseModelFromDBMSSysInfo();
            interactor.CreateDNDBTSysTables();
            interactor.PopulateDNDBTSysTables(oldDatabase);
        }

        public void UnregisterAsDNDBT(string connectionString)
        {
            Interactor interactor = Factory.CreateInteractor(Factory.CreateQueryExecutor(connectionString));
            interactor.DropDNDBTSysTables();
        }

        public void GenerateDefinition(string connectionString, string outputDirectory)
        {
            Interactor interactor = Factory.CreateInteractor(Factory.CreateQueryExecutor(connectionString));
            Database oldDatabase;
            if (interactor.DNDBTSysTablesExist())
                oldDatabase = interactor.GetDatabaseModelFromDNDBTSysInfo();
            else
                oldDatabase = interactor.GenerateDatabaseModelFromDBMSSysInfo();
            DbDefinitionGenerator.GenerateDefinition(oldDatabase, outputDirectory);
        }

        protected abstract Database GetDatabaseModelIfDbExistsOrCreateEmptyDbAndModel(string connectionString);
        protected abstract Database GetDatabaseModelIfDbExistsAndRegisteredOrCreateEmptyModel(string connectionString);
        protected abstract Database CreateEmptyDatabaseModel();

        private void GeneratePublishScript(Database newDatabase, Database oldDatabase, string outputPath)
        {
            IGenSqlScriptQueryExecutor genSqlScriptQueryExecutor = Factory.CreateGenSqlScriptQueryExecutor();
            Interactor interactor = Factory.CreateInteractor(genSqlScriptQueryExecutor);
            DatabaseDiff databaseDiff = AnalysisHelper.CreateDatabaseDiff(newDatabase, oldDatabase);
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
                database = DbModelConverter.FromAgnostic(database);

            if (!AnalysisHelper.DbIsValid(database, out DbError error))
                throw new Exception($"Db is invalid: {error}");
            return database;
        }
    }
}
