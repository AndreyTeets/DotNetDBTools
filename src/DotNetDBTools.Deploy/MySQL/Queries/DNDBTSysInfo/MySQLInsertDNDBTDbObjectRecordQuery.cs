using System.Collections.Generic;
using System.Data;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

internal class MySQLInsertDNDBTDbObjectRecordQuery : InsertDNDBTDbObjectRecordQuery
{
    private const string IDParameterName = "@ID";
    private const string ParentIDParameterName = "@ParentID";
    private const string TypeParameterName = "@Type";
    private const string NameParameterName = "@Name";
    private const string CodeParameterName = "@Code";

    public MySQLInsertDNDBTDbObjectRecordQuery(DbObject dbObject, DbObjectType objectType, string objectCode = null)
        : base(dbObject, objectType, objectCode) { }

    protected override string GetSql()
    {
        string query =
$@"INSERT INTO `{DNDBTSysTables.DNDBTDbObjects}`
(
    `{DNDBTSysTables.DNDBTDbObjects.ID}`,
    `{DNDBTSysTables.DNDBTDbObjects.ParentID}`,
    `{DNDBTSysTables.DNDBTDbObjects.Type}`,
    `{DNDBTSysTables.DNDBTDbObjects.Name}`,
    `{DNDBTSysTables.DNDBTDbObjects.Code}`
)
VALUES
(
    {IDParameterName},
    {ParentIDParameterName},
    {TypeParameterName},
    {NameParameterName},
    {CodeParameterName}
);";

        return query;
    }

    protected override List<QueryParameter> GetParameters(DbObject dbObject, DbObjectType objectType, string objectCode)
    {
        return new List<QueryParameter>
        {
            new QueryParameter(IDParameterName, dbObject.ID.ToString(), DbType.String),
            new QueryParameter(ParentIDParameterName, dbObject.Parent?.ID.ToString(), DbType.String),
            new QueryParameter(TypeParameterName, objectType.ToString(), DbType.String),
            new QueryParameter(NameParameterName, dbObject.Name, DbType.String),
            new QueryParameter(CodeParameterName, objectCode, DbType.String),
        };
    }
}
