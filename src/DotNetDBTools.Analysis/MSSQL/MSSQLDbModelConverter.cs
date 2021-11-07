using System;
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

        private static MSSQLDatabase ConvertToMSSQLModel(AgnosticDatabase database)
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
                Columns = ConvertToMSSQLModel(table.Columns),
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

        private static IEnumerable<Column> ConvertToMSSQLModel(IEnumerable<Column> columns)
        {
            foreach (Column column in columns)
            {
                column.DataType = column.DataType.Name switch
                {
                    AgnosticDataTypeNames.Int => ConvertIntSqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.Real => ConvertRealSqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.Decimal => ConvertDecimalSqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.Bool => new DataType { Name = MSSQLDataTypeNames.BIT },

                    AgnosticDataTypeNames.String => ConvertStringSqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.Binary => ConvertBinarySqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.Guid => new DataType { Name = MSSQLDataTypeNames.UNIQUEIDENTIFIER },

                    AgnosticDataTypeNames.Date => new DataType { Name = MSSQLDataTypeNames.DATE },
                    AgnosticDataTypeNames.Time => new DataType { Name = MSSQLDataTypeNames.TIME },
                    AgnosticDataTypeNames.DateTime => ConvertDateTimeSqlType((AgnosticDataType)column.DataType),

                    _ => throw new InvalidOperationException($"Invalid agnostic column datatype name: {column.DataType.Name}"),
                };
            }
            return columns;
        }

        private static DataType ConvertIntSqlType(AgnosticDataType dataType)
        {
            return dataType.Size switch
            {
                8 => new DataType { Name = MSSQLDataTypeNames.TINYINT },
                16 => new DataType { Name = MSSQLDataTypeNames.SMALLINT },
                32 => new DataType { Name = MSSQLDataTypeNames.INT },
                64 => new DataType { Name = MSSQLDataTypeNames.BIGINT },
                _ => throw new Exception($"Invalid int size: '{dataType.Size}'")
            };
        }

        private static DataType ConvertRealSqlType(AgnosticDataType dataType)
        {
            if (dataType.IsDoublePrecision)
                return new DataType { Name = MSSQLDataTypeNames.REAL };
            else
                return new DataType { Name = MSSQLDataTypeNames.FLOAT };
        }

        private static DataType ConvertDecimalSqlType(AgnosticDataType dataType)
        {
            return new DataType { Name = $"{MSSQLDataTypeNames.DECIMAL}({dataType.Precision}, {dataType.Scale})" };
        }

        private static DataType ConvertStringSqlType(AgnosticDataType dataType)
        {
            string stringTypeName = dataType.IsFixedLength ? MSSQLDataTypeNames.NCHAR : MSSQLDataTypeNames.NVARCHAR;

            string lengthStr = dataType.Length.ToString();
            if (dataType.Length > 4000 ||
                dataType.Length < 1)
            {
                if (dataType.IsFixedLength)
                    stringTypeName = MSSQLDataTypeNames.NVARCHAR;
                lengthStr = "MAX";
            }

            return new DataType { Name = $"{stringTypeName}({lengthStr})" };
        }

        private static DataType ConvertBinarySqlType(AgnosticDataType dataType)
        {
            string binaryTypeName = dataType.IsFixedLength ? MSSQLDataTypeNames.BINARY : MSSQLDataTypeNames.VARBINARY;

            string lengthStr = dataType.Length.ToString();
            if (dataType.Length > 8000 ||
                dataType.Length < 1)
            {
                if (dataType.IsFixedLength)
                    binaryTypeName = MSSQLDataTypeNames.VARBINARY;
                lengthStr = "MAX";
            }

            return new DataType { Name = $"{binaryTypeName}({lengthStr})" };
        }

        private static DataType ConvertDateTimeSqlType(AgnosticDataType dataType)
        {
            if (dataType.IsWithTimeZone)
                return new DataType { Name = MSSQLDataTypeNames.DATETIMEOFFSET };
            else
                return new DataType { Name = MSSQLDataTypeNames.DATETIME2 };
        }
    }
}
