using System;
using System.Collections.Generic;
using System.Data;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.SQLite.Queries.DNDBTSysInfo;

internal class SQLiteUpdateDNDBTDbObjectRecordQuery : UpdateDNDBTDbObjectRecordQuery
{
    private const string IDParameterName = "@ID";
    private const string NameParameterName = "@Name";
    private const string CodeParameterName = "@Code";

    public SQLiteUpdateDNDBTDbObjectRecordQuery(Guid objectID, string objectName, bool updateCode = false, string objectCode = null)
        : base(objectID, objectName, updateCode, objectCode) { }

    protected override string GetSql(bool updateCode)
    {
        string setCodeExpr = !updateCode ? "" :
$@",
    [{DNDBTSysTables.DNDBTDbObjects.Code}] = {CodeParameterName}";

        string query =
$@"UPDATE [{DNDBTSysTables.DNDBTDbObjects}] SET
    [{DNDBTSysTables.DNDBTDbObjects.Name}] = {NameParameterName}{setCodeExpr}
WHERE [{DNDBTSysTables.DNDBTDbObjects.ID}] = {IDParameterName};";

        return query;
    }

    protected override List<QueryParameter> GetParameters(Guid objectID, string objectName, bool updateCode, string objectCode)
    {
        List<QueryParameter> parameters = new()
        {
            new QueryParameter(IDParameterName, objectID.ToString(), DbType.String),
            new QueryParameter(NameParameterName, objectName, DbType.String),
        };

        if (updateCode)
            parameters.Add(new QueryParameter(CodeParameterName, objectCode, DbType.String));

        return parameters;
    }
}
