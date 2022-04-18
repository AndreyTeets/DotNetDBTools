using Antlr4.Runtime.Tree;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.CodeParsing.SQLite;

namespace DotNetDBTools.CodeParsing;

public class SQLiteCodeParser : CodeParser<SQLiteParser, SQLiteLexer>
{
    public ObjectInfo GetObjectInfo(string input)
    {
        if (ScriptDeclarationParser.TryParseScriptInfo(input, out ScriptInfo scriptInfo))
            return scriptInfo;
        IParseTree parseTree = Parse(input, x => x.sql_stmt());
        SQLiteGetObjectInfoVisitor visitor = new();
        return visitor.Visit(parseTree);
    }
}
