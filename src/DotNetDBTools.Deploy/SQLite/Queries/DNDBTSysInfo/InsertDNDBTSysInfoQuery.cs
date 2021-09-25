using System;
using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo
{
    internal class InsertDNDBTSysInfoQuery : IQuery
    {
        private const string MetadataParameterName = "@Metadata";
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public InsertDNDBTSysInfoQuery(Guid objectID, SQLiteDbObjectsTypes objectType, string objectName, string metadataParameterValue)
        {
            _sql = GetSql(objectID, objectType, objectName);
            _parameters = new List<QueryParameter>
            {
                new QueryParameter(MetadataParameterName, metadataParameterValue),
            };
        }

        private static string GetSql(Guid objectID, SQLiteDbObjectsTypes objectType, string objectName)
        {
            string query =
$@"INSERT INTO {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID},
    {DNDBTSysTables.DNDBTDbObjects.Type},
    {DNDBTSysTables.DNDBTDbObjects.Name},
    {DNDBTSysTables.DNDBTDbObjects.Metadata}
)
VALUES
(
    '{objectID}',
    '{objectType}',
    '{objectName}',
    {MetadataParameterName}
);";

            return query;
        }
    }
}
