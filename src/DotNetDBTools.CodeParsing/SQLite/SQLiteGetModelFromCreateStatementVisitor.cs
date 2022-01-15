using System;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Core.Models;
using DotNetDBTools.CodeParsing.Generated;
using static DotNetDBTools.CodeParsing.Generated.SQLiteParser;

namespace DotNetDBTools.CodeParsing.SQLite
{
    internal class SQLiteGetModelFromCreateStatementVisitor : SQLiteParserBaseVisitor<ObjectInfo>
    {
        public override ObjectInfo VisitCreate_table_stmt([NotNull] Create_table_stmtContext context)
        {
            TableInfo table = new();
            table.ID = GetObjectID(context.ID_DECLARATION_COMMENT()?.GetText());
            table.Name = UnquoteIdentifier(context.table_name().GetText());
            foreach (Column_defContext columnCtx in context.column_def())
                table.Columns.Add(GetTableColumnInfo(columnCtx));
            foreach (Table_constraintContext constraintCtx in context.table_constraint())
                table.Constraints.Add(GetTableConstraintInfo(constraintCtx));
            return table;
        }

        public override ObjectInfo VisitCreate_view_stmt([NotNull] Create_view_stmtContext context)
        {
            ViewInfo view = new();
            view.ID = GetObjectID(context.ID_DECLARATION_COMMENT()?.GetText());
            view.Name = UnquoteIdentifier(context.view_name().GetText());
            view.Code = GetInitialText(context) + ";";
            if (context.ID_DECLARATION_COMMENT() != null)
                view.Code = view.Code.Remove(0, context.ID_DECLARATION_COMMENT().GetText().Length);
            return view;
        }

        public override ObjectInfo VisitCreate_index_stmt([NotNull] Create_index_stmtContext context)
        {
            IndexInfo index = new();
            index.ID = GetObjectID(context.ID_DECLARATION_COMMENT()?.GetText());
            index.Name = UnquoteIdentifier(context.index_name().GetText());
            index.Table = UnquoteIdentifier(context.table_name().GetText());
            if (context.UNIQUE_() != null)
                index.Unique = true;
            foreach (string column in context.indexed_column().Select(x => UnquoteIdentifier(x.GetText())))
                index.Columns.Add(column);
            return index;
        }

        public override ObjectInfo VisitCreate_trigger_stmt([NotNull] Create_trigger_stmtContext context)
        {
            TriggerInfo trigger = new();
            trigger.ID = GetObjectID(context.ID_DECLARATION_COMMENT()?.GetText());
            trigger.Name = UnquoteIdentifier(context.trigger_name().GetText());
            trigger.Table = UnquoteIdentifier(context.table_name().GetText());
            trigger.Code = GetInitialText(context) + ";";
            if (context.ID_DECLARATION_COMMENT() != null)
                trigger.Code = trigger.Code.Remove(0, context.ID_DECLARATION_COMMENT().GetText().Length);
            return trigger;
        }

        private static ColumnInfo GetTableColumnInfo(Column_defContext context)
        {
            ColumnInfo column = new();
            column.ID = GetObjectID(context.ID_DECLARATION_COMMENT()?.GetText());
            column.Name = UnquoteIdentifier(context.column_name().GetText());
            column.DataType = UnquoteIdentifier(context.type_name().GetText());
            foreach (Column_constraintContext constraintCtx in context.column_constraint())
                AddColumnConstraint(column, constraintCtx);
            return column;

            static void AddColumnConstraint(ColumnInfo column, Column_constraintContext context)
            {
                if (context.NOT_() != null && context.NULL_() != null)
                    column.NotNull = true;
                else if (context.PRIMARY_() != null && context.KEY_() != null)
                    column.PrimaryKey = true;
                else if (context.UNIQUE_() != null)
                    column.Unique = true;
                else if (context.DEFAULT_() != null)
                    AddColumnDefault(column, context);
                else
                    throw new ParseException("Invalid column constraint context");

                if (column.PrimaryKey && context.AUTOINCREMENT_() != null)
                    column.Autoincrement = true;

                static void AddColumnDefault(ColumnInfo column, Column_constraintContext context)
                {
                    if (context.signed_number() != null)
                        AddColumnDefaultWithType(column, DefaultType.Number, context.signed_number().GetText());
                    else if (context.literal_value() != null)
                        AddColumnDefaultWithType(column, DefaultType.String, UnquoteLiteral(context.literal_value().GetText()));
                    else if (context.expr() != null)
                        AddColumnDefaultWithType(column, DefaultType.Function, GetInitialText(context.expr()));
                    else
                        throw new ParseException("Invalid column default constraint context");

                    static void AddColumnDefaultWithType(ColumnInfo column, DefaultType type, string value)
                    {
                        column.DefaultType = type;
                        column.DefaultValue = value;
                    }
                }
            }
        }

        private static ConstraintInfo GetTableConstraintInfo(Table_constraintContext context)
        {
            ConstraintInfo constraint = new();
            constraint.ID = GetObjectID(context.ID_DECLARATION_COMMENT()?.GetText());
            if (context.name() != null)
                constraint.Name = UnquoteIdentifier(context.name().GetText());

            if (context.CHECK_() != null)
                AddCheckConstraintInfo(constraint, context);
            else if (context.PRIMARY_() != null && context.KEY_() != null)
                AddPrimaryKeyConstraintInfo(constraint, context);
            else if (context.UNIQUE_() != null)
                AddUniqueConstraintInfo(constraint, context);
            else if (context.FOREIGN_() != null && context.KEY_() != null)
                AddForeignKeyConstraintInfo(constraint, context);
            else
                throw new ParseException("Invalid table constraint context");

            return constraint;

            static void AddCheckConstraintInfo(ConstraintInfo constraint, Table_constraintContext context)
            {
                constraint.Type = ConstraintType.Check;
                constraint.Code = $"CHECK ({GetInitialText(context.expr())})";
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
                        constraint.UpdateAction = GetInitialText(fkActionClause.foreign_key_action());
                    if (fkActionClause.ON_() != null && fkActionClause.DELETE_() != null)
                        constraint.DeleteAction = GetInitialText(fkActionClause.foreign_key_action());
                }
            }
        }

        private static string GetInitialText(ParserRuleContext context)
        {
            return context.Start.InputStream.GetText(
                new Interval(context.Start.StartIndex, context.Stop.StopIndex));
        }

        private static Guid? GetObjectID(string idDeclarationComment)
        {
            if (idDeclarationComment is null)
                return null;

            string idDeclStr = idDeclarationComment.Trim();
            int prefixLen = "--ID:#{".Length;
            int postfixLen = "}#".Length;
            string idStr = idDeclStr.Substring(prefixLen, idDeclStr.Length - prefixLen - postfixLen);
            if (Guid.TryParse(idStr, out Guid id))
                return id;
            else
                return null;
        }

        private static string UnquoteIdentifier(string quotedIdentifier)
        {
            return quotedIdentifier
                .Replace("[", "").Replace("]", "")
                .Replace("`", "")
                .Replace("\"", "");
        }

        private static string UnquoteLiteral(string stringLiteral)
        {
            return stringLiteral.Substring(1, stringLiteral.Length - 2);
        }
    }
}
