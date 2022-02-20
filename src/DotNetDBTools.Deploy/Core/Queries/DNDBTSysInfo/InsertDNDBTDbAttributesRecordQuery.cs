using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

internal abstract class InsertDNDBTDbAttributesRecordQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    protected const string VersionParameterName = "@Version";

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public InsertDNDBTDbAttributesRecordQuery(Database database)
    {
        _sql = GetSql();
        _parameters = GetParameters(database);
    }

    protected abstract string GetSql();
    protected abstract List<QueryParameter> GetParameters(Database database);
}
