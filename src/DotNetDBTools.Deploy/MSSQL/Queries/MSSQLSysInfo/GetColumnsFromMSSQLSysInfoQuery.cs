using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.MSSQLSysInfo
{
    internal class GetColumnsFromMSSQLSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    t.TABLE_NAME AS {nameof(ColumnRecord.TableName)},
    c.COLUMN_NAME AS {nameof(ColumnRecord.ColumnName)},
    c.DATA_TYPE AS {nameof(ColumnRecord.DataType)},
    c.DOMAIN_NAME AS {nameof(ColumnRecord.UserDefinedDataType)},
    CASE WHEN c.IS_NULLABLE='YES' THEN 1 ELSE 0 END AS {nameof(ColumnRecord.Nullable)},
    COLUMNPROPERTY(object_id(c.TABLE_NAME), c.COLUMN_NAME, 'IsIdentity') AS [{nameof(ColumnRecord.Identity)}],
    c.COLUMN_DEFAULT AS [{nameof(ColumnRecord.Default)}],
    c.CHARACTER_OCTET_LENGTH AS {nameof(ColumnRecord.Length)}
FROM INFORMATION_SCHEMA.TABLES t
INNER JOIN INFORMATION_SCHEMA.COLUMNS c
    ON c.TABLE_NAME = t.TABLE_NAME
WHERE t.TABLE_TYPE='BASE TABLE';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class ColumnRecord
        {
            public string TableName { get; set; }
            public string ColumnName { get; set; }
            public string DataType { get; set; }
            public string UserDefinedDataType { get; set; }
            public bool Nullable { get; set; }
            public bool Identity { get; set; }
            public string Default { get; set; }
            public string Length { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static Dictionary<string, MSSQLTableInfo> BuildTablesListWithColumns(IEnumerable<ColumnRecord> columnRecords)
            {
                Dictionary<string, MSSQLTableInfo> tables = new();
                foreach (ColumnRecord columnRecord in columnRecords)
                {
                    if (!tables.ContainsKey(columnRecord.TableName))
                    {
                        MSSQLTableInfo table = new()
                        {
                            ID = Guid.NewGuid(),
                            Name = columnRecord.TableName,
                            Columns = new List<ColumnInfo>(),
                            UniqueConstraints = new List<UniqueConstraintInfo>(),
                            ForeignKeys = new List<ForeignKeyInfo>(),
                        };
                        tables.Add(columnRecord.TableName, table);
                    }
                    ColumnInfo columnInfo = MapToColumnInfo(columnRecord);
                    ((List<ColumnInfo>)tables[columnRecord.TableName].Columns).Add(columnInfo);
                }
                return tables;
            }

            private static ColumnInfo MapToColumnInfo(ColumnRecord columnRecord)
            {
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
                };
            }

            private static object ParseDefault(string valueFromMSSQLSysTable)
            {
                if (valueFromMSSQLSysTable is null)
                    return null;
                string value = TrimOuterParantheses(valueFromMSSQLSysTable);

                if (IsNumber(value))
                    return int.Parse(TrimOuterParantheses(value));
                if (IsByte(value))
                    return ToByteArray(value);
                if (IsString(value))
                    return TrimOuterQuotes(value);
                if (IsFunction(value))
                    return new MSSQLDefaultValueAsFunction() { FunctionText = value };

                throw new ArgumentException($"Invalid parameter value '{valueFromMSSQLSysTable}'", nameof(valueFromMSSQLSysTable));

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
