using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DDL;
using DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.PostgreSQL.Editors
{
    internal class PostgreSQLIndexEditor : IndexEditor<
        PostgreSQLInsertDNDBTSysInfoQuery,
        PostgreSQLDeleteDNDBTSysInfoQuery,
        PostgreSQLCreateIndexQuery,
        PostgreSQLDropIndexQuery>
    {
        public PostgreSQLIndexEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }
    }
}
