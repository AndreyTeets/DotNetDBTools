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
                IUserDefinedType userDefinedType => MapToDataTypeModel(userDefinedType),

                IntDataType dt => MapToDataTypeModel(dt),
                RealDataType dt => MapToDataTypeModel(dt),
                DecimalDataType dt => MapToDataTypeModel(dt),
                BoolDataType dt => new DataType() { Name = MSSQLDataTypeNames.BIT },

                MoneyDataType dt => new DataType() { Name = dt.SqlType.ToString() },
                RowVersionDataType dt => new DataType() { Name = MSSQLDataTypeNames.ROWVERSION },

                StringDataType dt => MapToDataTypeModel(dt),
                BinaryDataType dt => MapToDataTypeModel(dt),
                GuidDataType dt => new DataType() { Name = MSSQLDataTypeNames.UNIQUEIDENTIFIER },

                DateDataType dt => new DataType() { Name = MSSQLDataTypeNames.DATE },
                TimeDataType dt => new DataType() { Name = MSSQLDataTypeNames.TIME },
                DateTimeDataType dt => new DataType() { Name = dt.SqlType.ToString() },

                _ => throw new InvalidOperationException($"Invalid dataType: '{dataType}'")
            };
        }

        private DataType MapToDataTypeModel(IUserDefinedType userDefinedType)
        {
            return new MSSQLUserDefinedDataType
            {
                Name = userDefinedType.GetType().Name,
                UnderlyingType = MapToDataTypeModel(userDefinedType.UnderlyingType),
            };
        }

        private static DataType MapToDataTypeModel(IntDataType intDataType)
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

        private static DataType MapToDataTypeModel(RealDataType realDataType)
        {
            if (realDataType.IsDoublePrecision)
                return new DataType() { Name = MSSQLDataTypeNames.FLOAT };
            else
                return new DataType() { Name = MSSQLDataTypeNames.REAL };
        }

        private static DataType MapToDataTypeModel(DecimalDataType decimalDataType)
        {
            return new DataType { Name = $"{MSSQLDataTypeNames.DECIMAL}({decimalDataType.Precision}, {decimalDataType.Scale})" };
        }

        private static DataType MapToDataTypeModel(StringDataType stringDataType)
        {
            string stringTypeName = stringDataType.SqlType.ToString();
            (bool isFixedLength, bool isUnicode) = stringTypeName switch
            {
                MSSQLDataTypeNames.CHAR => (true, false),
                MSSQLDataTypeNames.NCHAR => (true, true),
                MSSQLDataTypeNames.VARCHAR => (false, false),
                MSSQLDataTypeNames.NVARCHAR => (false, true),
                _ => throw new InvalidOperationException($"Invalid string data type sql type: {stringDataType.SqlType}"),
            };

            string lengthStr = stringDataType.Length.ToString();
            if (isUnicode && stringDataType.Length > 4000 ||
                !isUnicode && stringDataType.Length > 8000 ||
                stringDataType.Length < 1)
            {
                if (isFixedLength)
                    throw new Exception($"The size ({stringDataType.Length}) given to type {stringTypeName} exceeds maximum allowed length");
                lengthStr = "MAX";
            }

            return new DataType { Name = $"{stringTypeName}({lengthStr})" };
        }

        private static DataType MapToDataTypeModel(BinaryDataType binaryDataType)
        {
            string binaryTypeName = binaryDataType.SqlType.ToString();
            bool isFixedLength = binaryTypeName switch
            {
                MSSQLDataTypeNames.BINARY => true,
                MSSQLDataTypeNames.VARBINARY => false,
                _ => throw new InvalidOperationException($"Invalid binary data type sql type: {binaryDataType.SqlType}"),
            };

            string lengthStr = binaryDataType.Length.ToString();
            if (binaryDataType.Length > 8000 ||
                binaryDataType.Length < 1)
            {
                if (isFixedLength)
                    throw new Exception($"The size ({binaryDataType.Length}) given to type {binaryTypeName} exceeds maximum allowed length");
                lengthStr = "MAX";
            }

            return new DataType { Name = $"{binaryTypeName}({lengthStr})" };
        }
    }
}
