using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL
{
    public class MSSQLDbModelConverter : IDbModelConverter
    {
        public Database FromAgnostic(Database database)
        {
            return ConvertToMSSQLModel((AgnosticDatabase)database);
        }

        private MSSQLDatabase ConvertToMSSQLModel(AgnosticDatabase database)
           => new(database.Name)
           {
               Tables = database.Tables.Select(x => ConvertToMSSQLModel((AgnosticTable)x)).ToList(),
               Views = database.Views.Select(x => ConvertToMSSQLModel((AgnosticView)x)).ToList(),
           };

        private static MSSQLTable ConvertToMSSQLModel(AgnosticTable table)
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

        private static MSSQLView ConvertToMSSQLModel(AgnosticView view)
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
                    column.DataType.SqlTypeName = "DATETIME2";
            }
            return columns;
        }
    }
}
