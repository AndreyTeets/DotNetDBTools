﻿using System.Linq;
using DotNetDBTools.Generation.Core.Sql;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Generation.PostgreSQL.Sql;

internal class PostgreSQLIndexStatementsGenerator : StatementsGenerator<PostgreSQLIndex>
{
    protected override string GetCreateSqlImpl(PostgreSQLIndex index)
    {
        string res =
$@"{GetIdDeclarationText(index, 0)}CREATE{Statements.Unique(index)} INDEX ""{index.Name}""
    ON ""{index.TableName}"" ({string.Join(", ", index.Columns.Select(x => $@"""{x}"""))});";

        return res;
    }

    protected override string GetDropSqlImpl(PostgreSQLIndex index)
    {
        return $@"DROP INDEX ""{index.Name}"";";
    }

    private static class Statements
    {
        public static string Unique(Index index) =>
index.Unique ? " UNIQUE" : ""
            ;
    }
}