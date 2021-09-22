using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class CreateTypeQuery : IQuery
    {
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public CreateTypeQuery(MSSQLUserDefinedTypeInfo userDefinedType)
        {
            _sql = GetSql(userDefinedType);
            _parameters = new List<QueryParameter>();
        }

        private static string GetSql(MSSQLUserDefinedTypeInfo userDefinedType)
        {
            string query =
$@"CREATE TYPE {userDefinedType.Name} FROM {MSSQLSqlTypeMapper.GetSqlType(userDefinedType.UnderlyingDataType)};";

            return query;
        }
    }
}
