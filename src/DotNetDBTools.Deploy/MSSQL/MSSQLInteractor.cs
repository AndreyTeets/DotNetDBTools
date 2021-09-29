using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL.Queries;
using DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL
{
    internal class MSSQLInteractor : Interactor
    {
        public MSSQLInteractor(IQueryExecutor queryExecutor)
            : base(queryExecutor)
        {
        }

        public override bool DatabaseExists(string databaseName)
        {
            bool databaseExists = QueryExecutor.QuerySingleOrDefault<bool>(new CheckDatabaseExistsQuery(databaseName));
            return databaseExists;
        }

        public override void CreateDatabase(string databaseName)
        {
            QueryExecutor.Execute(new CreateDatabaseQuery(databaseName));
        }

        public override DatabaseInfo GetDatabaseModelFromDNDBTSysInfo()
        {
            MSSQLDatabaseInfo databaseInfo = (MSSQLDatabaseInfo)GenerateDatabaseModelFromDBMSSysInfo();

            GetAllDbObjectsFromDNDBTSysInfoQuery.ResultsInterpreter.ReplaceDbModelObjectsIDsWithRecordOnes(
                databaseInfo,
                QueryExecutor.Query<GetAllDbObjectsFromDNDBTSysInfoQuery.DNDBTDbObjectRecord>(
                    new GetAllDbObjectsFromDNDBTSysInfoQuery()));

            return databaseInfo;
        }

        public override DatabaseInfo GenerateDatabaseModelFromDBMSSysInfo()
        {
            Dictionary<string, TableInfo> tables =
                GetColumnsFromDBMSSysInfoQuery.ResultsInterpreter.BuildTablesListWithColumns(
                    QueryExecutor.Query<GetColumnsFromDBMSSysInfoQuery.ColumnRecord>(
                        new GetColumnsFromDBMSSysInfoQuery()));

            GetPrimaryKeysFromDBMSSysInfoQuery.ResultsInterpreter.BuildTablesPrimaryKeys(
                tables,
                QueryExecutor.Query<GetPrimaryKeysFromDBMSSysInfoQuery.PrimaryKeyRecord>(
                    new GetPrimaryKeysFromDBMSSysInfoQuery()));

            GetUniqueConstraintsFromDBMSSysInfoQuery.ResultsInterpreter.BuildTablesUniqueConstraints(
                tables,
                QueryExecutor.Query<GetUniqueConstraintsFromDBMSSysInfoQuery.UniqueConstraintRecord>(
                    new GetUniqueConstraintsFromDBMSSysInfoQuery()));

            GetForeignKeysFromDBMSSysInfoQuery.ResultsInterpreter.BuildTablesForeignKeys(
                tables,
                QueryExecutor.Query<GetForeignKeysFromDBMSSysInfoQuery.ForeignKeyRecord>(
                    new GetForeignKeysFromDBMSSysInfoQuery()));

            List<MSSQLUserDefinedTypeInfo> userDefinedTypes =
                GetTypesFromDBMSSysInfoQuery.ResultsInterpreter.BuildUserDefinedTypesList(
                    QueryExecutor.Query<GetTypesFromDBMSSysInfoQuery.UserDefinedTypeRecord>(
                        new GetTypesFromDBMSSysInfoQuery()));

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

        public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff)
        {
            MSSQLDatabaseDiff dbDiff = (MSSQLDatabaseDiff)databaseDiff;
            QueryExecutor.BeginTransaction();
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
                QueryExecutor.RollbackTransaction();
                throw;
            }
            QueryExecutor.CommitTransaction();
        }

        public override bool DNDBTSysTablesExist()
        {
            return QueryExecutor.QuerySingleOrDefault<bool>(new CheckDNDBTSysTablesExistQuery());
        }

        public override void CreateDNDBTSysTables()
        {
            QueryExecutor.Execute(new CreateDNDBTSysTablesQuery());
        }

        public override void DropDNDBTSysTables()
        {
            QueryExecutor.Execute(new DropDNDBTSysTablesQuery());
        }

        public override void PopulateDNDBTSysTables(DatabaseInfo database)
        {
            MSSQLDatabaseInfo db = (MSSQLDatabaseInfo)database;
            foreach (MSSQLUserDefinedTypeInfo udt in db.UserDefinedTypes)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(udt.ID, null, MSSQLDbObjectsTypes.UserDefinedType, udt.Name));
            foreach (MSSQLTableInfo table in db.Tables)
            {
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, null, MSSQLDbObjectsTypes.Table, table.Name));
                foreach (ColumnInfo column in table.Columns)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, table.ID, MSSQLDbObjectsTypes.Column, column.Name));
                PrimaryKeyInfo pk = table.PrimaryKey;
                if (pk is not null)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, table.ID, MSSQLDbObjectsTypes.PrimaryKey, pk.Name));
                foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, table.ID, MSSQLDbObjectsTypes.UniqueConstraint, uc.Name));
                foreach (CheckConstraintInfo cc in table.CheckConstraints)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, table.ID, MSSQLDbObjectsTypes.CheckConstraint, cc.Name));
                foreach (IndexInfo index in table.Indexes)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, table.ID, MSSQLDbObjectsTypes.Index, index.Name));
                foreach (TriggerInfo trigger in table.Triggers)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, table.ID, MSSQLDbObjectsTypes.Trigger, trigger.Name));
                foreach (ForeignKeyInfo fk in table.ForeignKeys)
                    QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, table.ID, MSSQLDbObjectsTypes.ForeignKey, fk.Name));
            }
            foreach (MSSQLUserDefinedTableTypeInfo udtt in db.UserDefinedTableTypes)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(udtt.ID, null, MSSQLDbObjectsTypes.UserDefinedTableType, udtt.Name));
            foreach (MSSQLFunctionInfo function in db.Functions)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(function.ID, null, MSSQLDbObjectsTypes.Function, function.Name));
            foreach (MSSQLViewInfo view in db.Views)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(view.ID, null, MSSQLDbObjectsTypes.View, view.Name));
            foreach (MSSQLProcedureInfo procedure in db.Procedures)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(procedure.ID, null, MSSQLDbObjectsTypes.Procedure, procedure.Name));
        }

        private void RenameUserDefinedTypeToTempInDbAndInDbDiff(MSSQLUserDefinedTypeInfo udt)
        {
            QueryExecutor.Execute(new RenameUserDefinedDataTypeQuery(udt));
            udt.Name = $"_DNDBTTemp_{udt.Name}";
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(udt.ID));
        }

        private void CreateUserDefinedType(MSSQLUserDefinedTypeInfo udt)
        {
            QueryExecutor.Execute(new CreateTypeQuery(udt));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(udt.ID, null, MSSQLDbObjectsTypes.UserDefinedType, udt.Name));
        }

        private void DropUserDefinedType(MSSQLUserDefinedTypeInfo udt)
        {
            QueryExecutor.Execute(new DropTypeQuery(udt));
        }

        private void UseNewUDTInAllTables(MSSQLUserDefinedTypeDiff udtDiff)
        {
            QueryExecutor.Execute(new UseNewUDTInAllTablesQuery(udtDiff));
        }

        private void CreateTable(MSSQLTableInfo table)
        {
            QueryExecutor.Execute(new CreateTableQuery(table));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(table.ID, null, MSSQLDbObjectsTypes.Table, table.Name));
            foreach (ColumnInfo column in table.Columns)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, table.ID, MSSQLDbObjectsTypes.Column, column.Name));
            PrimaryKeyInfo pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, table.ID, MSSQLDbObjectsTypes.PrimaryKey, pk.Name));
            foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, table.ID, MSSQLDbObjectsTypes.UniqueConstraint, uc.Name));
            foreach (CheckConstraintInfo cc in table.CheckConstraints)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, table.ID, MSSQLDbObjectsTypes.CheckConstraint, cc.Name));
            foreach (IndexInfo index in table.Indexes)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, table.ID, MSSQLDbObjectsTypes.Index, index.Name));
            foreach (TriggerInfo trigger in table.Triggers)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, table.ID, MSSQLDbObjectsTypes.Trigger, trigger.Name));
        }

        private void DropTable(MSSQLTableInfo table)
        {
            QueryExecutor.Execute(new DropTableQuery(table));
            foreach (TriggerInfo trigger in table.Triggers)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(trigger.ID));
            foreach (IndexInfo index in table.Indexes)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(index.ID));
            foreach (CheckConstraintInfo cc in table.CheckConstraints)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(cc.ID));
            foreach (UniqueConstraintInfo uc in table.UniqueConstraints)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(uc.ID));
            PrimaryKeyInfo pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(pk.ID));
            foreach (ColumnInfo column in table.Columns)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(column.ID));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(table.ID));
        }

        private void AlterTable(MSSQLTableDiff tableDiff)
        {
            QueryExecutor.Execute(new AlterTableQuery(tableDiff));

            foreach (TriggerInfo trigger in tableDiff.TriggersToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(trigger.ID));
            foreach (IndexInfo index in tableDiff.IndexesToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(index.ID));
            foreach (CheckConstraintInfo cc in tableDiff.CheckConstraintsToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(cc.ID));
            foreach (UniqueConstraintInfo uc in tableDiff.UniqueConstraintsToDrop)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(uc.ID));
            if (tableDiff.PrimaryKeyToDrop is not null)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(tableDiff.PrimaryKeyToDrop.ID));
            foreach (ColumnInfo column in tableDiff.RemovedColumns)
                QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(column.ID));

            QueryExecutor.Execute(new UpdateDNDBTSysInfoQuery(tableDiff.NewTable.ID, tableDiff.NewTable.Name));
            foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
                QueryExecutor.Execute(new UpdateDNDBTSysInfoQuery(columnDiff.NewColumn.ID, columnDiff.NewColumn.Name));

            foreach (ColumnInfo column in tableDiff.AddedColumns)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(column.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.Column, column.Name));
            PrimaryKeyInfo pk = tableDiff.PrimaryKeyToCreate;
            if (pk is not null)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(pk.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.PrimaryKey, pk.Name));
            foreach (UniqueConstraintInfo uc in tableDiff.UniqueConstraintsToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(uc.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.UniqueConstraint, uc.Name));
            foreach (CheckConstraintInfo cc in tableDiff.CheckConstraintsToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(cc.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.CheckConstraint, cc.Name));
            foreach (IndexInfo index in tableDiff.IndexesToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(index.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.Index, index.Name));
            foreach (TriggerInfo trigger in tableDiff.TriggersToCreate)
                QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(trigger.ID, tableDiff.NewTable.ID, MSSQLDbObjectsTypes.Trigger, trigger.Name));
        }

        private void CreateForeignKey(ForeignKeyInfo fk, Dictionary<Guid, TableInfo> fkToTableMap)
        {
            QueryExecutor.Execute(new CreateForeignKeyQuery(fk, fkToTableMap[fk.ID].Name));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(fk.ID, fkToTableMap[fk.ID].ID, MSSQLDbObjectsTypes.ForeignKey, fk.Name));
        }

        private void DropForeignKey(ForeignKeyInfo fk, Dictionary<Guid, TableInfo> fkToTableMap)
        {
            QueryExecutor.Execute(new DropForeignKeyQuery(fk, fkToTableMap[fk.ID].Name));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(fk.ID));
        }

        private void CreateUserDefinedTableType(MSSQLUserDefinedTableTypeInfo udtt)
        {
            //QueryExecutor.Execute(new CreateTableQuery(udtt));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(udtt.ID, null, MSSQLDbObjectsTypes.UserDefinedTableType, udtt.Name));
        }

        private void DropUserDefinedTableType(MSSQLUserDefinedTableTypeInfo udtt)
        {
            //QueryExecutor.Execute(new DropTableQuery(udtt));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(udtt.ID));
        }

        private void CreateFunction(MSSQLFunctionInfo function)
        {
            QueryExecutor.Execute(new GenericQuery($"{function.Code}"));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(function.ID, null, MSSQLDbObjectsTypes.Function, function.Name));
        }

        private void DropFunction(MSSQLFunctionInfo function)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP FUNCTION {function.Name};"));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(function.ID));
        }

        private void CreateView(MSSQLViewInfo view)
        {
            QueryExecutor.Execute(new GenericQuery($"{view.Code}"));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(view.ID, null, MSSQLDbObjectsTypes.View, view.Name));
        }

        private void DropView(MSSQLViewInfo view)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP VIEW {view.Name};"));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(view.ID));
        }

        private void CreateProcedure(MSSQLProcedureInfo procedure)
        {
            QueryExecutor.Execute(new GenericQuery($"{procedure.Code}"));
            QueryExecutor.Execute(new InsertDNDBTSysInfoQuery(procedure.ID, null, MSSQLDbObjectsTypes.Procedure, procedure.Name));
        }

        private void DropProcedure(MSSQLProcedureInfo procedure)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP PROCEDURE {procedure.Name};"));
            QueryExecutor.Execute(new DeleteDNDBTSysInfoQuery(procedure.ID));
        }
    }
}
