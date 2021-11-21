﻿using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.MSSQL.Queries.DNDBTSysInfo
{
    internal class CreateDNDBTSysTablesQuery : IQuery
    {
        public string Sql =>
$@"CREATE TABLE {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID} UNIQUEIDENTIFIER PRIMARY KEY,
    {DNDBTSysTables.DNDBTDbObjects.ParentID} UNIQUEIDENTIFIER NULL,
    {DNDBTSysTables.DNDBTDbObjects.Type} NVARCHAR(32) NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Name} NVARCHAR(256) NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.ExtraInfo} NVARCHAR(MAX) NULL
);";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}
