using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Common.Queries.DDL
{
    internal abstract class DropTriggerQuery : IQuery
    {
        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public DropTriggerQuery(Trigger trigger, Table table)
        {
            _sql = GetSql(trigger, table);
            _parameters = new List<QueryParameter>();
        }

        protected abstract string GetSql(Trigger trigger, Table table);
    }
}
