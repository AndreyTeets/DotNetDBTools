﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Generation.PostgreSQL;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL.Sql;

internal class PostgreSQLTableStatementsGenerator : TableStatementsGenerator<PostgreSQLTable, PostgreSQLTableDiff>
{
    protected override string GetCreateSqlImpl(PostgreSQLTable table)
    {
        string res =
$@"{GetIdDeclarationText(table, 0)}CREATE TABLE ""{table.Name}""
(
{GetTableDefinitionsText(table)}
);";

        return res;
    }

    protected override string GetDropSqlImpl(PostgreSQLTable table)
    {
        return $@"DROP TABLE ""{table.Name}"";";
    }

    protected override string GetAlterSqlImpl(PostgreSQLTableDiff tableDiff)
    {
        StringBuilder sb = new();

        if (tableDiff.NewName != tableDiff.OldName)
            sb.AppendLine(Statements.RenameTable(tableDiff.OldName, tableDiff.NewName));

        foreach (ColumnDiff columnDiff in tableDiff.ColumnsToAlter.Where(x => x.NewName != x.OldName))
            sb.AppendLine(Statements.RenameColumn(tableDiff.NewName, columnDiff.OldName, columnDiff.NewName));

        string tableAlters = GetTableAltersText(tableDiff);
        if (!string.IsNullOrEmpty(tableAlters))
            sb.AppendLine($@"ALTER TABLE ""{tableDiff.NewName}""{tableAlters};");

        return sb.ToString();
    }

    private string GetTableDefinitionsText(Table table)
    {
        List<string> tableDefinitions = new();

        tableDefinitions.AddRange(table.Columns.Select(c =>
$@"    {GetIdDeclarationText(c, 4)}{Statements.DefColumn((PostgreSQLColumn)c)}"));

        if (table.PrimaryKey is not null)
        {
            tableDefinitions.Add(
$@"    {GetIdDeclarationText(table.PrimaryKey, 4)}{Statements.DefPrimaryKey(table.PrimaryKey)}");
        }

        tableDefinitions.AddRange(table.UniqueConstraints.Select(uc =>
$@"    {GetIdDeclarationText(uc, 4)}{Statements.DefUniqueConstraint(uc)}"));

        tableDefinitions.AddRange(table.CheckConstraints.Select(ck =>
$@"    {GetIdDeclarationText(ck, 4)}{Statements.DefCheckConstraint(ck)}"));

        tableDefinitions.AddRange(table.ForeignKeys.Select(fk =>
$@"    {GetIdDeclarationText(fk, 4)}{Statements.DefForeignKey(fk)}"));

        return string.Join(",\n", tableDefinitions);
    }

    private string GetTableAltersText(TableDiff tableDiff)
    {
        StringBuilder sb = new();

        foreach (ForeignKey fk in tableDiff.ForeignKeysToDrop)
            sb.Append(Statements.DropForeignKey(fk));
        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToDrop)
            sb.Append(Statements.DropCheckConstraint(ck));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToDrop)
            sb.Append(Statements.DropUniqueConstraint(uc));
        if (tableDiff.PrimaryKeyToDrop is not null)
            sb.Append(Statements.DropPrimaryKey(tableDiff.PrimaryKeyToDrop));

        AppendColumnsAlters(sb, tableDiff);

        if (tableDiff.PrimaryKeyToCreate is not null)
            sb.Append(Statements.AddPrimaryKey(tableDiff.PrimaryKeyToCreate));
        foreach (UniqueConstraint uc in tableDiff.UniqueConstraintsToCreate)
            sb.Append(Statements.AddUniqueConstraint(uc));
        foreach (CheckConstraint ck in tableDiff.CheckConstraintsToCreate)
            sb.Append(Statements.AddCheckConstraint(ck));
        foreach (ForeignKey fk in tableDiff.ForeignKeysToCreate)
            sb.Append(Statements.AddForeignKey(fk));

        if (sb.Length > 0)
            sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }

    private void AppendColumnsAlters(StringBuilder sb, TableDiff tableDiff)
    {
        foreach (Column column in tableDiff.ColumnsToDrop)
            sb.Append(Statements.DropColumn(column));

        foreach (ColumnDiff columnDiff in tableDiff.ColumnsToAlter)
        {
            if (columnDiff.DataTypeToSet is not null)
                sb.Append(Statements.AlterColumnType(columnDiff.NewName, columnDiff.DataTypeToSet));

            if (columnDiff.NotNullToSet is not null && columnDiff.NotNullToSet == true)
                sb.Append(Statements.SetColumnNotNull(columnDiff.NewName));
            else if (columnDiff.NotNullToSet is not null && columnDiff.NotNullToSet == false)
                sb.Append(Statements.DropColumnNotNull(columnDiff.NewName));

            if (columnDiff.DefaultToSet is not null)
                sb.Append(Statements.SetColumnDefault(columnDiff.NewName, columnDiff.DefaultToSet));
            else if (columnDiff.DefaultToDrop is not null)
                sb.Append(Statements.DropColumnDefault(columnDiff.NewName));

            PostgreSQLColumnDiff cDiff = (PostgreSQLColumnDiff)columnDiff;
            if (cDiff.IdentityToSet is not null && cDiff.IdentityToSet == true)
            {
                if (cDiff.IdentityGenerationKindToSet is null || cDiff.IdentitySequenceOptionsToSet is null)
                    throw new Exception($"Invalid columnDiff with missing Identity[GenerationKind|SequenceOptions]ToSet");
                sb.Append(Statements.AddIdentity(columnDiff.NewName, cDiff.IdentityGenerationKindToSet, cDiff.IdentitySequenceOptionsToSet));
            }
            else if (cDiff.IdentityToSet is not null && cDiff.IdentityToSet == false)
            {
                sb.Append(Statements.DropIdentity(columnDiff.NewName));
            }
            else
            {
                if (cDiff.IdentityGenerationKindToSet is not null)
                    sb.Append(Statements.SetIdentityGenerationKind(columnDiff.NewName, cDiff.IdentityGenerationKindToSet));
                if (cDiff.IdentitySequenceOptionsToSet is not null)
                    SetIdentitySequenceOptions(columnDiff.NewName, cDiff.IdentitySequenceOptionsToSet);
            }
        }

        foreach (Column column in tableDiff.ColumnsToAdd)
            sb.Append(Statements.AddColumn(column));

        void SetIdentitySequenceOptions(string cName, PostgreSQLSequenceOptions so)
        {
            if (so.StartWith is not null)
                sb.Append(Statements.SetIdentitySequenceOption(cName, $"START {so.StartWith}"));
            if (so.IncrementBy is not null)
                sb.Append(Statements.SetIdentitySequenceOption(cName, $"INCREMENT {so.IncrementBy}"));
            if (so.MinValue is not null)
                sb.Append(Statements.SetIdentitySequenceOption(cName, $"MINVALUE {so.MinValue}"));
            if (so.MaxValue is not null)
                sb.Append(Statements.SetIdentitySequenceOption(cName, $"MAXVALUE {so.MaxValue}"));
            if (so.Cache is not null)
                sb.Append(Statements.SetIdentitySequenceOption(cName, $"CACHE {so.Cache}"));
            if (so.Cycle is not null)
                sb.Append(Statements.SetIdentitySequenceOption(cName, so.Cycle.Value ? $"CYCLE" : "NO CYCLE"));
        }
    }

    private static class Statements
    {
        public static string DefColumn(PostgreSQLColumn c) =>
$@"""{c.Name}"" {c.DataType.Name}{Identity(c.IdentityGenerationKind, c.IdentitySequenceOptions)} {Nullability(c)}{Default(c)}"
            ;
        public static string DefPrimaryKey(PrimaryKey pk) =>
$@"CONSTRAINT ""{pk.Name}"" PRIMARY KEY ({string.Join(", ", pk.Columns.Select(x => $@"""{x}"""))})"
            ;
        public static string DefUniqueConstraint(UniqueConstraint uc) =>
$@"CONSTRAINT ""{uc.Name}"" UNIQUE ({string.Join(", ", uc.Columns.Select(x => $@"""{x}"""))})"
            ;
        public static string DefCheckConstraint(CheckConstraint ck) =>
$@"CONSTRAINT ""{ck.Name}"" CHECK ({ck.GetExpression()})"
            ;
        public static string DefForeignKey(ForeignKey fk) =>
$@"CONSTRAINT ""{fk.Name}"" FOREIGN KEY ({string.Join(", ", fk.ThisColumnNames.Select(x => $@"""{x}"""))})
        REFERENCES ""{fk.ReferencedTableName}"" ({string.Join(", ", fk.ReferencedTableColumnNames.Select(x => $@"""{x}"""))})
        ON UPDATE {fk.OnUpdate} ON DELETE {fk.OnDelete}"
            ;

        public static string RenameTable(string oldTableName, string newTableName) =>
$@"ALTER TABLE ""{oldTableName}"" RENAME TO ""{newTableName}"";"
            ;
        public static string RenameColumn(string tableName, string oldColumnName, string newColumnName) =>
$@"
ALTER TABLE ""{tableName}"" RENAME COLUMN ""{oldColumnName}"" TO ""{newColumnName}"";"
            ;

        public static string AddColumn(Column c) =>
$@"
    ADD COLUMN {DefColumn((PostgreSQLColumn)c)},"
            ;
        public static string DropColumn(Column c) =>
$@"
    DROP COLUMN ""{c.Name}"","
            ;

        public static string AlterColumnType(string cName, DataType dataType) =>
$@"
    ALTER COLUMN ""{cName}"" SET DATA TYPE {dataType.Name}
        USING (""{cName}""::text::{dataType.Name}),"
            ; // TODO Add TypeChangeConversions<srcTypeName,destTypeName,usingCode> in deploy/generation options?

        public static string SetColumnNotNull(string cName) =>
$@"
    ALTER COLUMN ""{cName}"" SET NOT NULL,"
            ;
        public static string DropColumnNotNull(string cName) =>
$@"
    ALTER COLUMN ""{cName}"" DROP NOT NULL,"
            ;

        public static string SetColumnDefault(string cName, CodePiece dValue) =>
$@"
    ALTER COLUMN ""{cName}"" SET DEFAULT {dValue.Code},"
            ;
        public static string DropColumnDefault(string cName) =>
$@"
    ALTER COLUMN ""{cName}"" DROP DEFAULT,"
            ;

        public static string AddPrimaryKey(PrimaryKey pk) =>
$@"
    ADD {DefPrimaryKey(pk)},"
            ;
        public static string DropPrimaryKey(PrimaryKey pk) =>
$@"
    DROP CONSTRAINT ""{pk.Name}"","
            ;

        public static string AddUniqueConstraint(UniqueConstraint uc) =>
$@"
    ADD {DefUniqueConstraint(uc)},"
            ;
        public static string DropUniqueConstraint(UniqueConstraint uc) =>
$@"
    DROP CONSTRAINT ""{uc.Name}"","
            ;

        public static string AddCheckConstraint(CheckConstraint ck) =>
$@"
    ADD {DefCheckConstraint(ck)},"
            ;
        public static string DropCheckConstraint(CheckConstraint ck) =>
$@"
    DROP CONSTRAINT ""{ck.Name}"","
            ;

        public static string AddForeignKey(ForeignKey fk) =>
$@"
    ADD {DefForeignKey(fk)},"
            ;
        public static string DropForeignKey(ForeignKey fk) =>
$@"
    DROP CONSTRAINT ""{fk.Name}"","
            ;

        public static string AddIdentity(string cName, string generationKind, PostgreSQLSequenceOptions sequenceOptions) =>
$@"
    ALTER COLUMN ""{cName}"" ADD {Identity(generationKind, sequenceOptions)},"
            ;
        public static string DropIdentity(string cName) =>
$@"
    ALTER COLUMN ""{cName}"" DROP IDENTITY,"
            ;

        public static string SetIdentityGenerationKind(string cName, string generationKind) =>
$@"
    ALTER COLUMN ""{cName}"" SET GENERATED {generationKind},"
            ;
        public static string SetIdentitySequenceOption(string cName, string sequence_option) =>
$@"
    ALTER COLUMN ""{cName}"" SET {sequence_option},"
            ;

        private static string Identity(string generationKind, PostgreSQLSequenceOptions sequenceOptions) =>
generationKind is not null ? $" GENERATED {generationKind} AS IDENTITY{IdentitySequenceOptions(sequenceOptions)}" : ""
            ;
        private static string Nullability(Column c) =>
c.NotNull ? "NOT NULL" : "NULL"
            ;
        private static string Default(Column c) =>
c.Default is not null ? $@" DEFAULT {c.GetDefault()}" : ""
            ;

        private static string IdentitySequenceOptions(PostgreSQLSequenceOptions so)
        {
            List<string> res = new();
            if (so.StartWith is not null)
                res.Add($"START {so.StartWith}");
            if (so.IncrementBy is not null)
                res.Add($"INCREMENT {so.IncrementBy}");
            if (so.MinValue is not null)
                res.Add($"MINVALUE {so.MinValue}");
            if (so.MaxValue is not null)
                res.Add($"MAXVALUE {so.MaxValue}");
            if (so.Cache is not null)
                res.Add($"CACHE {so.Cache}");
            if (so.Cycle is not null)
                res.Add(so.Cycle.Value ? $"CYCLE" : "NO CYCLE");

            if (res.Count > 0)
                return $" ({string.Join(" ", res)})";
            else
                return "";
        }
    }
}
