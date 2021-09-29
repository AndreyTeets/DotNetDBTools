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
        public DatabaseInfo FromAgnostic(DatabaseInfo databaseInfo)
        {
            return ConvertToMSSQLInfo((AgnosticDatabaseInfo)databaseInfo);
        }

        private MSSQLDatabaseInfo ConvertToMSSQLInfo(AgnosticDatabaseInfo databaseInfo)
           => new(databaseInfo.Name)
           {
               Tables = databaseInfo.Tables.Select(x => ConvertToMSSQLInfo((AgnosticTableInfo)x)).ToList(),
               Views = databaseInfo.Views.Select(x => ConvertToMSSQLInfo((AgnosticViewInfo)x)).ToList(),
           };

        private static MSSQLTableInfo ConvertToMSSQLInfo(AgnosticTableInfo tableInfo)
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

        private static MSSQLViewInfo ConvertToMSSQLInfo(AgnosticViewInfo viewInfo)
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
                    column.DataType.SqlTypeName = "DATETIME2";
            }
            return columns;
        }
    }
}
