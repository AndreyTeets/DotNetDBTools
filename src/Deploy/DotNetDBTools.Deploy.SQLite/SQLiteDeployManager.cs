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
        public void UpdateDatabase(string dbAssemblyPath)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            UpdateDatabase(dbAssembly);
        }

        public void UpdateDatabase(Assembly dbAssembly)
        {
            GetNewAndOldDatabasesInfos(dbAssembly, out SQLiteDatabaseInfo database, out SQLiteDatabaseInfo existingDatabase);
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor());
            interactor.UpdateDatabase(database, existingDatabase);
        }

        public string GenerateUpdateScript(Assembly dbAssembly)
        {
            GetNewAndOldDatabasesInfos(dbAssembly, out SQLiteDatabaseInfo database, out SQLiteDatabaseInfo existingDatabase);
            return GenerateUpdateScript(database, existingDatabase);
        }

        public string GenerateUpdateScript(SQLiteDatabaseInfo database, SQLiteDatabaseInfo existingDatabase)
        {
            SQLiteGenSqlScriptQueryExecutor genSqlScriptQueryExecutor = new();
            SQLiteInteractor interactor = new(genSqlScriptQueryExecutor);
            interactor.UpdateDatabase(database, existingDatabase);
            return genSqlScriptQueryExecutor.GetFinalScript();
        }

        private static void GetNewAndOldDatabasesInfos(Assembly dbAssembly, out SQLiteDatabaseInfo database, out SQLiteDatabaseInfo existingDatabase)
        {
            database = CreateSQLiteDatabaseInfo(dbAssembly);
            existingDatabase = GetExistingDatabase();
            if (!SQLiteDbValidator.CanUpdate(database, existingDatabase, false, out string error))
                throw new Exception($"Can not update database: {error}");
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

        private static SQLiteDatabaseInfo GetExistingDatabase()
        {
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor());
            SQLiteDatabaseInfo existingDatabase = interactor.GetExistingDatabase();
            return existingDatabase;
        }
    }
}
