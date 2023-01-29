using System;
using System.Linq;
using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using static DotNetDBTools.CodeParsing.Generated.SQLiteParser;
using HM = DotNetDBTools.CodeParsing.Core.HelperMethods;
using HMs = DotNetDBTools.CodeParsing.SQLite.SQLiteHelperMethods;

namespace DotNetDBTools.CodeParsing.SQLite;

internal class SQLiteGetObjectInfoVisitor : SQLiteParserBaseVisitor<ObjectInfo>
{
    private readonly bool _ignoreIds;

    public SQLiteGetObjectInfoVisitor(bool ignoreIds)
    {
        _ignoreIds = ignoreIds;
    }

    public override ObjectInfo VisitDndbt_sqldef_create_statement([NotNull] Dndbt_sqldef_create_statementContext context)
    {
        if (context.create_table_stmt() is not null)
            return GetTableInfo(context.create_table_stmt());
        else if (context.create_view_stmt() is not null)
            return GetViewInfo(context.create_view_stmt());
        else if (context.create_index_stmt() is not null)
            return GetIndexInfo(context.create_index_stmt());
        else if (context.create_trigger_stmt() is not null)
            return GetTriggerInfo(context.create_trigger_stmt());
        else
            throw new ParseException($"Unexpected create statement type {HM.GetStartLineAndPos(context)}");
    }

    private TableInfo GetTableInfo([NotNull] Create_table_stmtContext context)
    {
        TableInfo table = new();
        table.Name = HMs.Unquote(context.table_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(table, $"table '{table.Name}'", context.dndbt_id?.Text);

        foreach (Column_defContext columnCtx in context.column_def())
        {
            table.Columns.Add(GetTableColumnInfo(columnCtx, table.Name, out ConstraintInfo pkConstraintInfo));
            if (pkConstraintInfo is not null)
                table.Constraints.Add(pkConstraintInfo);
        }
        foreach (Table_constraintContext constraintCtx in context.table_constraint())
            table.Constraints.Add(GetTableConstraintInfo(constraintCtx, table.Name));

        return table;
    }

    private ViewInfo GetViewInfo([NotNull] Create_view_stmtContext context)
    {
        ViewInfo view = new();
        view.Name = HMs.Unquote(context.view_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(view, $"view '{view.Name}'", context.dndbt_id?.Text);

        view.CreateStatement = HM.GetInitialText(context);
        if (context.dndbt_id is not null)
            view.CreateStatement = view.CreateStatement.Remove(0, context.dndbt_id.Text.Length);
        return view;
    }

    private IndexInfo GetIndexInfo([NotNull] Create_index_stmtContext context)
    {
        IndexInfo index = new();
        index.Name = HMs.Unquote(context.index_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(index, $"index '{index.Name}'", context.dndbt_id?.Text);

        index.Table = HMs.Unquote(context.table_name().GetText());
        if (context.UNIQUE_() is not null)
            index.Unique = true;
        foreach (string column in context.indexed_column().Select(x => HMs.Unquote(x.GetText())))
            index.Columns.Add(column);
        return index;
    }

    private TriggerInfo GetTriggerInfo([NotNull] Create_trigger_stmtContext context)
    {
        TriggerInfo trigger = new();
        trigger.Name = HMs.Unquote(context.trigger_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(trigger, $"trigger '{trigger.Name}'", context.dndbt_id?.Text);

        trigger.Table = HMs.Unquote(context.table_name().GetText());
        trigger.CreateStatement = HM.GetInitialText(context);
        if (context.dndbt_id is not null)
            trigger.CreateStatement = trigger.CreateStatement.Remove(0, context.dndbt_id.Text.Length);
        return trigger;
    }

    private ColumnInfo GetTableColumnInfo(Column_defContext context, string tableName, out ConstraintInfo pkConstraintInfo)
    {
        ColumnInfo column = new();
        column.Name = HMs.Unquote(context.column_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(column, $"column '{column.Name}' in table '{tableName}'", context.dndbt_id?.Text);

        column.DataType = HMs.Unquote(context.type_name().GetText());
        foreach (Column_constraintContext constraintCtx in context.column_constraint())
            AddColumnConstraint(column, constraintCtx);

        pkConstraintInfo = SetPkConstraintIfAny();

        return column;

        void AddColumnConstraint(ColumnInfo column, Column_constraintContext context)
        {
            if (context.NOT_() is not null && context.NULL_() is not null)
                column.NotNull = true;
            else if (context.PRIMARY_() is not null && context.KEY_() is not null)
                column.PrimaryKey = true;
            else if (context.UNIQUE_() is not null)
                column.Unique = true;
            else if (context.DEFAULT_() is not null)
                AddColumnDefault(column, context);

            if (column.PrimaryKey && context.AUTOINCREMENT_() is not null)
                column.Identity = true;

            void AddColumnDefault(ColumnInfo column, Column_constraintContext context)
            {
                if (context.signed_number() is not null)
                    column.Default = context.signed_number().GetText();
                else if (context.literal_value() is not null)
                    column.Default = context.literal_value().GetText();
                else if (context.expr() is not null)
                    column.Default = $"({HM.GetInitialText(context.expr())})";
            }
        }

        ConstraintInfo SetPkConstraintIfAny()
        {
            ConstraintInfo pkConstraintInfo;
            if (column.PrimaryKey)
            {
                pkConstraintInfo = new ConstraintInfo();
                if (!_ignoreIds)
                    pkConstraintInfo.ID = GetPkId(context.dndbt_pkid?.Text);

                pkConstraintInfo.Type = ConstraintType.PrimaryKey;
                pkConstraintInfo.Columns.Add(column.Name);
            }
            else
            {
                pkConstraintInfo = null;
            }
            return pkConstraintInfo;

            Guid GetPkId(string idDeclarationComment)
            {
                if (idDeclarationComment is null)
                    throw new ParseException($"PKID declaration comment is missing for pk-column '{column.Name}' in table '{tableName}'");

                string idDeclStr = idDeclarationComment.Trim();
                int prefixLen = "--PKID:#{".Length;
                int postfixLen = "}#".Length;
                string idStr = idDeclStr.Substring(prefixLen, idDeclStr.Length - prefixLen - postfixLen);
                if (Guid.TryParse(idStr, out Guid id))
                    return id;
                else
                    throw new ParseException($"Failed to parse pk id from PKID declaration comment '{idDeclStr}'");
            }
        }
    }

    private ConstraintInfo GetTableConstraintInfo(Table_constraintContext context, string tableName)
    {
        ConstraintInfo constraint = new();
        if (context.name() is not null)
            constraint.Name = HMs.Unquote(context.name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(constraint, $"constraint '{constraint.Name}' in table '{tableName}'", context.dndbt_id?.Text);

        if (context.CHECK_() is not null)
            AddCheckConstraintInfo(constraint, context);
        else if (context.PRIMARY_() is not null && context.KEY_() is not null)
            AddPrimaryKeyConstraintInfo(constraint, context);
        else if (context.UNIQUE_() is not null)
            AddUniqueConstraintInfo(constraint, context);
        else if (context.FOREIGN_() is not null && context.KEY_() is not null)
            AddForeignKeyConstraintInfo(constraint, context);

        return constraint;

        static void AddCheckConstraintInfo(ConstraintInfo constraint, Table_constraintContext context)
        {
            constraint.Type = ConstraintType.Check;
            constraint.Expression = HM.GetInitialText(context.expr());
        }

        static void AddPrimaryKeyConstraintInfo(ConstraintInfo constraint, Table_constraintContext context)
        {
            constraint.Type = ConstraintType.PrimaryKey;
            foreach (string column in context.indexed_column().Select(x => HMs.Unquote(x.GetText())))
                constraint.Columns.Add(column);
        }

        static void AddUniqueConstraintInfo(ConstraintInfo constraint, Table_constraintContext context)
        {
            constraint.Type = ConstraintType.Unique;
            foreach (string column in context.indexed_column().Select(x => HMs.Unquote(x.GetText())))
                constraint.Columns.Add(column);
        }

        static void AddForeignKeyConstraintInfo(ConstraintInfo constraint, Table_constraintContext context)
        {
            constraint.Type = ConstraintType.ForeignKey;
            foreach (string column in context.column_name().Select(x => HMs.Unquote(x.GetText())))
                constraint.Columns.Add(column);

            Foreign_key_clauseContext fkClause = context.foreign_key_clause();
            constraint.RefTable = HMs.Unquote(fkClause.foreign_table().GetText());
            foreach (string column in fkClause.column_name().Select(x => HMs.Unquote(x.GetText())))
                constraint.RefColumns.Add(column);

            foreach (Foreign_key_action_clauseContext fkActionClause in fkClause.foreign_key_action_clause())
            {
                if (fkActionClause.ON_() is not null && fkActionClause.UPDATE_() is not null)
                    constraint.UpdateAction = HM.GetInitialText(fkActionClause.foreign_key_action());
                if (fkActionClause.ON_() is not null && fkActionClause.DELETE_() is not null)
                    constraint.DeleteAction = HM.GetInitialText(fkActionClause.foreign_key_action());
            }
        }
    }
}
