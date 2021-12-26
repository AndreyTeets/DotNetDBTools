using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo
{
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
                return new Column()
                {
                    ID = Guid.NewGuid(),
                    Name = columnRecord.ColumnName,
                    DataType = ParseDataType(columnRecord),
                    Nullable = columnRecord.Nullable,
                    Identity = columnRecord.IsIdentityCandidate,
                    Default = ParseDefault(columnRecord.Default),
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

            private static object ParseDefault(string value)
            {
                if (value is null)
                    return null;

                if (IsByte(value))
                    return ToByteArray(value);
                if (IsString(value))
                    return TrimOuterQuotes(value);
                if (IsFunction(value))
                    return new CodePiece() { Code = value };
                return long.Parse(value);

                static bool IsByte(string val) => val.StartsWith("0x", StringComparison.Ordinal);
                static bool IsString(string val) => val.StartsWith("'", StringComparison.Ordinal);
                static bool IsFunction(string val) => !long.TryParse(val, out _);
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
