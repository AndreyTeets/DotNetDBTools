using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo
{
    internal class DeleteDNDBTSysInfoQuery : IQuery
    {
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public DeleteDNDBTSysInfoQuery(Guid objectID)
        {
            _sql = GetSql(objectID);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(Guid objectID)
        {
            string query =
$@"DELETE FROM {DNDBTSysTables.DNDBTDbObjects}
WHERE {DNDBTSysTables.DNDBTDbObjects.ID} = '{objectID}';";

            return query;
        }
    }
}
