using System;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Analysis.MySQL;

public class MySQLDataTypeConverter : IDataTypeConverter
{
    public DataType Convert(CSharpDataType dataType)
    {
        return dataType.Name switch
        {
            CSharpDataTypeNames.Int => ConvertIntSqlType(dataType),
            CSharpDataTypeNames.Real => ConvertRealSqlType(dataType),
            CSharpDataTypeNames.Decimal => ConvertDecimalSqlType(dataType),
            CSharpDataTypeNames.Bool => new DataType { Name = $"{MySQLDataTypeNames.TINYINT}(1)" },

            CSharpDataTypeNames.String => ConvertStringSqlType(dataType),
            CSharpDataTypeNames.Binary => ConvertBinarySqlType(dataType),
            CSharpDataTypeNames.Guid => new DataType { Name = $"{MySQLDataTypeNames.BINARY}(16)" },

            CSharpDataTypeNames.Date => new DataType { Name = MySQLDataTypeNames.DATE },
            CSharpDataTypeNames.Time => new DataType { Name = MySQLDataTypeNames.TIME },
            CSharpDataTypeNames.DateTime => ConvertDateTimeSqlType(dataType),

            _ => throw new InvalidOperationException($"Invalid csharp datatype name: {dataType.Name}"),
        };
    }

    private static DataType ConvertIntSqlType(CSharpDataType dataType)
    {
        return dataType.Size switch
        {
            8 => new DataType { Name = MySQLDataTypeNames.TINYINT },
            16 => new DataType { Name = MySQLDataTypeNames.SMALLINT },
            32 => new DataType { Name = MySQLDataTypeNames.INT },
            64 => new DataType { Name = MySQLDataTypeNames.BIGINT },
            _ => throw new Exception($"Invalid int size: '{dataType.Size}'")
        };
    }

    private static DataType ConvertRealSqlType(CSharpDataType dataType)
    {
        if (dataType.IsSinglePrecision)
            return new DataType { Name = MySQLDataTypeNames.FLOAT };
        else
            return new DataType { Name = MySQLDataTypeNames.DOUBLE };
    }

    private static DataType ConvertDecimalSqlType(CSharpDataType dataType)
    {
        return new DataType { Name = $"{MySQLDataTypeNames.DECIMAL}({dataType.Precision},{dataType.Scale})" };
    }

    private static DataType ConvertStringSqlType(CSharpDataType dataType)
    {
        int maxAllowedLength = dataType.IsFixedLength ? 255 : 65535;
        if (dataType.Length > maxAllowedLength ||
            dataType.Length < 1)
        {
            return new DataType { Name = MySQLDataTypeNames.LONGTEXT };
        }

        string stringTypeName = dataType.IsFixedLength ? MySQLDataTypeNames.CHAR : MySQLDataTypeNames.VARCHAR;
        string lengthStr = dataType.Length.ToString();
        return new DataType { Name = $"{stringTypeName}({lengthStr})" };
    }

    private static DataType ConvertBinarySqlType(CSharpDataType dataType)
    {
        int maxAllowedLength = dataType.IsFixedLength ? 255 : 65535;
        if (dataType.Length > maxAllowedLength ||
            dataType.Length < 1)
        {
            return new DataType { Name = MySQLDataTypeNames.LONGBLOB };
        }

        string binaryTypeName = dataType.IsFixedLength ? MySQLDataTypeNames.BINARY : MySQLDataTypeNames.VARBINARY;
        string lengthStr = dataType.Length.ToString();
        return new DataType { Name = $"{binaryTypeName}({lengthStr})" };
    }

    private static DataType ConvertDateTimeSqlType(CSharpDataType dataType)
    {
        if (dataType.IsWithTimeZone)
            return new DataType { Name = MySQLDataTypeNames.TIMESTAMP };
        else
            return new DataType { Name = MySQLDataTypeNames.DATETIME };
    }
}
