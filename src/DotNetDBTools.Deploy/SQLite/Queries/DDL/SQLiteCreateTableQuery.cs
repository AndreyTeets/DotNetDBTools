using DotNetDBTools.Deploy.Core.Queries.DDL;
using DotNetDBTools.Models.Core;
using static DotNetDBTools.Deploy.SQLite.SQLiteQueriesHelper;

namespace DotNetDBTools.Deploy.SQLite.Queries.DDL
{
    internal class SQLiteCreateTableQuery : CreateTableQuery
    {
        public SQLiteCreateTableQuery(Table table)
            : base(table) { }

        protected override string GetSql(Table table)
        {
            string query =
$@"CREATE TABLE {table.Name}
(
{GetTableDefinitionsText(table)}
);";

            foreach (Index index in table.Indexes)
            {
                string _ =
$@"CREATE INDEX {index.Name}
ON {table.Name} ({string.Join(", ", index.Columns)});";
            }

            foreach (Trigger trigger in table.Triggers)
            {
                string _ =
$@"{trigger.Code}";
            }

            return query;
        }
    }
}
