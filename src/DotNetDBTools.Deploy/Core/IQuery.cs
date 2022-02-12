using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core;

internal interface IQuery
{
    public string Sql { get; }
    public IEnumerable<QueryParameter> Parameters { get; }
}
