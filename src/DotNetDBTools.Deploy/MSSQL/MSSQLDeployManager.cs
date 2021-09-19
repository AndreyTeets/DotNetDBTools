using System;
using System.Data.SqlClient;
using System.Reflection;
using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.DefinitionParser.Shared;
using DotNetDBTools.DefinitionParser.MSSQL;
using DotNetDBTools.Models.Shared;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL
{
    public class MSSQLDeployManager : IDeployManager
    {
        private readonly bool _allowDbCreation; // TODO DeployOptions
        private readonly bool _allowDataLoss;

        public MSSQLDeployManager(bool allowDbCreation = default, bool allowDataLoss = default)
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
            MSSQLDatabaseInfo database = CreateMSSQLDatabaseInfo(dbAssembly);
            MSSQLDatabaseInfo existingDatabase = GetExistingDatabaseOrCreateEmptyIfNotExists(connectionString);

            if (!MSSQLDbValidator.CanUpdate(database, existingDatabase, _allowDataLoss, out string error))
                throw new Exception($"Can not update database: {error}");

            MSSQLDatabaseDiff databaseDiff = MSSQLDiffCreator.CreateDatabaseDiff(database, existingDatabase);
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(connectionString));
            interactor.UpdateDatabase(databaseDiff);
        }

        public string GenerateUpdateScript(string dbAssemblyPath, string connectionString)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            return GenerateUpdateScript(dbAssembly, connectionString);
        }

        public string GenerateUpdateScript(Assembly dbAssembly, string connectionString)
        {
            MSSQLDatabaseInfo database = CreateMSSQLDatabaseInfo(dbAssembly);

            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(connectionString));
            MSSQLDatabaseInfo existingDatabase = interactor.GetExistingDatabase();

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

        private static MSSQLDatabaseInfo CreateMSSQLDatabaseInfo(Assembly dbAssembly)
        {
            MSSQLDatabaseInfo database;
            if (DbAssemblyInfoHelper.GetDbKind(dbAssembly) == DatabaseKind.Agnostic)
                database = AgnosticToMSSQLConverter.ConvertToMSSQLInfo(AgnosticDefinitionParser.CreateDatabaseInfo(dbAssembly));
            else
                database = MSSQLDefinitionParser.CreateDatabaseInfo(dbAssembly);

            if (!MSSQLDbValidator.DbIsValid(database, out string error))
                throw new Exception($"Db is invalid: {error}");
            return database;
        }

        private MSSQLDatabaseInfo GetExistingDatabaseOrCreateEmptyIfNotExists(string connectionString)
        {
            SqlConnectionStringBuilder sqlConnectionBuilder = new(connectionString);
            string databaseName = sqlConnectionBuilder.InitialCatalog;
            sqlConnectionBuilder.InitialCatalog = string.Empty;
            string connectionStringWithoutDb = sqlConnectionBuilder.ConnectionString;

            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(connectionString));
            MSSQLInteractor interactorForEmptyChecks = new(new MSSQLQueryExecutor(connectionStringWithoutDb));
            bool databaseExists = interactorForEmptyChecks.DatabaseExists(databaseName);
            if (databaseExists)
            {
                MSSQLDatabaseInfo existingDatabase = interactor.GetExistingDatabase();
                return existingDatabase;
            }
            else
            {
                if (_allowDbCreation)
                {
                    interactorForEmptyChecks.CreateDatabase(databaseName);
                    interactor.CreateSystemTables();
                    return new MSSQLDatabaseInfo(null);
                }
                else
                {
                    throw new Exception($"Database doesn't exist and it's creation is not allowed");
                }
            }
        }
    }
}
