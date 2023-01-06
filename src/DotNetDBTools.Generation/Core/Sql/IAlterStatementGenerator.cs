using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Generation.Core.Sql;

public interface IAlterStatementGenerator
{
    public abstract string GetAlterSql(TableDiff tableDiff);
}
