using DotNetDBTools.Deploy.Core;
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
        }

        public override void DropTable(Table table)
        {
            base.DropTable(table);
            foreach (ForeignKey fk in table.ForeignKeys)
                QueryExecutor.Execute(new SQLiteDeleteDNDBTSysInfoQuery(fk.ID));
        }

        public override void AlterTable(TableDiff tableDiff)
        {
            base.AlterTable(tableDiff);
            foreach (ForeignKey fk in tableDiff.ForeignKeysToDrop)
                QueryExecutor.Execute(new SQLiteDeleteDNDBTSysInfoQuery(fk.ID));
            foreach (ForeignKey fk in tableDiff.ForeignKeysToCreate)
                QueryExecutor.Execute(new SQLiteInsertDNDBTSysInfoQuery(fk.ID, tableDiff.NewTable.ID, DbObjectsTypes.ForeignKey, fk.Name));
        }
    }
}
