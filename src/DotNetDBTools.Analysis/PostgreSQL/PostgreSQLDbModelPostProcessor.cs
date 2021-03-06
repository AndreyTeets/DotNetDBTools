using System;
using System.Collections.Generic;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.CodeParsing;
using DotNetDBTools.CodeParsing.Models;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.PostgreSQL;

namespace DotNetDBTools.Analysis.PostgreSQL;

internal class PostgreSQLDbModelPostProcessor : DbModelPostProcessor
{
    public override void DoSpecificDbmsDbModelCreationFromDefinitionPostProcessing(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;
        AddFunctionModelsFromTriggersCodeIfAny(db);
    }

    protected override void OrderAdditionalDbObjects(Database database)
    {
        PostgreSQLDatabase db = (PostgreSQLDatabase)database;

        db.CompositeTypes = db.CompositeTypes.OrderByName();
        db.DomainTypes = db.DomainTypes.OrderByName();
        db.EnumTypes = db.EnumTypes.OrderByName();
        db.RangeTypes = db.RangeTypes.OrderByName();

        db.Functions = db.Functions.OrderByName();
        db.Procedures = db.Procedures.OrderByName();
    }

    private void AddFunctionModelsFromTriggersCodeIfAny(PostgreSQLDatabase database)
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
                    ObjectInfo objectInfo = parser.GetObjectInfo(statements[0]);
                    if (objectInfo is FunctionInfo func)
                    {
                        if (!func.ID.HasValue)
                            throw new Exception($"ID is not declared for trigger-code-embedded function '{func.Name}'");

                        PostgreSQLFunction funcModel = new()
                        {
                            ID = func.ID.Value,
                            Name = func.Name,
                            CodePiece = new CodePiece { Code = func.Code.NormalizeLineEndings() },
                        };
                        database.Functions.Add(funcModel);
                        trg.CodePiece.Code = statements[1].NormalizeLineEndings();
                    }
                    else
                    {
                        throw new Exception($"Trigger '{trg.Name}' code contains 2 statements and first one is not a valid function");
                    }
                }
                else
                {
                    throw new Exception($"Found invalid count({statements.Count}) of statements in trigger code [{trg.CodePiece.Code}]");
                }
            }
        }
    }
}
