using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL.Queries.DDL;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MSSQL.Editors;

internal class MSSQLIndexEditor : IndexEditor<
    MSSQLInsertDNDBTSysInfoQuery,
    MSSQLDeleteDNDBTSysInfoQuery,
    MSSQLCreateIndexQuery,
    MSSQLDropIndexQuery>
{
    public MSSQLIndexEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
