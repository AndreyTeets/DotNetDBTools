using System.Collections.Generic;

namespace DotNetDBTools.DeployInteractor
{
    public interface IQuery
    {
        public string Sql { get; }
        public IEnumerable<QueryParameter> Parameters { get; }
    }
}
