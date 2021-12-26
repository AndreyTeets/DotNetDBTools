using System;
using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo
{
    internal abstract class InsertDNDBTSysInfoQuery : IQuery
    {
        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public InsertDNDBTSysInfoQuery(Guid objectID, Guid? parentObjectID, DbObjectsTypes objectType, string objectName, string objectCode)
        {
            _sql = GetSql(objectType);
            _parameters = GetParameters(objectID, parentObjectID, objectName, objectCode);
        }

        protected abstract string GetSql(DbObjectsTypes objectType);
        protected abstract List<QueryParameter> GetParameters(Guid objectID, Guid? parentObjectID, string objectName, string objectCode);
    }
}
