using System;
using System.Reflection;
using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.DefinitionParser;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.DefinitionParser.MSSQL;
using DotNetDBTools.DeployInteractor.MSSQL;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL
{
    public class MSSQLDeployManager
    {
        public void UpdateDatabase(string dbAssemblyPath)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            UpdateDatabase(dbAssembly);
        }

        public void UpdateDatabase(Assembly dbAssembly)
        {
            GetNewAndOldDatabasesInfos(dbAssembly, out MSSQLDatabaseInfo database, out MSSQLDatabaseInfo existingDatabase);
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor());
            MSSQLDatabaseDiff databaseDiff = MSSQLDiffCreator.CreateDatabaseDiff(database, existingDatabase);
            interactor.UpdateDatabase(databaseDiff);
        }

        public string GenerateUpdateScript(Assembly dbAssembly)
        {
            GetNewAndOldDatabasesInfos(dbAssembly, out MSSQLDatabaseInfo database, out MSSQLDatabaseInfo existingDatabase);
            return GenerateUpdateScript(database, existingDatabase);
        }

        public string GenerateUpdateScript(MSSQLDatabaseInfo database, MSSQLDatabaseInfo existingDatabase)
        {
            MSSQLGenSqlScriptQueryExecutor genSqlScriptQueryExecutor = new();
            MSSQLInteractor interactor = new(genSqlScriptQueryExecutor);
            MSSQLDatabaseDiff databaseDiff = MSSQLDiffCreator.CreateDatabaseDiff(database, existingDatabase);
            interactor.UpdateDatabase(databaseDiff);
            return genSqlScriptQueryExecutor.GetFinalScript();
        }

        private static void GetNewAndOldDatabasesInfos(Assembly dbAssembly, out MSSQLDatabaseInfo database, out MSSQLDatabaseInfo existingDatabase)
        {
            database = CreateMSSQLDatabaseInfo(dbAssembly);
            existingDatabase = GetExistingDatabase();
            if (!MSSQLDbValidator.CanUpdate(database, existingDatabase, false, out string error))
                throw new Exception($"Can not update database: {error}");
        }

        private static MSSQLDatabaseInfo CreateMSSQLDatabaseInfo(Assembly dbAssembly)
        {
            MSSQLDatabaseInfo database;
            if (DeployCommonFunctions.IsAgnosticDb(dbAssembly))
                database = AgnosticToMSSQLConverter.ConvertToMSSQLInfo(AgnosticDefinitionParser.CreateDatabaseInfo(dbAssembly));
            else
                database = MSSQLDefinitionParser.CreateDatabaseInfo(dbAssembly);

            if (!MSSQLDbValidator.DbIsValid(database, out string error))
                throw new Exception($"Db is invalid: {error}");
            return database;
        }

        private static MSSQLDatabaseInfo GetExistingDatabase()
        {
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor());
            MSSQLDatabaseInfo existingDatabase = interactor.GetExistingDatabase();
            return existingDatabase;
        }
    }
}
