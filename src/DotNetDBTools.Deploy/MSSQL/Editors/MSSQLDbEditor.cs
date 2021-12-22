using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.MSSQL.Queries.DDL;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Editors
{
    internal class MSSQLDbEditor : DbEditor<
        MSSQLCheckDNDBTSysTablesExistQuery,
        MSSQLCreateDNDBTSysTablesQuery,
        MSSQLDropDNDBTSysTablesQuery>
    {
        private readonly ITableEditor _tableEditor;
        private readonly IForeignKeyEditor _foreignKeyEditor;

        public MSSQLDbEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor)
        {
            _tableEditor = new MSSQLTableEditor(queryExecutor);
            _foreignKeyEditor = new MSSQLForeignKeyEditor(queryExecutor);
        }

        public override void PopulateDNDBTSysTables(Database database)
        {
            MSSQLDatabase db = (MSSQLDatabase)database;
            foreach (MSSQLUserDefinedType udt in db.UserDefinedTypes)
                QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(udt.ID, null, DbObjectsTypes.UserDefinedType, udt.Name));
            foreach (MSSQLTable table in db.Tables)
            {
                QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(table.ID, null, DbObjectsTypes.Table, table.Name));
                foreach (Column c in table.Columns)
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(c.ID, table.ID, DbObjectsTypes.Column, c.Name, c.GetExtraInfo()));
                PrimaryKey pk = table.PrimaryKey;
                if (pk is not null)
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(pk.ID, table.ID, DbObjectsTypes.PrimaryKey, pk.Name));
                foreach (UniqueConstraint uc in table.UniqueConstraints)
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(uc.ID, table.ID, DbObjectsTypes.UniqueConstraint, uc.Name));
                foreach (CheckConstraint ck in table.CheckConstraints)
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(ck.ID, table.ID, DbObjectsTypes.CheckConstraint, ck.Name, ck.GetExtraInfo()));
                foreach (Index index in table.Indexes)
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(index.ID, table.ID, DbObjectsTypes.Index, index.Name));
                foreach (Trigger trigger in table.Triggers)
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(trigger.ID, table.ID, DbObjectsTypes.Trigger, trigger.Name));
                foreach (ForeignKey fk in table.ForeignKeys)
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(fk.ID, table.ID, DbObjectsTypes.ForeignKey, fk.Name));
            }
            foreach (MSSQLUserDefinedTableType udtt in db.UserDefinedTableTypes)
                QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(udtt.ID, null, DbObjectsTypes.UserDefinedTableType, udtt.Name));
            foreach (MSSQLFunction function in db.Functions)
                QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(function.ID, null, DbObjectsTypes.Function, function.Name));
            foreach (MSSQLView view in db.Views)
                QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name));
            foreach (MSSQLProcedure procedure in db.Procedures)
                QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(procedure.ID, null, DbObjectsTypes.Procedure, procedure.Name));
        }

        public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff)
        {
            MSSQLDatabaseDiff dbDiff = (MSSQLDatabaseDiff)databaseDiff;
            QueryExecutor.BeginTransaction();
            try
            {
                Dictionary<Guid, Table> newDbFKToTableMap = ForeignKeysHelper.CreateFKToTableMap(dbDiff.NewDatabase.Tables);
                Dictionary<Guid, Table> oldDbFKToTableMap = ForeignKeysHelper.CreateFKToTableMap(dbDiff.OldDatabase.Tables);

                foreach (MSSQLProcedure procedure in dbDiff.ProceduresToDrop)
                    DropProcedure(procedure);
                foreach (MSSQLView view in dbDiff.ViewsToDrop)
                    DropView(view);
                foreach (MSSQLFunction function in dbDiff.FunctionsToDrop)
                    DropFunction(function);
                foreach (MSSQLUserDefinedTableType udtt in dbDiff.UserDefinedTableTypesToDrop)
                    DropUserDefinedTableType(udtt);
                foreach (ForeignKey fk in dbDiff.AllForeignKeysToDrop)
                    _foreignKeyEditor.DropForeignKey(fk, oldDbFKToTableMap);
                foreach (MSSQLTable table in dbDiff.RemovedTables)
                    _tableEditor.DropTable(table);

                foreach (MSSQLUserDefinedType udt in dbDiff.RemovedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.OldUserDefinedType)))
                    RenameUserDefinedTypeToTempInDbAndInDbDiff(udt);
                foreach (MSSQLUserDefinedType udt in dbDiff.AddedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.NewUserDefinedType)))
                    CreateUserDefinedType(udt);
                foreach (MSSQLUserDefinedTypeDiff udtDiff in dbDiff.ChangedUserDefinedTypes)
                    UseNewUDTInAllTables(udtDiff);
                foreach (MSSQLTableDiff tableDiff in dbDiff.ChangedTables)
                    _tableEditor.AlterTable(tableDiff);
                foreach (MSSQLUserDefinedType udt in dbDiff.RemovedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.OldUserDefinedType)))
                    DropUserDefinedType(udt);

                foreach (MSSQLTable table in dbDiff.AddedTables)
                    _tableEditor.CreateTable(table);
                foreach (ForeignKey fk in dbDiff.AllForeignKeysToCreate)
                    _foreignKeyEditor.CreateForeignKey(fk, newDbFKToTableMap);
                foreach (MSSQLUserDefinedTableType udtt in dbDiff.UserDefinedTableTypesToCreate)
                    CreateUserDefinedTableType(udtt);
                foreach (MSSQLFunction function in dbDiff.FunctionsToCreate)
                    CreateFunction(function);
                foreach (MSSQLView view in dbDiff.ViewsToCreate)
                    CreateView(view);
                foreach (MSSQLProcedure procedure in dbDiff.ProceduresToCreate)
                    CreateProcedure(procedure);
            }
            catch (Exception)
            {
                QueryExecutor.RollbackTransaction();
                throw;
            }
            QueryExecutor.CommitTransaction();
        }

        private void RenameUserDefinedTypeToTempInDbAndInDbDiff(MSSQLUserDefinedType udt)
        {
            QueryExecutor.Execute(new MSSQLRenameUserDefinedDataTypeQuery(udt));
            udt.Name = $"_DNDBTTemp_{udt.Name}";
            QueryExecutor.Execute(new MSSQLDeleteDNDBTSysInfoQuery(udt.ID));
        }

        private void CreateUserDefinedType(MSSQLUserDefinedType udt)
        {
            QueryExecutor.Execute(new MSSQLCreateTypeQuery(udt));
            QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(udt.ID, null, DbObjectsTypes.UserDefinedType, udt.Name));
        }

        private void DropUserDefinedType(MSSQLUserDefinedType udt)
        {
            QueryExecutor.Execute(new MSSQLDropTypeQuery(udt));
        }

        private void UseNewUDTInAllTables(MSSQLUserDefinedTypeDiff udtDiff)
        {
            QueryExecutor.Execute(new MSSQLUseNewUDTInAllTablesQuery(udtDiff));
        }

        private void CreateUserDefinedTableType(MSSQLUserDefinedTableType udtt)
        {
            //QueryExecutor.Execute(new CreateTableQuery(udtt));
            QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(udtt.ID, null, DbObjectsTypes.UserDefinedTableType, udtt.Name));
        }

        private void DropUserDefinedTableType(MSSQLUserDefinedTableType udtt)
        {
            //QueryExecutor.Execute(new DropTableQuery(udtt));
            QueryExecutor.Execute(new MSSQLDeleteDNDBTSysInfoQuery(udtt.ID));
        }

        private void CreateFunction(MSSQLFunction function)
        {
            QueryExecutor.Execute(new GenericQuery($"{function.Code}"));
            QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(function.ID, null, DbObjectsTypes.Function, function.Name));
        }

        private void DropFunction(MSSQLFunction function)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP FUNCTION {function.Name};"));
            QueryExecutor.Execute(new MSSQLDeleteDNDBTSysInfoQuery(function.ID));
        }

        private void CreateView(MSSQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($"{view.Code}"));
            QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name));
        }

        private void DropView(MSSQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP VIEW {view.Name};"));
            QueryExecutor.Execute(new MSSQLDeleteDNDBTSysInfoQuery(view.ID));
        }

        private void CreateProcedure(MSSQLProcedure procedure)
        {
            QueryExecutor.Execute(new GenericQuery($"{procedure.Code}"));
            QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(procedure.ID, null, DbObjectsTypes.Procedure, procedure.Name));
        }

        private void DropProcedure(MSSQLProcedure procedure)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP PROCEDURE {procedure.Name};"));
            QueryExecutor.Execute(new MSSQLDeleteDNDBTSysInfoQuery(procedure.ID));
        }
    }
}
