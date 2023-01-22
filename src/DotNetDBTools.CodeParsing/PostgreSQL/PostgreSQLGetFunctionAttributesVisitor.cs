using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Generated;
using static DotNetDBTools.CodeParsing.Generated.PostgreSQLParser;

namespace DotNetDBTools.CodeParsing.PostgreSQL;

internal class PostgreSQLGetFunctionAttributesVisitor : PostgreSQLParserBaseVisitor<object>
{
    public string FunctionLanguage { get; set; }
    public string FunctionBody { get; set; }

    public override object VisitFunction_body([NotNull] Function_bodyContext context)
    {
        FunctionBody = HelperMethods.GetInitialText(context);
        return base.VisitFunction_body(context);
    }

    public override object VisitFunction_def([NotNull] Function_defContext context)
    {
        if (context.character_string(0).BeginDollarStringConstant() is not null)
        {
            string dollarConstant = context.character_string(0).BeginDollarStringConstant().GetText();
            string quotedFuncBody = context.character_string(0).GetText();
            FunctionBody = quotedFuncBody
                .Substring(dollarConstant.Length, quotedFuncBody.Length - 2 * dollarConstant.Length);
        }
        else
        {
            string quotedFuncBody = context.character_string(0).GetText();
            FunctionBody = quotedFuncBody
                .Substring(1, quotedFuncBody.Length - 2)
                .Replace("''", "'");
        }
        return base.VisitFunction_def(context);
    }

    public override object VisitFunction_actions_common([NotNull] Function_actions_commonContext context)
    {
        if (context.lang_name is not null)
        {
            string lang = context.lang_name.GetText().ToUpper();
            if (context.lang_name.Character_String_Literal() is not null)
                FunctionLanguage = lang.Substring(1, lang.Length - 2).Replace("''", "'");
            else
                FunctionLanguage = lang;
        }
        return base.VisitFunction_actions_common(context);
    }
}
