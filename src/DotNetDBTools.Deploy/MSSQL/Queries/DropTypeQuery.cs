using System.Collections.Generic;
using DotNetDBTools.Deploy.Shared;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class DropTypeQuery : IQuery
    {
        private readonly string _sql;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        public DropTypeQuery(MSSQLUserDefinedTypeInfo userDefinedType)
        {
            _sql = GetSql(userDefinedType);
        }

        private static string GetSql(MSSQLUserDefinedTypeInfo userDefinedType)
        {
            string query =
$@"DROP TYPE {userDefinedType.Name};

DELETE FROM {DNDBTSysTables.DNDBTDbObjects}
WHERE {DNDBTSysTables.DNDBTDbObjects.ID} = '{userDefinedType.ID}';";

            return query;
        }
    }
}
