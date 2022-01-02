using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors
{
    internal class PostgreSQLTriggerEditor : TriggerEditor<
        PostgreSQLInsertDNDBTSysInfoQuery,
        PostgreSQLDeleteDNDBTSysInfoQuery,
        PostgreSQLCreateTriggerQuery,
        PostgreSQLDropTriggerQuery>
    {
        public PostgreSQLTriggerEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }
    }
}
