using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Analysis.MySQL
{
    public class MySQLDbModelConverter : IDbModelConverter
    {
        public Database FromAgnostic(Database database)
        {
            return ConvertToMySQLModel((AgnosticDatabase)database);
        }

        private static MySQLDatabase ConvertToMySQLModel(AgnosticDatabase database)
           => new(database.Name)
           {
               Tables = database.Tables.Select(x => ConvertToMySQLModel((AgnosticTable)x)).ToList(),
               Views = database.Views.Select(x => ConvertToMySQLModel((AgnosticView)x)).ToList(),
           };

        private static MySQLTable ConvertToMySQLModel(AgnosticTable table)
            => new()
            {
                ID = table.ID,
                Name = table.Name,
                Columns = ConvertToMySQLModel(table.Columns),
                PrimaryKey = table.PrimaryKey,
                UniqueConstraints = table.UniqueConstraints,
                CheckConstraints = table.CheckConstraints,
                Indexes = table.Indexes,
                Triggers = table.Triggers,
                ForeignKeys = table.ForeignKeys,
            };

        private static MySQLView ConvertToMySQLModel(AgnosticView view)
            => new()
            {
                ID = view.ID,
                Name = view.Name,
                Code = view.Code,
            };

        private static IEnumerable<Column> ConvertToMySQLModel(IEnumerable<Column> columns)
        {
            foreach (Column column in columns)
            {
                column.DataType = column.DataType.Name switch
                {
                    AgnosticDataTypeNames.Int => ConvertIntSqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.Real => ConvertRealSqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.Decimal => ConvertDecimalSqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.Bool => new DataType { Name = $"{MySQLDataTypeNames.TINYINT}(1)" },

                    AgnosticDataTypeNames.String => ConvertStringSqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.Binary => ConvertBinarySqlType((AgnosticDataType)column.DataType),

                    AgnosticDataTypeNames.Date => new DataType { Name = MySQLDataTypeNames.DATE },
                    AgnosticDataTypeNames.Time => new DataType { Name = MySQLDataTypeNames.TIME },
                    AgnosticDataTypeNames.DateTime => ConvertDateTimeSqlType((AgnosticDataType)column.DataType),

                    AgnosticDataTypeNames.Guid => new DataType { Name = $"{MySQLDataTypeNames.BINARY}(16)" },
                    _ => throw new InvalidOperationException($"Invalid agnostic column datatype name: {column.DataType.Name}"),
                };
            }
            return columns;
        }

        private static DataType ConvertIntSqlType(AgnosticDataType dataType)
        {
            return dataType.Size switch
            {
                8 => new DataType { Name = MySQLDataTypeNames.TINYINT },
                16 => new DataType { Name = MySQLDataTypeNames.SMALLINT },
                32 => new DataType { Name = MySQLDataTypeNames.INT },
                64 => new DataType { Name = MySQLDataTypeNames.BIGINT },
                _ => throw new Exception($"Invalid int size: '{dataType.Size}'")
            };
        }

        private static DataType ConvertRealSqlType(AgnosticDataType dataType)
        {
            if (dataType.IsDoublePrecision)
                return new DataType { Name = MySQLDataTypeNames.DOUBLE };
            else
                return new DataType { Name = MySQLDataTypeNames.FLOAT };
        }

        private static DataType ConvertDecimalSqlType(AgnosticDataType dataType)
        {
            return new DataType { Name = $"{MySQLDataTypeNames.DECIMAL}({dataType.Precision}, {dataType.Scale})" };
        }

        private static DataType ConvertStringSqlType(AgnosticDataType dataType)
        {
            int maxAllowedLength = dataType.IsFixedLength ? 255 : 65535;
            if (dataType.Length > maxAllowedLength ||
                dataType.Length < 1)
            {
                return new DataType { Name = MySQLDataTypeNames.LONGTEXT };
            }

            string stringTypeName = dataType.IsFixedLength ? MySQLDataTypeNames.CHAR : MySQLDataTypeNames.VARCHAR;
            string lengthStr = dataType.Length.ToString();
            return new DataType { Name = $"{stringTypeName}({lengthStr})" };
        }

        private static DataType ConvertBinarySqlType(AgnosticDataType dataType)
        {
            int maxAllowedLength = dataType.IsFixedLength ? 255 : 65535;
            if (dataType.Length > maxAllowedLength ||
                dataType.Length < 1)
            {
                return new DataType { Name = MySQLDataTypeNames.LONGBLOB };
            }

            string binaryTypeName = dataType.IsFixedLength ? MySQLDataTypeNames.BINARY : MySQLDataTypeNames.VARBINARY;
            string lengthStr = dataType.Length.ToString();
            return new DataType { Name = $"{binaryTypeName}({lengthStr})" };
        }

        private static DataType ConvertDateTimeSqlType(AgnosticDataType dataType)
        {
            if (dataType.IsWithTimeZone)
                return new DataType { Name = MySQLDataTypeNames.TIMESTAMP };
            else
                return new DataType { Name = MySQLDataTypeNames.DATETIME };
        }
    }
}
