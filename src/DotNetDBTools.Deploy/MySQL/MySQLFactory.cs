using System.Data.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.MySQL;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.MySQL.Editors;

namespace DotNetDBTools.Deploy.MySQL
{
    internal class MySQLFactory : IFactory
    {
        public IQueryExecutor CreateQueryExecutor(DbConnection connection)
        {
            return new MySQLQueryExecutor(connection);
        }

        public IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor()
        {
            return new MySQLGenSqlScriptQueryExecutor();
        }

        public IDbModelConverter CreateDbModelConverter()
        {
            return new MySQLDbModelConverter();
        }

        public IDbEditor CreateDbEditor(IQueryExecutor queryExecutor)
        {
            return new MySQLDbEditor(queryExecutor);
        }

        public IDbModelFromDbSysInfoBuilder CreateDbModelFromDbSysInfoBuilder(IQueryExecutor queryExecutor)
        {
            return new MySQLDbModelFromDbSysInfoBuilder(queryExecutor);
        }
    }
}
