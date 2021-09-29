using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Factories;

namespace DotNetDBTools.Deploy.SQLite.Factories
{
    internal class SQLiteGenSqlScriptQueryExecutorFactory : IGenSqlScriptQueryExecutorFactory
    {
        public IGenSqlScriptQueryExecutor Create()
        {
            return new SQLiteGenSqlScriptQueryExecutor();
        }
    }
}
