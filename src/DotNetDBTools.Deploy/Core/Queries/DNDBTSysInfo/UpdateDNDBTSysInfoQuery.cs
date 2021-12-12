using System;
using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo
{
    internal abstract class UpdateDNDBTSysInfoQuery : IQuery
    {
        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public UpdateDNDBTSysInfoQuery(Guid objectID, string objectName, string extraInfo)
        {
            _sql = GetSql();
            _parameters = GetParameters(objectID, objectName, extraInfo);
        }

        protected abstract string GetSql();
        protected abstract List<QueryParameter> GetParameters(Guid objectID, string objectName, string extraInfo);
    }
}
