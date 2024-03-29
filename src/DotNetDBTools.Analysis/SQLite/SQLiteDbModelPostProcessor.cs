﻿using System;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.SQLite;

namespace DotNetDBTools.Analysis.SQLite;

internal class SQLiteDbModelPostProcessor : DbModelPostProcessor
{
    protected override void PostProcessDataTypes(Database database)
    {
        foreach (Table table in database.Tables)
        {
            foreach (Column column in table.Columns)
            {
                if (string.IsNullOrEmpty(column.DataType.Name))
                    throw new Exception($"Column '{column.Name}' in table '{table.Name}' datatype is null or empty");

                string dataType = column.DataType.Name.ToNoWhiteSpace();
                dataType = dataType.ToUpper();

                if (dataType.Contains("INT"))
                    dataType = SQLiteDataTypeNames.INTEGER;
                else if (dataType.Contains("CHAR") || dataType.Contains("CLOB") || dataType.Contains("TEXT"))
                    dataType = SQLiteDataTypeNames.TEXT;
                else if (dataType.Contains("BLOB"))
                    dataType = SQLiteDataTypeNames.BLOB;
                else if (dataType.Contains("REAL") || dataType.Contains("FLOA") || dataType.Contains("DOUB"))
                    dataType = SQLiteDataTypeNames.REAL;
                else
                    dataType = SQLiteDataTypeNames.NUMERIC;

                column.DataType.Name = dataType;
            }
        }
    }
}
