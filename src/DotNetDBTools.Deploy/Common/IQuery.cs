using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Common
{
    public interface IQuery
    {
        public string Sql { get; }
        public IEnumerable<QueryParameter> Parameters { get; }
    }
}
