using System.Collections.Generic;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Analysis.MySQL;

public class MySQLDbModelPostProcessor : DbModelPostProcessor
{
    protected override void DoAdditional_CreateDbModelFromAgnostic_PostProcessing(Database database)
    {
        MySQLDatabase db = (MySQLDatabase)database;
        ReplaceUniqueConstraintsWithUniqueIndexes(db);
    }

    protected override void DoAdditional_CreateDbModelFromCSharpDefinition_PostProcessing(Database database)
    {
        MySQLDatabase db = (MySQLDatabase)database;
        ReplaceUniqueConstraintsWithUniqueIndexes(db);
    }

    protected override void OrderAdditionalDbObjects(Database database)
    {
        MySQLDatabase db = (MySQLDatabase)database;
        db.Functions = db.Functions.OrderByName();
        db.Procedures = db.Procedures.OrderByName();
    }

    private void ReplaceUniqueConstraintsWithUniqueIndexes(MySQLDatabase database)
    {
        foreach (Table table in database.Tables)
        {
            foreach (UniqueConstraint uc in table.UniqueConstraints)
            {
                Index index = new()
                {
                    ID = uc.ID,
                    Name = uc.Name,
                    Columns = uc.Columns,
                    Unique = true,
                };
                ((List<Index>)table.Indexes).Add(index);
            }
            ((List<UniqueConstraint>)table.UniqueConstraints).Clear();
        }
    }
}
