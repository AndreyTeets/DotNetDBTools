using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

internal abstract class GetDNDBTDbAttributesRecordQuery : IQuery
{
    public abstract string Sql { get; }
    public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

    public class DNDBTDbAttributesRecord
    {
        public long Version { get; set; }
    }
}
