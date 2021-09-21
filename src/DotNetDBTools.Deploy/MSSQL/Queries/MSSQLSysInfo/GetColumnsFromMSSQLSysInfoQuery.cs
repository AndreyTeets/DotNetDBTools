using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries.MSSQLSysInfo
{
    internal class GetColumnsFromMSSQLSysInfoQuery : IQuery
    {
        public string Sql =>
$@"SELECT
    t.TABLE_NAME AS {nameof(ColumnRecord.TableName)},
    c.COLUMN_NAME AS {nameof(ColumnRecord.ColumnName)},
    CASE WHEN c.DOMAIN_NAME IS NOT NULL THEN c.DOMAIN_NAME ELSE c.DATA_TYPE END AS {nameof(ColumnRecord.DataTypeName)},
    CASE WHEN c.IS_NULLABLE='YES' THEN '{true}' ELSE '{false}' END AS {nameof(ColumnRecord.IsNullable)},
    c.COLUMN_DEFAULT AS {nameof(ColumnRecord.DefaultValue)},
    c.CHARACTER_OCTET_LENGTH AS {nameof(ColumnRecord.Length)}
FROM INFORMATION_SCHEMA.TABLES t
INNER JOIN INFORMATION_SCHEMA.COLUMNS c
    ON c.TABLE_NAME = t.TABLE_NAME
WHERE t.TABLE_TYPE='BASE TABLE';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        internal class ColumnRecord
        {
            public string TableName { get; set; }
            public string ColumnName { get; set; }
            public string DataTypeName { get; set; }
            public string IsNullable { get; set; }
            public string DefaultValue { get; set; }
            public string Length { get; set; }
        }

        internal static class ResultsInterpreter
        {
            public static Dictionary<string, MSSQLTableInfo> BuildTablesListWithColumns(IEnumerable<ColumnRecord> columnRecords)
            {
                Dictionary<string, MSSQLTableInfo> tables = new();
                foreach (ColumnRecord columnRecord in columnRecords)
                {
                    if (!tables.ContainsKey(columnRecord.TableName))
                    {
                        MSSQLTableInfo table = new()
                        {
                            ID = Guid.NewGuid(),
                            Name = columnRecord.TableName,
                            Columns = new List<ColumnInfo>(),
                            ForeignKeys = new List<MSSQLForeignKeyInfo>(),
                        };
                        tables.Add(columnRecord.TableName, table);
                    }
                    ColumnInfo columnInfo = MapToColumnInfo(columnRecord);
                    ((List<ColumnInfo>)tables[columnRecord.TableName].Columns).Add(columnInfo);
                }
                return tables;
            }

            private static ColumnInfo MapToColumnInfo(ColumnRecord columnRecord)
            {
                return new ColumnInfo()
                {
                    ID = Guid.NewGuid(),
                    Name = columnRecord.ColumnName,
                    DataTypeName = MSSQLSqlTypeMapper.GetModelType(columnRecord.DataTypeName),
                    DefaultValue = columnRecord.DefaultValue,
                    IsUnicode = MSSQLSqlTypeMapper.IsUnicode(columnRecord.DataTypeName),
                    Length = columnRecord.Length,
                    IsFixedLength = MSSQLSqlTypeMapper.IsFixedLength(columnRecord.DataTypeName),
                };
            }
        }
    }
}
