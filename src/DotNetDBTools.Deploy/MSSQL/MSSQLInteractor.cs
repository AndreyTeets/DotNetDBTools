using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL.Queries;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL
{
    public class MSSQLInteractor
    {
        private readonly IQueryExecutor _queryExecutor;

        public MSSQLInteractor(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }

        public bool DatabaseExists(string databaseName)
        {
            bool databaseExists = _queryExecutor.QuerySingleOrDefault<bool>(new CheckDatabaseExistsQuery(databaseName));
            return databaseExists;
        }

        public MSSQLDatabaseInfo GetExistingDatabase()
        {
            List<MSSQLTableInfo> tables = new();
            IEnumerable<string> tablesMetadatas = _queryExecutor.Query<string>(new GetTablesFromDNDBTSysInfoQuery());
            foreach (string tableMetadata in tablesMetadatas)
            {
                MSSQLTableInfo table = MSSQLDbObjectsSerializer.TableFromJson(tableMetadata);
                tables.Add(table);
            }

            List<MSSQLUserDefinedTypeInfo> userDefinedTypes = new();
            IEnumerable<string> userDefinedTypesMetadatas = _queryExecutor.Query<string>(new GetTypesFromDNDBTSysInfoQuery());
            foreach (string userDefinedTypeMetadata in userDefinedTypesMetadatas)
            {
                MSSQLUserDefinedTypeInfo userDefinedType = MSSQLDbObjectsSerializer.UserDefinedTypeFromJson(userDefinedTypeMetadata);
                userDefinedTypes.Add(userDefinedType);
            }

            return new MSSQLDatabaseInfo(null)
            {
                Tables = tables,
                UserDefinedTypes = userDefinedTypes,
            };
        }

        public MSSQLDatabaseInfo GenerateExistingDatabaseSystemInfo()
        {
            List<MSSQLTableInfo> tables = new();
            IEnumerable<string> tablesMetadatas = _queryExecutor.Query<string>(new GetTablesFromMSSQLSysInfoQuery());
            foreach (string tableMetadata in tablesMetadatas)
            {
                MSSQLTableInfo table = MSSQLDbObjectsSerializer.TableFromJson(tableMetadata);
                tables.Add(table);
            }

            List<MSSQLUserDefinedTypeInfo> userDefinedTypes = new();
            IEnumerable<string> userDefinedTypesMetadatas = _queryExecutor.Query<string>(new GetTypesFromMSSQLSysInfoQuery());
            foreach (string userDefinedTypeMetadata in userDefinedTypesMetadatas)
            {
                MSSQLUserDefinedTypeInfo userDefinedType = MSSQLDbObjectsSerializer.UserDefinedTypeFromJson(userDefinedTypeMetadata);
                userDefinedTypes.Add(userDefinedType);
            }

            return new MSSQLDatabaseInfo(null)
            {
                Tables = tables,
                UserDefinedTypes = userDefinedTypes,
            };
        }

        public void CreateDatabase(string databaseName)
        {
            _queryExecutor.Execute(new CreateDatabaseQuery(databaseName));
        }

        public void AlterDatabase(MSSQLDatabaseDiff databaseDiff)
        {
            foreach (MSSQLUserDefinedTypeInfo userDefinedType in databaseDiff.RemovedUserDefinedTypes)
                DropUserDefinedType(userDefinedType);
            foreach (MSSQLUserDefinedTypeDiff userDefinedTypeDiff in databaseDiff.ChangedUserDefinedTypes)
                AlterUserDefinedType(userDefinedTypeDiff);
            foreach (MSSQLUserDefinedTypeInfo userDefinedType in databaseDiff.AddedUserDefinedTypes)
                CreateUserDefinedType(userDefinedType);

            foreach (MSSQLTableInfo table in databaseDiff.RemovedTables)
                DropTable(table);
            foreach (MSSQLTableDiff tableDiff in databaseDiff.ChangedTables)
                AlterTable(tableDiff);
            foreach (MSSQLTableInfo table in databaseDiff.AddedTables)
                CreateTable(table);

            foreach (MSSQLViewInfo view in databaseDiff.RemovedViews)
                DropView(view);
            foreach (MSSQLViewDiff viewDiff in databaseDiff.ChangedViews)
                AlterView(viewDiff);
            foreach (MSSQLViewInfo view in databaseDiff.AddedViews)
                CreateView(view);

            foreach (MSSQLFunctionInfo function in databaseDiff.RemovedFunctions)
                DropFunction(function);
            foreach (MSSQLFunctionDiff functionDiff in databaseDiff.ChangedFunctions)
                AlterFunction(functionDiff);
            foreach (MSSQLFunctionInfo function in databaseDiff.AddedFunctions)
                CreateFunction(function);
        }

        public void CreateSystemTables()
        {
            _queryExecutor.Execute(new CreateDNDBTSysTablesQuery());
        }

        public void DropSystemTables()
        {
            _queryExecutor.Execute(new DropDNDBTSysTablesQuery());
        }

        public void PopulateSystemTables(MSSQLDatabaseInfo existingDatabase)
        {
            foreach (MSSQLTableInfo table in existingDatabase.Tables)
            {
                string tableMetadata = MSSQLDbObjectsSerializer.TableToJson(table);
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, MSSQLDbObjectsTypes.Table, table.Name, tableMetadata));
            }
            foreach (MSSQLViewInfo view in existingDatabase.Views)
            {
                string tableMetadata = "MSSQLDbObjectsSerializer.ViewToJson(view)";
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(view.ID, MSSQLDbObjectsTypes.View, view.Name, tableMetadata));
            }
            foreach (MSSQLFunctionInfo function in existingDatabase.Functions)
            {
                string tableMetadata = "MSSQLDbObjectsSerializer.FunctionToJson(function)";
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(function.ID, MSSQLDbObjectsTypes.Function, function.Name, tableMetadata));
            }
            foreach (MSSQLUserDefinedTypeInfo userDefinedType in existingDatabase.UserDefinedTypes)
            {
                string userDefinedTypeMetadata = MSSQLDbObjectsSerializer.UserDefinedTypeToJson(userDefinedType);
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(userDefinedType.ID, MSSQLDbObjectsTypes.UserDefinedType, userDefinedType.Name, userDefinedTypeMetadata));
            }
        }

        private void CreateTable(MSSQLTableInfo table)
        {
            string tableMetadata = MSSQLDbObjectsSerializer.TableToJson(table);
            _queryExecutor.Execute(new CreateTableQuery(table));
            _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, MSSQLDbObjectsTypes.Table, table.Name, tableMetadata));
        }

        private void DropTable(MSSQLTableInfo table)
        {
            _queryExecutor.Execute(new DropTableQuery(table));
            _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(table.ID));
        }

        private void AlterTable(MSSQLTableDiff tableDiff)
        {
            string newTableMetadata = MSSQLDbObjectsSerializer.TableToJson(tableDiff.NewTable);
            _queryExecutor.Execute(new AlterTableQuery(tableDiff));
            _queryExecutor.Execute(new UpdateDNDBTSysInfoQuery(tableDiff.NewTable.ID, tableDiff.NewTable.Name, newTableMetadata));
        }

        private void CreateView(MSSQLViewInfo view)
        {
            _queryExecutor.Execute(new GenericQuery($"create view {view.Name} {view.Code};"));
        }

        private void DropView(MSSQLViewInfo view)
        {
            _queryExecutor.Execute(new GenericQuery($"drop view {view.Name};"));
        }

        private void AlterView(MSSQLViewDiff viewDiff)
        {
            DropView(viewDiff.OldView);
            CreateView(viewDiff.NewView);
        }

        private void CreateFunction(MSSQLFunctionInfo function)
        {
            _queryExecutor.Execute(new GenericQuery($"create function {function.Name} {function.Code};"));
        }

        private void DropFunction(MSSQLFunctionInfo function)
        {
            _queryExecutor.Execute(new GenericQuery($"drop function {function.Name};"));
        }

        private void AlterFunction(MSSQLFunctionDiff functionDiff)
        {
            DropFunction(functionDiff.OldFunction);
            CreateFunction(functionDiff.NewFunction);
        }

        private void CreateUserDefinedType(MSSQLUserDefinedTypeInfo userDefinedType)
        {
            string userDefinedTypeMetadata = MSSQLDbObjectsSerializer.UserDefinedTypeToJson(userDefinedType);
            _queryExecutor.Execute(new CreateTypeQuery(userDefinedType));
            _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(userDefinedType.ID, MSSQLDbObjectsTypes.UserDefinedType, userDefinedType.Name, userDefinedTypeMetadata));
        }

        private void DropUserDefinedType(MSSQLUserDefinedTypeInfo userDefinedType)
        {
            _queryExecutor.Execute(new DropTypeQuery(userDefinedType));
            _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(userDefinedType.ID));
        }

        private void AlterUserDefinedType(MSSQLUserDefinedTypeDiff userDefinedTypeDiff)
        {
            DropUserDefinedType(userDefinedTypeDiff.OldUserDefinedType);
            CreateUserDefinedType(userDefinedTypeDiff.NewUserDefinedType);
        }
    }
}
