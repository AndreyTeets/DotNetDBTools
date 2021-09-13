using System.Collections.Generic;
using DotNetDBTools.Models.MSSQL;

namespace DotNetDBTools.DeployInteractor.MSSQL.Queries
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
$@"INSERT INTO {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID},
    {DNDBTSysTables.DNDBTDbObjects.Type},
    {DNDBTSysTables.DNDBTDbObjects.Name},
    {DNDBTSysTables.DNDBTDbObjects.Metadata}
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
