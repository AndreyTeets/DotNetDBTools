using System;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL;

internal static class MSSQLQueriesHelper
{
    public static DataType CreateDataTypeModel(string dataType, int length, int precision, int scale)
    {
        switch (dataType)
        {
            case MSSQLDataTypeNames.TINYINT:
            case MSSQLDataTypeNames.SMALLINT:
            case MSSQLDataTypeNames.INT:
            case MSSQLDataTypeNames.BIGINT:

            case MSSQLDataTypeNames.REAL:
            case MSSQLDataTypeNames.FLOAT:
            case MSSQLDataTypeNames.BIT:

            case MSSQLDataTypeNames.SMALLMONEY:
            case MSSQLDataTypeNames.MONEY:

            case MSSQLDataTypeNames.DATE:
            case MSSQLDataTypeNames.TIME:
            case MSSQLDataTypeNames.SMALLDATETIME:
            case MSSQLDataTypeNames.DATETIME:
            case MSSQLDataTypeNames.DATETIME2:
            case MSSQLDataTypeNames.DATETIMEOFFSET:

            case MSSQLDataTypeNames.UNIQUEIDENTIFIER:
            case MSSQLDataTypeNames.ROWVERSION:
                return new DataType { Name = dataType };

            case MSSQLDataTypeNames.DECIMAL:
                return new DataType { Name = length == -1 ? dataType : $"{dataType}({precision}, {scale})" };

            case MSSQLDataTypeNames.CHAR:
            case MSSQLDataTypeNames.VARCHAR:

            case MSSQLDataTypeNames.BINARY:
            case MSSQLDataTypeNames.VARBINARY:
                return new DataType { Name = length != -1 ? $"{dataType}({length})" : $"{dataType}(MAX)" };

            case MSSQLDataTypeNames.NCHAR:
            case MSSQLDataTypeNames.NVARCHAR:
                return new DataType { Name = length != -1 ? $"{dataType}({length / 2})" : $"{dataType}(MAX)" };

            default:
                throw new InvalidOperationException($"Invalid datatype: {dataType}");
        }
    }
}
