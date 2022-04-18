using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite.Editors;

namespace DotNetDBTools.Deploy.SQLite;

internal class SQLiteFactory : Factory<
    SQLiteQueryExecutor,
    SQLiteGenSqlScriptQueryExecutor,
    SQLiteDbModelConverter,
    SQLiteDbEditor,
    SQLiteDbModelFromDBMSProvider>
{
}
