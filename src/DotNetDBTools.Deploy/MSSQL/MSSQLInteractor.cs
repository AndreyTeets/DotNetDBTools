using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL.Queries;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Deploy.MSSQL.Queries.MSSQLSysInfo;
using DotNetDBTools.Models.Core;
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

        public void CreateDatabase(string databaseName)
        {
            _queryExecutor.Execute(new CreateDatabaseQuery(databaseName));
        }

        public MSSQLDatabaseInfo GetDatabaseModelFromDNDBTSysInfo()
        {
            MSSQLDatabaseInfo databaseInfo = GenerateDatabaseModelFromMSSQLSysInfo();

            GetAllDbObjectsFromDNDBTSysInfoQuery.ResultsInterpreter.ReplaceDbModelObjectsIDsWithRecordOnes(
                databaseInfo,
                _queryExecutor.Query<GetAllDbObjectsFromDNDBTSysInfoQuery.DNDBTDbObjectRecord>(
                    new GetAllDbObjectsFromDNDBTSysInfoQuery()));

            return databaseInfo;
        }

        public MSSQLDatabaseInfo GenerateDatabaseModelFromMSSQLSysInfo()
        {
            Dictionary<string, MSSQLTableInfo> tables =
                GetColumnsFromMSSQLSysInfoQuery.ResultsInterpreter.BuildTablesListWithColumns(
                    _queryExecutor.Query<GetColumnsFromMSSQLSysInfoQuery.ColumnRecord>(
                        new GetColumnsFromMSSQLSysInfoQuery()));

            GetPrimaryKeysFromMSSQLSysInfoQuery.ResultsInterpreter.BuildTablesPrimaryKeys(
                tables,
                _queryExecutor.Query<GetPrimaryKeysFromMSSQLSysInfoQuery.PrimaryKeyRecord>(
                    new GetPrimaryKeysFromMSSQLSysInfoQuery()));

            GetUniqueConstraintsFromMSSQLSysInfoQuery.ResultsInterpreter.BuildTablesUniqueConstraints(
                tables,
                _queryExecutor.Query<GetUniqueConstraintsFromMSSQLSysInfoQuery.UniqueConstraintRecord>(
                    new GetUniqueConstraintsFromMSSQLSysInfoQuery()));

            GetForeignKeysFromMSSQLSysInfoQuery.ResultsInterpreter.BuildTablesForeignKeys(
                tables,
                _queryExecutor.Query<GetForeignKeysFromMSSQLSysInfoQuery.ForeignKeyRecord>(
                    new GetForeignKeysFromMSSQLSysInfoQuery()));

            List<MSSQLUserDefinedTypeInfo> userDefinedTypes =
                GetTypesFromMSSQLSysInfoQuery.ResultsInterpreter.BuildUserDefinedTypesList(
                    _queryExecutor.Query<GetTypesFromMSSQLSysInfoQuery.UserDefinedTypeRecord>(
                        new GetTypesFromMSSQLSysInfoQuery()));

            return new MSSQLDatabaseInfo(null)
            {
                Tables = tables.Select(x => x.Value),
                UserDefinedTypes = userDefinedTypes,
            };
        }

        public void ApplyDatabaseDiff(MSSQLDatabaseDiff dbDiff)
        {
            _queryExecutor.BeginTransaction();
            try
            {
                Dictionary<Guid, TableInfo> newDbFKToTableMap = ForeignKeysHelper.CreateFKToTableMap(dbDiff.NewDatabase.Tables);
                Dictionary<Guid, TableInfo> oldDbFKToTableMap = ForeignKeysHelper.CreateFKToTableMap(dbDiff.OldDatabase.Tables);

                // TODO DropProcedures
                // TODO reference-ordering for functions+views together since either can depend on one another
                foreach (MSSQLFunctionInfo function in dbDiff.RemovedFunctions.Concat(dbDiff.ChangedFunctions.Select(x => x.OldFunction)))
                    DropFunction(function);
                foreach (MSSQLViewInfo view in dbDiff.RemovedViews.Concat(dbDiff.ChangedViews.Select(x => x.OldView)))
                    DropView(view);
                foreach (ForeignKeyInfo fk in dbDiff.AllForeignKeysToDrop)
                    DropForeignKey(fk, oldDbFKToTableMap);
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
                foreach (ForeignKeyInfo fk in dbDiff.AllForeignKeysToAdd)
                    CreateForeignKey(fk, newDbFKToTableMap);
                foreach (MSSQLViewInfo view in dbDiff.AddedViews.Concat(dbDiff.ChangedViews.Select(x => x.NewView)))
                    CreateView(view);
                foreach (MSSQLFunctionInfo function in dbDiff.AddedFunctions.Concat(dbDiff.ChangedFunctions.Select(x => x.NewFunction)))
                    CreateFunction(function);
                // TODO CreateProcedures
            }
            catch (Exception)
            {
                _queryExecutor.RollbackTransaction();
                throw;
            }
            _queryExecutor.CommitTransaction();
        }

        public bool DNDBTSysTablesExist()
        {
            bool systemTablesExist = _queryExecutor.QuerySingleOrDefault<bool>(new CheckDNDBTSysTablesExistQuery());
            return systemTablesExist;
        }

        public void CreateDNDBTSysTables()
        {
            _queryExecutor.Execute(new CreateDNDBTSysTablesQuery());
        }

        public void DropDNDBTSysTables()
        {
            _queryExecutor.Execute(new DropDNDBTSysTablesQuery());
        }

        public void PopulateDNDBTSysTables(MSSQLDatabaseInfo database)
        {
            foreach (MSSQLTableInfo table in database.Tables)
            {
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, null, MSSQLDbObjectsTypes.Table, table.Name));
                foreach (ColumnInfo column in table.Columns)
                    _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, table.ID, MSSQLDbObjectsTypes.Column, column.Name));

                PrimaryKeyInfo pk = table.PrimaryKey;
                if (pk is not null)
                    _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, table.ID, MSSQLDbObjectsTypes.PrimaryKey, pk.Name));

                foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                    _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, table.ID, MSSQLDbObjectsTypes.UniqueConstraint, uc.Name));

                foreach (ForeignKeyInfo fk in table.ForeignKeys)
                    _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, table.ID, MSSQLDbObjectsTypes.ForeignKey, fk.Name));
            }
            foreach (MSSQLViewInfo view in database.Views)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(view.ID, null, MSSQLDbObjectsTypes.View, view.Name));
            foreach (MSSQLFunctionInfo function in database.Functions)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(function.ID, null, MSSQLDbObjectsTypes.Function, function.Name));
            foreach (MSSQLUserDefinedTypeInfo udt in database.UserDefinedTypes)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(udt.ID, null, MSSQLDbObjectsTypes.UserDefinedType, udt.Name));
        }

        private void CreateTable(MSSQLTableInfo table)
        {
            _queryExecutor.Execute(new CreateTableQuery(table));
            _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, null, MSSQLDbObjectsTypes.Table, table.Name));
            foreach (ColumnInfo column in table.Columns)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, table.ID, MSSQLDbObjectsTypes.Column, column.Name));

            PrimaryKeyInfo pk = table.PrimaryKey;
            if (pk is not null)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, table.ID, MSSQLDbObjectsTypes.PrimaryKey, pk.Name));

            foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, table.ID, MSSQLDbObjectsTypes.UniqueConstraint, uc.Name));
        }

        private void DropTable(MSSQLTableInfo table)
        {
            _queryExecutor.Execute(new DropTableQuery(table));
            _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(table.ID));
            foreach (ColumnInfo column in table.Columns)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(column.ID));

            PrimaryKeyInfo pk = table.PrimaryKey;
            if (pk is not null)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(pk.ID));

            foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(uc.ID));
        }

        private void AlterTable(MSSQLTableDiff tableDiff)
        {
            _queryExecutor.Execute(new AlterTableQuery(tableDiff));

            foreach (UniqueConstraintInfo uc in tableDiff.RemovedUniqueConstraints)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(uc.ID));
            if (tableDiff.RemovedPrimaryKey is not null)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(tableDiff.RemovedPrimaryKey.ID));
            foreach (ColumnInfo column in tableDiff.RemovedColumns)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(column.ID));

            _queryExecutor.Execute(new UpdateDNDBTSysInfoQuery(tableDiff.NewTable.ID, tableDiff.NewTable.Name));
            foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
                _queryExecutor.Execute(new UpdateDNDBTSysInfoQuery(columnDiff.NewColumn.ID, columnDiff.NewColumn.Name));

            foreach (ColumnInfo column in tableDiff.AddedColumns)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.Column, column.Name));
            if (tableDiff.AddedPrimaryKey is not null)
            {
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(
                    tableDiff.AddedPrimaryKey.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.PrimaryKey, tableDiff.AddedPrimaryKey.Name));
            }
            foreach (UniqueConstraintInfo uc in tableDiff.AddedUniqueConstraints)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.UniqueConstraint, uc.Name));
        }

        private void CreateForeignKey(ForeignKeyInfo fk, Dictionary<Guid, TableInfo> fkToTableMap)
        {
            _queryExecutor.Execute(new CreateForeignKeyQuery(fk, fkToTableMap[fk.ID].Name));
            _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, fkToTableMap[fk.ID].ID, MSSQLDbObjectsTypes.ForeignKey, fk.Name));
        }

        private void DropForeignKey(ForeignKeyInfo fk, Dictionary<Guid, TableInfo> fkToTableMap)
        {
            _queryExecutor.Execute(new DropForeignKeyQuery(fk, fkToTableMap[fk.ID].Name));
            _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(fk.ID));
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

        private void RenameUserDefinedTypeToTempInDbAndInDbDiff(MSSQLUserDefinedTypeInfo userDefinedType)
        {
            _queryExecutor.Execute(new RenameUserDefinedDataTypeQuery(userDefinedType));
            userDefinedType.Name = $"_DNDBTTemp_{userDefinedType.Name}";
            _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(userDefinedType.ID));
        }

        private void CreateUserDefinedType(MSSQLUserDefinedTypeInfo udt)
        {
            _queryExecutor.Execute(new CreateTypeQuery(udt));
            _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(udt.ID, null, MSSQLDbObjectsTypes.UserDefinedType, udt.Name));
        }

        private void DropUserDefinedType(MSSQLUserDefinedTypeInfo userDefinedType)
        {
            _queryExecutor.Execute(new DropTypeQuery(userDefinedType));
        }

        private void UseNewUDTInAllTables(MSSQLUserDefinedTypeDiff userDefinedTypeDiff)
        {
            _queryExecutor.Execute(new UseNewUDTInAllTablesQuery(userDefinedTypeDiff));
        }
    }
}
