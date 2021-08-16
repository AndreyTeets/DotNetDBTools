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

        public bool DatabaseExists()
        {
            bool databaseExists = _queryExecutor.QuerySingleOrDefault<bool>(Queries.DatabaseExists);
            return databaseExists;
        }

        public void CreateEmptyDatabase()
        {
            _queryExecutor.Execute(Queries.CreateEmptyDatabase);
        }

        public SQLiteDatabaseInfo GetExistingDatabase()
        {
            SQLiteDatabaseInfo databaseInfo = new();
            List<SQLiteTableInfo> tables = new();
            IEnumerable<string> tablesMetadatas = _queryExecutor.Query<string>(Queries.GetExistingTables);
            foreach (string tableMetadata in tablesMetadatas)
            {
                SQLiteTableInfo table = SQLiteDbObjectsSerializer.TableFromJson(tableMetadata);
                tables.Add(table);
            }
            databaseInfo.Tables = tables;
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
