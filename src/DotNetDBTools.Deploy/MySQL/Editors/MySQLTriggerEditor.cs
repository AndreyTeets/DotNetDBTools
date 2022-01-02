using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL.Queries.DDL;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Editors
{
    internal class MySQLTriggerEditor : TriggerEditor<
        MySQLInsertDNDBTSysInfoQuery,
        MySQLDeleteDNDBTSysInfoQuery,
        MySQLCreateTriggerQuery,
        MySQLDropTriggerQuery>
    {
        public MySQLTriggerEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }
    }
}
