using System.Data.Common;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.SQLite;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Editors;
using DotNetDBTools.Deploy.SQLite.Editors;

namespace DotNetDBTools.Deploy.SQLite
{
    internal class SQLiteFactory : IFactory
    {
        public IQueryExecutor CreateQueryExecutor(DbConnection connection)
        {
            return new SQLiteQueryExecutor(connection);
        }

        public IGenSqlScriptQueryExecutor CreateGenSqlScriptQueryExecutor()
        {
            return new SQLiteGenSqlScriptQueryExecutor();
        }

        public IDbModelConverter CreateDbModelConverter()
        {
            return new SQLiteDbModelConverter();
        }

        public IDbEditor CreateDbEditor(IQueryExecutor queryExecutor)
        {
            return new SQLiteDbEditor(queryExecutor);
        }

        public IDbModelFromDbSysInfoBuilder CreateDbModelFromDbSysInfoBuilder(IQueryExecutor queryExecutor)
        {
            return new SQLiteDbModelFromDbSysInfoBuilder(queryExecutor);
        }
    }
}
