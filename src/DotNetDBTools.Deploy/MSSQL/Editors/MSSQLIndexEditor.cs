using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MSSQL.Editors;

internal class MSSQLIndexEditor : IndexEditor<
    MSSQLInsertDNDBTDbObjectRecordQuery,
    MSSQLDeleteDNDBTDbObjectRecordQuery>
{
    public MSSQLIndexEditor(IQueryExecutor queryExecutor)
        : base(queryExecutor) { }
}
