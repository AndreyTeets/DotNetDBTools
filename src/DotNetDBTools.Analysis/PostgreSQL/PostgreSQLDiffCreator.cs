using System.Collections.Generic;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Analysis.PostgreSQL
{
    internal class PostgreSQLDiffCreator : DiffCreator
    {
        public override DatabaseDiff CreateDatabaseDiff(Database newDatabase, Database oldDatabase)
        {
            PostgreSQLDatabaseDiff databaseDiff = new()
            {
                NewDatabase = newDatabase,
                OldDatabase = oldDatabase,
                ViewsToCreate = new List<PostgreSQLView>(),
                ViewsToDrop = new List<PostgreSQLView>(),
                FunctionsToCreate = new List<PostgreSQLFunction>(),
                FunctionsToDrop = new List<PostgreSQLFunction>(),
                ProceduresToCreate = new List<PostgreSQLProcedure>(),
                ProceduresToDrop = new List<PostgreSQLProcedure>(),
            };

            BuildTablesDiff<PostgreSQLTableDiff>(databaseDiff, newDatabase, oldDatabase);
            ForeignKeysHelper.BuildAllForeignKeysToBeDroppedAndCreated(databaseDiff);
            return databaseDiff;
        }
    }
}
