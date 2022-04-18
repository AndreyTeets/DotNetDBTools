using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Analysis.MySQL;

internal class MySQLDbModelPostProcessor : DbModelPostProcessor
{
    public override void DoSpecificDbmsDbModelCreationFromDefinitionPostProcessing(Database database)
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
                table.Indexes.Add(index);
            }
            table.UniqueConstraints.Clear();
        }
    }
}
