using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DDL
{
    internal class MSSQLDropTableQuery : DropTableQuery
    {
        public MSSQLDropTableQuery(Table table)
            : base(table) { }

        protected override string GetSql(Table table)
        {
            string query =
$@"DROP TABLE {table.Name};";

            return query;
        }
    }
}
