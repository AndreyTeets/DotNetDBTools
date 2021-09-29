using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.SQLite;
using DotNetDBTools.Definition.SQLite.DataTypes;
using DotNetDBTools.DefinitionParser.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.DefinitionParser.SQLite
{
    internal class SQLiteDefaultValueMapper : IDefaultValueMapper
    {
        public object MapDefaultValue(BaseColumn column)
        {
            object value = column.Default;
            if (value is null)
                return null;
            if (((Column)column).DefaultIsFunction)
                return new SQLiteDefaultValueAsFunction() { FunctionText = (string)value };
            return MapByColumnDataType(column.DataType, value);
        }

        private static object MapByColumnDataType(IDataType dataType, object value)
        {
            return dataType switch
            {
                StringDataType stringDataType => (string)value,
                IntDataType intDataType => (long)(int)value,
                BinaryDataType binaryDataType => (byte[])value,
                _ => throw new InvalidOperationException($"Invalid default value type: '{value.GetType()}' for a column with type '{dataType.GetType()}'"),
            };
        }
    }
}
