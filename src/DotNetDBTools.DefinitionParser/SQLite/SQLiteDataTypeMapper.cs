using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.SQLite.DataTypes;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParser.SQLite
{
    internal static class SQLiteDataTypeMapper
    {
        public static DataTypeInfo GetDataTypeInfo(IDataType dataType)
        {
            return dataType switch
            {
                StringDataType stringDataType => GetStringDataTypeInfo(stringDataType),
                IntDataType intDataType => GetIntDataTypeInfo(intDataType),
                BinaryDataType binaryDataType => GetBinaryDataTypeInfo(binaryDataType),
                _ => throw new InvalidOperationException($"Invalid dataType: {dataType}")
            };
        }

        private static DataTypeInfo GetStringDataTypeInfo(StringDataType _)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.String,
            };
        }

        private static DataTypeInfo GetIntDataTypeInfo(IntDataType _)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Int,
            };
        }

        private static DataTypeInfo GetBinaryDataTypeInfo(BinaryDataType _)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.Binary,
            };
        }
    }
}
