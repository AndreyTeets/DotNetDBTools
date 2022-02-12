using System;
using System.Collections.Generic;
using System.Data;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo
{
    internal class MySQLInsertDNDBTSysInfoQuery : InsertDNDBTSysInfoQuery
    {
        private const string IDParameterName = "@ID";
        private const string ParentIDParameterName = "@ParentID";
        private const string NameParameterName = "@Name";
        private const string CodeParameterName = "@Code";

        public MySQLInsertDNDBTSysInfoQuery(Guid objectID, Guid? parentObjectID, DbObjectType objectType, string objectName, string objectCode = null)
            : base(objectID, parentObjectID, objectType, objectName, objectCode) { }

        protected override string GetSql(DbObjectType objectType)
        {
            string query =
$@"INSERT INTO `{DNDBTSysTables.DNDBTDbObjects}`
(
    `{DNDBTSysTables.DNDBTDbObjects.ID}`,
    `{DNDBTSysTables.DNDBTDbObjects.ParentID}`,
    `{DNDBTSysTables.DNDBTDbObjects.Type}`,
    `{DNDBTSysTables.DNDBTDbObjects.Name}`,
    `{DNDBTSysTables.DNDBTDbObjects.Code}`
)
VALUES
(
    {IDParameterName},
    {ParentIDParameterName},
    '{objectType}',
    {NameParameterName},
    {CodeParameterName}
);";

            return query;
        }

        protected override List<QueryParameter> GetParameters(Guid objectID, Guid? parentObjectID, string objectName, string objectCode)
        {
            return new List<QueryParameter>
            {
                new QueryParameter(IDParameterName, objectID.ToString(), DbType.String),
                new QueryParameter(ParentIDParameterName, parentObjectID?.ToString(), DbType.String),
                new QueryParameter(NameParameterName, objectName, DbType.String),
                new QueryParameter(CodeParameterName, objectCode, DbType.String),
            };
        }
    }
}
