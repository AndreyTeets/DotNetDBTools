using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParser.MSSQL
{
    internal static class MSSQLDataTypeMapper
    {
        public static DataTypeInfo GetDataTypeInfo(IDataType dataType)
        {
            return dataType switch
            {
                StringDataType stringDataType => GetStringDataTypeInfo(stringDataType),
                IntDataType intDataType => GetIntDataTypeInfo(intDataType),
                ByteDataType byteDataType => GetByteDataTypeInfo(byteDataType),
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
                IsUnicode = stringDataType.IsUnicode,
                IsFixedLength = stringDataType.IsFixedLength,
            };
        }

        private static DataTypeInfo GetIntDataTypeInfo(IntDataType _)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Int,
            };
        }

        private static DataTypeInfo GetByteDataTypeInfo(ByteDataType byteDataType)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Byte,
                Length = byteDataType.Length,
            };
        }

        private static DataTypeInfo GetUserDefinedTypeInfo(IUserDefinedType userDefinedType)
        {
            DataTypeInfo dataTypeInfo = GetDataTypeInfo(userDefinedType.UnderlyingType);
            dataTypeInfo.Name = userDefinedType.GetType().Name;
            dataTypeInfo.IsUserDefined = true;
            return dataTypeInfo;
        }
    }
}
