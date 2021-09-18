using System;
using System.Reflection;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.DefinitionParser.Common;
using DotNetDBTools.DefinitionParser.SQLite;
using DotNetDBTools.Models.Common;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite
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

        public void UpdateDatabase(string dbAssemblyPath, string connectionString)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            UpdateDatabase(dbAssembly, connectionString);
        }

        public void UpdateDatabase(Assembly dbAssembly, string connectionString)
        {
            SQLiteDatabaseInfo database = CreateSQLiteDatabaseInfo(dbAssembly);
            SQLiteDatabaseInfo existingDatabase = GetExistingDatabaseOrCreateEmptyIfNotExists(connectionString);

            if (!SQLiteDbValidator.CanUpdate(database, existingDatabase, _allowDataLoss, out string error))
                throw new Exception($"Can not update database: {error}");

            SQLiteDatabaseDiff databaseDiff = SQLiteDiffCreator.CreateDatabaseDiff(database, existingDatabase);
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            interactor.UpdateDatabase(databaseDiff);
        }

        public string GenerateUpdateScript(string dbAssemblyPath, string connectionString)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            return GenerateUpdateScript(dbAssembly, connectionString);
        }

        public string GenerateUpdateScript(Assembly dbAssembly, string connectionString)
        {
            SQLiteDatabaseInfo database = CreateSQLiteDatabaseInfo(dbAssembly);

            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            SQLiteDatabaseInfo existingDatabase = interactor.GetExistingDatabase();

            return GenerateUpdateScript(database, existingDatabase);
        }

        public string GenerateUpdateScript(SQLiteDatabaseInfo database, SQLiteDatabaseInfo existingDatabase)
        {
            SQLiteGenSqlScriptQueryExecutor genSqlScriptQueryExecutor = new();
            SQLiteInteractor interactor = new(genSqlScriptQueryExecutor);
            SQLiteDatabaseDiff databaseDiff = SQLiteDiffCreator.CreateDatabaseDiff(database, existingDatabase);
            interactor.UpdateDatabase(databaseDiff);
            return genSqlScriptQueryExecutor.GetFinalScript();
        }

        private static SQLiteDatabaseInfo CreateSQLiteDatabaseInfo(Assembly dbAssembly)
        {
            SQLiteDatabaseInfo database;
            if (DbAssemblyInfoHelper.GetDbType(dbAssembly) == DatabaseType.Agnostic)
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
    }
}
