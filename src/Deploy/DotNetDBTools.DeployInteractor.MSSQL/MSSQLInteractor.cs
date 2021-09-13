using System.Collections.Generic;
using DotNetDBTools.DeployInteractor.MSSQL.Queries;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.DeployInteractor.MSSQL
{
    public class MSSQLInteractor
    {
        private readonly IQueryExecutor _queryExecutor;

        public MSSQLInteractor(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }

        public void UpdateDatabase(MSSQLDatabaseDiff databaseDiff)
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

        public bool DatabaseExists(string databaseName)
        {
            bool databaseExists = _queryExecutor.QuerySingleOrDefault<bool>(new DatabaseExistsQuery(databaseName));
            return databaseExists;
        }

        public void CreateDatabase(string databaseName)
        {
            _queryExecutor.Execute(new CreateDatabaseQuery(databaseName));
        }

        public void CreateSystemTables()
        {
            _queryExecutor.Execute(new CreateSystemTablesQuery());
        }

        public MSSQLDatabaseInfo GetExistingDatabase()
        {
            List<MSSQLTableInfo> tables = new();
            IEnumerable<string> tablesMetadatas = _queryExecutor.Query<string>(new GetExistingTablesQuery());
            foreach (string tableMetadata in tablesMetadatas)
            {
                MSSQLTableInfo table = MSSQLDbObjectsSerializer.TableFromJson(tableMetadata);
                tables.Add(table);
            }

            List<MSSQLUserDefinedTypeInfo> userDefinedTypes = new();
            IEnumerable<string> userDefinedTypesMetadatas = _queryExecutor.Query<string>(new GetExistingTypesQuery());
            foreach (string userDefinedTypeMetadata in userDefinedTypesMetadatas)
            {
                MSSQLUserDefinedTypeInfo userDefinedType = MSSQLDbObjectsSerializer.UserDefinedTypeFromJson(userDefinedTypeMetadata);
                userDefinedTypes.Add(userDefinedType);
            }

            return new MSSQLDatabaseInfo()
            {
                Tables = tables,
                UserDefinedTypes = userDefinedTypes,
            };
        }

        private void CreateTable(MSSQLTableInfo table)
        {
            string tableMetadata = MSSQLDbObjectsSerializer.TableToJson(table);
            _queryExecutor.Execute(new CreateTableQuery(table, tableMetadata));
        }

        private void DropTable(MSSQLTableInfo table)
        {
            _queryExecutor.Execute(new DropTableQuery(table));
        }

        private void AlterTable(MSSQLTableDiff tableDiff)
        {
            string newTableMetadata = MSSQLDbObjectsSerializer.TableToJson(tableDiff.NewTable);
            _queryExecutor.Execute(new AlterTableQuery(tableDiff, newTableMetadata));
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
            _queryExecutor.Execute(new CreateTypeQuery(userDefinedType, userDefinedTypeMetadata));
        }

        private void DropUserDefinedType(MSSQLUserDefinedTypeInfo userDefinedType)
        {
            _queryExecutor.Execute(new DropTypeQuery(userDefinedType));
        }

        private void AlterUserDefinedType(MSSQLUserDefinedTypeDiff userDefinedTypeDiff)
        {
            DropUserDefinedType(userDefinedTypeDiff.OldUserDefinedType);
            CreateUserDefinedType(userDefinedTypeDiff.NewUserDefinedType);
        }
    }
}
