using System;
using System.Collections.Generic;
using System.Data;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo
{
    internal class UpdateDNDBTSysInfoQuery : IQuery
    {
        private const string IDParameterName = "@ID";
        private const string NameParameterName = "@Name";
        private const string ExtraInfoParameterName = "@ExtraInfo";
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public UpdateDNDBTSysInfoQuery(Guid objectID, string objectName, string extraInfo = null)
        {
            _sql = GetSql();
            _parameters = new List<QueryParameter>
            {
                new QueryParameter(IDParameterName, objectID, DbType.Guid),
                new QueryParameter(NameParameterName, objectName, DbType.String),
                new QueryParameter(ExtraInfoParameterName, extraInfo, DbType.String),
            };
        }

        private static string GetSql()
        {
            string query =
$@"UPDATE ""{DNDBTSysTables.DNDBTDbObjects}"" SET
    ""{DNDBTSysTables.DNDBTDbObjects.Name}"" = {NameParameterName},
    ""{DNDBTSysTables.DNDBTDbObjects.ExtraInfo}"" = {ExtraInfoParameterName}
WHERE ""{DNDBTSysTables.DNDBTDbObjects.ID}"" = {IDParameterName};";

            return query;
        }
    }
}
