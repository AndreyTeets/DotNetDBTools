using System.Collections.Generic;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

internal abstract class InsertDNDBTScriptExecutionRecordQuery : IQuery
{
    public string Sql => _sql;
    public IEnumerable<QueryParameter> Parameters => _parameters;

    protected const string IDParameterName = "@ID";
    protected const string TypeParameterName = "@Type";
    protected const string NameParameterName = "@Name";
    protected const string CodeParameterName = "@Code";
    protected const string MinDbVersionToExecuteParameterName = "@MinDbVersionToExecute";
    protected const string MaxDbVersionToExecuteParameterName = "@MaxDbVersionToExecute";
    protected const string ExecutedOnDbVersionParameterName = "@ExecutedOnDbVersion";

    private readonly string _sql;
    private readonly List<QueryParameter> _parameters;

    public InsertDNDBTScriptExecutionRecordQuery(Script script, long executedOnDbVersion)
    {
        _sql = GetSql();
        _parameters = GetParameters(script, executedOnDbVersion);
    }

    protected abstract string GetSql();
    protected abstract List<QueryParameter> GetParameters(Script script, long executedOnDbVersion);
}
