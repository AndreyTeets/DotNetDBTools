using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo
{
    internal class CheckDNDBTSysTablesExistQuery : IQuery
    {
        public string Sql =>
$@"SELECT TRUE FROM information_schema.tables WHERE TABLE_SCHEMA = (SELECT DATABASE()) AND TABLE_NAME = '{DNDBTSysTables.DNDBTDbObjects}' LIMIT 1;";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
