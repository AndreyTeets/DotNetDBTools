using System;
using DotNetDBTools.Analysis;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL;

internal static class PostgreSQLQueriesHelper
{
    public const int MultirangeTypeNameAvailableDbmsVersion = 140000;
    public const string SelectDbmsVersionStatement = "SELECT current_setting('server_version_num')::int";

    public static DataType CreateDataTypeModel(string dataType, string lengthStr, bool isBaseDataType)
    {
        if (!isBaseDataType)
            return new DataType { Name = dataType };

        int length = int.Parse(lengthStr);
        string dataTypeBaseName = AnalysisManager.TryGetTypeBaseName(dataType, DatabaseKind.PostgreSQL);
        switch (dataTypeBaseName)
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
                return new DataType { Name = dataTypeBaseName };

            case PostgreSQLDataTypeNames.DECIMAL:
                return new DataType { Name = length == -1 ? dataTypeBaseName : $"{dataTypeBaseName}({GetDecimalPrecisionAndScale(length)})" };

            case PostgreSQLDataTypeNames.CHAR:
            case PostgreSQLDataTypeNames.VARCHAR:
                return new DataType { Name = length == -1 ? dataTypeBaseName : $"{dataTypeBaseName}({length - 4})" };

            case PostgreSQLDataTypeNames.TIME:
            case PostgreSQLDataTypeNames.TIMETZ:
            case PostgreSQLDataTypeNames.TIMESTAMP:
            case PostgreSQLDataTypeNames.TIMESTAMPTZ:
                return new DataType { Name = length == -1 ? dataTypeBaseName : $"{dataTypeBaseName}({length})" };

            case PostgreSQLDataTypeNames.BIT:
            case PostgreSQLDataTypeNames.VARBIT:
                return new DataType { Name = length == -1 ? dataTypeBaseName : $"{dataTypeBaseName}({length})" };

            // TODO handle user defined base data types (probalby will require declaring their names in definition)
            default:
                throw new InvalidOperationException($"Invalid datatype base name: {dataTypeBaseName}");
        }

        string GetDecimalPrecisionAndScale(int length)
        {
            int precision = ((length - 4) >> 16) & 65535;
            int scale = (length - 4) & 65535;
            return $"{precision}, {scale}";
        }
    }

    public static CodePiece ParseDefault(string value)
    {
        return new CodePiece() { Code = value };
    }
}
