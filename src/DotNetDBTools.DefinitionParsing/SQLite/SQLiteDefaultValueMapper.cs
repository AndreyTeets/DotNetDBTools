using System;
using System.Globalization;
using DotNetDBTools.CodeParsing.Core.Models;
using DotNetDBTools.Definition.Core;
using DotNetDBTools.Definition.SQLite.DataTypes;
using DotNetDBTools.DefinitionParsing.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.DefinitionParsing.SQLite
{
    internal class SQLiteDefaultValueMapper : IDefaultValueMapper
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

        public object GetDefaultValueModel(ColumnInfo column)
        {
            if (column.DefaultValue is null)
                return null;

            return column.DefaultType switch
            {
                DefaultType.Number => ParseNumber(column.DefaultValue),
                DefaultType.String => column.DefaultValue,
                DefaultType.Function => new CodePiece { Code = column.DefaultValue },
                _ => throw new Exception($"Invalid column default type '{column.DefaultType}'"),
            };

            static object ParseNumber(string numberStr)
            {
                if (numberStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    return ToByteArray(numberStr);
                else if (long.TryParse(numberStr, out long res))
                    return res;
                else
                    return decimal.Parse(numberStr, CultureInfo.InvariantCulture);
            }

            static byte[] ToByteArray(string val)
            {
                string hex = val.Substring(2, val.Length - 2);
                int numChars = hex.Length;
                byte[] bytes = new byte[numChars / 2];
                for (int i = 0; i < numChars; i += 2)
                    bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
                return bytes;
            }
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
}
