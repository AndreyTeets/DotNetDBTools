using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.MySQL.Queries.DDL;
using DotNetDBTools.Deploy.MySQL.Queries.DNDBTSysInfo;

namespace DotNetDBTools.Deploy.MySQL.Editors
{
    internal class MySQLTableEditor : TableEditor<
        MySQLInsertDNDBTSysInfoQuery,
        MySQLDeleteDNDBTSysInfoQuery,
        MySQLUpdateDNDBTSysInfoQuery,
        MySQLCreateTableQuery,
        MySQLDropTableQuery,
        MySQLAlterTableQuery>
    {
        public MySQLTableEditor(IQueryExecutor queryExecutor)
            : base(queryExecutor) { }
    }
}
