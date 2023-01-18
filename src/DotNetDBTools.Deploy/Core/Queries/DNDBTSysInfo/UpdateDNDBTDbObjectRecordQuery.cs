using System;
using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

internal abstract class UpdateDNDBTDbObjectRecordQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public UpdateDNDBTDbObjectRecordQuery(Guid objectID, string objectName, bool updateCode, string objectCode)
    {
        _sql = GetSql(updateCode);
        _parameters = GetParameters(objectID, objectName, updateCode, objectCode);
    }

    protected abstract string GetSql(bool updateCode);
    protected abstract List<QueryParameter> GetParameters(Guid objectID, string objectName, bool updateCode, string objectCode);
}
