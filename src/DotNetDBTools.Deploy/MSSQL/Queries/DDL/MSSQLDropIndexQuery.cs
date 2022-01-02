using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DDL
{
    internal class MSSQLDropIndexQuery : DropIndexQuery
    {
        public MSSQLDropIndexQuery(Index index, Table table)
            : base(index, table) { }

        protected override string GetSql(Index index, Table table)
        {
            string query =
$@"DROP INDEX [{index.Name}] ON [{table.Name}];";

            return query;
        }
    }
}
