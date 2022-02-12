using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL.Queries.DDL;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Editors;

internal class MySQLIndexEditor : IndexEditor<
    MySQLInsertDNDBTSysInfoQuery,
    MySQLDeleteDNDBTSysInfoQuery,
    MySQLCreateIndexQuery,
    MySQLDropIndexQuery>
{
    public MySQLIndexEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
