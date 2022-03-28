using System;
using System.Collections.Generic;
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
    CASE WHEN c.IS_NULLABLE = 'NO' THEN TRUE ELSE FALSE END AS `{nameof(MySQLColumnRecord.NotNull)}`,
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
        public override Column MapToColumnModel(ColumnRecord columnRecord)
        {
            MySQLColumnRecord cr = (MySQLColumnRecord)columnRecord;
            DataType dataType = ParseDataType(cr);
            return new Column()
            {
                ID = Guid.NewGuid(),
                Name = cr.ColumnName,
                DataType = dataType,
                NotNull = cr.NotNull,
                Identity = cr.Identity,
                Default = ParseDefault(dataType, cr),
            };
        }

        private static DataType ParseDataType(MySQLColumnRecord columnRecord)
        {
            string dataType = columnRecord.DataType.ToUpper();
            string fullDataType = columnRecord.FullDataType.ToUpper();
            DataType dataTypeModel = MySQLQueriesHelper.CreateDataTypeModel(dataType, fullDataType);
            return dataTypeModel;
        }

        private static CodePiece ParseDefault(DataType dataType, MySQLColumnRecord columnRecord)
        {
            string value = columnRecord.Default;
            if (value is null)
                return new CodePiece() { Code = value };
            if (IsFunction(value))
                return new CodePiece() { Code = value };

            string baseDataType = dataType.Name.Split('(')[0];
            switch (baseDataType)
            {
                case MySQLDataTypeNames.CHAR:
                case MySQLDataTypeNames.VARCHAR:
                case MySQLDataTypeNames.TINYTEXT:
                case MySQLDataTypeNames.TEXT:
                case MySQLDataTypeNames.MEDIUMTEXT:
                case MySQLDataTypeNames.LONGTEXT:
                    return new CodePiece() { Code = $"'{TrimOuterQuotesIfExist(value.Replace("_utf8mb4", ""))}'" };
                case MySQLDataTypeNames.DATE:
                case MySQLDataTypeNames.TIME:
                case MySQLDataTypeNames.SMALLDATETIME:
                case MySQLDataTypeNames.DATETIME:
                case MySQLDataTypeNames.TIMESTAMP:
                    return new CodePiece() { Code = $"'{value}'" };
                default:
                    return new CodePiece() { Code = value };
            }

            bool IsFunction(string val) => val.Contains("(") && val.Contains(")") && columnRecord.DefaultIsFunction;
            static string TrimOuterQuotesIfExist(string val) => val.Contains(@"\'") ? val.Substring(2, val.Length - 4) : val;
        }
    }
}
