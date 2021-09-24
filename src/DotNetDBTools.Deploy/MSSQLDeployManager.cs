﻿using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.Analysis.MSSQL;
using DotNetDBTools.DefinitionGenerator;
using DotNetDBTools.DefinitionParser.Agnostic;
using DotNetDBTools.DefinitionParser.Core;
using DotNetDBTools.DefinitionParser.MSSQL;
using DotNetDBTools.Deploy.MSSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy
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

        public void PublishDatabase(string dbAssemblyPath, string connectionString)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            PublishDatabase(dbAssembly, connectionString);
        }

        public void PublishDatabase(Assembly dbAssembly, string connectionString)
        {
            MSSQLDatabaseInfo database = CreateMSSQLDatabaseInfo(dbAssembly);
            MSSQLDatabaseInfo existingDatabase = GetExistingDatabaseOrCreateEmptyIfNotExists(connectionString);

            if (!MSSQLDbValidator.CanUpdate(database, existingDatabase, _allowDataLoss, out string error))
                throw new Exception($"Can not update database: {error}");

            MSSQLDatabaseDiff databaseDiff = MSSQLDiffCreator.CreateDatabaseDiff(database, existingDatabase);
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(connectionString));
            interactor.ApplyDatabaseDiff(databaseDiff);
        }

        public void GeneratePublishScript(string dbAssemblyPath, string connectionString, string outputPath)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            GeneratePublishScript(dbAssembly, connectionString, outputPath);
        }

        public void GeneratePublishScript(Assembly dbAssembly, string connectionString, string outputPath)
        {
            MSSQLDatabaseInfo database = CreateMSSQLDatabaseInfo(dbAssembly);
            MSSQLDatabaseInfo existingDatabase = GetExistingDatabase(connectionString);
            GeneratePublishScript(database, existingDatabase, outputPath);
        }

        public void GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath)
        {
            MSSQLDatabaseInfo database = CreateMSSQLDatabaseInfo(newDbAssembly);
            MSSQLDatabaseInfo existingDatabase = CreateMSSQLDatabaseInfo(oldDbAssembly);
            GeneratePublishScript(database, existingDatabase, outputPath);
        }

        public void GenerateDefinition(string connectionString, string outputDirectory)
        {
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(connectionString));
            MSSQLDatabaseInfo existingDatabase;
            if (interactor.SystemTablesExist())
                existingDatabase = interactor.GetExistingDatabase();
            else
                existingDatabase = interactor.GenerateExistingDatabaseSystemInfo();
            DbDefinitionGenerator.GenerateDefinition(existingDatabase, outputDirectory);
        }

        public void RegisterAsDNDBT(string connectionString)
        {
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(connectionString));
            if (interactor.SystemTablesExist())
                throw new InvalidOperationException("Database is already registered");
            MSSQLDatabaseInfo existingDatabase = interactor.GenerateExistingDatabaseSystemInfo();
            interactor.CreateSystemTables();
            interactor.PopulateSystemTables(existingDatabase);
        }

        public void UnregisterAsDNDBT(string connectionString)
        {
            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(connectionString));
            interactor.DropSystemTables();
        }

        public void GeneratePublishScript(MSSQLDatabaseInfo database, MSSQLDatabaseInfo existingDatabase, string outputPath)
        {
            MSSQLGenSqlScriptQueryExecutor genSqlScriptQueryExecutor = new();
            MSSQLInteractor interactor = new(genSqlScriptQueryExecutor);
            MSSQLDatabaseDiff databaseDiff = MSSQLDiffCreator.CreateDatabaseDiff(database, existingDatabase);
            interactor.ApplyDatabaseDiff(databaseDiff);
            string generatedScript = genSqlScriptQueryExecutor.GetFinalScript();

            string fullPath = Path.GetFullPath(outputPath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllText(fullPath, generatedScript);
        }

        private static MSSQLDatabaseInfo CreateMSSQLDatabaseInfo(Assembly dbAssembly)
        {
            MSSQLDatabaseInfo database;
            if (DbAssemblyInfoHelper.GetDbKind(dbAssembly) == DatabaseKind.Agnostic)
                database = AgnosticToMSSQLConverter.ConvertToMSSQLInfo(AgnosticDefinitionParser.CreateDatabaseInfo(dbAssembly));
            else
                database = MSSQLDefinitionParser.CreateDatabaseInfo(dbAssembly);

            if (!MSSQLDbValidator.DbIsValid(database, out DbError error))
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
            MSSQLInteractor interactorForEmpty = new(new MSSQLQueryExecutor(connectionStringWithoutDb));
            bool databaseExists = interactorForEmpty.DatabaseExists(databaseName);
            if (databaseExists)
            {
                MSSQLDatabaseInfo existingDatabase = interactor.GetExistingDatabase();
                return existingDatabase;
            }
            else
            {
                if (_allowDbCreation)
                {
                    interactorForEmpty.CreateDatabase(databaseName);
                    interactor.CreateSystemTables();
                    return new MSSQLDatabaseInfo(null);
                }
                else
                {
                    throw new Exception($"Database doesn't exist and it's creation is not allowed");
                }
            }
        }

        private static MSSQLDatabaseInfo GetExistingDatabase(string connectionString)
        {
            SqlConnectionStringBuilder sqlConnectionBuilder = new(connectionString);
            string databaseName = sqlConnectionBuilder.InitialCatalog;
            sqlConnectionBuilder.InitialCatalog = string.Empty;
            string connectionStringWithoutDb = sqlConnectionBuilder.ConnectionString;

            MSSQLInteractor interactor = new(new MSSQLQueryExecutor(connectionString));
            MSSQLInteractor interactorForEmpty = new(new MSSQLQueryExecutor(connectionStringWithoutDb));
            bool databaseExists = interactorForEmpty.DatabaseExists(databaseName);
            if (databaseExists)
                return interactor.GetExistingDatabase();
            else
                return new MSSQLDatabaseInfo(null);
        }
    }
}