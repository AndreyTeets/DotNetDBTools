﻿using System.Collections.Generic;
using DotNetDBTools.Deploy.Core;

namespace DotNetDBTools.Deploy.PostgreSQL.Queries.DNDBTSysInfo
{
    internal class CreateDNDBTSysTablesQuery : IQuery
    {
        public string Sql =>
$@"CREATE TABLE ""{DNDBTSysTables.DNDBTDbObjects}""
(
    ""{DNDBTSysTables.DNDBTDbObjects.ID}"" UUID PRIMARY KEY,
    ""{DNDBTSysTables.DNDBTDbObjects.ParentID}"" UUID NULL,
    ""{DNDBTSysTables.DNDBTDbObjects.Type}"" VARCHAR(32) NOT NULL,
    ""{DNDBTSysTables.DNDBTDbObjects.Name}"" VARCHAR(256) NOT NULL,
    ""{DNDBTSysTables.DNDBTDbObjects.ExtraInfo}"" TEXT NULL
);";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}