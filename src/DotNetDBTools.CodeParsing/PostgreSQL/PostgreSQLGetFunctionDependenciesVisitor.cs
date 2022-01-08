using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Generated;

namespace DotNetDBTools.CodeParsing.PostgreSQL
{
    internal class PostgreSQLGetFunctionDependenciesVisitor : PostgreSQLParserBaseVisitor<object>
    {
        private readonly HashSet<Dependency> _dependencies = new();

        public List<Dependency> GetDependencies() => _dependencies.ToList();

        public override object VisitFunction_call([NotNull] PostgreSQLParser.Function_callContext context)
        {
            string functionName = Unquote(context.schema_qualified_name_nontype().GetText());
            _dependencies.Add(new Dependency { Type = ObjectType.Function, Name = functionName });
            return base.VisitFunction_call(context);
        }

        public override object VisitFrom_primary([NotNull] PostgreSQLParser.From_primaryContext context)
        {
            if (context.schema_qualified_name() != null)
            {
                string tableOrViewName = Unquote(context.schema_qualified_name().GetText());
                _dependencies.Add(new Dependency { Type = ObjectType.TableOrView, Name = tableOrViewName });
            }
            return base.VisitFrom_primary(context);
        }

        public override object VisitInsert_stmt_for_psql([NotNull] PostgreSQLParser.Insert_stmt_for_psqlContext context)
        {
            if (context.schema_qualified_name() != null)
            {
                string tableName = Unquote(context.schema_qualified_name().GetText());
                _dependencies.Add(new Dependency { Type = ObjectType.Table, Name = tableName });
            }
            return base.VisitInsert_stmt_for_psql(context);
        }

        private static string Unquote(string quotedIdentifier)
        {
            return quotedIdentifier.Replace("\"", "");
        }
    }
}
