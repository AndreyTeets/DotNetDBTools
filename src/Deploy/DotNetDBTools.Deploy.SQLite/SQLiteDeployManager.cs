using System;
using System.Reflection;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.DefinitionParser;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.DefinitionParser.SQLite;
using DotNetDBTools.DeployInteractor.SQLite;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite
{
    public class SQLiteDeployManager
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

            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            SQLiteDatabaseInfo existingDatabase = GetExistingDatabaseOrCreateEmptyIfNotExists(interactor);

            if (!SQLiteDbValidator.CanUpdate(database, existingDatabase, _allowDataLoss, out string error))
                throw new Exception($"Can not update database: {error}");

            SQLiteDatabaseDiff databaseDiff = SQLiteDiffCreator.CreateDatabaseDiff(database, existingDatabase);
            interactor.UpdateDatabase(databaseDiff);
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
            if (DeployCommonFunctions.IsAgnosticDb(dbAssembly))
                database = AgnosticToSQLiteConverter.ConvertToSQLiteInfo(AgnosticDefinitionParser.CreateDatabaseInfo(dbAssembly));
            else
                database = SQLiteDefinitionParser.CreateDatabaseInfo(dbAssembly);

            if (!SQLiteDbValidator.DbIsValid(database, out string error))
                throw new Exception($"Db is invalid: {error}");
            return database;
        }

        private SQLiteDatabaseInfo GetExistingDatabaseOrCreateEmptyIfNotExists(SQLiteInteractor interactor)
        {
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
                    interactor.CreateEmptyDatabase();
                    return new SQLiteDatabaseInfo();
                }
                else
                {
                    throw new Exception($"Database doesn't exist and it's creation is not allowed");
                }
            }
        }
    }
}
