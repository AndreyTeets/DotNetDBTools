using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;
using DotNetDBTools.DefinitionParser.Core;
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
                Length = stringDataType.Length.ToString(),
                IsUnicode = stringDataType.IsUnicode.ToString(),
                IsFixedLength = stringDataType.IsFixedLength.ToString(),
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
                Length = byteDataType.Length.ToString(),
            };
        }

        private static DataTypeInfo GetUserDefinedTypeInfo(IUserDefinedType userDefinedType)
        {
            DataTypeInfo dataTypeInfo = GetDataTypeInfo(userDefinedType.UnderlyingType);
            return new DataTypeInfo()
            {
                Name = userDefinedType.GetType().Name,
                Length = dataTypeInfo.Length.ToString(),
                IsUnicode = dataTypeInfo.IsUnicode.ToString(),
                IsFixedLength = dataTypeInfo.IsFixedLength.ToString(),
            };
        }
    }
}
