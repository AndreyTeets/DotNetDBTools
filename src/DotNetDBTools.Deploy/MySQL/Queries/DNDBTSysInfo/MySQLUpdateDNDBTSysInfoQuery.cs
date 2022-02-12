using System;
using System.Collections.Generic;
using System.Data;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

internal class MySQLUpdateDNDBTSysInfoQuery : UpdateDNDBTSysInfoQuery
{
    private const string IDParameterName = "@ID";
    private const string NameParameterName = "@Name";
    private const string CodeParameterName = "@Code";

    public MySQLUpdateDNDBTSysInfoQuery(Guid objectID, string objectName, string objectCode = null)
        : base(objectID, objectName, objectCode) { }

    protected override string GetSql()
    {
        string query =
$@"UPDATE `{DNDBTSysTables.DNDBTDbObjects}` SET
    `{DNDBTSysTables.DNDBTDbObjects.Name}` = {NameParameterName},
    `{DNDBTSysTables.DNDBTDbObjects.Code}` = {CodeParameterName}
WHERE `{DNDBTSysTables.DNDBTDbObjects.ID}` = {IDParameterName};";

        return query;
    }

    protected override List<QueryParameter> GetParameters(Guid objectID, string objectName, string objectCode)
    {
        return new List<QueryParameter>
        {
            new QueryParameter(IDParameterName, objectID.ToString(), DbType.String),
            new QueryParameter(NameParameterName, objectName, DbType.String),
            new QueryParameter(CodeParameterName, objectCode, DbType.String),
        };
    }
}
