using System;
using System.Data.SqlClient;
using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy
{
    public class MSSQLDeployManager : DeployManager
    {
        public MSSQLDeployManager(DeployOptions deployOptions) : base(
            deployOptions: deployOptions,
            dbModelConverter: new MSSQLDbModelConverter(),
            factory: new MSSQLFactory())
        {
        }

        protected override Database GetDatabaseModelIfDbExistsOrCreateEmptyDbAndModel(string connectionString)
        {
            SqlConnectionStringBuilder sqlConnectionBuilder = new(connectionString);
            string databaseName = sqlConnectionBuilder.InitialCatalog;
            sqlConnectionBuilder.InitialCatalog = string.Empty;
            string connectionStringWithoutDb = sqlConnectionBuilder.ConnectionString;

            Interactor interactor = Factory.CreateInteractor(Factory.CreateQueryExecutor(connectionString));
            Interactor interactorForEmpty = Factory.CreateInteractor(Factory.CreateQueryExecutor(connectionStringWithoutDb));
            bool databaseExists = interactorForEmpty.DatabaseExists(databaseName);
            if (databaseExists)
            {
                return interactor.GetDatabaseModelFromDNDBTSysInfo();
            }
            else
            {
                if (DeployOptions.AllowDbCreation)
                {
                    interactorForEmpty.CreateDatabase(databaseName);
                    interactor.CreateDNDBTSysTables();
                    return CreateEmptyDatabaseModel();
                }
                else
                {
                    throw new Exception($"Database doesn't exist and it's creation is not allowed");
                }
            }
        }

        protected override Database GetDatabaseModelIfDbExistsAndRegisteredOrCreateEmptyModel(string connectionString)
        {
            SqlConnectionStringBuilder sqlConnectionBuilder = new(connectionString);
            string databaseName = sqlConnectionBuilder.InitialCatalog;
            sqlConnectionBuilder.InitialCatalog = string.Empty;
            string connectionStringWithoutDb = sqlConnectionBuilder.ConnectionString;

            Interactor interactor = Factory.CreateInteractor(Factory.CreateQueryExecutor(connectionString));
            Interactor interactorForEmpty = Factory.CreateInteractor(Factory.CreateQueryExecutor(connectionStringWithoutDb));
            if (interactorForEmpty.DatabaseExists(databaseName) && interactor.DNDBTSysTablesExist())
                return interactor.GetDatabaseModelFromDNDBTSysInfo();
            return CreateEmptyDatabaseModel();
        }

        protected override Database CreateEmptyDatabaseModel()
        {
            return new MSSQLDatabase(null);
        }
    }
}
