using System;
using System.Collections.Generic;
using System.Data;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

internal class PostgreSQLUpdateDNDBTDbObjectRecordQuery : UpdateDNDBTDbObjectRecordQuery
{
    private const string IDParameterName = "@ID";
    private const string NameParameterName = "@Name";
    private const string CodeParameterName = "@Code";

    public PostgreSQLUpdateDNDBTDbObjectRecordQuery(Guid objectID, string objectName, string objectCode = null)
        : base(objectID, objectName, objectCode) { }

    protected override string GetSql()
    {
        string query =
$@"UPDATE ""{DNDBTSysTables.DNDBTDbObjects}"" SET
    ""{DNDBTSysTables.DNDBTDbObjects.Name}"" = {NameParameterName},
    ""{DNDBTSysTables.DNDBTDbObjects.Code}"" = {CodeParameterName}
WHERE ""{DNDBTSysTables.DNDBTDbObjects.ID}"" = {IDParameterName};";

        return query;
    }

    protected override List<QueryParameter> GetParameters(Guid objectID, string objectName, string objectCode)
    {
        return new List<QueryParameter>
        {
            new QueryParameter(IDParameterName, objectID, DbType.Guid),
            new QueryParameter(NameParameterName, objectName, DbType.String),
            new QueryParameter(CodeParameterName, objectCode, DbType.String),
        };
    }
}
