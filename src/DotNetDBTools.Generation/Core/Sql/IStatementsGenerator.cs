using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.Core.Sql;

public interface IStatementsGenerator
{
    public bool IncludeIdDeclarations { get; set; }

    public abstract string GetCreateSql(DbObject dbObject);
    public abstract string GetDropSql(DbObject dbObject);
}
