using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DeployInteractor.SQLite
{
    public class SQLiteInteractor
    {
        private readonly IQueryExecutor _queryExecutor;

        public SQLiteInteractor(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }


        public void UpdateDatabase(SQLiteDatabaseInfo database, SQLiteDatabaseInfo existingDatabase)
        {
            foreach (SQLiteTableInfo table in database.Tables)
            {
                SQLiteTableInfo existingTable = (SQLiteTableInfo)existingDatabase.Tables.FirstOrDefault(x => x.ID == table.ID);
                if (existingTable is null)
                    CreateTable(table);
                else
                    AlterTable(table, existingTable);
            }

            foreach (SQLiteViewInfo view in database.Views)
            {
                if (existingDatabase.Views.Any(x => x.ID == view.ID))
                    DropView(view);
                CreateView(view);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        public SQLiteDatabaseInfo GetExistingDatabase()
        {
            SQLiteDatabaseInfo databaseInfo = new();
            object tables = _queryExecutor.Execute(Queries.GetExistingTables);
            // foreach table in tables existingTable = SQLiteDbObjectsSerializer.TableFromJson(tableMetadata)
            SQLiteTableInfo existingTable = new()
            {
                ID = new Guid("299675E6-4FAA-4D0F-A36A-224306BA5BCB"),
                Name = "OldMyTable1Name",
                Columns = new List<SQLiteColumnInfo>()
                {
                    new SQLiteColumnInfo
                    {
                        ID = new Guid("A2F2A4DE-1337-4594-AE41-72ED4D05F317"),
                        Name = "OldMyColumn1Name",
                        DataType = "",
                        DefaultValue = 1,
                    }
                }
            };
            databaseInfo.Tables = new List<SQLiteTableInfo> { existingTable };
            return databaseInfo;
        }

        private void CreateTable(SQLiteTableInfo table)
        {
            string tableMetadata = SQLiteDbObjectsSerializer.TableToJson(table);
            _queryExecutor.Execute(Queries.CreateTable(table),
                new QueryParameter($"@{DNDBTSysTables.DNDBTDbObjects.Metadata}", tableMetadata));
        }
        private void AlterTable(SQLiteTableInfo table, SQLiteTableInfo existingTable)
        {
            Console.WriteLine($"AlterTable: ID={table.ID} Name={table.Name}");

            foreach (SQLiteColumnInfo column in table.Columns)
            {
                if (existingTable.Columns.Any(x => x.ID == column.ID))
                {
                    SQLiteColumnInfo existingColumn = (SQLiteColumnInfo)existingTable.Columns.Single(x => x.ID == column.ID);
                    if (column != existingColumn)
                        AlterColumn(table, column);
                }
                else
                {
                    AddColumn(table, column);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void DropTable(SQLiteTableInfo table)
        {
            Console.WriteLine($"DropTable: ID={table.ID} Name={table.Name}");
        }

        private void AddColumn(SQLiteTableInfo table, SQLiteColumnInfo column)
        {
            Console.WriteLine($"AddColumn: ID={column.ID} TableName={table.Name} ColumnName={column.Name} ColumnType={column.DataType} DefaultValue={column.DefaultValue}");
        }
        private void AlterColumn(SQLiteTableInfo table, SQLiteColumnInfo column)
        {
            Console.WriteLine($"AlterColumn: ID={column.ID} TableName={table.Name} ColumnName={column.Name} ColumnType={column.DataType} DefaultValue={column.DefaultValue}");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void DropColumn(SQLiteTableInfo table, SQLiteColumnInfo column)
        {
            Console.WriteLine($"DropColumn: ID={column.ID} TableName={table.Name} ColumnName={column.Name} ColumnType={column.DataType} DefaultValue={column.DefaultValue}");
        }

        private void CreateView(SQLiteViewInfo view)
        {
            Console.WriteLine($"CreateView: ID={view.ID} Name={view.Name} Code={view.Code}");
        }
        private void DropView(SQLiteViewInfo view)
        {
            Console.WriteLine($"DropView: ID={view.ID} Name={view.Name} Code={view.Code}");
        }
    }
}
