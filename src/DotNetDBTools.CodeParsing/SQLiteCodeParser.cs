using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.CodeParsing.SQLite;

namespace DotNetDBTools.CodeParsing;

public class SQLiteCodeParser : CodeParser<SQLiteParser, SQLiteLexer>
{
    public override ObjectInfo GetObjectInfo(string createStatement)
    {
        if (ScriptDeclarationParser.TryParseScriptInfo(createStatement, out ScriptInfo scriptInfo))
            return scriptInfo;
        return ParseObjectInfo<SQLiteGetObjectInfoVisitor>(createStatement, x => x.dndbt_sqldef_create_statement());
    }
}
