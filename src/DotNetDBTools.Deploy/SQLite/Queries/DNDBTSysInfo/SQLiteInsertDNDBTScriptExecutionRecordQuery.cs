using System.Collections.Generic;
using System.Data;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Generation.Core;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

internal class SQLiteInsertDNDBTScriptExecutionRecordQuery : InsertDNDBTScriptExecutionRecordQuery
{
    public SQLiteInsertDNDBTScriptExecutionRecordQuery(Script script, long executedOnDbVersion)
        : base(script, executedOnDbVersion) { }

    protected override string GetSql()
    {
        string query =
$@"INSERT INTO [{DNDBTSysTables.DNDBTScriptExecutions}]
(
    [{DNDBTSysTables.DNDBTScriptExecutions.ID}],
    [{DNDBTSysTables.DNDBTScriptExecutions.Type}],
    [{DNDBTSysTables.DNDBTScriptExecutions.Name}],
    [{DNDBTSysTables.DNDBTScriptExecutions.Code}],
    [{DNDBTSysTables.DNDBTScriptExecutions.MinDbVersionToExecute}],
    [{DNDBTSysTables.DNDBTScriptExecutions.MaxDbVersionToExecute}],
    [{DNDBTSysTables.DNDBTScriptExecutions.ExecutedOnDbVersion}]
)
VALUES
(
    {IDParameterName},
    {TypeParameterName},
    {NameParameterName},
    {CodeParameterName},
    {MinDbVersionToExecuteParameterName},
    {MaxDbVersionToExecuteParameterName},
    {ExecutedOnDbVersionParameterName}
);";

        return query;
    }

    protected override List<QueryParameter> GetParameters(Script script, long executedOnDbVersion)
    {
        return new List<QueryParameter>
        {
            new QueryParameter(IDParameterName, script.ID.ToString(), DbType.String),
            new QueryParameter(TypeParameterName, script.Kind.ToString(), DbType.String),
            new QueryParameter(NameParameterName, script.Name, DbType.String),
            new QueryParameter(CodeParameterName, script.GetText(), DbType.String),
            new QueryParameter(MinDbVersionToExecuteParameterName, script.MinDbVersionToExecute, DbType.Int64),
            new QueryParameter(MaxDbVersionToExecuteParameterName, script.MaxDbVersionToExecute, DbType.Int64),
            new QueryParameter(ExecutedOnDbVersionParameterName, executedOnDbVersion, DbType.Int64),
        };
    }
}
