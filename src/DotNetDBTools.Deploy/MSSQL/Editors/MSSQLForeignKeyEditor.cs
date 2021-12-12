using DotNetDBTools.Deploy.Common.Editors;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MSSQL.Queries.DDL;
using DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MSSQL.Editors
{
    internal class MSSQLForeignKeyEditor : ForeignKeyEditor<
        MSSQLInsertDNDBTSysInfoQuery,
        MSSQLDeleteDNDBTSysInfoQuery,
        MSSQLCreateForeignKeyQuery,
        MSSQLDropForeignKeyQuery>
    {
        public MSSQLForeignKeyEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }
    }
}
