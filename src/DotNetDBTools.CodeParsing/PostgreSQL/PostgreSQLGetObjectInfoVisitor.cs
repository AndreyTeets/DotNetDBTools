using System.Linq;
using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using static DotNetDBTools.CodeParsing.Generated.PostgreSQLParser;
using HM = DotNetDBTools.CodeParsing.Core.HelperMethods;

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
        table.Name = UnquoteIdentifier(context.schema_qualified_name().GetText());
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
        view.Name = UnquoteIdentifier(context.schema_qualified_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(view, $"view '{view.Name}'", context.dndbt_id?.Text);

        view.Code = HM.GetInitialText(context);
        if (context.dndbt_id != null)
            view.Code = view.Code.Remove(0, context.dndbt_id.Text.Length);
        return view;
    }

    private IndexInfo GetIndexInfo(Create_index_statementContext context)
    {
        IndexInfo index = new();
        index.Name = UnquoteIdentifier(context.name.GetText());
        if (!_ignoreIds)
            HM.SetObjectID(index, $"index '{index.Name}'", context.dndbt_id?.Text);

        index.Table = UnquoteIdentifier(context.schema_qualified_name().GetText());
        if (context.UNIQUE() != null)
            index.Unique = true;
        foreach (string column in context.index_columns().index_column().Select(x => UnquoteIdentifier(x.vex().GetText())))
            index.Columns.Add(column);
        return index;
    }

    private TriggerInfo GetTriggerInfo(Create_trigger_statementContext context)
    {
        TriggerInfo trigger = new();
        trigger.Name = UnquoteIdentifier(context.name.GetText());
        if (!_ignoreIds)
            HM.SetObjectID(trigger, $"trigger '{trigger.Name}'", context.dndbt_id?.Text);

        trigger.Table = UnquoteIdentifier(context.table_name.GetText());
        trigger.Code = HM.GetInitialText(context);
        if (context.dndbt_id != null)
            trigger.Code = trigger.Code.Remove(0, context.dndbt_id.Text.Length);
        return trigger;
    }

    private TypeInfo GetTypeInfo(Create_type_statementContext context)
    {
        TypeInfo type = new();
        type.Name = UnquoteIdentifier(context.name.GetText());
        if (!_ignoreIds)
            HM.SetObjectID(type, $"type '{type.Name}'", context.dndbt_id?.Text);

        if (context._attrs.Count > 0)
        {
            type.TypeType = TypeType.Composite;
            foreach (Table_column_definitionContext attrContext in context._attrs)
            {
                string attrName = UnquoteIdentifier(attrContext.identifier().GetText());
                string attrDataType = UnquoteIdentifier(HM.GetInitialText(attrContext.data_type()));
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
            type.Subtype = UnquoteIdentifier(HM.GetInitialText(context.subtype_name));
            type.SubtypeOperatorClass = UnquoteIdentifier(HM.GetInitialTextOrNull(context.subtype_operator_class));
            type.Collation = UnquoteIdentifier(HM.GetInitialTextOrNull(context.collation));
            type.CanonicalFunction = UnquoteIdentifier(HM.GetInitialTextOrNull(context.canonical_function));
            type.SubtypeDiff = UnquoteIdentifier(HM.GetInitialTextOrNull(context.subtype_diff_function));
            type.MultirangeTypeName = UnquoteIdentifier(HM.GetInitialTextOrNull(context.multirange_name));
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
        type.Name = UnquoteIdentifier(context.name.GetText());
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
                constraint.Name = UnquoteIdentifier(constrContext.name.GetText());
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
        function.Name = UnquoteIdentifier(context.function_parameters().schema_qualified_name().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(function, $"function '{function.Name}'", context.dndbt_id?.Text);

        function.Code = HM.GetInitialText(context);
        if (context.dndbt_id != null)
            function.Code = function.Code.Remove(0, context.dndbt_id.Text.Length);
        return function;
    }

    private ColumnInfo GetTableColumnInfo(Table_item_definitionContext context, string tableName)
    {
        ColumnInfo column = new();
        column.Name = UnquoteIdentifier(context.table_column_definition().identifier().GetText());
        if (!_ignoreIds)
            HM.SetObjectID(column, $"column '{column.Name}' in table '{tableName}'", context.dndbt_id?.Text);

        column.DataType = UnquoteIdentifier(HM.GetInitialText(context.table_column_definition().data_type()));
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
            constraint.Name = UnquoteIdentifier(context.constraint_common().identifier().GetText());
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
            foreach (string column in context.cols.names_references().schema_qualified_name().Select(x => UnquoteIdentifier(x.GetText())))
                constraint.Columns.Add(column);
        }

        static void AddUniqueConstraintInfo(ConstraintInfo constraint, Constr_bodyContext context)
        {
            constraint.Type = ConstraintType.Unique;
            foreach (string column in context.cols.names_references().schema_qualified_name().Select(x => UnquoteIdentifier(x.GetText())))
                constraint.Columns.Add(column);
        }

        static void AddForeignKeyConstraintInfo(ConstraintInfo constraint, Constr_bodyContext context)
        {
            constraint.Type = ConstraintType.ForeignKey;
            foreach (string column in context.cols.names_references().schema_qualified_name().Select(x => UnquoteIdentifier(x.GetText())))
                constraint.Columns.Add(column);

            constraint.RefTable = UnquoteIdentifier(context.schema_qualified_name().GetText());
            foreach (string column in context.refcols.names_references().schema_qualified_name().Select(x => UnquoteIdentifier(x.GetText())))
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

    private static string UnquoteIdentifier(string quotedIdentifier)
    {
        return quotedIdentifier?.Replace("\"", "");
    }
}
