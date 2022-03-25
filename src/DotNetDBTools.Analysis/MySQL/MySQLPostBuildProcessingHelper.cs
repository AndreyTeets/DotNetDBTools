using System.Collections.Generic;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MySQL;

namespace DotNetDBTools.Analysis.MySQL;

public static class MySQLPostBuildProcessingHelper
{
    public static void ReplaceUniqueConstraintsWithUniqueIndexes(MySQLDatabase database)
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
