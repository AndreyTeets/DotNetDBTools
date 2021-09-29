using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite
{
    public class SQLiteDbModelConverter : IDbModelConverter
    {
        public DatabaseInfo FromAgnostic(DatabaseInfo databaseInfo)
        {
            return ConvertToSQLiteInfo((AgnosticDatabaseInfo)databaseInfo);
        }

        private SQLiteDatabaseInfo ConvertToSQLiteInfo(AgnosticDatabaseInfo databaseInfo)
           => new(databaseInfo.Name)
           {
               Tables = databaseInfo.Tables.Select(x => ConvertToSQLiteInfo((AgnosticTableInfo)x)).ToList(),
               Views = databaseInfo.Views.Select(x => ConvertToSQLiteInfo((AgnosticViewInfo)x)).ToList(),
           };

        private static SQLiteTableInfo ConvertToSQLiteInfo(AgnosticTableInfo tableInfo)
            => new()
            {
                ID = tableInfo.ID,
                Name = tableInfo.Name,
                Columns = ConvertToSQLiteInfo(tableInfo.Columns),
                PrimaryKey = tableInfo.PrimaryKey,
                UniqueConstraints = tableInfo.UniqueConstraints,
                CheckConstraints = tableInfo.CheckConstraints,
                Indexes = tableInfo.Indexes,
                Triggers = tableInfo.Triggers,
                ForeignKeys = tableInfo.ForeignKeys,
            };

        private static SQLiteViewInfo ConvertToSQLiteInfo(AgnosticViewInfo viewInfo)
            => new()
            {
                ID = viewInfo.ID,
                Name = viewInfo.Name,
                Code = viewInfo.Code,
            };

        private static IEnumerable<ColumnInfo> ConvertToSQLiteInfo(IEnumerable<ColumnInfo> columns)
        {
            foreach (ColumnInfo column in columns)
            {
                if (column.DataType.Name == DataTypeNames.DateTime)
                {
                    throw new InvalidOperationException(
                        $"Unable to convert agnostic database to sqlite:" +
                        $" data type {column.DataType.Name} is not supported by sqlite.");
                }

                column.DataType = new DataTypeInfo()
                {
                    Name = column.DataType.Name,
                };
            }
            return columns;
        }
    }
}
