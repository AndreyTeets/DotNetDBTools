using System.Collections.Generic;
using DotNetDBTools.Deploy.Common;

namespace DotNetDBTools.Deploy.MSSQL.Queries
{
    internal class CreateSystemTablesQuery : IQuery
    {
        public string Sql =>
$@"CREATE TABLE {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID} UNIQUEIDENTIFIER PRIMARY KEY,
    {DNDBTSysTables.DNDBTDbObjects.Type} NVARCHAR(64) NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Name} NVARCHAR(256) NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Metadata} NVARCHAR(MAX) NOT NULL
);";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
