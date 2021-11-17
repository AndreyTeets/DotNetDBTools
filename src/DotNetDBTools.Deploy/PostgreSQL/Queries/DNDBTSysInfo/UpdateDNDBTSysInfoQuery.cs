using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo
{
    internal class UpdateDNDBTSysInfoQuery : IQuery
    {
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public UpdateDNDBTSysInfoQuery(Guid objectID, string objectName)
        {
            _sql = GetSql(objectID, objectName);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(Guid objectID, string objectName)
        {
            string query =
$@"UPDATE ""{DNDBTSysTables.DNDBTDbObjects}"" SET
    ""{DNDBTSysTables.DNDBTDbObjects.Name}"" = '{objectName}'
WHERE ""{DNDBTSysTables.DNDBTDbObjects.ID}"" = '{objectID}';";

            return query;
        }
    }
}
