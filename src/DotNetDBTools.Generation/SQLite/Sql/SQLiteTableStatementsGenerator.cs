using System.Collections.Generic;
using System.Linq;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Generation.SQLite.Sql;

internal class SQLiteTableStatementsGenerator : TableStatementsGenerator<SQLiteTable, SQLiteTableDiff>
{
    private const string DNDBTTempPrefix = "_DNDBTTemp_";

    protected override string GetCreateSqlImpl(SQLiteTable table)
    {
        string res =
$@"{GetIdDeclarationText(table, 0)}CREATE TABLE [{table.Name}]
(
{GetTableDefinitionsText(table)}
);";

        return res;
    }

    protected override string GetDropSqlImpl(SQLiteTable table)
    {
        return $"DROP TABLE [{table.Name}];";
    }

    protected override string GetAlterSqlImpl(SQLiteTableDiff tableDiff)
    {
        string res =
$@"CREATE TABLE [{DNDBTTempPrefix}{tableDiff.NewTable.Name}]
(
{GetTableDefinitionsText((SQLiteTable)tableDiff.NewTable)}
);

INSERT INTO [{DNDBTTempPrefix}{tableDiff.NewTable.Name}]
(
{GetCommonColumnsNewNamesText(tableDiff)}
)
SELECT
{GetCommonColumnsOldNamesText(tableDiff)}
FROM [{tableDiff.OldTable.Name}];

DROP TABLE [{tableDiff.OldTable.Name}];

ALTER TABLE [{DNDBTTempPrefix}{tableDiff.NewTable.Name}] RENAME TO [{tableDiff.NewTable.Name}];";

        return res;

        static string GetCommonColumnsNewNamesText(SQLiteTableDiff tableDiff)
        {
            IEnumerable<string> commonNewOldColumnsNames = tableDiff.NewTable.Columns.Select(x => x.Name)
                .Except(tableDiff.ColumnsToAdd.Select(x => x.Name));
            return string.Join(",\n", commonNewOldColumnsNames.Select(x => $@"    [{x}]"));
        }

        static string GetCommonColumnsOldNamesText(SQLiteTableDiff tableDiff)
        {
            IEnumerable<string> commonNewOldColumnsNames = tableDiff.OldTable.Columns.Select(x => x.Name)
                .Except(tableDiff.ColumnsToDrop.Select(x => x.Name));
            return string.Join(",\n", commonNewOldColumnsNames.Select(x => $@"    [{x}]"));
        }
    }

    private string GetTableDefinitionsText(Table table)
    {
        List<string> tableDefinitions = new();

        tableDefinitions.AddRange(table.Columns.Select(c =>
$@"    {GetIdDeclarationText(c, 4)}{GetPkIdDeclarationTextIfAny(c)}{Statements.DefColumn(c, table.PrimaryKey)}"));

        if (table.PrimaryKey is not null && table.PrimaryKey.Columns.Count() > 1)
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

        string GetPkIdDeclarationTextIfAny(Column c)
        {
            if (IncludeIdDeclarations && ShouldDoInColumnPkDeclaration(c, table.PrimaryKey))
                return $"--PKID:#{{{table.PrimaryKey.ID}}}#\n    ";
            else
                return "";
        }
    }

    private static bool ShouldDoInColumnPkDeclaration(Column c, PrimaryKey pk)
    {
        return pk is not null
            && pk.Columns.Count() == 1
            && pk.Columns.Single() == c.Name;
    }

    private static class Statements
    {
        public static string DefColumn(Column c, PrimaryKey pk) =>
$@"[{c.Name}] {c.DataType.Name}{PrimaryKeyWithIdentityIfAny(c, pk)} {Nullability(c)}{Default(c)}"
            ;
        public static string DefPrimaryKey(PrimaryKey pk) =>
$@"CONSTRAINT [{pk.Name}] PRIMARY KEY ({string.Join(", ", pk.Columns.Select(x => $@"[{x}]"))})"
            ;
        public static string DefUniqueConstraint(UniqueConstraint uc) =>
$@"CONSTRAINT [{uc.Name}] UNIQUE ({string.Join(", ", uc.Columns.Select(x => $@"[{x}]"))})"
            ;
        public static string DefCheckConstraint(CheckConstraint ck) =>
$@"CONSTRAINT [{ck.Name}] CHECK ({ck.GetExpression()})"
            ;
        public static string DefForeignKey(ForeignKey fk) =>
$@"CONSTRAINT [{fk.Name}] FOREIGN KEY ({string.Join(", ", fk.ThisColumnNames.Select(x => $@"[{x}]"))})
        REFERENCES [{fk.ReferencedTableName}]({string.Join(", ", fk.ReferencedTableColumnNames.Select(x => $@"[{x}]"))})
        ON UPDATE {fk.OnUpdate} ON DELETE {fk.OnDelete}"
            ;

        private static string PrimaryKeyWithIdentityIfAny(Column c, PrimaryKey pk)
        {
            if (ShouldDoInColumnPkDeclaration(c, pk))
            {
                string identityStatement = c.Identity ? " AUTOINCREMENT" : "";
                return $" PRIMARY KEY{identityStatement}";
            }
            return "";
        }
        private static string Nullability(Column c) =>
c.NotNull ? "NOT NULL" : "NULL"
            ;
        private static string Default(Column c) =>
c.Default is not null ? $@" DEFAULT {c.GetDefault()}" : ""
            ;
    }
}
