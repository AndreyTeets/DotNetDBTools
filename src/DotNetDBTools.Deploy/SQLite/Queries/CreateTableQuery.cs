using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;
using static DotNetDBTools.Deploy.SQLite.SQLiteQueriesHelper;

namespace DotNetDBTools.Deploy.SQLite.Queries
{
    internal class CreateTableQuery : IQuery
    {
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public CreateTableQuery(SQLiteTableInfo table)
        {
            _sql = GetSql(table);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(SQLiteTableInfo table)
        {
            string query =
$@"CREATE TABLE {table.Name}
(
{GetTableDefinitionsText(table)}
);";

            foreach (IndexInfo index in table.Indexes)
            {
                string _ =
$@"CREATE INDEX {index.Name}
ON {table.Name} ({string.Join(", ", index.Columns)});";
            }

            foreach (TriggerInfo trigger in table.Triggers)
            {
                string _ =
$@"{trigger.Code}";
            }

            return query;
        }
    }
}
