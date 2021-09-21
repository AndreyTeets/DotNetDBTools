using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL.Queries;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Deploy.MSSQL.Queries.MSSQLSysInfo;
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
            Dictionary<string, MSSQLTableInfo> tables = GetColumnsFromMSSQLSysInfoQuery.ResultsInterpreter.BuildTablesListWithColumns(
                _queryExecutor.Query<GetColumnsFromMSSQLSysInfoQuery.ColumnRecord>(new GetColumnsFromMSSQLSysInfoQuery()));

            GetForeignKeysFromMSSQLSysInfoQuery.ResultsInterpreter.BuildTablesForeignKeys(
                tables,
                _queryExecutor.Query<GetForeignKeysFromMSSQLSysInfoQuery.ForeignKeyRecord>(new GetForeignKeysFromMSSQLSysInfoQuery()));

            List<MSSQLUserDefinedTypeInfo> userDefinedTypes = GetTypesFromMSSQLSysInfoQuery.ResultsInterpreter.BuildUserDefinedTypesList(
                _queryExecutor.Query<GetTypesFromMSSQLSysInfoQuery.UserDefinedTypeRecord>(new GetTypesFromMSSQLSysInfoQuery()));

            return new MSSQLDatabaseInfo(null)
            {
                Tables = tables.Select(x => x.Value),
                UserDefinedTypes = userDefinedTypes,
            };
        }

        public void CreateDatabase(string databaseName)
        {
            _queryExecutor.Execute(new CreateDatabaseQuery(databaseName));
        }

        public void ApplyDatabaseDiff(MSSQLDatabaseDiff dbDiff)
        {
            // TODO DropProcedures
            // TODO reference-ordering for functions+views together since either can depend on one another
            foreach (MSSQLFunctionInfo function in dbDiff.RemovedFunctions.Concat(dbDiff.ChangedFunctions.Select(x => x.OldFunction)))
                DropFunction(function);
            foreach (MSSQLViewInfo view in dbDiff.RemovedViews.Concat(dbDiff.ChangedViews.Select(x => x.OldView)))
                DropView(view);
            foreach (MSSQLTableInfo table in dbDiff.RemovedTables)
                DropTable(table);

            foreach (MSSQLUserDefinedTypeInfo userDefinedType in dbDiff.RemovedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.OldUserDefinedType)))
                RenameUserDefinedTypeToTempInDbAndInDbDiff(userDefinedType);
            foreach (MSSQLUserDefinedTypeInfo userDefinedType in dbDiff.AddedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.NewUserDefinedType)))
                CreateUserDefinedType(userDefinedType);
            foreach (MSSQLUserDefinedTypeDiff userDefinedTypeDiff in dbDiff.ChangedUserDefinedTypes)
                UseNewUDTInAllTables(userDefinedTypeDiff);
            foreach (MSSQLTableDiff tableDiff in dbDiff.ChangedTables)
                AlterTable(tableDiff);
            foreach (MSSQLUserDefinedTypeInfo userDefinedType in dbDiff.RemovedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.OldUserDefinedType)))
                DropUserDefinedType(userDefinedType);

            foreach (MSSQLTableInfo table in dbDiff.AddedTables)
                CreateTable(table);
            foreach (MSSQLViewInfo view in dbDiff.AddedViews.Concat(dbDiff.ChangedViews.Select(x => x.NewView)))
                CreateView(view);
            foreach (MSSQLFunctionInfo function in dbDiff.AddedFunctions.Concat(dbDiff.ChangedFunctions.Select(x => x.NewFunction)))
                CreateFunction(function);
            // TODO CreateProcedures
        }

        public bool SystemTablesExist()
        {
            bool systemTablesExist = _queryExecutor.QuerySingleOrDefault<bool>(new CheckDNDBTSysTablesExistQuery());
            return systemTablesExist;
        }

        public void CreateSystemTables()
        {
            _queryExecutor.Execute(new CreateDNDBTSysTablesQuery());
        }

        public void DropSystemTables()
        {
            _queryExecutor.Execute(new DropDNDBTSysTablesQuery());
        }

        public void PopulateSystemTables(MSSQLDatabaseInfo database)
        {
            foreach (MSSQLTableInfo table in database.Tables)
            {
                string tableMetadata = MSSQLDbObjectsSerializer.TableToJson(table);
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, MSSQLDbObjectsTypes.Table, table.Name, tableMetadata));
            }
            foreach (MSSQLViewInfo view in database.Views)
            {
                string tableMetadata = "MSSQLDbObjectsSerializer.ViewToJson(view)";
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(view.ID, MSSQLDbObjectsTypes.View, view.Name, tableMetadata));
            }
            foreach (MSSQLFunctionInfo function in database.Functions)
            {
                string tableMetadata = "MSSQLDbObjectsSerializer.FunctionToJson(function)";
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(function.ID, MSSQLDbObjectsTypes.Function, function.Name, tableMetadata));
            }
            foreach (MSSQLUserDefinedTypeInfo userDefinedType in database.UserDefinedTypes)
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

        private void CreateFunction(MSSQLFunctionInfo function)
        {
            _queryExecutor.Execute(new GenericQuery($"create function {function.Name} {function.Code};"));
        }

        private void DropFunction(MSSQLFunctionInfo function)
        {
            _queryExecutor.Execute(new GenericQuery($"drop function {function.Name};"));
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

        private void RenameUserDefinedTypeToTempInDbAndInDbDiff(MSSQLUserDefinedTypeInfo userDefinedType)
        {
            _queryExecutor.Execute(new RenameUserDefinedDataTypeQuery(userDefinedType));
            userDefinedType.Name = $"_DNDBTTemp_{userDefinedType.Name}";
        }

        private void UseNewUDTInAllTables(MSSQLUserDefinedTypeDiff userDefinedTypeDiff)
        {
            _queryExecutor.Execute(new UseNewUDTInAllTablesQuery(userDefinedTypeDiff));
        }
    }
}
