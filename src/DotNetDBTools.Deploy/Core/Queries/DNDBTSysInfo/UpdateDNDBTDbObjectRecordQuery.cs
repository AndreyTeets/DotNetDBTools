using System;
using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

internal abstract class UpdateDNDBTDbObjectRecordQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public UpdateDNDBTDbObjectRecordQuery(Guid objectID, string objectName, string objectCode)
    {
        _sql = GetSql();
        _parameters = GetParameters(objectID, objectName, objectCode);
    }

    protected abstract string GetSql();
    protected abstract List<QueryParameter> GetParameters(Guid objectID, string objectName, string objectCode);
}
