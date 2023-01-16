using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors;

internal class PostgreSQLForeignKeysEditor : ForeignKeyEditor<
    PostgreSQLInsertDNDBTDbObjectRecordQuery,
    PostgreSQLDeleteDNDBTDbObjectRecordQuery,
    PostgreSQLCreateForeignKeyQuery,
    PostgreSQLDropForeignKeyQuery>
{
    public PostgreSQLForeignKeysEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
