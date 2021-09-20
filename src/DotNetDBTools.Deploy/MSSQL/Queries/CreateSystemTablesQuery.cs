using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class CreateSystemTablesQuery : IQuery
    {
        public string Sql =>
$@"CREATE TABLE {DNDBTSystemTables.DNDBTDbObjects}
(
    {DNDBTSystemTables.DNDBTDbObjects.ID} UNIQUEIDENTIFIER PRIMARY KEY,
    {DNDBTSystemTables.DNDBTDbObjects.Type} NVARCHAR(64) NOT NULL,
    {DNDBTSystemTables.DNDBTDbObjects.Name} NVARCHAR(256) NOT NULL,
    {DNDBTSystemTables.DNDBTDbObjects.Metadata} NVARCHAR(MAX) NOT NULL
);";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
