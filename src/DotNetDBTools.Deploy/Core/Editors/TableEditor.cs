using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.Core.InstanceCreator;

namespace DotNetDBTools.Deploy.Core.Editors
{
    internal abstract class TableEditor<
        TInsertDNDBTSysInfoQuery,
        TDeleteDNDBTSysInfoQuery,
        TUpdateDNDBTSysInfoQuery,
        TCreateTableQuery,
        TDropTableQuery,
        TAlterTableQuery>
        : ITableEditor
        where TInsertDNDBTSysInfoQuery : InsertDNDBTSysInfoQuery
        where TDeleteDNDBTSysInfoQuery : DeleteDNDBTSysInfoQuery
        where TUpdateDNDBTSysInfoQuery : UpdateDNDBTSysInfoQuery
        where TCreateTableQuery : CreateTableQuery
        where TDropTableQuery : DropTableQuery
        where TAlterTableQuery : AlterTableQuery
    {
        protected readonly IQueryExecutor QueryExecutor;

        protected TableEditor(IQueryExecutor queryExecutor)
        {
            QueryExecutor = queryExecutor;
        }

        public void CreateTables(DatabaseDiff dbDiff)
        {
            foreach (Table table in dbDiff.AddedTables)
                CreateTable(table);
        }

        public void DropTables(DatabaseDiff dbDiff)
        {
            foreach (Table table in dbDiff.RemovedTables)
                DropTable(table);
        }

        public void AlterTables(DatabaseDiff dbDiff)
        {
            foreach (TableDiff tableDiff in dbDiff.ChangedTables)
                AlterTable(tableDiff);
        }

        protected virtual void CreateTable(Table table)
        {
            QueryExecutor.Execute(Create<TCreateTableQuery>(table));
            QueryExecutor.Execute(Create<TInsertDNDBTSysInfoQuery>(table.ID, null, DbObjectsTypes.Table, table.Name));
            foreach (Column c in table.Columns)
                QueryExecutor.Execute(Create<TInsertDNDBTSysInfoQuery>(c.ID, table.ID, DbObjectsTypes.Column, c.Name, c.GetCode()));
            PrimaryKey pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(Create<TInsertDNDBTSysInfoQuery>(pk.ID, table.ID, DbObjectsTypes.PrimaryKey, pk.Name));
            foreach (UniqueConstraint uc in table.UniqueConstraints)
                QueryExecutor.Execute(Create<TInsertDNDBTSysInfoQuery>(uc.ID, table.ID, DbObjectsTypes.UniqueConstraint, uc.Name));
            foreach (CheckConstraint ck in table.CheckConstraints)
                QueryExecutor.Execute(Create<TInsertDNDBTSysInfoQuery>(ck.ID, table.ID, DbObjectsTypes.CheckConstraint, ck.Name, ck.GetCode()));
        }

        protected virtual void DropTable(Table table)
        {
            QueryExecutor.Execute(Create<TDropTableQuery>(table));
            foreach (CheckConstraint ck in table.CheckConstraints)
                QueryExecutor.Execute(Create<TDeleteDNDBTSysInfoQuery>(ck.ID));
            foreach (UniqueConstraint uc in table.UniqueConstraints)
                QueryExecutor.Execute(Create<TDeleteDNDBTSysInfoQuery>(uc.ID));
            PrimaryKey pk = table.PrimaryKey;
            if (pk is not null)
                QueryExecutor.Execute(Create<TDeleteDNDBTSysInfoQuery>(pk.ID));
            foreach (Column column in table.Columns)
                QueryExecutor.Execute(Create<TDeleteDNDBTSysInfoQuery>(column.ID));
            QueryExecutor.Execute(Create<TDeleteDNDBTSysInfoQuery>(table.ID));
        }

        protected virtual void AlterTable(TableDiff tableDiff)
        {
            QueryExecutor.Execute(Create<TAlterTableQuery>(tableDiff));

            foreach (CheckConstraint ck in tableDiff.CheckConstraintsToDrop)
                QueryExecutor.Execute(Create<TDeleteDNDBTSysInfoQuery>(ck.ID));
            foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
                QueryExecutor.Execute(Create<TDeleteDNDBTSysInfoQuery>(uc.ID));
            if (tableDiff.PrimaryKeyToDrop is not null)
                QueryExecutor.Execute(Create<TDeleteDNDBTSysInfoQuery>(tableDiff.PrimaryKeyToDrop.ID));
            foreach (Column column in tableDiff.RemovedColumns)
                QueryExecutor.Execute(Create<TDeleteDNDBTSysInfoQuery>(column.ID));

            QueryExecutor.Execute(Create<TUpdateDNDBTSysInfoQuery>(tableDiff.NewTable.ID, tableDiff.NewTable.Name));
            foreach (ColumnDiff cDiff in tableDiff.ChangedColumns)
                QueryExecutor.Execute(Create<TUpdateDNDBTSysInfoQuery>(cDiff.NewColumn.ID, cDiff.NewColumn.Name, cDiff.NewColumn.GetCode()));

            foreach (Column c in tableDiff.AddedColumns)
                QueryExecutor.Execute(Create<TInsertDNDBTSysInfoQuery>(c.ID, tableDiff.NewTable.ID, DbObjectsTypes.Column, c.Name, c.GetCode()));
            PrimaryKey pk = tableDiff.PrimaryKeyToCreate;
            if (pk is not null)
                QueryExecutor.Execute(Create<TInsertDNDBTSysInfoQuery>(pk.ID, tableDiff.NewTable.ID, DbObjectsTypes.PrimaryKey, pk.Name));
            foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
                QueryExecutor.Execute(Create<TInsertDNDBTSysInfoQuery>(uc.ID, tableDiff.NewTable.ID, DbObjectsTypes.UniqueConstraint, uc.Name));
            foreach (CheckConstraint ck in tableDiff.CheckConstraintsToCreate)
                QueryExecutor.Execute(Create<TInsertDNDBTSysInfoQuery>(ck.ID, tableDiff.NewTable.ID, DbObjectsTypes.CheckConstraint, ck.Name, ck.GetCode()));
        }
    }
}
