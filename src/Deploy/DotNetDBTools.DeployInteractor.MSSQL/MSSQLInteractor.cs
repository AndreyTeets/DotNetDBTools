using System;
using System.Collections.Generic;
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
            foreach (MSSQLTableInfo table in databaseDiff.AddedTables)
                CreateTable(table);
            foreach (MSSQLTableInfo table in databaseDiff.RemovedTables)
                DropTable(table);
            foreach (MSSQLTableDiff tableDiff in databaseDiff.ChangedTables)
                AlterTable(tableDiff);

            foreach (MSSQLViewInfo view in databaseDiff.AddedViews)
                CreateView(view);
            foreach (MSSQLViewInfo view in databaseDiff.RemovedViews)
                DropView(view);
            foreach (MSSQLViewDiff viewDiff in databaseDiff.ChangedViews)
                AlterView(viewDiff);

            foreach (MSSQLFunctionInfo function in databaseDiff.AddedFunctions)
                CreateFunction(function);
            foreach (MSSQLFunctionInfo function in databaseDiff.RemovedFunctions)
                DropFunction(function);
            foreach (MSSQLFunctionDiff functionDiff in databaseDiff.ChangedFunctions)
                AlterFunction(functionDiff);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        public MSSQLDatabaseInfo GetExistingDatabase()
        {
            MSSQLDatabaseInfo databaseInfo = new();
            object tables = _queryExecutor.Execute(Queries.GetExistingTables);
            // foreach table in tables existingTable = MSSQLDbObjectsSerializer.TableFromJson(tableMetadata)
            MSSQLTableInfo existingTable = new()
            {
                ID = new Guid("299675E6-4FAA-4D0F-A36A-224306BA5BCB"),
                Name = "OldMyTable1Name",
                Columns = new List<MSSQLColumnInfo>()
                {
                    new MSSQLColumnInfo
                    {
                        ID = new Guid("A2F2A4DE-1337-4594-AE41-72ED4D05F317"),
                        Name = "OldMyColumn1Name",
                        DataType = "",
                        DefaultValue = 1,
                    }
                }
            };
            databaseInfo.Tables = new List<MSSQLTableInfo> { existingTable };
            return databaseInfo;
        }

        private void CreateTable(MSSQLTableInfo table)
        {
            string tableMetadata = MSSQLDbObjectsSerializer.TableToJson(table);
            _queryExecutor.Execute(Queries.CreateTable(table),
                new QueryParameter($"@{DNDBTSysTables.DNDBTDbObjects.Metadata}", tableMetadata));
        }

        private void DropTable(MSSQLTableInfo table)
        {
            _queryExecutor.Execute(Queries.DropTable(table));
        }

        private void AlterTable(MSSQLTableDiff tableDiff)
        {
            if (tableDiff.NewTable.Name != tableDiff.OldTable.Name)
                _queryExecutor.Execute($"sp_rename '{tableDiff.OldTable.Name}', '{tableDiff.NewTable.Name}'");

            foreach (MSSQLColumnInfo column in tableDiff.AddedColumns)
                AddColumn(tableDiff.NewTable.Name, column);

            foreach (MSSQLColumnInfo column in tableDiff.RemovedColumns)
                DropColumn(tableDiff.NewTable.Name, column);

            foreach (MSSQLColumnDiff columnDiff in tableDiff.ChangedColumns)
                AlterColumn(tableDiff.NewTable.Name, columnDiff);
        }

        private void AddColumn(string tableName, MSSQLColumnInfo column)
        {
            _queryExecutor.Execute($"alter table {tableName} add {column.Name} INT NOT NULL");
        }

        private void DropColumn(string tableName, MSSQLColumnInfo column)
        {
            _queryExecutor.Execute($"alter table {tableName} drop column {column.Name}");
        }

        private void AlterColumn(string tableName, MSSQLColumnDiff columnDiff)
        {
            if (columnDiff.NewColumn.Name != columnDiff.OldColumn.Name)
                _queryExecutor.Execute($"sp_rename '{tableName}.{columnDiff.OldColumn.Name}', '{columnDiff.NewColumn.Name}', 'COLUMN'");

            _queryExecutor.Execute($"alter table {tableName} alter column {columnDiff.NewColumn.Name} INT NOT NULL");
        }

        private void CreateView(MSSQLViewInfo view)
        {
            _queryExecutor.Execute($"create view {view.Name} {view.Code}");
        }

        private void DropView(MSSQLViewInfo view)
        {
            _queryExecutor.Execute($"drop view {view.Name}");
        }

        private void AlterView(MSSQLViewDiff viewDiff)
        {
            DropView(viewDiff.OldView);
            CreateView(viewDiff.NewView);
        }

        private void CreateFunction(MSSQLFunctionInfo function)
        {
            _queryExecutor.Execute($"create function {function.Name} {function.Code}");
        }

        private void DropFunction(MSSQLFunctionInfo function)
        {
            _queryExecutor.Execute($"drop function {function.Name}");
        }

        private void AlterFunction(MSSQLFunctionDiff functionDiff)
        {
            DropFunction(functionDiff.OldFunction);
            CreateFunction(functionDiff.NewFunction);
        }
    }
}
