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
                Views = new List<ViewInfo>(),
                UserDefinedTypes = userDefinedTypes,
                UserDefinedTableTypes = new List<MSSQLUserDefinedTableTypeInfo>(),
                Functions = new List<MSSQLFunctionInfo>(),
                Procedures = new List<MSSQLProcedureInfo>(),
            };
        }

        public void ApplyDatabaseDiff(MSSQLDatabaseDiff dbDiff)
        {
            _queryExecutor.BeginTransaction();
            try
            {
                Dictionary<Guid, TableInfo> newDbFKToTableMap = ForeignKeysHelper.CreateFKToTableMap(dbDiff.NewDatabase.Tables);
                Dictionary<Guid, TableInfo> oldDbFKToTableMap = ForeignKeysHelper.CreateFKToTableMap(dbDiff.OldDatabase.Tables);

                foreach (MSSQLProcedureInfo procedure in dbDiff.ProceduresToDrop)
                    DropProcedure(procedure);
                foreach (MSSQLViewInfo view in dbDiff.ViewsToDrop)
                    DropView(view);
                foreach (MSSQLFunctionInfo function in dbDiff.FunctionsToDrop)
                    DropFunction(function);
                foreach (MSSQLUserDefinedTableTypeInfo udtt in dbDiff.UserDefinedTableTypesToDrop)
                    DropUserDefinedTableType(udtt);
                foreach (ForeignKeyInfo fk in dbDiff.AllForeignKeysToDrop)
                    DropForeignKey(fk, oldDbFKToTableMap);
                foreach (MSSQLTableInfo table in dbDiff.RemovedTables)
                    DropTable(table);

                foreach (MSSQLUserDefinedTypeInfo udt in dbDiff.RemovedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.OldUserDefinedType)))
                    RenameUserDefinedTypeToTempInDbAndInDbDiff(udt);
                foreach (MSSQLUserDefinedTypeInfo udt in dbDiff.AddedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.NewUserDefinedType)))
                    CreateUserDefinedType(udt);
                foreach (MSSQLUserDefinedTypeDiff udtDiff in dbDiff.ChangedUserDefinedTypes)
                    UseNewUDTInAllTables(udtDiff);
                foreach (MSSQLTableDiff tableDiff in dbDiff.ChangedTables)
                    AlterTable(tableDiff);
                foreach (MSSQLUserDefinedTypeInfo udt in dbDiff.RemovedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.OldUserDefinedType)))
                    DropUserDefinedType(udt);

                foreach (MSSQLTableInfo table in dbDiff.AddedTables)
                    CreateTable(table);
                foreach (ForeignKeyInfo fk in dbDiff.AllForeignKeysToCreate)
                    CreateForeignKey(fk, newDbFKToTableMap);
                foreach (MSSQLUserDefinedTableTypeInfo udtt in dbDiff.UserDefinedTableTypesToCreate)
                    CreateUserDefinedTableType(udtt);
                foreach (MSSQLFunctionInfo function in dbDiff.FunctionsToCreate)
                    CreateFunction(function);
                foreach (MSSQLViewInfo view in dbDiff.ViewsToCreate)
                    CreateView(view);
                foreach (MSSQLProcedureInfo procedure in dbDiff.ProceduresToCreate)
                    CreateProcedure(procedure);
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
            foreach (MSSQLUserDefinedTypeInfo udt in database.UserDefinedTypes)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(udt.ID, null, MSSQLDbObjectsTypes.UserDefinedType, udt.Name));
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
                foreach (CheckConstraintInfo cc in table.CheckConstraints)
                    _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, table.ID, MSSQLDbObjectsTypes.CheckConstraint, cc.Name));
                foreach (IndexInfo index in table.Indexes)
                    _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, table.ID, MSSQLDbObjectsTypes.Index, index.Name));
                foreach (TriggerInfo trigger in table.Triggers)
                    _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, table.ID, MSSQLDbObjectsTypes.Trigger, trigger.Name));
                foreach (ForeignKeyInfo fk in table.ForeignKeys)
                    _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, table.ID, MSSQLDbObjectsTypes.ForeignKey, fk.Name));
            }
            foreach (MSSQLUserDefinedTableTypeInfo udtt in database.UserDefinedTableTypes)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(udtt.ID, null, MSSQLDbObjectsTypes.UserDefinedTableType, udtt.Name));
            foreach (MSSQLFunctionInfo function in database.Functions)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(function.ID, null, MSSQLDbObjectsTypes.Function, function.Name));
            foreach (MSSQLViewInfo view in database.Views)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(view.ID, null, MSSQLDbObjectsTypes.View, view.Name));
            foreach (MSSQLProcedureInfo procedure in database.Procedures)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(procedure.ID, null, MSSQLDbObjectsTypes.Procedure, procedure.Name));
        }

        private void RenameUserDefinedTypeToTempInDbAndInDbDiff(MSSQLUserDefinedTypeInfo udt)
        {
            _queryExecutor.Execute(new RenameUserDefinedDataTypeQuery(udt));
            udt.Name = $"_DNDBTTemp_{udt.Name}";
            _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(udt.ID));
        }

        private void CreateUserDefinedType(MSSQLUserDefinedTypeInfo udt)
        {
            _queryExecutor.Execute(new CreateTypeQuery(udt));
            _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(udt.ID, null, MSSQLDbObjectsTypes.UserDefinedType, udt.Name));
        }

        private void DropUserDefinedType(MSSQLUserDefinedTypeInfo udt)
        {
            _queryExecutor.Execute(new DropTypeQuery(udt));
        }

        private void UseNewUDTInAllTables(MSSQLUserDefinedTypeDiff udtDiff)
        {
            _queryExecutor.Execute(new UseNewUDTInAllTablesQuery(udtDiff));
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
            foreach (CheckConstraintInfo cc in table.CheckConstraints)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, table.ID, MSSQLDbObjectsTypes.CheckConstraint, cc.Name));
            foreach (IndexInfo index in table.Indexes)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, table.ID, MSSQLDbObjectsTypes.Index, index.Name));
            foreach (TriggerInfo trigger in table.Triggers)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, table.ID, MSSQLDbObjectsTypes.Trigger, trigger.Name));
        }

        private void DropTable(MSSQLTableInfo table)
        {
            _queryExecutor.Execute(new DropTableQuery(table));
            foreach (TriggerInfo trigger in table.Triggers)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(trigger.ID));
            foreach (IndexInfo index in table.Indexes)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(index.ID));
            foreach (CheckConstraintInfo cc in table.CheckConstraints)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(cc.ID));
            foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(uc.ID));
            PrimaryKeyInfo pk = table.PrimaryKey;
            if (pk is not null)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(pk.ID));
            foreach (ColumnInfo column in table.Columns)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(column.ID));
            _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(table.ID));
        }

        private void AlterTable(MSSQLTableDiff tableDiff)
        {
            _queryExecutor.Execute(new AlterTableQuery(tableDiff));

            foreach (TriggerInfo trigger in tableDiff.TriggersToDrop)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(trigger.ID));
            foreach (IndexInfo index in tableDiff.IndexesToDrop)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(index.ID));
            foreach (CheckConstraintInfo cc in tableDiff.CheckConstraintsToDrop)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(cc.ID));
            foreach (UniqueConstraintInfo uc in tableDiff.UniqueConstraintsToDrop)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(uc.ID));
            if (tableDiff.PrimaryKeyToDrop is not null)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(tableDiff.PrimaryKeyToDrop.ID));
            foreach (ColumnInfo column in tableDiff.RemovedColumns)
                _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(column.ID));

            _queryExecutor.Execute(new UpdateDNDBTSysInfoQuery(tableDiff.NewTable.ID, tableDiff.NewTable.Name));
            foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
                _queryExecutor.Execute(new UpdateDNDBTSysInfoQuery(columnDiff.NewColumn.ID, columnDiff.NewColumn.Name));

            foreach (ColumnInfo column in tableDiff.AddedColumns)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.Column, column.Name));
            PrimaryKeyInfo pk = tableDiff.PrimaryKeyToCreate;
            if (pk is not null)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.PrimaryKey, pk.Name));
            foreach (UniqueConstraintInfo uc in tableDiff.UniqueConstraintsToCreate)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.UniqueConstraint, uc.Name));
            foreach (CheckConstraintInfo cc in tableDiff.CheckConstraintsToCreate)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.CheckConstraint, cc.Name));
            foreach (IndexInfo index in tableDiff.IndexesToCreate)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.Index, index.Name));
            foreach (TriggerInfo trigger in tableDiff.TriggersToCreate)
                _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.Trigger, trigger.Name));
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

        private void CreateUserDefinedTableType(MSSQLUserDefinedTableTypeInfo udtt)
        {
            //_queryExecutor.Execute(new CreateTableQuery(udtt));
            _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(udtt.ID, null, MSSQLDbObjectsTypes.UserDefinedTableType, udtt.Name));
        }

        private void DropUserDefinedTableType(MSSQLUserDefinedTableTypeInfo udtt)
        {
            //_queryExecutor.Execute(new DropTableQuery(udtt));
            _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(udtt.ID));
        }

        private void CreateFunction(MSSQLFunctionInfo function)
        {
            _queryExecutor.Execute(new GenericQuery($"{function.Code}"));
            _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(function.ID, null, MSSQLDbObjectsTypes.Function, function.Name));
        }

        private void DropFunction(MSSQLFunctionInfo function)
        {
            _queryExecutor.Execute(new GenericQuery($"DROP FUNCTION {function.Name};"));
            _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(function.ID));
        }

        private void CreateView(MSSQLViewInfo view)
        {
            _queryExecutor.Execute(new GenericQuery($"{view.Code}"));
            _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(view.ID, null, MSSQLDbObjectsTypes.View, view.Name));
        }

        private void DropView(MSSQLViewInfo view)
        {
            _queryExecutor.Execute(new GenericQuery($"DROP VIEW {view.Name};"));
            _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(view.ID));
        }

        private void CreateProcedure(MSSQLProcedureInfo procedure)
        {
            _queryExecutor.Execute(new GenericQuery($"{procedure.Code}"));
            _queryExecutor.Execute(new InsertDNDBTSysInfoQuery(procedure.ID, null, MSSQLDbObjectsTypes.Procedure, procedure.Name));
        }

        private void DropProcedure(MSSQLProcedureInfo procedure)
        {
            _queryExecutor.Execute(new GenericQuery($"DROP PROCEDURE {procedure.Name};"));
            _queryExecutor.Execute(new DeleteDNDBTSysInfoQuery(procedure.ID));
        }
    }
}
