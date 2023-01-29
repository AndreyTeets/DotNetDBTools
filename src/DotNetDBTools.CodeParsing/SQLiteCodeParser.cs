using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using DotNetDBTools.CodeParsing.Core;
using DotNetDBTools.CodeParsing.Generated;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.CodeParsing.SQLite;

namespace DotNetDBTools.CodeParsing;

public class SQLiteCodeParser : CodeParser<SQLiteParser, SQLiteLexer>
{
    /// <inheritdoc />
    public override ObjectInfo GetObjectInfo(string createStatement)
    {
        if (ScriptDeclarationParser.TryParseScriptInfo(createStatement, out ScriptInfo scriptInfo))
            return scriptInfo;
        return ParseObjectInfo<SQLiteGetObjectInfoVisitor>(createStatement, x => x.dndbt_sqldef_create_statement());
    }

    /// <summary>
    /// Parses the list of tables|views referenced in the provided create view statement.
    /// </summary>
    public List<Dependency> GetViewDependencies(string createViewStatement)
    {
        IParseTree parseTree = Parse(createViewStatement, x => x.create_view_stmt());
        SQLiteGetViewDependenciesVisitor visitor = new();
        parseTree.Accept(visitor);
        return visitor.GetDependencies();
    }
}
