using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.SQLite;
using DotNetDBTools.Definition.SQLite.DataTypes;

namespace DotNetDBTools.DefinitionParser.SQLite
{
    public static class SQLiteDefaultValueMapper
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
