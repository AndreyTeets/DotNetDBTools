using System;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL;

internal static class MySQLQueriesHelper
{
    public static string GetIdentityStatement(Column column)
    {
        return column.Identity ? " AUTO_INCREMENT" : "";
    }

    public static string GetNullabilityStatement(Column column)
    {
        return column.NotNull switch
        {
            false => "NULL",
            true => "NOT NULL",
        };
    }

    public static string GetDefaultValStatement(Column column)
    {
        if (column.Default.Code is not null)
            return $" DEFAULT {column.Default.Code}";
        else
            return "";
    }

    public static DataType CreateDataTypeModel(string dataType, string fullDataType)
    {
        switch (dataType)
        {
            case MySQLDataTypeNames.TINYINT:
            case MySQLDataTypeNames.SMALLINT:
            case MySQLDataTypeNames.MEDIUMINT:
            case MySQLDataTypeNames.INT:
            case MySQLDataTypeNames.BIGINT:

            case MySQLDataTypeNames.FLOAT:
            case MySQLDataTypeNames.DOUBLE:
            case MySQLDataTypeNames.DECIMAL:

            case MySQLDataTypeNames.TINYTEXT:
            case MySQLDataTypeNames.TEXT:
            case MySQLDataTypeNames.MEDIUMTEXT:
            case MySQLDataTypeNames.LONGTEXT:

            case MySQLDataTypeNames.TINYBLOB:
            case MySQLDataTypeNames.BLOB:
            case MySQLDataTypeNames.MEDIUMBLOB:
            case MySQLDataTypeNames.LONGBLOB:

            case MySQLDataTypeNames.DATE:
            case MySQLDataTypeNames.TIME:
            case MySQLDataTypeNames.SMALLDATETIME:
            case MySQLDataTypeNames.DATETIME:
            case MySQLDataTypeNames.TIMESTAMP:
            case MySQLDataTypeNames.YEAR:

            case MySQLDataTypeNames.JSON:
            case MySQLDataTypeNames.ENUM:
            case MySQLDataTypeNames.SET:

            case MySQLDataTypeNames.CHAR:
            case MySQLDataTypeNames.VARCHAR:

            case MySQLDataTypeNames.BINARY:
            case MySQLDataTypeNames.VARBINARY:

            case MySQLDataTypeNames.BIT:
                return new DataType { Name = fullDataType };

            default:
                throw new InvalidOperationException($"Invalid datatype: {dataType}");
        }
    }
}
