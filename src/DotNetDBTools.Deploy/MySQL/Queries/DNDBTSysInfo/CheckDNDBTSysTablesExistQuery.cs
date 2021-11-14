using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo
{
    internal class CheckDNDBTSysTablesExistQuery : IQuery
    {
        public string Sql =>
$@"SELECT TOP 1 1 FROM sys.tables WHERE name = '{DNDBTSysTables.DNDBTDbObjects}';";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
