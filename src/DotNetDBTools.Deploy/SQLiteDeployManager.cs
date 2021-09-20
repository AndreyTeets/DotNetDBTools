using System;
using System.IO;
using System.Reflection;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.DefinitionGenerator;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.DefinitionParser.Core;
using DotNetDBTools.DefinitionParser.SQLite;
using DotNetDBTools.Deploy.SQLite;
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
            SQLiteDatabaseInfo database = CreateSQLiteDatabaseInfo(dbAssembly);
            SQLiteDatabaseInfo existingDatabase = GetExistingDatabaseOrCreateEmptyIfNotExists(connectionString);

            if (!SQLiteDbValidator.CanUpdate(database, existingDatabase, _allowDataLoss, out string error))
                throw new Exception($"Can not update database: {error}");

            SQLiteDatabaseDiff databaseDiff = SQLiteDiffCreator.CreateDatabaseDiff(database, existingDatabase);
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            interactor.UpdateDatabase(databaseDiff);
        }

        public void GeneratePublishScript(string dbAssemblyPath, string connectionString, string outputPath)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            GeneratePublishScript(dbAssembly, connectionString, outputPath);
        }

        public void GeneratePublishScript(Assembly dbAssembly, string connectionString, string outputPath)
        {
            SQLiteDatabaseInfo database = CreateSQLiteDatabaseInfo(dbAssembly);
            SQLiteDatabaseInfo existingDatabase = GetExistingDatabase(connectionString);
            GeneratePublishScript(database, existingDatabase, outputPath);
        }

        public void GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath)
        {
            SQLiteDatabaseInfo database = CreateSQLiteDatabaseInfo(newDbAssembly);
            SQLiteDatabaseInfo existingDatabase = CreateSQLiteDatabaseInfo(oldDbAssembly);
            GeneratePublishScript(database, existingDatabase, outputPath);
        }

        public void GenerateDefinition(string connectionString, string outputDirectory)
        {
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            SQLiteDatabaseInfo existingDatabase = interactor.GetExistingDatabase();
            DbDefinitionGenerator.GenerateDefinition(existingDatabase, outputDirectory);
        }

        public void RegisterAsDNDBT(string connectionString)
        {
            return;
#pragma warning disable CS0162 // Unreachable code detected
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
#pragma warning restore CS0162 // Unreachable code detected
            interactor.CreateSystemTables();
        }

        public void UnregisterAsDNDBT(string connectionString)
        {
            return;
#pragma warning disable CS0162 // Unreachable code detected
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
#pragma warning restore CS0162 // Unreachable code detected
            interactor.DeleteSystemTables();
        }

        public void GeneratePublishScript(SQLiteDatabaseInfo database, SQLiteDatabaseInfo existingDatabase, string outputPath)
        {
            SQLiteGenSqlScriptQueryExecutor genSqlScriptQueryExecutor = new();
            SQLiteInteractor interactor = new(genSqlScriptQueryExecutor);
            SQLiteDatabaseDiff databaseDiff = SQLiteDiffCreator.CreateDatabaseDiff(database, existingDatabase);
            interactor.UpdateDatabase(databaseDiff);
            string generatedScript = genSqlScriptQueryExecutor.GetFinalScript();

            string fullPath = Path.GetFullPath(outputPath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllText(fullPath, generatedScript);
        }

        private static SQLiteDatabaseInfo CreateSQLiteDatabaseInfo(Assembly dbAssembly)
        {
            SQLiteDatabaseInfo database;
            if (DbAssemblyInfoHelper.GetDbKind(dbAssembly) == DatabaseKind.Agnostic)
                database = AgnosticToSQLiteConverter.ConvertToSQLiteInfo(AgnosticDefinitionParser.CreateDatabaseInfo(dbAssembly));
            else
                database = SQLiteDefinitionParser.CreateDatabaseInfo(dbAssembly);

            if (!SQLiteDbValidator.DbIsValid(database, out string error))
                throw new Exception($"Db is invalid: {error}");
            return database;
        }

        private SQLiteDatabaseInfo GetExistingDatabaseOrCreateEmptyIfNotExists(string connectionString)
        {
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            bool databaseExists = interactor.DatabaseExists();
            if (databaseExists)
            {
                SQLiteDatabaseInfo existingDatabase = interactor.GetExistingDatabase();
                return existingDatabase;
            }
            else
            {
                if (_allowDbCreation)
                {
                    interactor.CreateSystemTables();
                    return new SQLiteDatabaseInfo(null);
                }
                else
                {
                    throw new Exception($"Database doesn't exist and it's creation is not allowed");
                }
            }
        }

        private static SQLiteDatabaseInfo GetExistingDatabase(string connectionString)
        {
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            bool databaseExists = interactor.DatabaseExists();
            if (databaseExists)
                return interactor.GetExistingDatabase();
            else
                return new SQLiteDatabaseInfo(null);
        }
    }
}
