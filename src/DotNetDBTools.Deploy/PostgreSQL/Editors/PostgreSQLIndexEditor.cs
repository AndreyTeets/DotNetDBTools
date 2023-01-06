using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors;

internal class PostgreSQLIndexEditor : IndexEditor<
    PostgreSQLInsertDNDBTDbObjectRecordQuery,
    PostgreSQLDeleteDNDBTDbObjectRecordQuery>
{
    public PostgreSQLIndexEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
