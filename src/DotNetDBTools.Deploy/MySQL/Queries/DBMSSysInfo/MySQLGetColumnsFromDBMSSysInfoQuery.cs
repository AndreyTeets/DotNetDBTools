using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo
{
    internal class MySQLGetColumnsFromDBMSSysInfoQuery : GetColumnsFromDBMSSysInfoQuery
    {
        public override string Sql =>
$@"SELECT
    c.TABLE_NAME AS {nameof(MySQLColumnRecord.TableName)},
    c.COLUMN_NAME AS {nameof(MySQLColumnRecord.ColumnName)},
    c.DATA_TYPE AS {nameof(MySQLColumnRecord.DataType)},
    c.COLUMN_TYPE AS {nameof(MySQLColumnRecord.FullDataType)},
    CASE WHEN c.IS_NULLABLE = 'YES' THEN TRUE ELSE FALSE END AS {nameof(MySQLColumnRecord.Nullable)},
    CASE WHEN c.EXTRA = 'auto_increment' THEN TRUE ELSE FALSE END AS {nameof(MySQLColumnRecord.Identity)},
    c.COLUMN_DEFAULT AS `{nameof(MySQLColumnRecord.Default)}`
FROM INFORMATION_SCHEMA.COLUMNS c
WHERE c.TABLE_SCHEMA = (select DATABASE())
    AND c.TABLE_NAME != '{DNDBTSysTables.DNDBTDbObjects}';";

        public override RecordsLoader Loader => new MySQLRecordsLoader();
        public override RecordMapper Mapper => new MySQLRecordMapper();

        public class MySQLColumnRecord : ColumnRecord
        {
            public string FullDataType { get; set; }
            public bool Identity { get; set; }
        }

        public class MySQLRecordsLoader : RecordsLoader
        {
            public override IEnumerable<ColumnRecord> GetRecords(IQueryExecutor queryExecutor, GetColumnsFromDBMSSysInfoQuery query) =>
                queryExecutor.Query<MySQLColumnRecord>(query);
        }

        public class MySQLRecordMapper : RecordMapper
        {
            public override Column MapToColumnModel(ColumnRecord builderColumnRecord)
            {
                MySQLColumnRecord columnRecord = (MySQLColumnRecord)builderColumnRecord;
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

            private static DataType ParseDataType(MySQLColumnRecord columnRecord)
            {
                string dataType = columnRecord.DataType.ToUpper();
                string fullDataType = columnRecord.FullDataType.ToUpper();
                DataType dataTypeModel = MySQLQueriesHelper.CreateDataTypeModel(dataType, fullDataType);
                return dataTypeModel;
            }

            private static object ParseDefault(MySQLColumnRecord columnRecord)
            {
                if (columnRecord.Default is null)
                    return null;
                string value = columnRecord.Default;

                if (IsFunction(value))
                    return new CodePiece() { Code = value };
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
