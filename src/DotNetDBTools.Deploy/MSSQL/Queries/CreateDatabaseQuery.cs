using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class CreateDatabaseQuery : IQuery
    {
        private readonly string _databaseName;

        public CreateDatabaseQuery(string databaseName)
        {
            _databaseName = databaseName;
        }

        public string Sql =>
$@"CREATE DATABASE {_databaseName};";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
