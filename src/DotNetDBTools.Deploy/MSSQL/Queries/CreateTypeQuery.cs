using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class CreateTypeQuery : IQuery
    {
        private const string MetadataParameterName = "@Metadata";
        private readonly string _sql;
        private readonly List<QueryParameter> _parameters;

        public string Sql => _sql;
        public IEnumerable<QueryParameter> Parameters => _parameters;

        public CreateTypeQuery(MSSQLUserDefinedTypeInfo userDefinedType, string metadataParameterValue)
        {
            _sql = GetSql(userDefinedType);
            _parameters = new List<QueryParameter>
            {
                new QueryParameter(MetadataParameterName, metadataParameterValue),
            };
        }

        private static string GetSql(MSSQLUserDefinedTypeInfo userDefinedType)
        {
            string query =
$@"INSERT INTO {DNDBTSystemTables.DNDBTDbObjects}
(
    {DNDBTSystemTables.DNDBTDbObjects.ID},
    {DNDBTSystemTables.DNDBTDbObjects.Type},
    {DNDBTSystemTables.DNDBTDbObjects.Name},
    {DNDBTSystemTables.DNDBTDbObjects.Metadata}
)
VALUES
(
    '{userDefinedType.ID}',
    '{MSSQLDbObjectsTypes.UserDefinedType}',
    '{userDefinedType.Name}',
    {MetadataParameterName}
);

CREATE TYPE {userDefinedType.Name} FROM NVARCHAR(11);";

            return query;
        }
    }
}
