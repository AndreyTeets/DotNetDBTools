using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.MSSQL
{
    internal class MSSQLDataTypeMapper : IDataTypeMapper
    {
        public DataTypeInfo GetDataTypeInfo(IDataType dataType)
        {
            return dataType switch
            {
                StringDataType stringDataType => GetStringDataTypeInfo(stringDataType),
                IntDataType intDataType => GetIntDataTypeInfo(intDataType),
                BinaryDataType binaryDataType => GetBinaryDataTypeInfo(binaryDataType),
                DateTimeDataType dateTimeDataType => GetDateTimeDataTypeInfo(dateTimeDataType),
                IUserDefinedType userDefinedType => GetUserDefinedTypeInfo(userDefinedType),
                _ => throw new InvalidOperationException($"Invalid dataType: '{dataType}'")
            };
        }

        private static DataTypeInfo GetStringDataTypeInfo(StringDataType stringDataType)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.String,
                Length = stringDataType.Length,
                IsFixedLength = stringDataType.IsFixedLength,
                IsUnicode = stringDataType.IsUnicode,
            };
        }

        private static DataTypeInfo GetIntDataTypeInfo(IntDataType intDataType)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Int,
                Size = int.Parse($"{intDataType.Size}".Replace("Int", "")),
            };
        }

        private static DataTypeInfo GetBinaryDataTypeInfo(BinaryDataType binaryDataType)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Binary,
                Length = binaryDataType.Length,
                IsFixedLength = binaryDataType.IsFixedLength,
            };
        }

        private static DataTypeInfo GetDateTimeDataTypeInfo(DateTimeDataType dateTimeDataType)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.DateTime,
                SqlTypeName = $"{dateTimeDataType.MSSQLType}",
            };
        }

        private DataTypeInfo GetUserDefinedTypeInfo(IUserDefinedType userDefinedType)
        {
            DataTypeInfo dataTypeInfo = GetDataTypeInfo(userDefinedType.UnderlyingType);
            dataTypeInfo.Name = userDefinedType.GetType().Name;
            dataTypeInfo.IsUserDefined = true;
            return dataTypeInfo;
        }
    }
}
