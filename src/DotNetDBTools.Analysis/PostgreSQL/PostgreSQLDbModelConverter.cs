using System;
using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Agnostic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Analysis.PostgreSQL
{
    public class PostgreSQLDbModelConverter : IDbModelConverter
    {
        public Database FromAgnostic(Database database)
        {
            return ConvertToPostgreSQLModel((AgnosticDatabase)database);
        }

        private static PostgreSQLDatabase ConvertToPostgreSQLModel(AgnosticDatabase database)
           => new(database.Name)
           {
               Tables = database.Tables.Select(x => ConvertToPostgreSQLModel((AgnosticTable)x)).ToList(),
               Views = database.Views.Select(x => ConvertToPostgreSQLModel((AgnosticView)x)).ToList(),
           };

        private static PostgreSQLTable ConvertToPostgreSQLModel(AgnosticTable table)
            => new()
            {
                ID = table.ID,
                Name = table.Name,
                Columns = ConvertToPostgreSQLModel(table.Columns),
                PrimaryKey = table.PrimaryKey,
                UniqueConstraints = table.UniqueConstraints,
                CheckConstraints = table.CheckConstraints,
                Indexes = table.Indexes,
                Triggers = table.Triggers,
                ForeignKeys = table.ForeignKeys,
            };

        private static PostgreSQLView ConvertToPostgreSQLModel(AgnosticView view)
            => new()
            {
                ID = view.ID,
                Name = view.Name,
                CodePiece = view.CodePiece,
            };

        private static IEnumerable<Column> ConvertToPostgreSQLModel(IEnumerable<Column> columns)
        {
            foreach (Column column in columns)
            {
                column.DataType = column.DataType.Name switch
                {
                    AgnosticDataTypeNames.Int => ConvertIntSqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.Real => ConvertRealSqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.Decimal => ConvertDecimalSqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.Bool => new DataType { Name = PostgreSQLDataTypeNames.BOOL },

                    AgnosticDataTypeNames.String => ConvertStringSqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.Binary => new DataType { Name = PostgreSQLDataTypeNames.BYTEA },

                    AgnosticDataTypeNames.Date => new DataType { Name = PostgreSQLDataTypeNames.DATE },
                    AgnosticDataTypeNames.Time => ConvertTimeSqlType((AgnosticDataType)column.DataType),
                    AgnosticDataTypeNames.DateTime => ConvertDateTimeSqlType((AgnosticDataType)column.DataType),

                    AgnosticDataTypeNames.Guid => new DataType { Name = PostgreSQLDataTypeNames.UUID },
                    _ => throw new InvalidOperationException($"Invalid agnostic column datatype name: {column.DataType.Name}"),
                };
            }
            return columns;
        }

        private static DataType ConvertIntSqlType(AgnosticDataType dataType)
        {
            return dataType.Size switch
            {
                8 => new DataType { Name = PostgreSQLDataTypeNames.SMALLINT },
                16 => new DataType { Name = PostgreSQLDataTypeNames.SMALLINT },
                32 => new DataType { Name = PostgreSQLDataTypeNames.INT },
                64 => new DataType { Name = PostgreSQLDataTypeNames.BIGINT },
                _ => throw new Exception($"Invalid int size: '{dataType.Size}'")
            };
        }

        private static DataType ConvertRealSqlType(AgnosticDataType dataType)
        {
            if (dataType.IsDoublePrecision)
                return new DataType { Name = PostgreSQLDataTypeNames.FLOAT8 };
            else
                return new DataType { Name = PostgreSQLDataTypeNames.FLOAT4 };
        }

        private static DataType ConvertDecimalSqlType(AgnosticDataType dataType)
        {
            return new DataType { Name = $"{PostgreSQLDataTypeNames.DECIMAL}({dataType.Precision}, {dataType.Scale})" };
        }

        private static DataType ConvertStringSqlType(AgnosticDataType dataType)
        {
            string stringTypeName = dataType.IsFixedLength ? PostgreSQLDataTypeNames.CHAR : PostgreSQLDataTypeNames.VARCHAR;
            string lengthStr = dataType.Length.ToString();
            return new DataType { Name = $"{stringTypeName}({lengthStr})" };
        }

        private static DataType ConvertTimeSqlType(AgnosticDataType dataType)
        {
            if (dataType.IsWithTimeZone)
                return new DataType { Name = PostgreSQLDataTypeNames.TIMETZ };
            else
                return new DataType { Name = PostgreSQLDataTypeNames.TIME };
        }

        private static DataType ConvertDateTimeSqlType(AgnosticDataType dataType)
        {
            if (dataType.IsWithTimeZone)
                return new DataType { Name = PostgreSQLDataTypeNames.TIMESTAMPTZ };
            else
                return new DataType { Name = PostgreSQLDataTypeNames.TIMESTAMP };
        }
    }
}
