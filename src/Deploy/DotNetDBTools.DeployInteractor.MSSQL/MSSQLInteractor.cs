using System;
using System.Collections.Generic;
using System.Linq;
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


        public void UpdateDatabase(MSSQLDatabaseInfo database, MSSQLDatabaseInfo existingDatabase)
        {
            foreach (MSSQLTableInfo table in database.Tables)
            {
                MSSQLTableInfo existingTable = (MSSQLTableInfo)existingDatabase.Tables.FirstOrDefault(x => x.ID == table.ID);
                if (existingTable is null)
                    CreateTable(table);
                else
                    AlterTable(table, existingTable);
            }

            foreach (MSSQLViewInfo view in database.Views)
            {
                if (existingDatabase.Views.Any(x => x.ID == view.ID))
                    DropView(view);
                CreateView(view);
            }

            foreach (MSSQLFunctionInfo function in database.Functions)
            {
                if (existingDatabase.Functions.Any(x => x.ID == function.ID))
                    DropFunction(function);
                CreateFunction(function);
            }
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
        private void AlterTable(MSSQLTableInfo table, MSSQLTableInfo existingTable)
        {
            Console.WriteLine($"AlterTable: ID={table.ID} Name={table.Name}");

            foreach (MSSQLColumnInfo column in table.Columns)
            {
                if (existingTable.Columns.Any(x => x.ID == column.ID))
                {
                    MSSQLColumnInfo existingColumn = (MSSQLColumnInfo)existingTable.Columns.Single(x => x.ID == column.ID);
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
        private void DropTable(MSSQLTableInfo table)
        {
            Console.WriteLine($"DropTable: ID={table.ID} Name={table.Name}");
        }

        private void AddColumn(MSSQLTableInfo table, MSSQLColumnInfo column)
        {
            Console.WriteLine($"AddColumn: ID={column.ID} TableName={table.Name} ColumnName={column.Name} ColumnType={column.DataType} DefaultValue={column.DefaultValue}");
        }
        private void AlterColumn(MSSQLTableInfo table, MSSQLColumnInfo column)
        {
            Console.WriteLine($"AlterColumn: ID={column.ID} TableName={table.Name} ColumnName={column.Name} ColumnType={column.DataType} DefaultValue={column.DefaultValue}");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
        private void DropColumn(MSSQLTableInfo table, MSSQLColumnInfo column)
        {
            Console.WriteLine($"DropColumn: ID={column.ID} TableName={table.Name} ColumnName={column.Name} ColumnType={column.DataType} DefaultValue={column.DefaultValue}");
        }

        private void CreateView(MSSQLViewInfo view)
        {
            Console.WriteLine($"CreateView: ID={view.ID} Name={view.Name} Code={view.Code}");
        }
        private void DropView(MSSQLViewInfo view)
        {
            Console.WriteLine($"DropView: ID={view.ID} Name={view.Name} Code={view.Code}");
        }

        private void CreateFunction(MSSQLFunctionInfo function)
        {
            Console.WriteLine($"CreateFunction: ID={function.ID} Name={function.Name} Code={function.Code}");
        }
        private void DropFunction(MSSQLFunctionInfo function)
        {
            Console.WriteLine($"DropFunction: ID={function.ID} Name={function.Name} Code={function.Code}");
        }
    }
}
