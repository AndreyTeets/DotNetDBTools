using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Factories;

namespace DotNetDBTools.Deploy.MSSQL.Factories
{
    internal class MSSQLGenSqlScriptQueryExecutorFactory : IGenSqlScriptQueryExecutorFactory
    {
        public IGenSqlScriptQueryExecutor Create()
        {
            return new MSSQLGenSqlScriptQueryExecutor();
        }
    }
}
