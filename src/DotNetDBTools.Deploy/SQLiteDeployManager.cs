using System;
using System.IO;
using System.Reflection;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.DefinitionGenerator;
using DotNetDBTools.DefinitionParser;
using DotNetDBTools.DefinitionParser.Core;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy
{
    public class SQLiteDeployManager : IDeployManager
    {
        private readonly bool _allowDbCreation; // TODO DeployOptions
        private readonly bool _allowDataLoss;

        public SQLiteDeployManager(bool allowDbCreation = default, bool allowDataLoss = default)
        {
            _allowDbCreation = allowDbCreation;
            _allowDataLoss = allowDataLoss;
        }

        public void PublishDatabase(string dbAssemblyPath, string connectionString)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            PublishDatabase(dbAssembly, connectionString);
        }

        public void PublishDatabase(Assembly dbAssembly, string connectionString)
        {
            SQLiteDatabaseInfo database = CreateSQLiteDatabaseModelFromDbAssembly(dbAssembly);
            SQLiteDatabaseInfo existingDatabase = GetDatabaseModelIfDbExistsOrCreateEmptyDbAndModel(connectionString);

            if (!SQLiteDbValidator.CanUpdate(database, existingDatabase, _allowDataLoss, out string error))
                throw new Exception($"Can not update database: {error}");

            SQLiteDatabaseDiff databaseDiff = SQLiteDiffCreator.CreateDatabaseDiff(database, existingDatabase);
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            interactor.ApplyDatabaseDiff(databaseDiff);
        }

        public void GeneratePublishScript(string dbAssemblyPath, string connectionString, string outputPath)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            GeneratePublishScript(dbAssembly, connectionString, outputPath);
        }

        public void GeneratePublishScript(Assembly dbAssembly, string connectionString, string outputPath)
        {
            SQLiteDatabaseInfo database = CreateSQLiteDatabaseModelFromDbAssembly(dbAssembly);
            SQLiteDatabaseInfo existingDatabase = GetDatabaseModelIfDbExistsAndRegisteredOrCreateEmptyModel(connectionString);
            GeneratePublishScript(database, existingDatabase, outputPath);
        }

        public void GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath)
        {
            SQLiteDatabaseInfo database = CreateSQLiteDatabaseModelFromDbAssembly(newDbAssembly);
            SQLiteDatabaseInfo existingDatabase = CreateSQLiteDatabaseModelFromDbAssembly(oldDbAssembly);
            GeneratePublishScript(database, existingDatabase, outputPath);
        }

        public void GenerateDefinition(string connectionString, string outputDirectory)
        {
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            SQLiteDatabaseInfo existingDatabase;
            if (interactor.DNDBTSysTablesExist())
                existingDatabase = interactor.GetDatabaseModelFromDNDBTSysInfo();
            else
                existingDatabase = interactor.GenerateDatabaseModelFromSQLiteSysInfo();
            DbDefinitionGenerator.GenerateDefinition(existingDatabase, outputDirectory);
        }

        public void RegisterAsDNDBT(string connectionString)
        {
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            if (interactor.DNDBTSysTablesExist())
                throw new InvalidOperationException("Database is already registered");
            SQLiteDatabaseInfo existingDatabase = interactor.GenerateDatabaseModelFromSQLiteSysInfo();
            interactor.CreateDNDBTSysTables();
            interactor.PopulateDNDBTSysTables(existingDatabase);
        }

        public void UnregisterAsDNDBT(string connectionString)
        {
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            interactor.DropDNDBTSysTables();
        }

        public void GeneratePublishScript(SQLiteDatabaseInfo database, SQLiteDatabaseInfo existingDatabase, string outputPath)
        {
            SQLiteGenSqlScriptQueryExecutor genSqlScriptQueryExecutor = new();
            SQLiteInteractor interactor = new(genSqlScriptQueryExecutor);
            SQLiteDatabaseDiff databaseDiff = SQLiteDiffCreator.CreateDatabaseDiff(database, existingDatabase);
            interactor.ApplyDatabaseDiff(databaseDiff);
            string generatedScript = genSqlScriptQueryExecutor.GetFinalScript();

            string fullPath = Path.GetFullPath(outputPath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllText(fullPath, generatedScript);
        }

        private static SQLiteDatabaseInfo CreateSQLiteDatabaseModelFromDbAssembly(Assembly dbAssembly)
        {
            SQLiteDatabaseInfo database;
            DatabaseInfo x = DbDefinitionParser.CreateDatabaseInfo(dbAssembly);
            if (x.Kind == DatabaseKind.Agnostic)
                database = AgnosticToSQLiteConverter.ConvertToSQLiteInfo((AgnosticDatabaseInfo)x);
            else
                database = (SQLiteDatabaseInfo)DbDefinitionParser.CreateDatabaseInfo(dbAssembly);

            if (!SQLiteDbValidator.DbIsValid(database, out DbError error))
                throw new Exception($"Db is invalid: {error}");
            return database;
        }

        private SQLiteDatabaseInfo GetDatabaseModelIfDbExistsOrCreateEmptyDbAndModel(string connectionString)
        {
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            bool databaseExists = interactor.DNDBTSysTablesExist();
            if (databaseExists)
            {
                SQLiteDatabaseInfo existingDatabase = interactor.GetDatabaseModelFromDNDBTSysInfo();
                return existingDatabase;
            }
            else
            {
                if (_allowDbCreation)
                {
                    interactor.CreateDNDBTSysTables();
                    return new SQLiteDatabaseInfo(null);
                }
                else
                {
                    throw new Exception($"Database doesn't exist and it's creation is not allowed");
                }
            }
        }

        private static SQLiteDatabaseInfo GetDatabaseModelIfDbExistsAndRegisteredOrCreateEmptyModel(string connectionString)
        {
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            if (interactor.DNDBTSysTablesExist())
                return interactor.GetDatabaseModelFromDNDBTSysInfo();
            return new SQLiteDatabaseInfo(null);
        }
    }
}
