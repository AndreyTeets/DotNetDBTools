using System;
using System.Linq;
using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using static DotNetDBTools.CodeParsing.Generated.SQLiteParser;
using HM = DotNetDBTools.CodeParsing.Core.HelperMethods;

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
        if (context.create_table_stmt() != null)
            return GetTableInfo(context.create_table_stmt());
        else if (context.create_view_stmt() != null)
            return GetViewInfo(context.create_view_stmt());
        else if (context.create_index_stmt() != null)
            return GetIndexInfo(context.create_index_stmt());
        else if (context.create_trigger_stmt() != null)
            return GetTriggerInfo(context.create_trigger_stmt());
        else
            throw new ParseException($"Unexpected create statement type {HM.GetStartLineAndPos(context)}");
    }

    private TableInfo GetTableInfo([NotNull] Create_table_stmtContext context)
    {
        TableInfo table = new();
        table.Name = UnquoteIdentifier(context.table_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(table, $"table '{table.Name}'", context.dndbt_id?.Text);

        foreach (Column_defContext columnCtx in context.column_def())
            table.Columns.Add(GetTableColumnInfo(columnCtx, table.Name));
        foreach (Table_constraintContext constraintCtx in context.table_constraint())
            table.Constraints.Add(GetTableConstraintInfo(constraintCtx, table.Name));
        return table;
    }

    private ViewInfo GetViewInfo([NotNull] Create_view_stmtContext context)
    {
        ViewInfo view = new();
        view.Name = UnquoteIdentifier(context.view_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(view, $"view '{view.Name}'", context.dndbt_id?.Text);

        view.Code = HM.GetInitialText(context);
        if (context.dndbt_id != null)
            view.Code = view.Code.Remove(0, context.dndbt_id.Text.Length);
        return view;
    }

    private IndexInfo GetIndexInfo([NotNull] Create_index_stmtContext context)
    {
        IndexInfo index = new();
        index.Name = UnquoteIdentifier(context.index_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(index, $"index '{index.Name}'", context.dndbt_id?.Text);

        index.Table = UnquoteIdentifier(context.table_name().GetText());
        if (context.UNIQUE_() != null)
            index.Unique = true;
        foreach (string column in context.indexed_column().Select(x => UnquoteIdentifier(x.GetText())))
            index.Columns.Add(column);
        return index;
    }

    private TriggerInfo GetTriggerInfo([NotNull] Create_trigger_stmtContext context)
    {
        TriggerInfo trigger = new();
        trigger.Name = UnquoteIdentifier(context.trigger_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(trigger, $"trigger '{trigger.Name}'", context.dndbt_id?.Text);

        trigger.Table = UnquoteIdentifier(context.table_name().GetText());
        trigger.Code = HM.GetInitialText(context);
        if (context.dndbt_id != null)
            trigger.Code = trigger.Code.Remove(0, context.dndbt_id.Text.Length);
        return trigger;
    }

    private ColumnInfo GetTableColumnInfo(Column_defContext context, string tableName)
    {
        ColumnInfo column = new();
        column.Name = UnquoteIdentifier(context.column_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(column, $"column '{column.Name}' in table '{tableName}'", context.dndbt_id?.Text);

        column.DataType = UnquoteIdentifier(context.type_name().GetText());
        foreach (Column_constraintContext constraintCtx in context.column_constraint())
            AddColumnConstraint(column, constraintCtx);
        return column;

        void AddColumnConstraint(ColumnInfo column, Column_constraintContext context)
        {
            if (context.NOT_() != null && context.NULL_() != null)
                column.NotNull = true;
            else if (context.PRIMARY_() != null && context.KEY_() != null)
                column.PrimaryKey = true;
            else if (context.UNIQUE_() != null)
                column.Unique = true;
            else if (context.DEFAULT_() != null)
                AddColumnDefault(column, context);

            if (column.PrimaryKey && context.AUTOINCREMENT_() != null)
                column.Identity = true;

            void AddColumnDefault(ColumnInfo column, Column_constraintContext context)
            {
                if (context.signed_number() != null)
                    column.Default = context.signed_number().GetText();
                else if (context.literal_value() != null)
                    column.Default = context.literal_value().GetText();
                else if (context.expr() != null)
                    column.Default = $"({HM.GetInitialText(context.expr())})";
            }
        }
    }

    private ConstraintInfo GetTableConstraintInfo(Table_constraintContext context, string tableName)
    {
        ConstraintInfo constraint = new();
        if (context.name() != null)
            constraint.Name = UnquoteIdentifier(context.name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(constraint, $"constraint '{constraint.Name}' in table '{tableName}'", context.dndbt_id?.Text);

        if (context.CHECK_() != null)
            AddCheckConstraintInfo(constraint, context);
        else if (context.PRIMARY_() != null && context.KEY_() != null)
            AddPrimaryKeyConstraintInfo(constraint, context);
        else if (context.UNIQUE_() != null)
            AddUniqueConstraintInfo(constraint, context);
        else if (context.FOREIGN_() != null && context.KEY_() != null)
            AddForeignKeyConstraintInfo(constraint, context);

        return constraint;

        static void AddCheckConstraintInfo(ConstraintInfo constraint, Table_constraintContext context)
        {
            constraint.Type = ConstraintType.Check;
            constraint.Code = $"CHECK ({HM.GetInitialText(context.expr())})";
        }

        static void AddPrimaryKeyConstraintInfo(ConstraintInfo constraint, Table_constraintContext context)
        {
            constraint.Type = ConstraintType.PrimaryKey;
            foreach (string column in context.indexed_column().Select(x => UnquoteIdentifier(x.GetText())))
                constraint.Columns.Add(column);
        }

        static void AddUniqueConstraintInfo(ConstraintInfo constraint, Table_constraintContext context)
        {
            constraint.Type = ConstraintType.Unique;
            foreach (string column in context.indexed_column().Select(x => UnquoteIdentifier(x.GetText())))
                constraint.Columns.Add(column);
        }

        static void AddForeignKeyConstraintInfo(ConstraintInfo constraint, Table_constraintContext context)
        {
            constraint.Type = ConstraintType.ForeignKey;
            foreach (string column in context.column_name().Select(x => UnquoteIdentifier(x.GetText())))
                constraint.Columns.Add(column);

            Foreign_key_clauseContext fkClause = context.foreign_key_clause();
            constraint.RefTable = UnquoteIdentifier(fkClause.foreign_table().GetText());
            foreach (string column in fkClause.column_name().Select(x => UnquoteIdentifier(x.GetText())))
                constraint.RefColumns.Add(column);

            foreach (Foreign_key_action_clauseContext fkActionClause in fkClause.foreign_key_action_clause())
            {
                if (fkActionClause.ON_() != null && fkActionClause.UPDATE_() != null)
                    constraint.UpdateAction = HM.GetInitialText(fkActionClause.foreign_key_action());
                if (fkActionClause.ON_() != null && fkActionClause.DELETE_() != null)
                    constraint.DeleteAction = HM.GetInitialText(fkActionClause.foreign_key_action());
            }
        }
    }

    private static string UnquoteIdentifier(string quotedIdentifier)
    {
        return quotedIdentifier
            .Replace("[", "").Replace("]", "")
            .Replace("`", "")
            .Replace("\"", "");
    }
}
