﻿using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.ModelBuilders;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Deploy.SQLite.Queries.DBMSSysInfo
{
    internal class GetColumnsFromDBMSSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    sm.name AS {nameof(ColumnRecord.TableName)},
    ti.name AS {nameof(ColumnRecord.ColumnName)},
    ti.type AS {nameof(ColumnRecord.DataType)},
    CASE WHEN ti.[notnull] = 1 THEN 0 ELSE 1 END AS {nameof(ColumnRecord.Nullable)},
    CASE WHEN ti.pk = 1 AND lower(ti.type) = 'integer' THEN 1 ELSE 0 END AS {nameof(ColumnRecord.IsIdentityCandidate)},
    ti.dflt_value AS [{nameof(ColumnRecord.Default)}]
FROM sqlite_master sm
INNER JOIN pragma_table_info(sm.name) ti
WHERE sm.type = 'table'
    AND sm.name != 'sqlite_sequence'
    AND sm.name != '{DNDBTSysTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class ColumnRecord : ColumnsBuilder.ColumnRecord
        {
            public bool IsIdentityCandidate { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static Dictionary<string, Table> BuildTablesListWithColumns(IEnumerable<ColumnRecord> columnRecords)
            {
                return ColumnsBuilder.BuildTablesListWithColumns<SQLiteTable>(columnRecords, MapToColumnModel);
            }

            private static Column MapToColumnModel(ColumnsBuilder.ColumnRecord builderColumnRecord)
            {
                ColumnRecord columnRecord = (ColumnRecord)builderColumnRecord;
                DataType dataType = SQLiteSqlTypeMapper.GetModelType(columnRecord.DataType);
                return new Column()
                {
                    ID = Guid.NewGuid(),
                    Name = columnRecord.ColumnName,
                    DataType = dataType,
                    Nullable = columnRecord.Nullable,
                    Identity = columnRecord.IsIdentityCandidate,
                    Default = ParseDefault(columnRecord.Default),
                    DefaultConstraintName = columnRecord.Default is not null
                        ? $"DF_{columnRecord.TableName}_{columnRecord.ColumnName}"
                        : null,
                };
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
                    return new SQLiteDefaultValueAsFunction() { FunctionText = value };
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