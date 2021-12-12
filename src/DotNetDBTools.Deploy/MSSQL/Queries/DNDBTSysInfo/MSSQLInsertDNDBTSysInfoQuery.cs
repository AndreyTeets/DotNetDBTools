using System;
using System.Collections.Generic;
using System.Data;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo
{
    internal class MSSQLInsertDNDBTSysInfoQuery : InsertDNDBTSysInfoQuery
    {
        private const string IDParameterName = "@ID";
        private const string ParentIDParameterName = "@ParentID";
        private const string NameParameterName = "@Name";
        private const string ExtraInfoParameterName = "@ExtraInfo";

        public MSSQLInsertDNDBTSysInfoQuery(Guid objectID, Guid? parentObjectID, DbObjectsTypes objectType, string objectName, string extraInfo = null)
            : base(objectID, parentObjectID, objectType, objectName, extraInfo) { }

        protected override string GetSql(DbObjectsTypes objectType)
        {
            string query =
$@"INSERT INTO {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID},
    {DNDBTSysTables.DNDBTDbObjects.ParentID},
    {DNDBTSysTables.DNDBTDbObjects.Type},
    {DNDBTSysTables.DNDBTDbObjects.Name},
    {DNDBTSysTables.DNDBTDbObjects.ExtraInfo}
)
VALUES
(
    {IDParameterName},
    {ParentIDParameterName},
    '{objectType}',
    {NameParameterName},
    {ExtraInfoParameterName}
);";

            return query;
        }

        protected override List<QueryParameter> GetParameters(Guid objectID, Guid? parentObjectID, string objectName, string extraInfo)
        {
            return new List<QueryParameter>
            {
                new QueryParameter(IDParameterName, objectID, DbType.Guid),
                new QueryParameter(ParentIDParameterName, parentObjectID, DbType.Guid),
                new QueryParameter(NameParameterName, objectName, DbType.String),
                new QueryParameter(ExtraInfoParameterName, extraInfo, DbType.String),
            };
        }
    }
}
