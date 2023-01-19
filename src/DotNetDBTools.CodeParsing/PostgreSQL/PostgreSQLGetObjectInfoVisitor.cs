using System.Linq;
using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using static DotNetDBTools.CodeParsing.Generated.PostgreSQLParser;
using HM = DotNetDBTools.CodeParsing.Core.HelperMethods;
using HMs = DotNetDBTools.CodeParsing.PostgreSQL.PostgreSQLHelperMethods;

namespace DotNetDBTools.CodeParsing.PostgreSQL;

internal class PostgreSQLGetObjectInfoVisitor : PostgreSQLParserBaseVisitor<ObjectInfo>
{
    private readonly bool _ignoreIds;

    public PostgreSQLGetObjectInfoVisitor(bool ignoreIds)
    {
        _ignoreIds = ignoreIds;
    }

    public override ObjectInfo VisitDndbt_sqldef_create_statement([NotNull] Dndbt_sqldef_create_statementContext context)
    {
        if (context.create_table_statement() != null)
            return GetTableInfo(context.create_table_statement());
        else if (context.create_view_statement() != null)
            return GetViewInfo(context.create_view_statement());
        else if (context.create_index_statement() != null)
            return GetIndexInfo(context.create_index_statement());
        else if (context.create_trigger_statement() != null)
            return GetTriggerInfo(context.create_trigger_statement());
        else if (context.create_sequence_statement() != null)
            return GetSequenceInfo(context.create_sequence_statement());
        else if (context.create_type_statement() != null)
            return GetTypeInfo(context.create_type_statement());
        else if (context.create_domain_statement() != null)
            return GetDomainInfo(context.create_domain_statement());
        else if (context.create_function_statement() != null)
            return GetFunctionInfo(context.create_function_statement());
        else
            throw new ParseException($"Unexpected create statement type {HM.GetStartLineAndPos(context)}");
    }

    private TableInfo GetTableInfo(Create_table_statementContext context)
    {
        TableInfo table = new();
        table.Name = HMs.Unquote(context.schema_qualified_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(table, $"table '{table.Name}'", context.dndbt_id?.Text);

        Table_item_definitionContext[] tableItems = context.define_table().define_table_items().table_item_definition();
        foreach (Table_item_definitionContext columnCtx in tableItems.Where(x => x.table_column_definition() != null))
            table.Columns.Add(GetTableColumnInfo(columnCtx, table.Name));
        foreach (Table_item_definitionContext constraintCtx in tableItems.Where(x => x.constraint_common() != null))
            table.Constraints.Add(GetTableConstraintInfo(constraintCtx, table.Name));

        return table;
    }

    private ViewInfo GetViewInfo(Create_view_statementContext context)
    {
        ViewInfo view = new();
        view.Name = HMs.Unquote(context.schema_qualified_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(view, $"view '{view.Name}'", context.dndbt_id?.Text);

        view.CreateStatement = HM.GetInitialText(context);
        if (context.dndbt_id != null)
            view.CreateStatement = view.CreateStatement.Remove(0, context.dndbt_id.Text.Length);
        return view;
    }

    private IndexInfo GetIndexInfo(Create_index_statementContext context)
    {
        IndexInfo index = new();
        index.Name = HMs.Unquote(context.name.GetText());
        if (!_ignoreIds)
            HM.SetObjectID(index, $"index '{index.Name}'", context.dndbt_id?.Text);

        index.Table = HMs.Unquote(context.schema_qualified_name().GetText());
        if (context.UNIQUE() != null)
            index.Unique = true;
        foreach (Index_columnContext columnContext in context.index_columns().index_column())
            AddColumnOrSetExpression(columnContext);
        return index;

        void AddColumnOrSetExpression(Index_columnContext context)
        {
            if (context.vex().value_expression_primary().indirection_var() != null)
                index.Columns.Add(HMs.Unquote(context.vex().GetText()));
            else
                index.Expression = HM.GetInitialText(context);
        }
    }

    private TriggerInfo GetTriggerInfo(Create_trigger_statementContext context)
    {
        TriggerInfo trigger = new();
        trigger.Name = HMs.Unquote(context.name.GetText());
        if (!_ignoreIds)
            HM.SetObjectID(trigger, $"trigger '{trigger.Name}'", context.dndbt_id?.Text);

        trigger.Table = HMs.Unquote(HMs.RemoveSchemeIfAny(context.table_name.GetText(), out string scheme));
        trigger.CreateStatement = HM.GetInitialText(context);
        if (context.dndbt_id != null)
            trigger.CreateStatement = trigger.CreateStatement.Remove(0, context.dndbt_id.Text.Length);
        return trigger;
    }

    private SequenceInfo GetSequenceInfo(Create_sequence_statementContext context)
    {
        SequenceInfo sequence = new();
        sequence.Name = HMs.Unquote(context.schema_qualified_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(sequence, $"sequence '{sequence.Name}'", context.dndbt_id?.Text);

        foreach (Sequence_bodyContext opt in context.sequence_body())
        {
            if (opt.AS() != null)
                sequence.DataType = opt.type.Text;
            else if (opt.START() != null)
                sequence.StartWith = long.Parse(opt.start_val.GetText());
            else if (opt.INCREMENT() != null)
                sequence.IncrementBy = long.Parse(opt.incr.GetText());
            else if (opt.MINVALUE() != null && opt.NO() == null)
                sequence.MinValue = long.Parse(opt.minval.GetText());
            else if (opt.MAXVALUE() != null && opt.NO() == null)
                sequence.MaxValue = long.Parse(opt.maxval.GetText());
            else if (opt.CACHE() != null)
                sequence.Cache = long.Parse(opt.cache_val.GetText());
            else if (opt.CYCLE() != null)
                sequence.Cycle = opt.NO() == null;
            else if (opt.OWNED() != null)
                SetOwnedBy(sequence, opt.owned_by.GetText());
        }
        return sequence;

        static void SetOwnedBy(SequenceInfo sequence, string ownedByText)
        {
            string[] tableNameColumnName = ownedByText.Split('.');
            sequence.OwnedByTableName = HMs.Unquote(tableNameColumnName[0]);
            sequence.OwnedByColumnName = HMs.Unquote(tableNameColumnName[1]);
        }
    }

    private TypeInfo GetTypeInfo(Create_type_statementContext context)
    {
        TypeInfo type = new();
        type.Name = HMs.Unquote(context.name.GetText());
        if (!_ignoreIds)
            HM.SetObjectID(type, $"type '{type.Name}'", context.dndbt_id?.Text);

        if (context._attrs.Count > 0)
        {
            type.TypeType = TypeType.Composite;
            foreach (Table_column_definitionContext attrContext in context._attrs)
            {
                string attrName = HMs.Unquote(attrContext.identifier().GetText());
                string attrDataType = HMs.Unquote(HM.GetInitialText(attrContext.data_type()));
                type.Attributes.Add(attrName, attrDataType);
            }
        }
        else if (context.ENUM() != null)
        {
            type.TypeType = TypeType.Enum;
            type.AllowedValues = context._enums.Select(x => GetAllowedEnumValue(x)).ToList();
        }
        else if (context.RANGE() != null)
        {
            type.TypeType = TypeType.Range;
            type.Subtype = HMs.Unquote(HM.GetInitialText(context.subtype_name));
            type.SubtypeOperatorClass = HMs.Unquote(HM.GetInitialTextOrNull(context.subtype_operator_class));
            type.Collation = HMs.Unquote(HM.GetInitialTextOrNull(context.collation));
            type.CanonicalFunction = HMs.Unquote(HM.GetInitialTextOrNull(context.canonical_function));
            type.SubtypeDiff = HMs.Unquote(HM.GetInitialTextOrNull(context.subtype_diff_function));
            type.MultirangeTypeName = HMs.Unquote(HM.GetInitialTextOrNull(context.multirange_name));
        }

        return type;

        static string GetAllowedEnumValue(Character_stringContext context)
        {
            if (context.BeginDollarStringConstant() != null)
            {
                string dollarConstant = context.BeginDollarStringConstant().GetText();
                string quotedValue = context.GetText();
                return quotedValue.Substring(dollarConstant.Length, quotedValue.Length - 2 * dollarConstant.Length);
            }
            else
            {
                string quotedFuncBody = context.GetText();
                return quotedFuncBody.Substring(1, quotedFuncBody.Length - 2).Replace("''", "'");
            }
        }
    }

    private TypeInfo GetDomainInfo(Create_domain_statementContext context)
    {
        TypeInfo type = new();
        type.TypeType = TypeType.Domain;
        type.Name = HMs.Unquote(context.name.GetText());
        if (!_ignoreIds)
            HM.SetObjectID(type, $"type '{type.Name}'", context.dndbt_id?.Text);

        type.UnderlyingType = HM.GetInitialText(context.data_type());
        if (context.def_value != null)
            type.Default = HM.GetInitialText(context.def_value);
        foreach (Domain_constraintContext constrContext in context.domain_constraint())
        {
            if (constrContext.NOT() != null && constrContext.NULL() != null)
                type.NotNull = true;
            else if (constrContext.CHECK() != null)
                type.CheckConstraints.Add(GetCheckConstraintInfo(constrContext, type.Name));
        }

        return type;

        ConstraintInfo GetCheckConstraintInfo(Domain_constraintContext constrContext, string domainName)
        {
            ConstraintInfo constraint = new();
            if (constrContext.name != null)
                constraint.Name = HMs.Unquote(constrContext.name.GetText());
            if (!_ignoreIds)
                HM.SetObjectID(constraint, $"constraint '{constraint.Name}' in domain '{domainName}'", constrContext.dndbt_id?.Text);

            constraint.Type = ConstraintType.Check;
            constraint.Expression = HM.GetInitialText(constrContext.vex());
            return constraint;
        }
    }

    private FunctionInfo GetFunctionInfo(Create_function_statementContext context)
    {
        FunctionInfo function = new();
        string funcNameText = context.function_parameters().schema_qualified_name().GetText();
        function.Name = HMs.Unquote(HMs.RemoveSchemeIfAny(funcNameText, out string scheme));
        if (!_ignoreIds)
            HM.SetObjectID(function, $"function '{function.Name}'", context.dndbt_id?.Text);

        function.CreateStatement = HM.GetInitialText(context);
        if (context.dndbt_id != null)
            function.CreateStatement = function.CreateStatement.Remove(0, context.dndbt_id.Text.Length);
        return function;
    }

    private ColumnInfo GetTableColumnInfo(Table_item_definitionContext context, string tableName)
    {
        ColumnInfo column = new();
        column.Name = HMs.Unquote(context.table_column_definition().identifier().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(column, $"column '{column.Name}' in table '{tableName}'", context.dndbt_id?.Text);

        column.DataType = HMs.Unquote(HM.GetInitialText(context.table_column_definition().data_type()));
        foreach (Constraint_commonContext constraintCtx in context.table_column_definition().constraint_common())
            AddColumnConstraint(column, constraintCtx.constr_body());
        return column;

        void AddColumnConstraint(ColumnInfo column, Constr_bodyContext context)
        {
            if (context.NOT() != null && context.NULL() != null)
                column.NotNull = true;
            else if (context.PRIMARY() != null && context.KEY() != null)
                column.PrimaryKey = true;
            else if (context.UNIQUE() != null)
                column.Unique = true;
            else if (context.identity_body() != null && context.identity_body().ALWAYS() != null)
                column.Identity = true;
            else if (context.DEFAULT() != null)
                column.Default = HM.GetInitialText(context.vex());
        }
    }

    private ConstraintInfo GetTableConstraintInfo(Table_item_definitionContext context, string tableName)
    {
        ConstraintInfo constraint = new();
        if (context.constraint_common().identifier() != null)
            constraint.Name = HMs.Unquote(context.constraint_common().identifier().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(constraint, $"constraint '{constraint.Name}' in table '{tableName}'", context.dndbt_id?.Text);

        Constr_bodyContext constrBodyContext = context.constraint_common().constr_body();
        if (constrBodyContext.CHECK() != null)
            AddCheckConstraintInfo(constraint, constrBodyContext);
        else if (constrBodyContext.PRIMARY() != null && constrBodyContext.KEY() != null)
            AddPrimaryKeyConstraintInfo(constraint, constrBodyContext);
        else if (constrBodyContext.UNIQUE() != null)
            AddUniqueConstraintInfo(constraint, constrBodyContext);
        else if (constrBodyContext.FOREIGN() != null && constrBodyContext.KEY() != null)
            AddForeignKeyConstraintInfo(constraint, constrBodyContext);

        return constraint;

        static void AddCheckConstraintInfo(ConstraintInfo constraint, Constr_bodyContext context)
        {
            constraint.Type = ConstraintType.Check;
            constraint.Expression = HM.GetInitialText(context.vex());
        }

        static void AddPrimaryKeyConstraintInfo(ConstraintInfo constraint, Constr_bodyContext context)
        {
            constraint.Type = ConstraintType.PrimaryKey;
            foreach (string column in context.cols.names_references().schema_qualified_name().Select(x => HMs.Unquote(x.GetText())))
                constraint.Columns.Add(column);
        }

        static void AddUniqueConstraintInfo(ConstraintInfo constraint, Constr_bodyContext context)
        {
            constraint.Type = ConstraintType.Unique;
            foreach (string column in context.cols.names_references().schema_qualified_name().Select(x => HMs.Unquote(x.GetText())))
                constraint.Columns.Add(column);
        }

        static void AddForeignKeyConstraintInfo(ConstraintInfo constraint, Constr_bodyContext context)
        {
            constraint.Type = ConstraintType.ForeignKey;
            foreach (string column in context.cols.names_references().schema_qualified_name().Select(x => HMs.Unquote(x.GetText())))
                constraint.Columns.Add(column);

            constraint.RefTable = HMs.Unquote(context.schema_qualified_name().GetText());
            foreach (string column in context.refcols.names_references().schema_qualified_name().Select(x => HMs.Unquote(x.GetText())))
                constraint.RefColumns.Add(column);

            foreach (Fk_action_clauseContext fkActionClause in context.fk_action_clause())
            {
                if (fkActionClause.ON() != null && fkActionClause.UPDATE() != null)
                    constraint.UpdateAction = HM.GetInitialText(fkActionClause.fk_action());
                if (fkActionClause.ON() != null && fkActionClause.DELETE() != null)
                    constraint.DeleteAction = HM.GetInitialText(fkActionClause.fk_action());
            }
        }
    }
}
