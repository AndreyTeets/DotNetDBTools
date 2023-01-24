using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

internal abstract class InsertDNDBTDbObjectRecordQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public InsertDNDBTDbObjectRecordQuery(DbObject dbObject, DbObjectType objectType, string objectCode)
    {
        _sql = GetSql();
        _parameters = GetParameters(dbObject, objectType, objectCode);
    }

    protected abstract string GetSql();
    protected abstract List<QueryParameter> GetParameters(DbObject dbObject, DbObjectType objectType, string objectCode);
}
