using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core.Queries;

internal abstract class SqlTextOnlyQuery : IQuery
{
    public abstract string Sql { get; }
    public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
}
