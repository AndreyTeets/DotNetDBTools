using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MySQL.DataTypes;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.DefinitionParsing.MySQL
{
    internal class MySQLDataTypeMapper : IDataTypeMapper
    {
        public DataType MapToDataTypeModel(IDataType dataType)
        {
            return dataType switch
            {
                IntDataType dt => Map(dt),
                RealDataType dt => Map(dt),
                DecimalDataType dt => Map(dt),
                BoolDataType dt => new DataType() { Name = $"{MySQLDataTypeNames.TINYINT}(1)" },

                StringDataType dt => Map(dt),
                BinaryDataType dt => Map(dt),

                DateDataType dt => new DataType() { Name = MySQLDataTypeNames.DATE },
                TimeDataType dt => new DataType() { Name = MySQLDataTypeNames.TIME },
                DateTimeDataType dt => new DataType() { Name = dt.SqlType.ToString() },
                YearDataType dt => new DataType() { Name = MySQLDataTypeNames.YEAR },

                GuidDataType dt => new DataType() { Name = $"{MySQLDataTypeNames.BINARY}(16)" },
                BitStringDataType dt => Map(dt),
                JsonDataType dt => new DataType() { Name = MySQLDataTypeNames.JSON },
                EnumDataType dt => Map(dt),
                SetDataType dt => Map(dt),

                _ => throw new InvalidOperationException($"Invalid dataType: {dataType}")
            };
        }

        private static DataType Map(IntDataType intDataType)
        {
            return intDataType.Size switch
            {
                IntSize.Int8 => new DataType() { Name = MySQLDataTypeNames.TINYINT },
                IntSize.Int16 => new DataType() { Name = MySQLDataTypeNames.SMALLINT },
                IntSize.Int24 => new DataType() { Name = MySQLDataTypeNames.MEDIUMINT },
                IntSize.Int32 => new DataType() { Name = MySQLDataTypeNames.INT },
                IntSize.Int64 => new DataType() { Name = MySQLDataTypeNames.BIGINT },
                _ => throw new InvalidOperationException($"Invalid int size: {intDataType.Size}"),
            };
        }

        private static DataType Map(RealDataType realDataType)
        {
            if (realDataType.IsDoublePrecision)
                return new DataType() { Name = MySQLDataTypeNames.DOUBLE };
            else
                return new DataType() { Name = MySQLDataTypeNames.FLOAT };
        }

        private static DataType Map(DecimalDataType decimalDataType)
        {
            return new DataType { Name = $"{MySQLDataTypeNames.DECIMAL}({decimalDataType.Precision}, {decimalDataType.Scale})" };
        }

        private static DataType Map(StringDataType stringDataType)
        {
            string stringTypeName = stringDataType.SqlType.ToString();
            (bool isLengthBased, int maxAllowedLength) = stringTypeName switch
            {
                MySQLDataTypeNames.CHAR => (true, 255),
                MySQLDataTypeNames.VARCHAR => (true, 65535),
                MySQLDataTypeNames.TINYTEXT => (false, -1),
                MySQLDataTypeNames.TEXT => (false, -1),
                MySQLDataTypeNames.MEDIUMTEXT => (false, -1),
                MySQLDataTypeNames.LONGTEXT => (false, -1),
                _ => throw new InvalidOperationException($"Invalid string data type sql type: {stringDataType.SqlType}"),
            };

            if (isLengthBased)
            {
                string lengthStr = stringTypeName.Length.ToString();
                if (stringTypeName.Length > maxAllowedLength ||
                    stringTypeName.Length < 1)
                {
                    lengthStr = maxAllowedLength.ToString();
                }
                return new DataType { Name = $"{stringTypeName}({lengthStr})" };
            }

            return new DataType { Name = stringTypeName };
        }

        private static DataType Map(BinaryDataType binaryDataType)
        {
            string binaryTypeName = binaryDataType.SqlType.ToString();
            (bool isLengthBased, int maxAllowedLength) = binaryTypeName switch
            {
                MySQLDataTypeNames.BINARY => (true, 255),
                MySQLDataTypeNames.VARBINARY => (true, 65535),
                MySQLDataTypeNames.TINYBLOB => (false, -1),
                MySQLDataTypeNames.BLOB => (false, -1),
                MySQLDataTypeNames.MEDIUMBLOB => (false, -1),
                MySQLDataTypeNames.LONGBLOB => (false, -1),
                _ => throw new InvalidOperationException($"Invalid string data type sql type: {binaryDataType.SqlType}"),
            };

            if (isLengthBased)
            {
                string lengthStr = binaryTypeName.Length.ToString();
                if (binaryTypeName.Length > maxAllowedLength ||
                    binaryTypeName.Length < 1)
                {
                    lengthStr = maxAllowedLength.ToString();
                }
                return new DataType { Name = $"{binaryTypeName}({lengthStr})" };
            }

            return new DataType { Name = binaryTypeName };
        }

        private static DataType Map(BitStringDataType bitStringDataType)
        {
            return new DataType { Name = $"{MySQLDataTypeNames.BIT}({bitStringDataType.Length})" };
        }

        private static DataType Map(EnumDataType enumDataType)
        {
            return new DataType { Name = $"{MySQLDataTypeNames.ENUM}({string.Join(", ", enumDataType.AllowedValues)})" };
        }

        private static DataType Map(SetDataType setDataType)
        {
            return new DataType { Name = $"{MySQLDataTypeNames.SET}({string.Join(", ", setDataType.AllowedValues)})" };
        }
    }
}
