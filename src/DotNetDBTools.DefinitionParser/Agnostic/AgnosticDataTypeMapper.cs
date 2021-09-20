using System;
using DotNetDBTools.Definition.Agnostic.DataTypes;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParser.Agnostic
{
    public static class AgnosticDataTypeMapper
    {
        public static DataTypeInfo GetDataTypeInfo(IDataType dataType)
        {
            return dataType switch
            {
                StringDataType stringDataType => GetStringDataTypeInfo(stringDataType),
                IntDataType intDataType => GetIntDataTypeInfo(intDataType),
                ByteDataType byteDataType => GetByteDataTypeInfo(byteDataType),
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
    }
}
