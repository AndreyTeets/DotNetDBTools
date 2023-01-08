using System;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Analysis.PostgreSQL;

internal class PostgreSQLDataTypeConverter : IDataTypeConverter
{
    public DataType Convert(CSharpDataType dataType)
    {
        return dataType.Name switch
        {
            CSharpDataTypeNames.Int => ConvertIntSqlType(dataType),
            CSharpDataTypeNames.Real => ConvertRealSqlType(dataType),
            CSharpDataTypeNames.Decimal => ConvertDecimalSqlType(dataType),
            CSharpDataTypeNames.Bool => new DataType { Name = PostgreSQLDataTypeNames.BOOL },

            CSharpDataTypeNames.String => ConvertStringSqlType(dataType),
            CSharpDataTypeNames.Binary => new DataType { Name = PostgreSQLDataTypeNames.BYTEA },
            CSharpDataTypeNames.Guid => new DataType { Name = PostgreSQLDataTypeNames.UUID },

            CSharpDataTypeNames.Date => new DataType { Name = PostgreSQLDataTypeNames.DATE },
            CSharpDataTypeNames.Time => new DataType { Name = PostgreSQLDataTypeNames.TIME },
            CSharpDataTypeNames.DateTime => ConvertDateTimeSqlType(dataType),

            _ => throw new InvalidOperationException($"Invalid csharp datatype name: {dataType.Name}"),
        };
    }

    private static DataType ConvertIntSqlType(CSharpDataType dataType)
    {
        return dataType.Size switch
        {
            8 => new DataType { Name = PostgreSQLDataTypeNames.SMALLINT },
            16 => new DataType { Name = PostgreSQLDataTypeNames.SMALLINT },
            32 => new DataType { Name = PostgreSQLDataTypeNames.INT },
            64 => new DataType { Name = PostgreSQLDataTypeNames.BIGINT },
            _ => throw new Exception($"Invalid int size: '{dataType.Size}'")
        };
    }

    private static DataType ConvertRealSqlType(CSharpDataType dataType)
    {
        if (dataType.IsSinglePrecision)
            return new DataType { Name = PostgreSQLDataTypeNames.FLOAT4 };
        else
            return new DataType { Name = PostgreSQLDataTypeNames.FLOAT8 };
    }

    private static DataType ConvertDecimalSqlType(CSharpDataType dataType)
    {
        return new DataType { Name = $"{PostgreSQLDataTypeNames.DECIMAL}({dataType.Precision}, {dataType.Scale})" };
    }

    private static DataType ConvertStringSqlType(CSharpDataType dataType)
    {
        string stringTypeName = dataType.IsFixedLength ? PostgreSQLDataTypeNames.CHAR : PostgreSQLDataTypeNames.VARCHAR;
        string lengthStr = dataType.Length.ToString();
        return new DataType { Name = $"{stringTypeName}({lengthStr})" };
    }

    private static DataType ConvertDateTimeSqlType(CSharpDataType dataType)
    {
        if (dataType.IsWithTimeZone)
            return new DataType { Name = PostgreSQLDataTypeNames.TIMESTAMPTZ };
        else
            return new DataType { Name = PostgreSQLDataTypeNames.TIMESTAMP };
    }
}
