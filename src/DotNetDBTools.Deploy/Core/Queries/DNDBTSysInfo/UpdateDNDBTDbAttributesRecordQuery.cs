using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

internal abstract class UpdateDNDBTDbAttributesRecordQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    protected const string VersionParameterName = "@Version";

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public UpdateDNDBTDbAttributesRecordQuery(long databaseVersion)
    {
        _sql = GetSql();
        _parameters = GetParameters(databaseVersion);
    }

    protected abstract string GetSql();
    protected abstract List<QueryParameter> GetParameters(long databaseVersion);
}
