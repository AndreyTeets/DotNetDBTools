using System;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy
{
    public class SQLiteDeployManager : DeployManager
    {
        public SQLiteDeployManager(DeployOptions deployOptions) : base(
            deployOptions: deployOptions,
            dbModelConverter: new SQLiteDbModelConverter(),
            factory: new SQLiteFactory())
        {
        }

        protected override Database GetDatabaseModelIfDbExistsOrCreateEmptyDbAndModel(string connectionString)
        {
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            bool databaseExists = interactor.DNDBTSysTablesExist();
            if (databaseExists)
            {
                return interactor.GetDatabaseModelFromDNDBTSysInfo();
            }
            else
            {
                if (DeployOptions.AllowDbCreation)
                {
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
            SQLiteInteractor interactor = new(new SQLiteQueryExecutor(connectionString));
            if (interactor.DNDBTSysTablesExist())
                return interactor.GetDatabaseModelFromDNDBTSysInfo();
            return CreateEmptyDatabaseModel();
        }

        protected override Database CreateEmptyDatabaseModel()
        {
            return new SQLiteDatabase(null);
        }
    }
}
