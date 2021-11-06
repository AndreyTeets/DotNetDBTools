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
        public MSSQLDeployManager(bool allowDbCreation = default, bool allowDataLoss = default) : base(
            allowDbCreation: allowDbCreation,
            allowDataLoss: allowDataLoss,
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

            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(connectionString));
            MSSQLInteractor interactorForEmpty = new(new MSSQLQueryExecutor(connectionStringWithoutDb));
            bool databaseExists = interactorForEmpty.DatabaseExists(databaseName);
            if (databaseExists)
            {
                return interactor.GetDatabaseModelFromDNDBTSysInfo();
            }
            else
            {
                if (AllowDbCreation)
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

            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(connectionString));
            MSSQLInteractor interactorForEmpty = new(new MSSQLQueryExecutor(connectionStringWithoutDb));
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
