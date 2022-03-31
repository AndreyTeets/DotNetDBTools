using System;
using System.Collections.Generic;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Analysis.PostgreSQL;

public static class PostgreSQLPostBuildProcessingHelper
{
    public static void AddFunctionsFromTriggersCode_And_RemoveFunctionsCodeFromTriggersCode_IfAny(PostgreSQLDatabase database)
    {
        foreach (Table table in database.Tables)
        {
            foreach (Trigger trg in table.Triggers)
            {
                List<string> statements = PostgreSQLStatementsParser.ParseToStatementsList(trg.CodePiece.Code);
                if (statements.Count == 1)
                    continue;

                if (statements.Count == 2)
                {
                    PostgreSQLFunction func = PostgreSQLObjectsFromCodeParser.ParseFunction(statements[0]);
                    ((List<PostgreSQLFunction>)database.Functions).Add(func);
                    trg.CodePiece.Code = statements[1].NormalizeLineEndings();
                    continue;
                }

                throw new Exception($"Found invalid count({statements.Count}) of statements in trigger code [{trg.CodePiece.Code}]");
            }
        }
    }
}
