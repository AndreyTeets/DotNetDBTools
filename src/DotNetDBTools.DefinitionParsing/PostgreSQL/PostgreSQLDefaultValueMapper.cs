using System;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.PostgreSQL.DataTypes;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.PostgreSQL
{
    internal class PostgreSQLDefaultValueMapper : IDefaultValueMapper
    {
        public object MapDefaultValue(BaseColumn column)
        {
            object value = column.Default;
            if (value is null)
                return null;
            if (column.DefaultIsFunction)
                return new CodePiece() { Code = (string)value };
            return MapByColumnDataType(column.DataType, value);
        }

        public static object MapByColumnDataType(IDataType dataType, object value)
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
