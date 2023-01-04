using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.CodeParsing.SQLite;

namespace DotNetDBTools.CodeParsing;

public class SQLiteCodeParser : CodeParser<SQLiteParser, SQLiteLexer>
{
    public override ObjectInfo GetObjectInfo(string input)
    {
        if (ScriptDeclarationParser.TryParseScriptInfo(input, out ScriptInfo scriptInfo))
            return scriptInfo;
        return ParseObjectInfo<SQLiteGetObjectInfoVisitor>(input, x => x.dndbt_sqldef_create_statement());
    }
}
