﻿using System;
using System.Linq;
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

    protected override void PostProcessDataTypes(Database database)
    {
        foreach (Table table in database.Tables)
        {
            foreach (Column column in table.Columns)
            {
                if (string.IsNullOrEmpty(column.DataType.Name))
                    throw new Exception($"Column '{column.Name}' in table '{table.Name}' datatype is null or empty");

                column.DataType.Name = column.DataType.Name.ToNoWhiteSpace();
                column.DataType.Name = column.DataType.Name.ToUpper();
            }
        }
    }

    protected override void OrderAdditionalDbObjects(Database database)
    {
        MySQLDatabase db = (MySQLDatabase)database;
        db.Functions = db.Functions.OrderByNameThenByType().ToList();
        db.Procedures = db.Procedures.OrderByNameThenByType().ToList();
    }

    private void ReplaceUniqueConstraintsWithUniqueIndexes(MySQLDatabase database)
    {
        foreach (Table table in database.Tables)
        {
            foreach (UniqueConstraint uc in table.UniqueConstraints)
            {
                MySQLIndex index = new()
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
