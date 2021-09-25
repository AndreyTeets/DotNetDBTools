using System;
using System.Collections.Generic;
using System.Data;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo
{
    internal class InsertDNDBTSysInfoQuery : IQuery
    {
        private const string IDParameterName = "@ID";
        private const string ParentIDParameterName = "@ParentID";
        private const string NameParameterName = "@Name";
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public InsertDNDBTSysInfoQuery(Guid objectID, Guid? parentObjectID, SQLiteDbObjectsTypes objectType, string objectName)
        {
            _sql = GetSql(objectType);
            _parameters = new List<QueryParameter>
            {
                new QueryParameter(IDParameterName, objectID.ToString(), DbType.String),
                new QueryParameter(ParentIDParameterName, parentObjectID?.ToString(), DbType.String),
                new QueryParameter(NameParameterName, objectName, DbType.String),
            };
        }

        private static string GetSql(SQLiteDbObjectsTypes objectType)
        {
            string query =
$@"INSERT INTO {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID},
    {DNDBTSysTables.DNDBTDbObjects.ParentID},
    {DNDBTSysTables.DNDBTDbObjects.Type},
    {DNDBTSysTables.DNDBTDbObjects.Name}
)
VALUES
(
    {IDParameterName},
    {ParentIDParameterName},
    '{objectType}',
    {NameParameterName}
);";

            return query;
        }
    }
}
