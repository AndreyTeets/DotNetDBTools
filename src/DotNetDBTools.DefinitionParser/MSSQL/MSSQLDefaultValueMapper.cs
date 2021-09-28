using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.DefinitionParser.MSSQL
{
    public static class MSSQLDefaultValueMapper
    {
        public static object MapDefaultValue(Column column)
        {
            object value = column.Default;
            if (value is null)
                return null;
            if (column.DefaultIsFunction)
                return new MSSQLDefaultValueAsFunction() { FunctionText = (string)value };
            return MapByColumnDataType(column.DataType, value);
        }

        private static object MapByColumnDataType(IDataType dataType, object value)
        {
            return dataType switch
            {
                StringDataType stringDataType => (string)value,
                IntDataType intDataType => (long)(int)value,
                BinaryDataType binaryDataType => (byte[])value,
                IUserDefinedType userDefinedType => MapByColumnDataType(userDefinedType.UnderlyingType, value),
                _ => throw new InvalidOperationException($"Invalid default value type: '{value.GetType()}' for a column with type '{dataType.GetType()}'"),
            };
        }
    }
}
