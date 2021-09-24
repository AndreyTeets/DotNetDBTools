﻿using System;
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
                ByteDataType byteDataType => GetByteDataTypeInfo(byteDataType),
                _ => throw new InvalidOperationException($"Invalid dataType: {dataType}")
            };
        }

        private static DataTypeInfo GetStringDataTypeInfo(StringDataType stringDataType)
        {
            return new DataTypeInfo()
            {
                Name = DataTypeNames.String,
                Length = stringDataType.Length,
                IsUnicode = stringDataType.IsUnicode,
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
    }
}