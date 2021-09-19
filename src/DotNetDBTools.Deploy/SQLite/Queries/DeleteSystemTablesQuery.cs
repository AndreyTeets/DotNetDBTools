using System.Collections.Generic;
using DotNetDBTools.Deploy.Shared;

namespace DotNetDBTools.Deploy.SQLite.Queries
{
    internal class DeleteSystemTablesQuery : IQuery
    {
        public string Sql =>
$@"DROP TABLE {DNDBTSystemTables.DNDBTDbObjects};";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
