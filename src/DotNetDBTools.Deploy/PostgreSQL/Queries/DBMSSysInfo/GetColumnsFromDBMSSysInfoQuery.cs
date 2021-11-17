using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DBMSSysInfo
{
    internal class GetColumnsFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    c.relname AS ""{nameof(ColumnRecord.TableName)}"",
    a.attname AS ""{nameof(ColumnRecord.ColumnName)}"",
    t.typname AS ""{nameof(ColumnRecord.DataType)}"",
    NOT a.attnotnull AS ""{nameof(ColumnRecord.Nullable)}"",
    pg_get_serial_sequence('""' || n.nspname || '"".""' || c.relname || '""', a.attname) IS NOT NULL AS ""{nameof(ColumnRecord.Identity)}"",
    pg_get_expr(d.adbin, d.adrelid) AS ""{nameof(ColumnRecord.Default)}"",
    a.atttypmod AS ""{nameof(ColumnRecord.Length)}""
FROM pg_catalog.pg_class c
INNER JOIN pg_catalog.pg_namespace n
    ON n.oid = c.relnamespace
INNER JOIN pg_catalog.pg_attribute a
    ON a.attrelid = c.oid
        AND a.attnum > 0
        AND NOT a.attisdropped
INNER JOIN pg_catalog.pg_type t
    ON t.oid = a.atttypid
LEFT JOIN pg_catalog.pg_attrdef d
    ON (d.adrelid,  d.adnum) = (a.attrelid, a.attnum)
WHERE c.relkind = 'r'
    AND n.nspname NOT IN ('information_schema', 'pg_catalog')
    AND c.relname != '{DNDBTSysTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class ColumnRecord : ColumnsBuilder.ColumnRecord
        {
            public bool Identity { get; set; }
            public string Length { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static Dictionary<string, Table> BuildTablesListWithColumns(IEnumerable<ColumnRecord> columnRecords)
            {
                return ColumnsBuilder.BuildTablesListWithColumns<PostgreSQLTable>(columnRecords, MapToColumnModel);
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
                    Default = ParseDefault(columnRecord.Default),
                    DefaultConstraintName = columnRecord.Default is not null
                        ? $"DF_{columnRecord.TableName}_{columnRecord.ColumnName}"
                        : null,
                };
            }

            private static DataType ParseDataType(ColumnRecord columnRecord)
            {
                string dataType = columnRecord.DataType.ToUpper();
                int length = int.Parse(columnRecord.Length);
                DataType dataTypeModel = PostgreSQLQueriesHelper.CreateDataTypeModel(dataType, length);
                return dataTypeModel;
            }

            private static object ParseDefault(string valueFromDBMSSysTable)
            {
                if (valueFromDBMSSysTable is null)
                    return null;
                string value = valueFromDBMSSysTable;

                if (IsFunction(value))
                    return new PostgreSQLDefaultValueAsFunction() { FunctionText = value };
                if (IsByte(value))
                    return ToByteArray(value);
                if (IsString(value))
                    return TrimOuterQuotes(value.Remove(value.LastIndexOf("::", StringComparison.Ordinal)));
                if (IsNumber(value))
                    return long.Parse(value);

                throw new ArgumentException($"Invalid parameter value '{valueFromDBMSSysTable}'", nameof(valueFromDBMSSysTable));

                static bool IsFunction(string val) => val.Contains("(") && val.Contains(")");
                static bool IsByte(string val) => !IsFunction(val) && val.Contains("::bytea");
                static bool IsString(string val) => !IsFunction(val) && new string[] { "::text", "::character" }.Any(x => val.Contains(x));
                static bool IsNumber(string val) => !IsFunction(val) && long.TryParse(val, out _);
                static string TrimOuterQuotes(string val) => val.Substring(1, val.Length - 2);
                static byte[] ToByteArray(string val)
                {
                    string hex = TrimOuterQuotes(val.Substring(2, val.Length - 2).Replace("::bytea", ""));
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
