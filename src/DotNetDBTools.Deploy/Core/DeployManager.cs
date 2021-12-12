﻿using System;
using System.Data.Common;
using System.IO;
using System.Reflection;
using DotNetDBTools.Analysis;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Core.Errors;
using DotNetDBTools.DefinitionParsing;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Generation;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core
{
    public abstract class DeployManager<TDatabase> : IDeployManager
        where TDatabase : Database, new()
    {
        public DeployOptions Options { get; set; }

        private readonly IFactory _factory;
        private readonly IDbModelConverter _dbModelConverter;

        private protected DeployManager(
            DeployOptions options,
            IFactory factory)
        {
            Options = options;
            _factory = factory;
            _dbModelConverter = _factory.CreateDbModelConverter();
        }

        public void PublishDatabase(string dbAssemblyPath, DbConnection connection)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            PublishDatabase(dbAssembly, connection);
        }

        public void PublishDatabase(Assembly dbAssembly, DbConnection connection)
        {
            Database newDatabase = CreateDatabaseModelFromDbAssembly(dbAssembly);
            Database oldDatabase = GetDatabaseModelFromRegisteredDb(connection);
            DatabaseDiff databaseDiff = AnalysisHelper.CreateDatabaseDiff(newDatabase, oldDatabase);
            ValidateDatabaseDiff(databaseDiff);

            IDbEditor dbEditor = _factory.CreateDbEditor(_factory.CreateQueryExecutor(connection));
            dbEditor.ApplyDatabaseDiff(databaseDiff);
        }

        public void GeneratePublishScript(string dbAssemblyPath, DbConnection connection, string outputPath)
        {
            Assembly newDbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            GeneratePublishScript(newDbAssembly, connection, outputPath);
        }

        public void GeneratePublishScript(Assembly dbAssembly, DbConnection connection, string outputPath)
        {
            Database newDatabase = CreateDatabaseModelFromDbAssembly(dbAssembly);
            Database oldDatabase = GetDatabaseModelFromRegisteredDb(connection);
            GeneratePublishScript(newDatabase, oldDatabase, outputPath);
        }

        public void GeneratePublishScript(string dbAssemblyPath, string outputPath)
        {
            Assembly dbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(dbAssemblyPath);
            GeneratePublishScript(dbAssembly, outputPath);
        }

        public void GeneratePublishScript(Assembly dbAssembly, string outputPath)
        {
            Database newDatabase = CreateDatabaseModelFromDbAssembly(dbAssembly);
            Database oldDatabase = new TDatabase();
            GeneratePublishScript(newDatabase, oldDatabase, outputPath);
        }

        public void GeneratePublishScript(string newDbAssemblyPath, string oldDbAssemblyPath, string outputPath)
        {
            Assembly newDbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(newDbAssemblyPath);
            Assembly oldDbAssembly = AssemblyLoader.LoadDbAssemblyFromDll(oldDbAssemblyPath);
            GeneratePublishScript(newDbAssembly, oldDbAssembly, outputPath);
        }

        public void GeneratePublishScript(Assembly newDbAssembly, Assembly oldDbAssembly, string outputPath)
        {
            Database newDatabase = CreateDatabaseModelFromDbAssembly(newDbAssembly);
            Database oldDatabase = CreateDatabaseModelFromDbAssembly(oldDbAssembly);
            GeneratePublishScript(newDatabase, oldDatabase, outputPath);
        }

        public void RegisterAsDNDBT(DbConnection connection)
        {
            IQueryExecutor queryExecutor = _factory.CreateQueryExecutor(connection);
            IDbEditor dbEditor = _factory.CreateDbEditor(queryExecutor);
            IDbModelFromDbSysInfoBuilder dbModelFromDbSysInfoBuilder = _factory.CreateDbModelFromDbSysInfoBuilder(queryExecutor);

            if (dbEditor.DNDBTSysTablesExist())
                throw new InvalidOperationException("Database is already registered");
            Database oldDatabase = dbModelFromDbSysInfoBuilder.GenerateDatabaseModelFromDBMSSysInfo();
            dbEditor.CreateDNDBTSysTables();
            dbEditor.PopulateDNDBTSysTables(oldDatabase);
        }

        public void UnregisterAsDNDBT(DbConnection connection)
        {
            IDbEditor dbEditor = _factory.CreateDbEditor(_factory.CreateQueryExecutor(connection));
            dbEditor.DropDNDBTSysTables();
        }

        public void GenerateDefinition(DbConnection connection, string outputDirectory)
        {
            IQueryExecutor queryExecutor = _factory.CreateQueryExecutor(connection);
            IDbEditor dbEditor = _factory.CreateDbEditor(queryExecutor);
            IDbModelFromDbSysInfoBuilder dbModelFromDbSysInfoBuilder = _factory.CreateDbModelFromDbSysInfoBuilder(queryExecutor);

            Database database;
            if (dbEditor.DNDBTSysTablesExist())
                database = dbModelFromDbSysInfoBuilder.GetDatabaseModelFromDNDBTSysInfo();
            else
                database = dbModelFromDbSysInfoBuilder.GenerateDatabaseModelFromDBMSSysInfo();
            DbDefinitionGenerator.GenerateDefinition(database, outputDirectory);
        }

        private void GeneratePublishScript(Database newDatabase, Database oldDatabase, string outputPath)
        {
            DatabaseDiff databaseDiff = AnalysisHelper.CreateDatabaseDiff(newDatabase, oldDatabase);
            ValidateDatabaseDiff(databaseDiff);

            IGenSqlScriptQueryExecutor genSqlScriptQueryExecutor = _factory.CreateGenSqlScriptQueryExecutor();
            IDbEditor dbEditor = _factory.CreateDbEditor(genSqlScriptQueryExecutor);
            dbEditor.ApplyDatabaseDiff(databaseDiff);
            string generatedScript = genSqlScriptQueryExecutor.GetFinalScript();

            string fullPath = Path.GetFullPath(outputPath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            File.WriteAllText(fullPath, generatedScript);
        }

        private Database CreateDatabaseModelFromDbAssembly(Assembly dbAssembly)
        {
            Database database = DbDefinitionParser.CreateDatabaseModel(dbAssembly);
            if (database.Kind == DatabaseKind.Agnostic)
                database = _dbModelConverter.FromAgnostic(database);

            if (!AnalysisHelper.DbIsValid(database, out DbError error))
                throw new Exception($"Db is invalid: {error}");
            return database;
        }

        private Database GetDatabaseModelFromRegisteredDb(DbConnection connection)
        {
            IQueryExecutor queryExecutor = _factory.CreateQueryExecutor(connection);
            IDbEditor dbEditor = _factory.CreateDbEditor(queryExecutor);
            IDbModelFromDbSysInfoBuilder dbModelFromDbSysInfoBuilder = _factory.CreateDbModelFromDbSysInfoBuilder(queryExecutor);

            if (dbEditor.DNDBTSysTablesExist())
                return dbModelFromDbSysInfoBuilder.GetDatabaseModelFromDNDBTSysInfo();
            else
                throw new InvalidOperationException("Database is not registered");
        }

        private void ValidateDatabaseDiff(DatabaseDiff databaseDiff)
        {
            if (!Options.AllowDataLoss && AnalysisHelper.LeadsToDataLoss(databaseDiff))
                throw new Exception("Update would lead to data loss and it's not allowed.");
        }
    }
}
