using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL.Editors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.MySQL;

internal class MySQLFactory : Factory<
    MySQLQueryExecutor,
    MySQLGenSqlScriptQueryExecutor,
    MySQLDbEditor,
    MySQLDbModelFromDBMSProvider>
{
    public MySQLFactory() : base(DatabaseKind.MySQL) { }
}
