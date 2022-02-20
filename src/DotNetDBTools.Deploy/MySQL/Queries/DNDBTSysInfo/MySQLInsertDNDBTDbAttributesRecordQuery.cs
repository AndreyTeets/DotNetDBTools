using System.Collections.Generic;
using System.Data;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries.DNDBTSysInfo;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

internal class MySQLInsertDNDBTDbAttributesRecordQuery : InsertDNDBTDbAttributesRecordQuery
{
    public MySQLInsertDNDBTDbAttributesRecordQuery(Database database)
        : base(database) { }

    protected override string GetSql()
    {
        string query =
$@"INSERT INTO `{DNDBTSysTables.DNDBTDbAttributes}`
(
    `{DNDBTSysTables.DNDBTDbAttributes.Version}`
)
VALUES
(
    {VersionParameterName}
);";

        return query;
    }

    protected override List<QueryParameter> GetParameters(Database database)
    {
        return new List<QueryParameter>
        {
            new QueryParameter(VersionParameterName, database.Version, DbType.Int64),
        };
    }
}
