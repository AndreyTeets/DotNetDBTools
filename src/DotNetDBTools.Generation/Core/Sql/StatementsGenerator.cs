using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.Core.Sql;

internal abstract class StatementsGenerator<TDbObject> : IStatementsGenerator
    where TDbObject : DbObject
{
    public bool IncludeIdDeclarations { get; set; }

    public string GetCreateSql(DbObject dbObject) => GetCreateSqlImpl((TDbObject)dbObject);
    public string GetDropSql(DbObject dbObject) => GetDropSqlImpl((TDbObject)dbObject);

    protected abstract string GetCreateSqlImpl(TDbObject dbObject);
    protected abstract string GetDropSqlImpl(TDbObject dbObject);

    protected string GetIdDeclarationText(DbObject dbObject, int indentAfter)
    {
        if (IncludeIdDeclarations)
            return $"--ID:#{{{dbObject.ID}}}#\n{new string(' ', indentAfter)}";
        else
            return "";
    }
}
