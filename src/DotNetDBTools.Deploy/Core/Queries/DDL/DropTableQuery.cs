using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DDL
{
    internal abstract class DropTableQuery : IQuery
    {
        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        private readonly string _sql;

        public DropTableQuery(Table table)
        {
            _sql = GetSql(table);
        }

        protected abstract string GetSql(Table table);
    }
}
