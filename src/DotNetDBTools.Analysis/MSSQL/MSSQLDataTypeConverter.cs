using System;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Analysis.MSSQL;

public class MSSQLDataTypeConverter
{
    public static DataType ConvertToMSSQL(CSharpDataType dataType)
    {
        return dataType.Name switch
        {
            CSharpDataTypeNames.Int => ConvertIntSqlType(dataType),
            CSharpDataTypeNames.Real => ConvertRealSqlType(dataType),
            CSharpDataTypeNames.Decimal => ConvertDecimalSqlType(dataType),
            CSharpDataTypeNames.Bool => new DataType { Name = MSSQLDataTypeNames.BIT },

            CSharpDataTypeNames.String => ConvertStringSqlType(dataType),
            CSharpDataTypeNames.Binary => ConvertBinarySqlType(dataType),
            CSharpDataTypeNames.Guid => new DataType { Name = MSSQLDataTypeNames.UNIQUEIDENTIFIER },

            CSharpDataTypeNames.Date => new DataType { Name = MSSQLDataTypeNames.DATE },
            CSharpDataTypeNames.Time => new DataType { Name = MSSQLDataTypeNames.TIME },
            CSharpDataTypeNames.DateTime => ConvertDateTimeSqlType(dataType),

            _ => throw new InvalidOperationException($"Invalid csharp datatype name: {dataType.Name}"),
        };
    }

    private static DataType ConvertIntSqlType(CSharpDataType dataType)
    {
        return dataType.Size switch
        {
            8 => new DataType { Name = MSSQLDataTypeNames.TINYINT },
            16 => new DataType { Name = MSSQLDataTypeNames.SMALLINT },
            32 => new DataType { Name = MSSQLDataTypeNames.INT },
            64 => new DataType { Name = MSSQLDataTypeNames.BIGINT },
            _ => throw new Exception($"Invalid int size: '{dataType.Size}'")
        };
    }

    private static DataType ConvertRealSqlType(CSharpDataType dataType)
    {
        if (dataType.IsSinglePrecision)
            return new DataType { Name = MSSQLDataTypeNames.FLOAT };
        else
            return new DataType { Name = MSSQLDataTypeNames.REAL };
    }

    private static DataType ConvertDecimalSqlType(CSharpDataType dataType)
    {
        return new DataType { Name = $"{MSSQLDataTypeNames.DECIMAL}({dataType.Precision}, {dataType.Scale})" };
    }

    private static DataType ConvertStringSqlType(CSharpDataType dataType)
    {
        string stringTypeName = dataType.IsFixedLength ? MSSQLDataTypeNames.NCHAR : MSSQLDataTypeNames.NVARCHAR;

        string lengthStr = dataType.Length.ToString();
        if (dataType.Length > 4000 ||
            dataType.Length < 1)
        {
            if (dataType.IsFixedLength)
                stringTypeName = MSSQLDataTypeNames.NVARCHAR;
            lengthStr = "MAX";
        }

        return new DataType { Name = $"{stringTypeName}({lengthStr})" };
    }

    private static DataType ConvertBinarySqlType(CSharpDataType dataType)
    {
        string binaryTypeName = dataType.IsFixedLength ? MSSQLDataTypeNames.BINARY : MSSQLDataTypeNames.VARBINARY;

        string lengthStr = dataType.Length.ToString();
        if (dataType.Length > 8000 ||
            dataType.Length < 1)
        {
            if (dataType.IsFixedLength)
                binaryTypeName = MSSQLDataTypeNames.VARBINARY;
            lengthStr = "MAX";
        }

        return new DataType { Name = $"{binaryTypeName}({lengthStr})" };
    }

    private static DataType ConvertDateTimeSqlType(CSharpDataType dataType)
    {
        if (dataType.IsWithTimeZone)
            return new DataType { Name = MSSQLDataTypeNames.DATETIMEOFFSET };
        else
            return new DataType { Name = MSSQLDataTypeNames.DATETIME2 };
    }
}
