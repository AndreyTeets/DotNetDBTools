using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo
{
    internal class UpdateDNDBTSysInfoQuery : IQuery
    {
        private const string MetadataParameterName = "@Metadata";
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public UpdateDNDBTSysInfoQuery(Guid objectID, string objectName, string metadataParameterValue)
        {
            _sql = GetSql(objectID, objectName);
            _parameters = new List<QueryParameter>
            {
                new QueryParameter(MetadataParameterName, metadataParameterValue),
            };
        }

        private static string GetSql(Guid objectID, string objectName)
        {
            string query =
$@"UPDATE {DNDBTSysTables.DNDBTDbObjects} SET
    {DNDBTSysTables.DNDBTDbObjects.Name} = '{objectName}',
    {DNDBTSysTables.DNDBTDbObjects.Metadata} = {MetadataParameterName}
WHERE {DNDBTSysTables.DNDBTDbObjects.ID} = '{objectID}';";

            return query;
        }
    }
}
