using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo;

internal class SQLiteGetColumnsFromDBMSSysInfoQuery : GetColumnsFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    sm.name AS [{nameof(SQLiteColumnRecord.TableName)}],
    ti.name AS [{nameof(SQLiteColumnRecord.ColumnName)}],
    ti.type AS [{nameof(SQLiteColumnRecord.DataType)}],
    CASE WHEN ti.[notnull] = 1 THEN 1 ELSE 0 END AS [{nameof(SQLiteColumnRecord.NotNull)}],
    ti.dflt_value AS [{nameof(SQLiteColumnRecord.Default)}]
FROM sqlite_master sm
INNER JOIN pragma_table_info(sm.name) ti
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public override RecordsLoader Loader => new SQLiteRecordsLoader();
    public override RecordMapper Mapper => new SQLiteRecordMapper();

    public class SQLiteColumnRecord : ColumnRecord
    {
    }

    public class SQLiteRecordsLoader : RecordsLoader
    {
        public override IEnumerable<ColumnRecord> GetRecords(IQueryExecutor queryExecutor, GetColumnsFromDBMSSysInfoQuery query)
        {
            return queryExecutor.Query<SQLiteColumnRecord>(query);
        }
    }

    public class SQLiteRecordMapper : RecordMapper
    {
        public override Column MapToColumnModel(ColumnRecord columnRecord)
        {
            SQLiteColumnRecord cr = (SQLiteColumnRecord)columnRecord;
            DataType dataType = ParseDataType(cr);
            return new Column()
            {
                ID = Guid.NewGuid(),
                Name = cr.ColumnName,
                DataType = ParseDataType(cr),
                NotNull = cr.NotNull,
                Identity = false,
                Default = ParseDefault(cr),
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

        private static CodePiece ParseDefault(SQLiteColumnRecord columnRecord)
        {
            string value = columnRecord.Default;
            if (value is null)
                return new CodePiece { Code = null };
            if (IsFunction(value))
                return new CodePiece { Code = $"({value})" };
            return new CodePiece { Code = value };

            static bool IsFunction(string val) => val.Contains("(") && val.Contains(")") && !val.StartsWith("'", StringComparison.Ordinal);
        }
    }
}
