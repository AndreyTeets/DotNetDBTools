﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DotNetDBTools.Analysis.Core;
using DotNetDBTools.Analysis.Extensions;
using DotNetDBTools.Models.Core;
using DotNetDBTools.Models.MSSQL;
using DotNetDBTools.Models.MSSQL.UserDefinedTypes;

namespace DotNetDBTools.Analysis.MSSQL;

internal class MSSQLDbModelPostProcessor : DbModelPostProcessor
{
    protected override void PostProcessDataTypes(Database database)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        HashSet<string> userDefinedTypesNames = MSSQLHelperMethods.GetUserDefinedTypesName(database);

        foreach (Table table in db.Tables)
        {
            foreach (Column column in table.Columns)
                PostProcessDataType(column.DataType, $"Column '{column.Name}' in table '{table.Name}'");
        }
        foreach (MSSQLUserDefinedType type in db.UserDefinedTypes)
        {
            PostProcessDataType(type.UnderlyingType, $"Domain type '{type.Name}' underlying");
        }

        void PostProcessDataType(DataType dataType, string displayedObjectInfoIfInvalid)
        {
            if (string.IsNullOrEmpty(dataType.Name))
                throw new Exception($"{displayedObjectInfoIfInvalid} datatype is null or empty");

            dataType.Name = Regex.Replace(dataType.Name, @"\s", "");
            if (MSSQLHelperMethods.IsStandardSqlType(dataType.Name))
                dataType.Name = dataType.Name.ToUpper();
            else if (userDefinedTypesNames.Contains(dataType.Name))
                dataType.IsUserDefined = true;
        }
    }

    protected override void OrderAdditionalDbObjects(Database database)
    {
        MSSQLDatabase db = (MSSQLDatabase)database;
        db.UserDefinedTypes = db.UserDefinedTypes.OrderByName();
        db.UserDefinedTableTypes = db.UserDefinedTableTypes.OrderByName();
        db.Functions = db.Functions.OrderByName();
        db.Procedures = db.Procedures.OrderByName();
    }
}
