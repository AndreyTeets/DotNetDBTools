using System.Collections.Generic;
using DotNetDBTools.Deploy.Common;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class GenericQuery : IQuery
    {
        private readonly string _sql;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        public GenericQuery(string sql)
        {
            _sql = sql;
        }
    }
}
