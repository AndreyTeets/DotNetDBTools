using System;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite;
using DotNetDBTools.Deploy.SQLite.Factories;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy
{
    public class SQLiteDeployManager : DeployManager
    {
        public SQLiteDeployManager(bool allowDbCreation = default, bool allowDataLoss = default) : base(
            allowDbCreation: allowDbCreation,
            allowDataLoss: allowDataLoss,
            dbModelConverter: new SQLiteDbModelConverter(),
            queryExecutorFactory: new SQLiteQueryExecutorFactory(),
            genSqlScriptQueryExecutorFactory: new SQLiteGenSqlScriptQueryExecutorFactory(),
            interactorFactory: new SQLiteInteractorFactory())
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
                if (AllowDbCreation)
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
