using System;
using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo
{
    internal abstract class DeleteDNDBTSysInfoQuery : IQuery
    {
        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public DeleteDNDBTSysInfoQuery(Guid objectID)
        {
            _sql = GetSql(objectID);
            _parameters = new List<QueryParameter>();
        }

        protected abstract string GetSql(Guid objectID);
    }
}
