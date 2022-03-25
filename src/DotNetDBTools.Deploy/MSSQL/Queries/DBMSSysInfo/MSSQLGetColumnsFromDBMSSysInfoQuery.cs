using System;
using System.Collections.Generic;
using System.Globalization;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo;

internal class MSSQLGetColumnsFromDBMSSysInfoQuery : GetColumnsFromDBMSSysInfoQuery
{
    public override string Sql =>
$@"SELECT
    t.name AS {nameof(MSSQLColumnRecord.TableName)},
    c.name AS {nameof(MSSQLColumnRecord.ColumnName)},
    (SELECT tp.name FROM sys.types tp WHERE tp.user_type_id = c.system_type_id) AS {nameof(MSSQLColumnRecord.DataType)},
    (SELECT tp.name FROM sys.types tp WHERE tp.user_type_id = c.user_type_id AND tp.is_user_defined = 1) AS {nameof(MSSQLColumnRecord.UserDefinedDataType)},
    ~c.is_nullable AS [{nameof(MSSQLColumnRecord.NotNull)}],
    c.is_identity AS [{nameof(MSSQLColumnRecord.Identity)}],
    dc.definition AS [{nameof(MSSQLColumnRecord.Default)}],
    dc.name AS {nameof(MSSQLColumnRecord.DefaultConstraintName)},
    c.max_length AS {nameof(MSSQLColumnRecord.Length)},
    c.precision AS {nameof(MSSQLColumnRecord.Precision)},
    c.scale AS {nameof(MSSQLColumnRecord.Scale)}
FROM sys.tables t
INNER JOIN sys.columns c
    ON c.object_id = t.object_id
LEFT JOIN sys.default_constraints dc
    ON dc.object_id = c.default_object_id
WHERE t.name NOT IN ({DNDBTSysTables.AllTablesForInClause});";

    public override RecordsLoader Loader => new MSSQLRecordsLoader();
    public override RecordMapper Mapper => new MSSQLRecordMapper();

    public class MSSQLColumnRecord : ColumnRecord
    {
        public string UserDefinedDataType { get; set; }
        public bool Identity { get; set; }
        public string DefaultConstraintName { get; set; }
        public int Length { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
    }

    public class MSSQLRecordsLoader : RecordsLoader
    {
        public override IEnumerable<ColumnRecord> GetRecords(IQueryExecutor queryExecutor, GetColumnsFromDBMSSysInfoQuery query)
        {
            return queryExecutor.Query<MSSQLColumnRecord>(query);
        }
    }

    public class MSSQLRecordMapper : RecordMapper
    {
        public override Column MapToColumnModel(ColumnRecord columnRecord)
        {
            MSSQLColumnRecord cr = (MSSQLColumnRecord)columnRecord;
            DataType dataType = ParseDataType(cr);
            return new MSSQLColumn()
            {
                ID = Guid.NewGuid(),
                Name = cr.ColumnName,
                DataType = dataType,
                NotNull = cr.NotNull,
                Identity = cr.Identity,
                Default = ParseDefault(dataType, cr),
                DefaultConstraintName = cr.DefaultConstraintName,
            };
        }

        private static DataType ParseDataType(MSSQLColumnRecord columnRecord)
        {
            if (columnRecord.UserDefinedDataType is not null)
                return new DataType { Name = columnRecord.UserDefinedDataType, IsUserDefined = true };

            return MSSQLQueriesHelper.CreateDataTypeModel(
                columnRecord.DataType.ToUpper(),
                columnRecord.Length,
                columnRecord.Precision,
                columnRecord.Scale);
        }

        private static object ParseDefault(DataType dataType, MSSQLColumnRecord columnRecord)
        {
            string valueFromDBMSSysTable = columnRecord.Default;
            if (valueFromDBMSSysTable is null)
                return null;
            string value = TrimOuterParantheses(valueFromDBMSSysTable);

            if (IsFunction(value))
                return new CodePiece() { Code = value };

            string baseDataType = columnRecord.DataType.ToUpper();
            switch (baseDataType)
            {
                case MSSQLDataTypeNames.TINYINT:
                case MSSQLDataTypeNames.SMALLINT:
                case MSSQLDataTypeNames.INT:
                case MSSQLDataTypeNames.BIGINT:
                    return long.Parse(TrimOuterParantheses(value));
                case MSSQLDataTypeNames.DECIMAL:
                    return decimal.Parse(TrimOuterParantheses(value), CultureInfo.InvariantCulture);
                case MSSQLDataTypeNames.CHAR:
                case MSSQLDataTypeNames.VARCHAR:
                case MSSQLDataTypeNames.NCHAR:
                case MSSQLDataTypeNames.NVARCHAR:
                    return TrimOuterQuotes(value);
                case MSSQLDataTypeNames.BINARY:
                case MSSQLDataTypeNames.VARBINARY:
                    return ToByteArray(value);
                default:
                    throw new InvalidOperationException($"Invalid default value [{valueFromDBMSSysTable}] for data type [{dataType.Name}]");
            }

            static bool IsFunction(string val) => val.Contains("(") && val.Contains(")") && !IsString(val) && !IsNumber(val);
            static bool IsNumber(string val) =>
                double.TryParse(TrimOuterParantheses(val), NumberStyles.Number, CultureInfo.InvariantCulture, out double _);
            static bool IsString(string val) =>
                val.StartsWith("'", StringComparison.Ordinal) &&
                val.EndsWith("'", StringComparison.Ordinal) &&
                !TrimOuterQuotes(val).Replace("''", "").Contains("'");
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
