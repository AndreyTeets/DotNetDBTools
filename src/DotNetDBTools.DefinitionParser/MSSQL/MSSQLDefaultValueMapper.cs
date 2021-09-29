using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MSSQL;
using DotNetDBTools.Definition.MSSQL.DataTypes;
using DotNetDBTools.DefinitionParser.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.DefinitionParser.MSSQL
{
    internal class MSSQLDefaultValueMapper : IDefaultValueMapper
    {
        public object MapDefaultValue(BaseColumn column)
        {
            object value = column.Default;
            if (value is null)
                return null;
            if (((Column)column).DefaultIsFunction)
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
