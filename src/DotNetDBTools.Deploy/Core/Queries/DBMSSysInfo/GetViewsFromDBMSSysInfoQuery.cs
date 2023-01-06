namespace DotNetDBTools.Deploy.Core.Queries.DBMSSysInfo;

internal abstract class GetViewsFromDBMSSysInfoQuery : NoParametersQuery
{
    public class ViewRecord
    {
        public string ViewName { get; set; }
        public string ViewCode { get; set; }
    }
}
