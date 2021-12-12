using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DDL
{
    internal class MySQLDropTableQuery : DropTableQuery
    {
        public MySQLDropTableQuery(Table table)
            : base(table) { }

        protected override string GetSql(Table table)
        {
            string query =
$@"DROP TABLE `{table.Name}`;";

            return query;
        }
    }
}
