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
        public Database FromAgnostic(Database database)
        {
            return ConvertToSQLiteModel((AgnosticDatabase)database);
        }

        private SQLiteDatabase ConvertToSQLiteModel(AgnosticDatabase database)
           => new(database.Name)
           {
               Tables = database.Tables.Select(x => ConvertToSQLiteModel((AgnosticTable)x)).ToList(),
               Views = database.Views.Select(x => ConvertToSQLiteModel((AgnosticView)x)).ToList(),
           };

        private static SQLiteTable ConvertToSQLiteModel(AgnosticTable table)
            => new()
            {
                ID = table.ID,
                Name = table.Name,
                Columns = ConvertToSQLiteModel(table.Columns),
                PrimaryKey = table.PrimaryKey,
                UniqueConstraints = table.UniqueConstraints,
                CheckConstraints = table.CheckConstraints,
                Indexes = table.Indexes,
                Triggers = table.Triggers,
                ForeignKeys = table.ForeignKeys,
            };

        private static SQLiteView ConvertToSQLiteModel(AgnosticView view)
            => new()
            {
                ID = view.ID,
                Name = view.Name,
                Code = view.Code,
            };

        private static IEnumerable<Column> ConvertToSQLiteModel(IEnumerable<Column> columns)
        {
            foreach (Column column in columns)
            {
                if (column.DataType.Name == DataTypeNames.DateTime)
                {
                    throw new InvalidOperationException(
                        $"Unable to convert agnostic database to sqlite:" +
                        $" data type {column.DataType.Name} is not supported by sqlite.");
                }

                column.DataType = new DataType()
                {
                    Name = column.DataType.Name,
                };
            }
            return columns;
        }
    }
}
