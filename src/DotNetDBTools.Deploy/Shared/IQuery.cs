using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Shared
{
    public interface IQuery
    {
        public string Sql { get; }
        public IEnumerable<QueryParameter> Parameters { get; }
    }
}
