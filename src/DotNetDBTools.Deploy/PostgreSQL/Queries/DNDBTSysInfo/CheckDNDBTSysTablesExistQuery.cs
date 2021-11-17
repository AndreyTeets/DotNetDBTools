using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo
{
    internal class CheckDNDBTSysTablesExistQuery : IQuery
    {
        public string Sql =>
$@"SELECT TRUE FROM pg_catalog.pg_class WHERE relname = '{DNDBTSysTables.DNDBTDbObjects}' LIMIT 1;";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
