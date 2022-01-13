using Antlr4.Runtime.Tree;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Core.Models;
using DotNetDBTools.CodeParsing.Generated;

namespace DotNetDBTools.CodeParsing.SQLite
{
    public class SQLiteCodeParser : CodeParser<SQLiteParser, SQLiteLexer>
    {
        public ObjectInfo GetModelFromCreateStatement(string input)
        {
            IParseTree parseTree = Parse(input, x => x.sql_stmt());
            SQLiteGetModelFromCreateStatementVisitor visitor = new();
            return visitor.Visit(parseTree);
        }
    }
}
