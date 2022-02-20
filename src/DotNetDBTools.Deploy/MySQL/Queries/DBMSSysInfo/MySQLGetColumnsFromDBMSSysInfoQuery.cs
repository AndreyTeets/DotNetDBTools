using System;
using System.Collections.Generic;
using System.Globalization;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Deploy.MySQL.Queries.DBMSSysInfo;

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
    c.COLUMN_DEFAULT AS `{nameof(MySQLColumnRecord.Default)}`,
    CASE WHEN c.EXTRA = 'DEFAULT_GENERATED' THEN TRUE ELSE FALSE END AS {nameof(MySQLColumnRecord.DefaultIsFunction)}
FROM INFORMATION_SCHEMA.TABLES t
INNER JOIN INFORMATION_SCHEMA.COLUMNS c
    ON c.TABLE_SCHEMA = t.TABLE_SCHEMA
        AND c.TABLE_NAME = t.TABLE_NAME
WHERE t.TABLE_SCHEMA = (select DATABASE())
    AND t.TABLE_TYPE = 'BASE TABLE'
    AND t.TABLE_NAME NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public override RecordsLoader Loader => new MySQLRecordsLoader();
    public override RecordMapper Mapper => new MySQLRecordMapper();

    public class MySQLColumnRecord : ColumnRecord
    {
        public string FullDataType { get; set; }
        public bool Identity { get; set; }
        public bool DefaultIsFunction { get; set; }
    }

    public class MySQLRecordsLoader : RecordsLoader
    {
        public override IEnumerable<ColumnRecord> GetRecords(IQueryExecutor queryExecutor, GetColumnsFromDBMSSysInfoQuery query)
        {
            return queryExecutor.Query<MySQLColumnRecord>(query);
        }
    }

    public class MySQLRecordMapper : RecordMapper
    {
        public override Column MapToColumnModel(ColumnRecord builderColumnRecord)
        {
            MySQLColumnRecord columnRecord = (MySQLColumnRecord)builderColumnRecord;
            DataType dataType = ParseDataType(columnRecord);
            return new Column()
            {
                ID = Guid.NewGuid(),
                Name = columnRecord.ColumnName,
                DataType = dataType,
                Nullable = columnRecord.Nullable,
                Identity = columnRecord.Identity,
                Default = ParseDefault(dataType, columnRecord),
            };
        }

        private static DataType ParseDataType(MySQLColumnRecord columnRecord)
        {
            string dataType = columnRecord.DataType.ToUpper();
            string fullDataType = columnRecord.FullDataType.ToUpper();
            DataType dataTypeModel = MySQLQueriesHelper.CreateDataTypeModel(dataType, fullDataType);
            return dataTypeModel;
        }

        private static object ParseDefault(DataType dataType, MySQLColumnRecord columnRecord)
        {
            string value = columnRecord.Default;
            if (value is null)
                return null;

            if (IsFunction(value))
                return new CodePiece() { Code = value };

            string baseDataType = dataType.Name.Split('(')[0];
            switch (baseDataType)
            {
                case MySQLDataTypeNames.TINYINT:
                case MySQLDataTypeNames.SMALLINT:
                case MySQLDataTypeNames.MEDIUMINT:
                case MySQLDataTypeNames.INT:
                case MySQLDataTypeNames.BIGINT:
                    return long.Parse(value);
                case MySQLDataTypeNames.DECIMAL:
                    return decimal.Parse(value, CultureInfo.InvariantCulture);
                case MySQLDataTypeNames.CHAR:
                case MySQLDataTypeNames.VARCHAR:
                case MySQLDataTypeNames.TINYTEXT:
                case MySQLDataTypeNames.TEXT:
                case MySQLDataTypeNames.MEDIUMTEXT:
                case MySQLDataTypeNames.LONGTEXT:
                    return TrimOuterQuotesIfExist(value.Replace("_utf8mb4", ""));
                case MySQLDataTypeNames.BINARY:
                case MySQLDataTypeNames.VARBINARY:
                case MySQLDataTypeNames.TINYBLOB:
                case MySQLDataTypeNames.BLOB:
                case MySQLDataTypeNames.MEDIUMBLOB:
                case MySQLDataTypeNames.LONGBLOB:
                    return ToByteArray(value);
                default:
                    throw new InvalidOperationException($"Invalid default value [{value}] for data type [{dataType.Name}]");
            }

            bool IsFunction(string val) => val.Contains("(") && val.Contains(")") && columnRecord.DefaultIsFunction;
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
