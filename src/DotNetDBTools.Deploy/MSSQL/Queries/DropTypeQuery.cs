using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class DropTypeQuery : IQuery
    {
        private readonly string _sql;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        public DropTypeQuery(MSSQLUserDefinedType userDefinedType)
        {
            _sql = GetSql(userDefinedType);
        }

        private static string GetSql(MSSQLUserDefinedType userDefinedType)
        {
            string query =
$@"DROP TYPE {userDefinedType.Name};";

            return query;
        }
    }
}
