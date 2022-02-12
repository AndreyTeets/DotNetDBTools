using System;
using System.Collections.Generic;
using System.Globalization;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo;

internal class SQLiteGetColumnsFromDBMSSysInfoQuery : GetColumnsFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    sm.name AS {nameof(SQLiteColumnRecord.TableName)},
    ti.name AS {nameof(SQLiteColumnRecord.ColumnName)},
    ti.type AS {nameof(SQLiteColumnRecord.DataType)},
    CASE WHEN ti.[notnull] = 1 THEN 0 ELSE 1 END AS {nameof(SQLiteColumnRecord.Nullable)},
    CASE WHEN ti.pk = 1 AND lower(ti.type) = 'integer' THEN 1 ELSE 0 END AS {nameof(SQLiteColumnRecord.IsIdentityCandidate)},
    ti.dflt_value AS ""{nameof(SQLiteColumnRecord.Default)}""
FROM sqlite_master sm
INNER JOIN pragma_table_info(sm.name) ti
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name != '{DNDBTSysTables.DNDBTDbObjects}';";

    public override RecordsLoader Loader => new SQLiteRecordsLoader();
    public override RecordMapper Mapper => new SQLiteRecordMapper();

    public class SQLiteColumnRecord : ColumnRecord
    {
        public bool IsIdentityCandidate { get; set; }
    }

    public class SQLiteRecordsLoader : RecordsLoader
    {
        public override IEnumerable<ColumnRecord> GetRecords(IQueryExecutor queryExecutor, GetColumnsFromDBMSSysInfoQuery query) =>
            queryExecutor.Query<SQLiteColumnRecord>(query);
    }

    public class SQLiteRecordMapper : RecordMapper
    {
        public override Column MapToColumnModel(ColumnRecord builderColumnRecord)
        {
            SQLiteColumnRecord columnRecord = (SQLiteColumnRecord)builderColumnRecord;
            DataType dataType = ParseDataType(columnRecord);
            return new Column()
            {
                ID = Guid.NewGuid(),
                Name = columnRecord.ColumnName,
                DataType = ParseDataType(columnRecord),
                Nullable = columnRecord.Nullable,
                Identity = columnRecord.IsIdentityCandidate,
                Default = ParseDefault(dataType, columnRecord.Default),
            };
        }

        private static DataType ParseDataType(SQLiteColumnRecord columnRecord)
        {
            string dataType = columnRecord.DataType.ToUpper();
            switch (dataType)
            {
                case SQLiteDataTypeNames.INTEGER:
                case SQLiteDataTypeNames.REAL:
                case SQLiteDataTypeNames.NUMERIC:
                case SQLiteDataTypeNames.TEXT:
                case SQLiteDataTypeNames.BLOB:
                    return new DataType { Name = dataType };
                default:
                    throw new InvalidOperationException($"Invalid column record datatype: {columnRecord.DataType}");
            }
        }

        private static object ParseDefault(DataType dataType, string valueFromDBMSSysTable)
        {
            if (valueFromDBMSSysTable is null)
                return null;
            string value = valueFromDBMSSysTable;

            if (IsFunction(value))
                return new CodePiece() { Code = value };

            string baseDataType = dataType.Name;
            switch (baseDataType)
            {
                case SQLiteDataTypeNames.INTEGER:
                    return long.Parse(value);
                case SQLiteDataTypeNames.NUMERIC:
                    return decimal.Parse(value, CultureInfo.InvariantCulture);
                case SQLiteDataTypeNames.TEXT:
                    return TrimOuterQuotes(value);
                case SQLiteDataTypeNames.BLOB:
                    return ToByteArray(value);
                default:
                    throw new InvalidOperationException($"Invalid default value [{valueFromDBMSSysTable}] for data type [{dataType.Name}]");
            }

            static bool IsFunction(string val) => val.Contains("(") && val.Contains(")") && !val.StartsWith("'", StringComparison.Ordinal);
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
