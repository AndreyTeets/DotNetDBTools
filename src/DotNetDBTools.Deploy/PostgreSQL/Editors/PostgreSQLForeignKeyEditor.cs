using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors
{
    internal class PostgreSQLForeignKeyEditor : ForeignKeyEditor<
        PostgreSQLInsertDNDBTSysInfoQuery,
        PostgreSQLDeleteDNDBTSysInfoQuery,
        PostgreSQLCreateForeignKeyQuery,
        PostgreSQLDropForeignKeyQuery>
    {
        public PostgreSQLForeignKeyEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }
    }
}
