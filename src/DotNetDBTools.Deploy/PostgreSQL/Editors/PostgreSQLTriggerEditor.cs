using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors;

internal class PostgreSQLTriggerEditor : TriggerEditor<
    PostgreSQLInsertDNDBTDbObjectRecordQuery,
    PostgreSQLDeleteDNDBTDbObjectRecordQuery>
{
    public PostgreSQLTriggerEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
