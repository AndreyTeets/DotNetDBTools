using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.SQLite.Editors;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Deploy.SQLite;

internal class SQLiteFactory : Factory<
    SQLiteQueryExecutor,
    SQLiteGenSqlScriptQueryExecutor,
    SQLiteDbEditor,
    SQLiteDbModelFromDBMSProvider>
{
    public SQLiteFactory() : base(DatabaseKind.SQLite) { }
}
