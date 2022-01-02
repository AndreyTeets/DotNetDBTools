using DotNetDBTools.Deploy.Common.Queries.DDL;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DDL
{
    internal class PostgreSQLDropIndexQuery : DropIndexQuery
    {
        public PostgreSQLDropIndexQuery(Index index, Table table)
            : base(index, table) { }

        protected override string GetSql(Index index, Table table)
        {
            string query =
$@"DROP INDEX ""{index.Name}"";";

            return query;
        }
    }
}
