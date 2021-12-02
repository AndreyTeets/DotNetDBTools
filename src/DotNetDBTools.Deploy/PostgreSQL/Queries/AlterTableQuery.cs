﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;
using static DotNetDBTools.Deploy.PostgreSQL.PostgreSQLQueriesHelper;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries
{
    internal class AlterTableQuery : IQuery
    {
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public AlterTableQuery(PostgreSQLTableDiff tableDiff)
        {
            _sql = GetSql(tableDiff);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(PostgreSQLTableDiff tableDiff)
        {
            StringBuilder sb = new();

            if (tableDiff.OldTable.Name != tableDiff.NewTable.Name)
                sb.Append(Queries.RenameTable(tableDiff.OldTable.Name, tableDiff.NewTable.Name));

            foreach (Trigger trigger in tableDiff.TriggersToDrop)
                sb.Append(Queries.DropTrigger(trigger.Name));
            foreach (Index index in tableDiff.IndexesToDrop)
                sb.Append(Queries.DropIndex(index.Name));

            foreach (CheckConstraint cc in tableDiff.CheckConstraintsToDrop)
                sb.Append(Queries.DropCheckConstraint(tableDiff.NewTable.Name, cc.Name));
            foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
                sb.Append(Queries.DropUniqueConstraint(tableDiff.NewTable.Name, uc.Name));
            if (tableDiff.PrimaryKeyToDrop is not null)
                sb.Append(Queries.DropPrimaryKey(tableDiff.NewTable.Name, tableDiff.PrimaryKeyToDrop.Name));

            AppendColumnsAlters(sb, tableDiff);

            if (tableDiff.PrimaryKeyToCreate is not null)
                sb.Append(Queries.AddPrimaryKey(tableDiff.NewTable.Name, tableDiff.PrimaryKeyToCreate));
            foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
                sb.Append(Queries.AddUniqueConstraint(tableDiff.NewTable.Name, uc));
            foreach (CheckConstraint cc in tableDiff.CheckConstraintsToCreate)
                sb.Append(Queries.AddCheckConstraint(tableDiff.NewTable.Name, cc));

            foreach (Index index in tableDiff.IndexesToCreate)
                sb.Append(Queries.CreateIndex(tableDiff.NewTable.Name, index));
            foreach (Trigger trigger in tableDiff.TriggersToCreate)
                sb.Append(Queries.CreateTrigger(trigger));

            return sb.ToString();
        }

        private static void AppendColumnsAlters(StringBuilder sb, PostgreSQLTableDiff tableDiff)
        {
            foreach (Column column in tableDiff.RemovedColumns)
            {
                if (column.Default is not null)
                    sb.Append(Queries.DropDefaultConstraint(tableDiff.NewTable.Name, column));
                sb.Append(Queries.DropColumn(tableDiff.NewTable.Name, column.Name));
            }

            foreach (ColumnDiff columnDiff in tableDiff.ChangedColumns)
            {
                if (columnDiff.OldColumn.Default is not null)
                    sb.Append(Queries.DropDefaultConstraint(tableDiff.NewTable.Name, columnDiff.OldColumn));

                if (columnDiff.OldColumn.Name != columnDiff.NewColumn.Name)
                    sb.Append(Queries.RenameColumn(tableDiff.NewTable.Name, columnDiff.OldColumn.Name, columnDiff.NewColumn.Name));

                sb.Append(Queries.AlterColumnType(tableDiff.NewTable.Name, columnDiff.NewColumn));
                if (!columnDiff.NewColumn.Nullable && columnDiff.OldColumn.Nullable)
                    sb.Append(Queries.SetColumnNotNull(tableDiff.NewTable.Name, columnDiff.NewColumn));
                else if (columnDiff.NewColumn.Nullable && !columnDiff.OldColumn.Nullable)
                    sb.Append(Queries.DropColumnNotNull(tableDiff.NewTable.Name, columnDiff.NewColumn));

                if (columnDiff.NewColumn.Default is not null)
                    sb.Append(Queries.AddDefaultConstraint(tableDiff.NewTable.Name, columnDiff.NewColumn));
            }

            foreach (Column column in tableDiff.AddedColumns)
            {
                sb.Append(Queries.AddColumn(tableDiff.NewTable.Name, column));
                if (column.Default is not null)
                    sb.Append(Queries.AddDefaultConstraint(tableDiff.NewTable.Name, column));
            }
        }

        private static class Queries
        {
            public static string RenameTable(string oldTableName, string newTableName) =>
$@"ALTER TABLE ""{oldTableName}"" RENAME TO ""{newTableName}"";"
                ;
            public static string RenameColumn(string tableName, string oldColumnName, string newColumnName) =>
$@"
ALTER TABLE ""{tableName}"" RENAME COLUMN ""{oldColumnName}"" TO ""{newColumnName}"";"
                ;

            public static string AddColumn(string tableName, Column c) =>
$@"
ALTER TABLE ""{tableName}"" ADD COLUMN ""{c.Name}"" {c.DataType.Name}{GetIdentityStatement(c)} {GetNullabilityStatement(c)};"
                ;
            public static string DropColumn(string tableName, string columnName) =>
$@"
ALTER TABLE ""{tableName}"" DROP COLUMN ""{columnName}"";"
                ;
            public static string AlterColumnType(string tableName, Column c) =>
$@"
ALTER TABLE ""{tableName}"" ALTER COLUMN ""{c.Name}"" SET DATA TYPE {c.DataType.Name};"
                ;
            public static string SetColumnNotNull(string tableName, Column c) =>
$@"
ALTER TABLE ""{tableName}"" ALTER COLUMN ""{c.Name}"" SET NOT NULL;"
    ;
            public static string DropColumnNotNull(string tableName, Column c) =>
$@"
ALTER TABLE ""{tableName}"" ALTER COLUMN ""{c.Name}"" DROP NOT NULL;"
    ;

            public static string AddPrimaryKey(string tableName, PrimaryKey pk) =>
$@"
ALTER TABLE ""{tableName}"" ADD CONSTRAINT ""{pk.Name}"" PRIMARY KEY ({string.Join(", ", pk.Columns.Select(x => $@"""{x}"""))});"
                ;
            public static string DropPrimaryKey(string tableName, string pkName) =>
$@"
ALTER TABLE ""{tableName}"" DROP CONSTRAINT ""{pkName}"";"
                ;

            public static string AddUniqueConstraint(string tableName, UniqueConstraint uc) =>
$@"
ALTER TABLE ""{tableName}"" ADD CONSTRAINT ""{uc.Name}"" UNIQUE ({string.Join(", ", uc.Columns.Select(x => $@"""{x}"""))});"
                ;
            public static string DropUniqueConstraint(string tableName, string ucName) =>
$@"
ALTER TABLE ""{tableName}"" DROP CONSTRAINT ""{ucName}"";"
                ;

            public static string AddCheckConstraint(string tableName, CheckConstraint cc) =>
$@"
ALTER TABLE ""{tableName}"" ADD CONSTRAINT ""{cc.Name}"" CHECK ({cc.Code});"
                ;
            public static string DropCheckConstraint(string tableName, string ccName) =>
$@"
ALTER TABLE ""{tableName}"" DROP CONSTRAINT ""{ccName}"";"
                ;

            public static string AddDefaultConstraint(string tableName, Column c) =>
$@"
ALTER TABLE ""{tableName}"" ALTER COLUMN ""{c.Name}"" SET DEFAULT {QuoteDefaultValue(c.Default)};"
                ;
            public static string DropDefaultConstraint(string tableName, Column c) =>
$@"
ALTER TABLE ""{tableName}"" ALTER COLUMN ""{c.Name}"" DROP DEFAULT;"
                ;

            public static string CreateIndex(string tableName, Index index) =>
$@"
CREATE INDEX ""{index.Name}""
ON ""{tableName}"" ({string.Join(", ", index.Columns.Select(x => $@"""{x}"""))});"
                ;
            public static string DropIndex(string indexName) =>
$@"
DROP INDEX ""{indexName}"";"
                ;

            public static string CreateTrigger(Trigger trigger) =>
$@"
{trigger.Code}"
                ;
            public static string DropTrigger(string triggerName) =>
$@"
DROP TRIGGER ""{triggerName}"";"
                ;
        }
    }
}