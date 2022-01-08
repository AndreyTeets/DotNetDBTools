using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Generated;

namespace DotNetDBTools.CodeParsing.PostgreSQL
{
    internal class PostgreSQLGetFunctionAttributesVisitor : PostgreSQLParserBaseVisitor<object>
    {
        public string FunctionLanguage { get; set; }
        public string FunctionBody { get; set; }

        public override object VisitFunction_def([NotNull] PostgreSQLParser.Function_defContext context)
        {
            if (context.character_string(0).BeginDollarStringConstant() != null)
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

        public override object VisitFunction_actions_common([NotNull] PostgreSQLParser.Function_actions_commonContext context)
        {
            if (context.LANGUAGE() != null)
            {
                FunctionLanguage = context.GetText().Replace(context.LANGUAGE().GetText(), "").Trim().ToUpper();
            }
            return base.VisitFunction_actions_common(context);
        }
    }
}
