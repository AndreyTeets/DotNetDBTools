using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class RenameUserDefinedDataTypeQuery : IQuery
    {
        private readonly string _sql;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();

        public RenameUserDefinedDataTypeQuery(MSSQLUserDefinedType userDefinedType)
        {
            _sql = GetSql(userDefinedType);
        }

        private static string GetSql(MSSQLUserDefinedType userDefinedType)
        {
            string query =
$@"EXEC sp_rename '{userDefinedType.Name}', '_DNDBTTemp_{userDefinedType.Name}', 'USERDATATYPE';";

            return query;
        }
    }
}
