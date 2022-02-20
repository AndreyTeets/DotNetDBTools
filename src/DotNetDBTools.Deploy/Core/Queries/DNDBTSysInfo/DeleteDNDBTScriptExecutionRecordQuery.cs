using System;
using System.Collections.Generic;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

internal abstract class DeleteDNDBTScriptExecutionRecordQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public DeleteDNDBTScriptExecutionRecordQuery(Guid scriptID)
    {
        _sql = GetSql(scriptID);
        _parameters = new List<QueryParameter>();
    }

    protected abstract string GetSql(Guid scriptID);
}
