using DotNetDBTools.Deploy.Core;
using DotNetDBTools.Deploy.Core.Queries;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo
{
    internal class MSSQLCreateDNDBTSysTablesQuery : SqlTextOnlyQuery
    {
        public override string Sql =>
$@"CREATE TABLE {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID} UNIQUEIDENTIFIER PRIMARY KEY,
    {DNDBTSysTables.DNDBTDbObjects.ParentID} UNIQUEIDENTIFIER NULL,
    {DNDBTSysTables.DNDBTDbObjects.Type} NVARCHAR(32) NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Name} NVARCHAR(256) NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Code} NVARCHAR(MAX) NULL
);";
    }
}
