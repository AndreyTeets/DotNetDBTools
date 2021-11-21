using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo
{
    internal class GetColumnsFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    c.TABLE_NAME AS {nameof(ColumnRecord.TableName)},
    c.COLUMN_NAME AS {nameof(ColumnRecord.ColumnName)},
    c.DATA_TYPE AS {nameof(ColumnRecord.DataType)},
    c.COLUMN_TYPE AS {nameof(ColumnRecord.FullDataType)},
    CASE WHEN c.IS_NULLABLE = 'YES' THEN TRUE ELSE FALSE END AS {nameof(ColumnRecord.Nullable)},
    CASE WHEN c.EXTRA = 'auto_increment' THEN TRUE ELSE FALSE END AS {nameof(ColumnRecord.Identity)},
    c.COLUMN_DEFAULT AS `{nameof(ColumnRecord.Default)}`
FROM INFORMATION_SCHEMA.COLUMNS c
WHERE c.TABLE_SCHEMA = (select DATABASE())
    AND c.TABLE_NAME != '{DNDBTSysTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class ColumnRecord : ColumnsBuilder.ColumnRecord
        {
            public string FullDataType { get; set; }
            public bool Identity { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static Dictionary<string, Table> BuildTablesListWithColumns(IEnumerable<ColumnRecord> columnRecords)
            {
                return ColumnsBuilder.BuildTablesListWithColumns<MySQLTable>(columnRecords, MapToColumnModel);
            }

            private static Column MapToColumnModel(ColumnsBuilder.ColumnRecord builderColumnRecord)
            {
                ColumnRecord columnRecord = (ColumnRecord)builderColumnRecord;
                return new Column()
                {
                    ID = Guid.NewGuid(),
                    Name = columnRecord.ColumnName,
                    DataType = ParseDataType(columnRecord),
                    Nullable = columnRecord.Nullable,
                    Identity = columnRecord.Identity,
                    Default = ParseDefault(columnRecord),
                };
            }

            private static DataType ParseDataType(ColumnRecord columnRecord)
            {
                string dataType = columnRecord.DataType.ToUpper();
                string fullDataType = columnRecord.FullDataType.ToUpper();
                DataType dataTypeModel = MySQLQueriesHelper.CreateDataTypeModel(dataType, fullDataType);
                return dataTypeModel;
            }

            private static object ParseDefault(ColumnRecord columnRecord)
            {
                if (columnRecord.Default is null)
                    return null;
                string value = columnRecord.Default;

                if (IsFunction(value))
                    return new DefaultValueAsFunction() { FunctionText = value };
                if (IsString(columnRecord.DataType.ToUpper()))
                    return TrimOuterQuotesIfExist(value.Replace("_utf8mb4", ""));
                if (IsByte(value))
                    return ToByteArray(value);
                if (IsNumber(value))
                    return long.Parse(value);

                throw new InvalidOperationException($"Invalid default parameter value '{value}'");

                static bool IsFunction(string val) => val.Contains("(") && val.Contains(")");
                static bool IsString(string dataType) => new string[] { "TEXT", "CHAR" }.Any(x => dataType.Contains(x));
                static bool IsByte(string val) => val.StartsWith("0x", StringComparison.Ordinal);
                static bool IsNumber(string val) => long.TryParse(val, out long _);
                static string TrimOuterQuotesIfExist(string val) => val.Contains(@"\'") ? val.Substring(2, val.Length - 4) : val;
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
