using System;
using System.Collections.Generic;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.CodeParsing.Core.Models;
using DotNetDBTools.CodeParsing.PostgreSQL;
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
                List<string> statements = PostgreSQLStatementsSplitter.Split(trg.CodePiece.Code);
                if (statements.Count == 1)
                    continue;

                if (statements.Count == 2)
                {
                    PostgreSQLCodeParser parser = new();
                    FunctionInfo func = (FunctionInfo)parser.GetObjectInfo(statements[0]);

                    if (!func.ID.HasValue)
                        throw new Exception($"ID is not declared for trigger-code-embedded function '{func.Name}'");

                    PostgreSQLFunction funcModel = new()
                    {
                        ID = func.ID.Value,
                        Name = func.Name,
                        CodePiece = new CodePiece { Code = func.Code.NormalizeLineEndings() },
                    };
                    ((List<PostgreSQLFunction>)database.Functions).Add(funcModel);
                    trg.CodePiece.Code = statements[1].NormalizeLineEndings();
                    continue;
                }

                throw new Exception($"Found invalid count({statements.Count}) of statements in trigger code [{trg.CodePiece.Code}]");
            }
        }
    }
}
