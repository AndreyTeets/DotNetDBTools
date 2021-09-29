using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo
{
    internal class GetColumnsFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    t.name AS {nameof(ColumnRecord.TableName)},
    c.name AS {nameof(ColumnRecord.ColumnName)},
    (SELECT tp.name FROM sys.types tp WHERE tp.user_type_id = c.system_type_id) AS {nameof(ColumnRecord.DataType)},
    (SELECT tp.name FROM sys.types tp WHERE tp.user_type_id = c.user_type_id AND tp.is_user_defined = 1) AS {nameof(ColumnRecord.UserDefinedDataType)},
    c.is_nullable AS {nameof(ColumnRecord.Nullable)},
    c.is_identity AS [{nameof(ColumnRecord.Identity)}],
    dc.definition AS [{nameof(ColumnRecord.Default)}],
    dc.name AS {nameof(ColumnRecord.DefaultConstraintName)},
    c.max_length AS {nameof(ColumnRecord.Length)}
FROM sys.tables t
INNER JOIN sys.columns c
    ON c.object_id = t.object_id
LEFT JOIN sys.default_constraints dc
    ON dc.object_id = c.default_object_id
WHERE t.name != '{DNDBTSysTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class ColumnRecord : ColumnsBuilder.ColumnRecord
        {
            public string UserDefinedDataType { get; set; }
            public bool Identity { get; set; }
            public string DefaultConstraintName { get; set; }
            public string Length { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static Dictionary<string, TableInfo> BuildTablesListWithColumns(IEnumerable<ColumnRecord> columnRecords)
            {
                return ColumnsBuilder.BuildTablesListWithColumns<MSSQLTableInfo>(columnRecords, MapToColumnInfo);
            }

            private static ColumnInfo MapToColumnInfo(ColumnsBuilder.ColumnRecord builderColumnRecord)
            {
                ColumnRecord columnRecord = (ColumnRecord)builderColumnRecord;
                DataTypeInfo dataTypeInfo = MSSQLSqlTypeMapper.GetModelType(columnRecord.DataType, columnRecord.Length);
                if (columnRecord.UserDefinedDataType is not null)
                {
                    dataTypeInfo.Name = columnRecord.UserDefinedDataType;
                    dataTypeInfo.IsUserDefined = true;
                }

                return new ColumnInfo()
                {
                    ID = Guid.NewGuid(),
                    Name = columnRecord.ColumnName,
                    DataType = dataTypeInfo,
                    Nullable = columnRecord.Nullable,
                    Identity = columnRecord.Identity,
                    Default = ParseDefault(columnRecord.Default),
                    DefaultConstraintName = columnRecord.DefaultConstraintName,
                };
            }

            private static object ParseDefault(string valueFromDBMSSysTable)
            {
                if (valueFromDBMSSysTable is null)
                    return null;
                string value = TrimOuterParantheses(valueFromDBMSSysTable);

                if (IsNumber(value))
                    return long.Parse(TrimOuterParantheses(value));
                if (IsByte(value))
                    return ToByteArray(value);
                if (IsString(value))
                    return TrimOuterQuotes(value);
                if (IsFunction(value))
                    return new MSSQLDefaultValueAsFunction() { FunctionText = value };

                throw new ArgumentException($"Invalid parameter value '{valueFromDBMSSysTable}'", nameof(valueFromDBMSSysTable));

                static bool IsNumber(string val) => val.StartsWith("(", StringComparison.Ordinal);
                static bool IsByte(string val) => val.StartsWith("0x", StringComparison.Ordinal);
                static bool IsString(string val) => val.StartsWith("'", StringComparison.Ordinal);
                static bool IsFunction(string val) => !long.TryParse(val, out _);
                static string TrimOuterParantheses(string val) => val.Substring(1, val.Length - 2);
                static string TrimOuterQuotes(string val) => val.Substring(1, val.Length - 2);
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
        }
    }
}
