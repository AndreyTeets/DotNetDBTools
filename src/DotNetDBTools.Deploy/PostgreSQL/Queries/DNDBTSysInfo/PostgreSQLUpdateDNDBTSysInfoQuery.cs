using System;
using System.Collections.Generic;
using System.Data;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo
{
    internal class PostgreSQLUpdateDNDBTSysInfoQuery : UpdateDNDBTSysInfoQuery
    {
        private const string IDParameterName = "@ID";
        private const string NameParameterName = "@Name";
        private const string ExtraInfoParameterName = "@ExtraInfo";

        public PostgreSQLUpdateDNDBTSysInfoQuery(Guid objectID, string objectName, string extraInfo = null)
            : base(objectID, objectName, extraInfo) { }

        protected override string GetSql()
        {
            string query =
$@"UPDATE ""{DNDBTSysTables.DNDBTDbObjects}"" SET
    ""{DNDBTSysTables.DNDBTDbObjects.Name}"" = {NameParameterName},
    ""{DNDBTSysTables.DNDBTDbObjects.ExtraInfo}"" = {ExtraInfoParameterName}
WHERE ""{DNDBTSysTables.DNDBTDbObjects.ID}"" = {IDParameterName};";

            return query;
        }

        protected override List<QueryParameter> GetParameters(Guid objectID, string objectName, string extraInfo)
        {
            return new List<QueryParameter>
            {
                new QueryParameter(IDParameterName, objectID, DbType.Guid),
                new QueryParameter(NameParameterName, objectName, DbType.String),
                new QueryParameter(ExtraInfoParameterName, extraInfo, DbType.String),
            };
        }
    }
}
