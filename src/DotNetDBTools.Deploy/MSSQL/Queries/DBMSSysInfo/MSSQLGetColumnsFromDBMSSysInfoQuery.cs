using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DBMSSysInfo
{
    internal class MSSQLGetColumnsFromDBMSSysInfoQuery : GetColumnsFromDBMSSysInfoQuery
    {
        public override string Sql =>
$@"SELECT
    t.name AS {nameof(MSSQLColumnRecord.TableName)},
    c.name AS {nameof(MSSQLColumnRecord.ColumnName)},
    (SELECT tp.name FROM sys.types tp WHERE tp.user_type_id = c.system_type_id) AS {nameof(MSSQLColumnRecord.DataType)},
    (SELECT tp.name FROM sys.types tp WHERE tp.user_type_id = c.user_type_id AND tp.is_user_defined = 1) AS {nameof(MSSQLColumnRecord.UserDefinedDataType)},
    c.is_nullable AS {nameof(MSSQLColumnRecord.Nullable)},
    c.is_identity AS [{nameof(MSSQLColumnRecord.Identity)}],
    dc.definition AS [{nameof(MSSQLColumnRecord.Default)}],
    dc.name AS {nameof(MSSQLColumnRecord.DefaultConstraintName)},
    c.max_length AS {nameof(MSSQLColumnRecord.Length)}
FROM sys.tables t
INNER JOIN sys.columns c
    ON c.object_id = t.object_id
LEFT JOIN sys.default_constraints dc
    ON dc.object_id = c.default_object_id
WHERE t.name != '{DNDBTSysTables.DNDBTDbObjects}';";

        public override RecordsLoader Loader => new MSSQLRecordsLoader();
        public override RecordMapper Mapper => new MSSQLRecordMapper();

        public class MSSQLColumnRecord : ColumnRecord
        {
            public string UserDefinedDataType { get; set; }
            public bool Identity { get; set; }
            public string DefaultConstraintName { get; set; }
            public string Length { get; set; }
        }

        public class MSSQLRecordsLoader : RecordsLoader
        {
            public override IEnumerable<ColumnRecord> GetRecords(IQueryExecutor queryExecutor, GetColumnsFromDBMSSysInfoQuery query) =>
                queryExecutor.Query<MSSQLColumnRecord>(query);
        }

        public class MSSQLRecordMapper : RecordMapper
        {
            public override Column MapToColumnModel(ColumnRecord builderColumnRecord)
            {
                MSSQLColumnRecord columnRecord = (MSSQLColumnRecord)builderColumnRecord;
                return new MSSQLColumn()
                {
                    ID = Guid.NewGuid(),
                    Name = columnRecord.ColumnName,
                    DataType = ParseDataType(columnRecord),
                    Nullable = columnRecord.Nullable,
                    Identity = columnRecord.Identity,
                    Default = ParseDefault(columnRecord.Default),
                    DefaultConstraintName = columnRecord.DefaultConstraintName,
                };
            }

            private static DataType ParseDataType(MSSQLColumnRecord columnRecord)
            {
                if (columnRecord.UserDefinedDataType is not null)
                    return new DataType { Name = columnRecord.UserDefinedDataType, IsUserDefined = true };

                string dataType = columnRecord.DataType.ToUpper();
                int length = int.Parse(columnRecord.Length);
                return MSSQLQueriesHelper.CreateDataTypeModel(dataType, length);
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
                    return new DefaultValueAsFunction() { FunctionText = value };

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
