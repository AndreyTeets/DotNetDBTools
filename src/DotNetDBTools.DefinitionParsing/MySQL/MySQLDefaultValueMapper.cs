using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.MySQL.DataTypes;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.MySQL;

internal class MySQLDefaultValueMapper : IDefaultValueMapper
{
    public object MapDefaultValue(BaseColumn column)
    {
        object value = column.Default;
        if (value is null)
            return null;
        if (value is Expression expression)
            return new CodePiece() { Code = expression.Code };
        return MapByColumnDataType(column.DataType, value);
    }

    private static object MapByColumnDataType(IDataType dataType, object value)
    {
        return dataType switch
        {
            StringDataType => (string)value,
            IntDataType => (long)(int)value,
            DecimalDataType => (decimal)value,
            BinaryDataType => (byte[])value,
            _ => throw new InvalidOperationException($"Invalid default value type: '{value.GetType()}' for a column with type '{dataType.GetType()}'"),
        };
    }
}
