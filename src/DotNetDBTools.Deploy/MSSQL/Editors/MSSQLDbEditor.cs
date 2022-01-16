using System;
using System.Linq;
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
        private readonly IIndexEditor _indexEditor;
        private readonly ITriggerEditor _triggerEditor;
        private readonly IForeignKeyEditor _foreignKeyEditor;

        public MSSQLDbEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor)
        {
            _tableEditor = new MSSQLTableEditor(queryExecutor);
            _indexEditor = new MSSQLIndexEditor(queryExecutor);
            _triggerEditor = new MSSQLTriggerEditor(queryExecutor);
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
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(c.ID, table.ID, DbObjectsTypes.Column, c.Name, c.GetCode()));
                PrimaryKey pk = table.PrimaryKey;
                if (pk is not null)
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(pk.ID, table.ID, DbObjectsTypes.PrimaryKey, pk.Name));
                foreach (UniqueConstraint uc in table.UniqueConstraints)
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(uc.ID, table.ID, DbObjectsTypes.UniqueConstraint, uc.Name));
                foreach (CheckConstraint ck in table.CheckConstraints)
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(ck.ID, table.ID, DbObjectsTypes.CheckConstraint, ck.Name, ck.GetCode()));
                foreach (Index idx in table.Indexes)
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(idx.ID, table.ID, DbObjectsTypes.Index, idx.Name));
                foreach (Trigger trg in table.Triggers)
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(trg.ID, table.ID, DbObjectsTypes.Trigger, trg.Name, trg.GetCode()));
                foreach (ForeignKey fk in table.ForeignKeys)
                    QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(fk.ID, table.ID, DbObjectsTypes.ForeignKey, fk.Name));
            }
            foreach (MSSQLUserDefinedTableType udtt in db.UserDefinedTableTypes)
                QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(udtt.ID, null, DbObjectsTypes.UserDefinedTableType, udtt.Name));
            foreach (MSSQLFunction func in db.Functions)
                QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(func.ID, null, DbObjectsTypes.Function, func.Name, func.GetCode()));
            foreach (MSSQLView view in db.Views)
                QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name, view.GetCode()));
            foreach (MSSQLProcedure proc in db.Procedures)
                QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(proc.ID, null, DbObjectsTypes.Procedure, proc.Name, proc.GetCode()));
        }

        public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff, DeployOptions options)
        {
            MSSQLDatabaseDiff dbDiff = (MSSQLDatabaseDiff)databaseDiff;
            QueryExecutor.BeginTransaction();
            try
            {
                _triggerEditor.DropTriggers(dbDiff);
                foreach (MSSQLProcedure procedure in dbDiff.ProceduresToDrop)
                    DropProcedure(procedure);
                foreach (MSSQLView view in dbDiff.ViewsToDrop)
                    DropView(view);
                foreach (MSSQLFunction function in dbDiff.FunctionsToDrop)
                    DropFunction(function);
                foreach (MSSQLUserDefinedTableType udtt in dbDiff.UserDefinedTableTypesToDrop)
                    DropUserDefinedTableType(udtt);
                _foreignKeyEditor.DropForeignKeys(dbDiff);
                _indexEditor.DropIndexes(dbDiff);
                _tableEditor.DropTables(dbDiff);

                foreach (MSSQLUserDefinedType udt in dbDiff.RemovedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.OldUserDefinedType)))
                    RenameUserDefinedTypeToTempInDbAndInDbDiff(udt);
                foreach (MSSQLUserDefinedType udt in dbDiff.AddedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.NewUserDefinedType)))
                    CreateUserDefinedType(udt);
                foreach (MSSQLUserDefinedTypeDiff udtDiff in dbDiff.ChangedUserDefinedTypes)
                    UseNewUDTInAllTables(udtDiff);
                _tableEditor.AlterTables(dbDiff);
                foreach (MSSQLUserDefinedType udt in dbDiff.RemovedUserDefinedTypes.Concat(dbDiff.ChangedUserDefinedTypes.Select(x => x.OldUserDefinedType)))
                    DropUserDefinedType(udt);

                _tableEditor.CreateTables(dbDiff);
                _indexEditor.CreateIndexes(dbDiff);
                _foreignKeyEditor.CreateForeignKeys(dbDiff);
                foreach (MSSQLUserDefinedTableType udtt in dbDiff.UserDefinedTableTypesToCreate)
                    CreateUserDefinedTableType(udtt);
                foreach (MSSQLFunction function in dbDiff.FunctionsToCreate)
                    CreateFunction(function);
                foreach (MSSQLView view in dbDiff.ViewsToCreate)
                    CreateView(view);
                foreach (MSSQLProcedure procedure in dbDiff.ProceduresToCreate)
                    CreateProcedure(procedure);
                _triggerEditor.CreateTriggers(dbDiff);
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

        private void CreateFunction(MSSQLFunction func)
        {
            QueryExecutor.Execute(new GenericQuery($"{func.GetCode()}"));
            QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(func.ID, null, DbObjectsTypes.Function, func.Name, func.GetCode()));
        }

        private void DropFunction(MSSQLFunction func)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP FUNCTION {func.Name};"));
            QueryExecutor.Execute(new MSSQLDeleteDNDBTSysInfoQuery(func.ID));
        }

        private void CreateView(MSSQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($"{view.GetCode()}"));
            QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name, view.GetCode()));
        }

        private void DropView(MSSQLView view)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP VIEW {view.Name};"));
            QueryExecutor.Execute(new MSSQLDeleteDNDBTSysInfoQuery(view.ID));
        }

        private void CreateProcedure(MSSQLProcedure proc)
        {
            QueryExecutor.Execute(new GenericQuery($"{proc.GetCode()}"));
            QueryExecutor.Execute(new MSSQLInsertDNDBTSysInfoQuery(proc.ID, null, DbObjectsTypes.Procedure, proc.Name, proc.GetCode()));
        }

        private void DropProcedure(MSSQLProcedure proc)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP PROCEDURE {proc.Name};"));
            QueryExecutor.Execute(new MSSQLDeleteDNDBTSysInfoQuery(proc.ID));
        }
    }
}
