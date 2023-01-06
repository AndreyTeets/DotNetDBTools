namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

internal abstract class GetDNDBTDbAttributesRecordQuery : NoParametersQuery
{
    public class DNDBTDbAttributesRecord
    {
        public long Version { get; set; }
    }
}
