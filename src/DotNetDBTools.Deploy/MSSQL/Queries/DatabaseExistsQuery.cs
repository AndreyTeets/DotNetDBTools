using System.Collections.Generic;
using DotNetDBTools.Deploy.Shared;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class DatabaseExistsQuery : IQuery
    {
        private readonly string _databaseName;

        public DatabaseExistsQuery(string databaseName)
        {
            _databaseName = databaseName;
        }

        public string Sql =>
$@"SELECT
    CASE
        WHEN DB_ID('{_databaseName}') IS NOT NULL THEN 1
        ELSE 0
    END;";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
