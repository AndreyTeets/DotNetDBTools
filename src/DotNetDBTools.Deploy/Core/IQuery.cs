using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core
{
    public interface IQuery
    {
        public string Sql { get; }
        public IEnumerable<QueryParameter> Parameters { get; }
    }
}
