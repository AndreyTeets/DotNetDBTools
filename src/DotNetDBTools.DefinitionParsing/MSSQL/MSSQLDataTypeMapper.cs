using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.DefinitionParsing.MSSQL
{
    internal class MSSQLDataTypeMapper : IDataTypeMapper
    {
        public DataType MapToDataTypeModel(IDataType dataType)
        {
            return dataType switch
            {
                IntDataType dt => Map(dt),
                RealDataType dt => Map(dt),
                DecimalDataType dt => Map(dt),
                BoolDataType dt => new DataType() { Name = MSSQLDataTypeNames.BIT },

                MoneyDataType dt => new DataType() { Name = dt.SqlType.ToString() },

                StringDataType dt => Map(dt),
                BinaryDataType dt => Map(dt),

                DateDataType dt => new DataType() { Name = MSSQLDataTypeNames.DATE },
                TimeDataType dt => new DataType() { Name = MSSQLDataTypeNames.TIME },
                DateTimeDataType dt => new DataType() { Name = dt.SqlType.ToString() },

                GuidDataType dt => new DataType() { Name = MSSQLDataTypeNames.UNIQUEIDENTIFIER },
                RowVersionDataType dt => new DataType() { Name = MSSQLDataTypeNames.ROWVERSION },

                IUserDefinedType userDefinedType => Map(userDefinedType),
                _ => throw new InvalidOperationException($"Invalid dataType: '{dataType}'")
            };
        }

        private static DataType Map(IntDataType intDataType)
        {
            return intDataType.Size switch
            {
                IntSize.Int8 => new DataType() { Name = MSSQLDataTypeNames.TINYINT },
                IntSize.Int16 => new DataType() { Name = MSSQLDataTypeNames.SMALLINT },
                IntSize.Int32 => new DataType() { Name = MSSQLDataTypeNames.INT },
                IntSize.Int64 => new DataType() { Name = MSSQLDataTypeNames.BIGINT },
                _ => throw new InvalidOperationException($"Invalid int size: {intDataType.Size}"),
            };
        }

        private static DataType Map(RealDataType realDataType)
        {
            if (realDataType.IsDoublePrecision)
                return new DataType() { Name = MSSQLDataTypeNames.FLOAT };
            else
                return new DataType() { Name = MSSQLDataTypeNames.REAL };
        }

        private static DataType Map(DecimalDataType decimalDataType)
        {
            return new DataType { Name = $"{MSSQLDataTypeNames.DECIMAL}({decimalDataType.Precision}, {decimalDataType.Scale})" };
        }

        private static DataType Map(StringDataType stringDataType)
        {
            string stringTypeName = stringDataType.SqlType.ToString();
            (bool isFixedLength, int maxAllowedLength) = stringTypeName switch
            {
                MSSQLDataTypeNames.CHAR => (true, 8000),
                MSSQLDataTypeNames.NCHAR => (true, 4000),
                MSSQLDataTypeNames.VARCHAR => (false, 8000),
                MSSQLDataTypeNames.NVARCHAR => (false, 4000),
                _ => throw new InvalidOperationException($"Invalid string data type sql type: {stringDataType.SqlType}"),
            };

            string lengthStr = stringDataType.Length.ToString();
            if (stringDataType.Length > maxAllowedLength ||
                stringDataType.Length < 1)
            {
                if (isFixedLength)
                    throw new Exception($"The size ({stringDataType.Length}) given to type {stringTypeName} exceeds maximum allowed length");
                lengthStr = "MAX";
            }

            return new DataType { Name = $"{stringTypeName}({lengthStr})" };
        }

        private static DataType Map(BinaryDataType binaryDataType)
        {
            string binaryTypeName = binaryDataType.SqlType.ToString();
            (bool isFixedLength, int maxAllowedLength) = binaryTypeName switch
            {
                MSSQLDataTypeNames.BINARY => (true, 8000),
                MSSQLDataTypeNames.VARBINARY => (false, 8000),
                _ => throw new InvalidOperationException($"Invalid binary data type sql type: {binaryDataType.SqlType}"),
            };

            string lengthStr = binaryDataType.Length.ToString();
            if (binaryDataType.Length > maxAllowedLength ||
                binaryDataType.Length < 1)
            {
                if (isFixedLength)
                    throw new Exception($"The size ({binaryDataType.Length}) given to type {binaryTypeName} exceeds maximum allowed length");
                lengthStr = "MAX";
            }

            return new DataType { Name = $"{binaryTypeName}({lengthStr})" };
        }

        private DataType Map(IUserDefinedType userDefinedType)
        {
            return new MSSQLUserDefinedDataType
            {
                Name = userDefinedType.GetType().Name,
                UnderlyingType = MapToDataTypeModel(userDefinedType.UnderlyingType),
            };
        }
    }
}
