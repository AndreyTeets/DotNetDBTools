﻿using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.SQLite.Queries.DDL;
using DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite.Editors
{
    internal class SQLiteTableEditor : TableEditor<
        SQLiteInsertDNDBTSysInfoQuery,
        SQLiteDeleteDNDBTSysInfoQuery,
        SQLiteUpdateDNDBTSysInfoQuery,
        SQLiteCreateTableQuery,
        SQLiteDropTableQuery,
        SQLiteAlterTableQuery>
    {
        public SQLiteTableEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }

        public override void CreateTable(Table table)
        {
            base.CreateTable(table);
            foreach (ForeignKey fk in table.ForeignKeys)
                QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(fk.ID, table.ID, DbObjectsTypes.ForeignKey, fk.Name));
            foreach (Trigger trg in table.Triggers)
                QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(trg.ID, table.ID, DbObjectsTypes.Trigger, trg.Name, trg.GetCode()));
            foreach (Index idx in table.Indexes)
                QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(idx.ID, table.ID, DbObjectsTypes.Index, idx.Name));
        }

        public override void DropTable(Table table)
        {
            base.DropTable(table);
            foreach (Index idx in table.Indexes)
                QueryExecutor.Execute(new SQLiteDeleteDNDBTSysInfoQuery(idx.ID));
            foreach (Trigger trg in table.Triggers)
                QueryExecutor.Execute(new SQLiteDeleteDNDBTSysInfoQuery(trg.ID));
            foreach (ForeignKey fk in table.ForeignKeys)
                QueryExecutor.Execute(new SQLiteDeleteDNDBTSysInfoQuery(fk.ID));
        }

        public override void AlterTable(TableDiff tableDiff)
        {
            base.AlterTable(tableDiff);

            foreach (Index idx in tableDiff.IndexesToDrop)
                QueryExecutor.Execute(new SQLiteDeleteDNDBTSysInfoQuery(idx.ID));
            foreach (Trigger trg in tableDiff.TriggersToDrop)
                QueryExecutor.Execute(new SQLiteDeleteDNDBTSysInfoQuery(trg.ID));
            foreach (ForeignKey fk in tableDiff.ForeignKeysToDrop)
                QueryExecutor.Execute(new SQLiteDeleteDNDBTSysInfoQuery(fk.ID));

            foreach (ForeignKey fk in tableDiff.ForeignKeysToCreate)
                QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(fk.ID, tableDiff.NewTable.ID, DbObjectsTypes.ForeignKey, fk.Name));
            foreach (Trigger trg in tableDiff.TriggersToCreate)
                QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(trg.ID, tableDiff.NewTable.ID, DbObjectsTypes.Trigger, trg.Name, trg.GetCode()));
            foreach (Index idx in tableDiff.IndexesToCreate)
                QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(idx.ID, tableDiff.NewTable.ID, DbObjectsTypes.Index, idx.Name));
        }
    }
}