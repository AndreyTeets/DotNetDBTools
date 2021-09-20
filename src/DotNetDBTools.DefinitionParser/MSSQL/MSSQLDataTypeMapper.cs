using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParser.MSSQL
{
    public static class MSSQLDataTypeMapper
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
                Attributes = new()
                {
                    { DataTypeAttributes.Length, stringDataType.Length.ToString() },
                    { DataTypeAttributes.IsUnicode, stringDataType.IsUnicode.ToString() },
                    { DataTypeAttributes.IsFixedLength, stringDataType.IsFixedLength.ToString() },
                },
            };
        }

        private static DataTypeInfo GetIntDataTypeInfo(IntDataType _)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Int,
                Attributes = new(),
            };
        }

        private static DataTypeInfo GetByteDataTypeInfo(ByteDataType byteDataType)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Byte,
                Attributes = new()
                {
                    { DataTypeAttributes.Length, byteDataType.Length.ToString() },
                },
            };
        }

        private static DataTypeInfo GetUserDefinedTypeInfo(IUserDefinedType userDefinedType)
        {
            return new DataTypeInfo()
            {
                Name = userDefinedType.GetType().Name,
                Attributes = new(),
            };
        }
    }
}
