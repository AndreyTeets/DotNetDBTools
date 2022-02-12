using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

internal abstract class GetViewsFromDBMSSysInfoQuery : IQuery
{
    public abstract string Sql { get; }
    public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

    public class ViewRecord
    {
        public string ViewName { get; set; }
        public string ViewCode { get; set; }
    }
}
