﻿using System.Collections.Generic;

namespace DotNetDBTools.DeployInteractor.SQLite.Queries
{
    internal class CreateSystemTablesQuery : IQuery
    {
        public string Sql =>
$@"CREATE TABLE {DNDBTSysTables.DNDBTDbObjects}
(
    {DNDBTSysTables.DNDBTDbObjects.ID} BLOB PRIMARY KEY,
    {DNDBTSysTables.DNDBTDbObjects.Type} TEXT NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Name} TEXT NOT NULL,
    {DNDBTSysTables.DNDBTDbObjects.Metadata} TEXT NOT NULL
) WITHOUT ROWID;";

        public IEnumerable<QueryParameter> Parameters => new List<QueryParameter>();
    }
}