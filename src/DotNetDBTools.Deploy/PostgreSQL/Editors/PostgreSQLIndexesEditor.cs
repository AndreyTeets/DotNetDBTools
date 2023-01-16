using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors;

internal class PostgreSQLIndexesEditor : IndexEditor<
    PostgreSQLInsertDNDBTDbObjectRecordQuery,
    PostgreSQLDeleteDNDBTDbObjectRecordQuery>
{
    public PostgreSQLIndexesEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
