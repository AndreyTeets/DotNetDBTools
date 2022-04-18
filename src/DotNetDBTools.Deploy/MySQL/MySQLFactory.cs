using DotNetDBTools.Analysis.MySQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.MySQL.Editors;

namespace DotNetDBTools.Deploy.MySQL;

internal class MySQLFactory : Factory<
    MySQLQueryExecutor,
    MySQLGenSqlScriptQueryExecutor,
    MySQLDbModelConverter,
    MySQLDbEditor,
    MySQLDbModelFromDBMSProvider>
{
}
