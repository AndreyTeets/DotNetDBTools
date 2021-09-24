﻿using System;
using DotNetDBTools.Definition.Agnostic;
using DotNetDBTools.Definition.Agnostic.DataTypes;
using DotNetDBTools.Definition.Core;

namespace DotNetDBTools.DefinitionParser.Agnostic
{
    public static class AgnosticDefaultValueMapper
    {
        public static object MapDefaultValue(Column column)
        {
            object value = column.Default;
            if (value is null)
                return null;
            return MapByColumnDataType(column.DataType, value);
        }

        private static object MapByColumnDataType(IDataType dataType, object value)
        {
            return dataType switch
            {
                StringDataType stringDataType => (string)value,
                IntDataType intDataType => (int)value,
                ByteDataType byteDataType => (byte[])value,
                _ => throw new InvalidOperationException($"Invalid default value type: '{value.GetType()}' for a column with type '{dataType.GetType()}'"),
            };
        }
    }
}