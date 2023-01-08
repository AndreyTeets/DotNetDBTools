using System;
using DotNetDBTools.Analysis;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL;

internal static class PostgreSQLQueriesHelper
{
    public const int MultirangeTypeNameAvailableDbmsVersion = 140000;
    public const string SelectDbmsVersionStatement = "SELECT current_setting('server_version_num')::int";

    public static DataType CreateDataTypeModel(string dataType, string lengthStr, int arrayDims)
    {
        string baseName = AnalysisManager.GetStandardSqlTypeNameBase(dataType, DatabaseKind.PostgreSQL);
        if (baseName == null)
            return new DataType { Name = AppendArrayDims(dataType, arrayDims) };
        else
            return new DataType { Name = AppendArrayDims(AddPrecisionToBaseName(baseName, lengthStr), arrayDims) };

        static string AddPrecisionToBaseName(string baseName, string lengthStr)
        {
            int length = int.Parse(lengthStr);
            switch (baseName)
            {
                case PostgreSQLDataTypeNames.SMALLINT:
                case PostgreSQLDataTypeNames.INT:
                case PostgreSQLDataTypeNames.BIGINT:

                case PostgreSQLDataTypeNames.FLOAT4:
                case PostgreSQLDataTypeNames.FLOAT8:
                case PostgreSQLDataTypeNames.BOOL:

                case PostgreSQLDataTypeNames.MONEY:

                case PostgreSQLDataTypeNames.TEXT:
                case PostgreSQLDataTypeNames.BYTEA:

                case PostgreSQLDataTypeNames.DATE:

                case PostgreSQLDataTypeNames.UUID:
                case PostgreSQLDataTypeNames.JSON:
                case PostgreSQLDataTypeNames.JSONB:
                case PostgreSQLDataTypeNames.XML:

                case PostgreSQLDataTypeNames.TSQUERY:
                case PostgreSQLDataTypeNames.TSVECTOR:

                case PostgreSQLDataTypeNames.POINT:
                case PostgreSQLDataTypeNames.LINE:
                case PostgreSQLDataTypeNames.LSEG:
                case PostgreSQLDataTypeNames.BOX:
                case PostgreSQLDataTypeNames.PATH:
                case PostgreSQLDataTypeNames.POLYGON:
                case PostgreSQLDataTypeNames.CIRCLE:

                case PostgreSQLDataTypeNames.INET:
                case PostgreSQLDataTypeNames.CIDR:
                case PostgreSQLDataTypeNames.MACADDR:
                case PostgreSQLDataTypeNames.MACADDR8:
                    return baseName;

                case PostgreSQLDataTypeNames.DECIMAL:
                    return length == -1 ? baseName : $"{baseName}{GetDecimalPrecisionAndScale(length)}";

                case PostgreSQLDataTypeNames.CHAR:
                case PostgreSQLDataTypeNames.VARCHAR:
                    return length == -1 || length - 4 == 1 ? baseName : $"{baseName}({length - 4})";

                case PostgreSQLDataTypeNames.TIME:
                case PostgreSQLDataTypeNames.TIMETZ:
                case PostgreSQLDataTypeNames.TIMESTAMP:
                case PostgreSQLDataTypeNames.TIMESTAMPTZ:
                    return length == -1 ? baseName : $"{baseName}({length})";

                case PostgreSQLDataTypeNames.INTERVAL:
                    return length == -1 ? baseName : $"{baseName}{GetIntervalFields(length)}{GetIntervalPrecision(length)}";

                case PostgreSQLDataTypeNames.BIT:
                    return length == -1 || length == 1 ? baseName : $"{baseName}({length})";
                case PostgreSQLDataTypeNames.VARBIT:
                    return length == -1 ? baseName : $"{baseName}({length})";

                default:
                    throw new InvalidOperationException($"Invalid datatype base name '{baseName}'");
            }
        }

        static string GetDecimalPrecisionAndScale(int length)
        {
            int precision = ((length - 4) >> 16) & 65535;
            int scale = (length - 4) & 65535;
            return $"({precision}, {scale})";
        }

        static string GetIntervalFields(int length)
        {
            bool month = (length & (1 << 17)) != 0;
            bool year = (length & (1 << 18)) != 0;
            bool day = (length & (1 << 19)) != 0;
            bool hour = (length & (1 << 26)) != 0;
            bool minute = (length & (1 << 27)) != 0;
            bool second = (length & (1 << 28)) != 0;

            if (year && month && day && hour && minute && second)
                return "";

            if (year && month)
                return $" YEAR TO MONTH";
            else if (year)
                return $" YEAR";
            else if (month)
                return $" MONTH";
            else if (day && second)
                return $" DAY TO SECOND";
            else if (day && minute)
                return $" DAY TO MINUTE";
            else if (day && hour)
                return $" DAY TO HOUR";
            else if (day)
                return $" DAY";
            else if (hour && second)
                return $" HOUR TO SECOND";
            else if (hour && minute)
                return $" HOUR TO MINUTE";
            else if (hour)
                return $" HOUR";
            else if (minute && second)
                return $" MINUTE TO SECOND";
            else if (minute)
                return $" MINUTE";
            else if (second)
                return $" SECOND";
            else
                throw new Exception($"Invalid length={length} while getting interval type");
        }

        static string GetIntervalPrecision(int length)
        {
            int precision = length & 65535;
            if (precision != 65535)
                return $"({precision})";
            else
                return "";
        }

        static string AppendArrayDims(string dataTypeName, int arrayDims)
        {
            string res = dataTypeName;
            for (int i = 0; i < arrayDims; i++)
                res += "[]";
            return res;
        }
    }

    public static CodePiece ParseDefault(string value)
    {
        return new CodePiece() { Code = value };
    }
}
