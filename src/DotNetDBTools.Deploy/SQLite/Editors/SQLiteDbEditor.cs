using System;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.Core.Queries;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Editors
{
    internal class SQLiteDbEditor : DbEditor<
        SQLiteCheckDNDBTSysTablesExistQuery,
        SQLiteCreateDNDBTSysTablesQuery,
        SQLiteDropDNDBTSysTablesQuery>
    {
        private readonly ITableEditor _tableEditor;

        public SQLiteDbEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor)
        {
            _tableEditor = new SQLiteTableEditor(queryExecutor);
        }

        public override void PopulateDNDBTSysTables(Database database)
        {
            SQLiteDatabase db = (SQLiteDatabase)database;
            foreach (SQLiteTable table in db.Tables)
            {
                QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(table.ID, null, DbObjectsTypes.Table, table.Name));
                foreach (Column c in table.Columns)
                    QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(c.ID, table.ID, DbObjectsTypes.Column, c.Name, c.GetExtraInfo()));
                PrimaryKey pk = table.PrimaryKey;
                if (pk is not null)
                    QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(pk.ID, table.ID, DbObjectsTypes.PrimaryKey, pk.Name));
                foreach (UniqueConstraint uc in table.UniqueConstraints)
                    QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(uc.ID, table.ID, DbObjectsTypes.UniqueConstraint, uc.Name));
                foreach (CheckConstraint cc in table.CheckConstraints)
                    QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(cc.ID, table.ID, DbObjectsTypes.CheckConstraint, cc.Name));
                foreach (Index index in table.Indexes)
                    QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(index.ID, table.ID, DbObjectsTypes.Index, index.Name));
                foreach (Trigger trigger in table.Triggers)
                    QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(trigger.ID, table.ID, DbObjectsTypes.Trigger, trigger.Name));
                foreach (ForeignKey fk in table.ForeignKeys)
                    QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(fk.ID, table.ID, DbObjectsTypes.ForeignKey, fk.Name));
            }
            foreach (SQLiteView view in db.Views)
                QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name));
        }

        public override void ApplyDatabaseDiff(DatabaseDiff databaseDiff)
        {
            SQLiteDatabaseDiff dbDiff = (SQLiteDatabaseDiff)databaseDiff;
            QueryExecutor.BeginTransaction();
            try
            {
                foreach (SQLiteView view in dbDiff.ViewsToDrop)
                    DropView(view);
                foreach (SQLiteTable table in dbDiff.RemovedTables)
                    _tableEditor.DropTable(table);
                foreach (SQLiteTableDiff tableDiff in dbDiff.ChangedTables)
                    _tableEditor.AlterTable(tableDiff);
                foreach (SQLiteTable table in dbDiff.AddedTables)
                    _tableEditor.CreateTable(table);
                foreach (SQLiteView view in dbDiff.ViewsToCreate)
                    CreateView(view);
            }
            catch (Exception)
            {
                QueryExecutor.RollbackTransaction();
                throw;
            }
            QueryExecutor.CommitTransaction();
        }

        private void CreateView(SQLiteView view)
        {
            QueryExecutor.Execute(new GenericQuery($"{view.Code}"));
            QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(view.ID, null, DbObjectsTypes.View, view.Name));
        }

        private void DropView(SQLiteView view)
        {
            QueryExecutor.Execute(new GenericQuery($"DROP VIEW {view.Name};"));
            QueryExecutor.Execute(new SQLiteDeleteDNDBTSysInfoQuery(view.ID));
        }
    }
}
