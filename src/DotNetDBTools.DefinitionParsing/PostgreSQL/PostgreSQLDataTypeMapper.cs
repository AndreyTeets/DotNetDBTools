using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.PostgreSQL.DataTypes;
using DotNetDBTools.Definition.PostgreSQL.UserDefinedTypes;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.DefinitionParsing.PostgreSQL
{
    internal class PostgreSQLDataTypeMapper : IDataTypeMapper
    {
        public DataType MapToDataTypeModel(IDataType dataType)
        {
            return dataType switch
            {
                IntDataType dt => Map(dt),
                RealDataType dt => Map(dt),
                DecimalDataType dt => Map(dt),
                BoolDataType dt => new DataType() { Name = PostgreSQLDataTypeNames.BOOL },

                MoneyDataType dt => new DataType() { Name = PostgreSQLDataTypeNames.MONEY },

                StringDataType dt => Map(dt),
                BinaryDataType dt => new DataType() { Name = PostgreSQLDataTypeNames.BYTEA },

                DateDataType dt => new DataType() { Name = PostgreSQLDataTypeNames.DATE },
                TimeDataType dt => Map(dt),
                DateTimeDataType dt => Map(dt),

                GuidDataType dt => new DataType() { Name = PostgreSQLDataTypeNames.UUID },
                BitStringDataType dt => Map(dt),
                JsonDataType dt => new DataType() { Name = dt.SqlType.ToString() },

                ArrayDataType dt => Map(dt),
                IBaseType t => new DataType { Name = t.GetType().Name, IsUserDefined = true },
                ICompositeType t => new DataType { Name = t.GetType().Name, IsUserDefined = true },
                IDomain t => new DataType { Name = t.GetType().Name, IsUserDefined = true },
                IEnumType t => new DataType { Name = t.GetType().Name, IsUserDefined = true },
                IRangeType t => new DataType { Name = t.GetType().Name, IsUserDefined = true },
                _ => throw new InvalidOperationException($"Invalid dataType: {dataType}")
            };
        }

        private static DataType Map(IntDataType intDataType)
        {
            return intDataType.Size switch
            {
                IntSize.Int16 => new DataType() { Name = PostgreSQLDataTypeNames.SMALLINT },
                IntSize.Int32 => new DataType() { Name = PostgreSQLDataTypeNames.INT },
                IntSize.Int64 => new DataType() { Name = PostgreSQLDataTypeNames.BIGINT },
                _ => throw new InvalidOperationException($"Invalid int size: {intDataType.Size}"),
            };
        }

        private static DataType Map(RealDataType realDataType)
        {
            if (realDataType.IsDoublePrecision)
                return new DataType() { Name = PostgreSQLDataTypeNames.FLOAT8 };
            else
                return new DataType() { Name = PostgreSQLDataTypeNames.FLOAT4 };
        }

        private static DataType Map(DecimalDataType decimalDataType)
        {
            return new DataType { Name = $"{PostgreSQLDataTypeNames.DECIMAL}({decimalDataType.Precision}, {decimalDataType.Scale})" };
        }

        private static DataType Map(StringDataType stringDataType)
        {
            string stringTypeName = stringDataType.SqlType.ToString();
            bool isLengthBased = stringTypeName switch
            {
                PostgreSQLDataTypeNames.CHAR => true,
                PostgreSQLDataTypeNames.VARCHAR => true,
                PostgreSQLDataTypeNames.TEXT => false,
                _ => throw new InvalidOperationException($"Invalid string data type sql type: {stringDataType.SqlType}"),
            };

            if (isLengthBased)
            {
                string lengthStr = stringDataType.Length.ToString();
                return new DataType { Name = $"{stringTypeName}({lengthStr})" };
            }

            return new DataType { Name = stringTypeName };
        }

        private static DataType Map(TimeDataType timeDataType)
        {
            if (timeDataType.IsWithTimeZone)
                return new DataType { Name = $"{PostgreSQLDataTypeNames.TIMETZ}({timeDataType.Precision})" };
            else
                return new DataType { Name = $"{PostgreSQLDataTypeNames.TIME}({timeDataType.Precision})" };
        }

        private static DataType Map(DateTimeDataType dateTimeDataType)
        {
            if (dateTimeDataType.IsWithTimeZone)
                return new DataType { Name = $"{PostgreSQLDataTypeNames.TIMESTAMPTZ}({dateTimeDataType.Precision})" };
            else
                return new DataType { Name = $"{PostgreSQLDataTypeNames.TIMESTAMP}({dateTimeDataType.Precision})" };
        }

        private static DataType Map(BitStringDataType bitStringDataType)
        {
            if (bitStringDataType.IsFixedLength)
                return new DataType { Name = $"{PostgreSQLDataTypeNames.BIT}({bitStringDataType.Length})" };
            else
                return new DataType { Name = $"{PostgreSQLDataTypeNames.VARBIT}({bitStringDataType.Length})" };
        }

        private DataType Map(ArrayDataType arrayDataType)
        {
            string underlyingTypeName = MapToDataTypeModel(arrayDataType.UnderlyingType).Name;
            string arrayTypeName = underlyingTypeName;
            foreach (int dimSize in arrayDataType.Dimensions)
            {
                arrayTypeName += $"[{dimSize}]";
            }
            return new DataType { Name = arrayTypeName };
        }
    }
}
